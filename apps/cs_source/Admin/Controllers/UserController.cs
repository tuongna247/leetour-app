using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;
using Vinaday.Admin.Models;
using Vinaday.Data.Models;
using Vinaday.Services;
using Vinaday.Web.Framework;
using Vinaday.Web.Framework.Services;
using Utilities = Vinaday.Web.Framework.Utilities;

namespace Vinaday.Admin.Controllers
{
    //  [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        public UserController(ICustomerService customerService, IUnitOfWorkAsync unitOfWorkAsync)
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new SecurityContext())), customerService, unitOfWorkAsync)
        { }
        public UserController(UserManager<ApplicationUser> userManager, ICustomerService customerService, IUnitOfWorkAsync unitOfWorkAsync)
        {
            UserManager = userManager;
            _customerService = customerService;
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        //
        // POST: /Users/Create
        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel userViewModel)
        {
            var objectModel = new ObjectModel();
            if (userViewModel == null) return Json(objectModel);
            var user = new ApplicationUser
            {
                UserName = userViewModel.UserName,
                Email = userViewModel.Email,
                EmailConfirmed = true,
                FirstName = userViewModel.FirstName,
                LastName = userViewModel.LastName,
                JoinDate = DateTime.Now,
                Level = 1,
                Avatar = "/Content/img/NoProfile.jpg",
                PhoneNumber = userViewModel.PhoneNumber,
                PhoneNumberConfirmed = true

            };

            var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);

            //Add User Admin to Role Admin
            if (adminresult.Succeeded)
            {
                objectModel.Status = (int)Utilities.Status.Active;
                objectModel.Url = string.Format("~/User/Index");
                if (String.IsNullOrEmpty(userViewModel.RoleId)) return Json(objectModel);
                //Find Role Admin
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new SecurityContext()));
                var role = await roleManager.FindByIdAsync(userViewModel.RoleId);
                var result = await UserManager.AddToRoleAsync(user.Id, role.Name);
                if (result.Succeeded)
                {
                    //Add customer for customer table.
                    var customer = new Customer
                    {
                        Firstname = user.FirstName,
                        Lastname = user.LastName,
                        UserName = user.UserName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        ISSENDMAIL = true,
                        Status = (int)Utilities.Status.Active,
                        MemberId = user.Id
                    };
                    InsertCustomer(customer);
                    return Json(objectModel);
                }
                objectModel.Status = (int)Utilities.Status.Inactive;
                objectModel.Message = string.Format("Insert user is error!");
            }
            else
            {
                objectModel.Status = (int)Utilities.Status.Inactive;
                objectModel.Message = string.Format("Insert user is error!");

            }
            return Json(objectModel);
        }

        //
        // GET: /Users/
        public async Task<ActionResult> Index()
        {

            var context = new SecurityContext();
            ViewBag.Roles = context.Roles.OrderBy(x => x.Name);

            return View(await UserManager.Users.ToListAsync());
        }
        // POST: /Users/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(ObjectModel userViewModel)
        {
            var objectModel = new ObjectModel
            {
                Status = (int)Utilities.Status.Inactive,
                Message = "Insert user is error!"
            };
            if (ModelState.IsValid)
            {
                if (userViewModel == null)
                {
                    return Json(objectModel);
                }

                var user = await UserManager.FindByIdAsync(userViewModel.StrParam1);
                var logins = user.Logins;

                foreach (var login in logins.ToList())
                {
                    await UserManager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                }

                var rolesForUser = await UserManager.GetRolesAsync(userViewModel.StrParam1);

                if (rolesForUser.Any())
                {
                    foreach (var item in rolesForUser.ToList())
                    {
                        // item should be the name of the role
                        var result = await UserManager.RemoveFromRoleAsync(user.Id, item);
                    }
                }

                await UserManager.DeleteAsync(user);
                objectModel.Status = (int)Utilities.Status.Active;
                objectModel.Message = "This rrecord is deleted!";
                DeleteCustomer(user.Id);
                return Json(objectModel);
            }
            else
            {
                return Json(objectModel);
            }
        }
        [HttpPost]
        public async Task<ActionResult> ChangePassword(ObjectModel userViewModel)
        {
            var objectModel = new ObjectModel
            {
                Status = (int)Utilities.Status.Inactive,
                Message = "Change password user is error!"
            };
            if (userViewModel == null)
            {
                return Json(objectModel);
            }
            var hashedNewPassword = UserManager.PasswordHasher.HashPassword(userViewModel.StrParam2);
            var user = await UserManager.FindByIdAsync(userViewModel.StrParam1);

            //UserManager.ChangePasswordAsync(user.Id, user.PasswordHash, hashedNewPassword);
            //UserManager.UpdateAsync(user);

            UserManager.RemovePassword(user.Id);

            UserManager.AddPassword(user.Id, userViewModel.StrParam2);
            
            return Json(objectModel);
        }
        private void DeleteCustomer(string id)
        {
            var customer = _customerService.GetCustomerByMemberId(id);
            if (customer != null)
            {
                customer.ObjectState = ObjectState.Deleted;
                _customerService.Delete(customer);
            }
            _unitOfWorkAsync.SaveChanges();
        }
        private void InsertCustomer(Customer customer)
        {
            customer.ObjectState = ObjectState.Added;
            _customerService.Insert(customer);
            _unitOfWorkAsync.SaveChanges();
        }
        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            // Add more custom claims here if you want. Eg HomeTown can be a claim for the User
            var homeclaim = new Claim(ClaimTypes.UserData, user.UserName);
            identity.AddClaim(homeclaim);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }

    public static class IdentityExtensions
    {
        public static async Task<ApplicationUser> FindByNameOrEmailAsync(this UserManager<ApplicationUser> userManager, string usernameOrEmail, string password)
        {
            var username = usernameOrEmail;
            if (!usernameOrEmail.Contains("@")) return await userManager.FindAsync(username, password);
            var userForEmail = await userManager.FindByEmailAsync(usernameOrEmail);
            if (userForEmail != null)
            {
                username = userForEmail.UserName;
            }
            return await userManager.FindAsync(username, password);
        }
    }
}