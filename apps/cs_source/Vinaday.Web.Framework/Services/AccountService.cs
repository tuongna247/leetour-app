using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using Vinaday.Data.Models;
using Vinaday.Data.Models.Extention;
using Vinaday.Services;
using System.Web;
namespace Vinaday.Web.Framework.Services
{
    public interface IAccountService
    {
        bool Login(string userName, string password);
        void Logout();
        string ResetPassword(string userName, bool notify = true);
        bool ChangePassword(string useName, string newPassword);
        string ErrorCodeToString(MembershipCreateStatus createStatus);
        bool CreateRole(string roleName);
        string CreateMember(UserModel user);
        aspnet_Membership GetMembershipById(Guid userGuid);
        aspnet_Membership GetMembershipByName(string email);
        bool AddUserToRole(string userName, string roleName);
        List<MembershipUser> GetAllUsers();
        bool DeleteUser(string userName);
        bool Approve(UserModel user);
        bool UnlockUser(UserModel user);
        //bool UpdateUser(UserModel user);
        List<MembershipUser> GetAllUsersByRole(string roleName);
    }
    public class AccountService : IAccountService
    {
        
        private readonly MembershipProvider _provider;
        private readonly IUserService _userService;
        public AccountService(IUserService userService)
        {
            _userService = userService;
            _provider = Membership.Provider;
        }

        public AccountService()
        {
            _provider = Membership.Provider;
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Login(string userName, string password)
        {
            var validateUser = _provider.ValidateUser(userName, password);
            if (!validateUser) return false;
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException(string.Format("value cannot be null or empty."), string.Format("userName"));
            FormsAuthentication.SetAuthCookie(userName, true);
            return true;
        }

        public void Logout()
        {
            IFormsAuthenticationService formseService = new FormsAuthenticationService();
            formseService.SignOut();
        }
        public string ResetPassword(string userName, bool notify = true)
        {
            var user = Membership.GetUser(userName);
            if (user == null) return string.Empty;
            user.UnlockUser();
            var newPass = user.ResetPassword();
            return newPass;
        }
        public bool ChangePassword(string useName, string newPassword){
            try{
                var tmpPass = ResetPassword(useName, false);
                var user = Membership.GetUser(useName);
                if (user != null) user.ChangePassword(tmpPass, newPassword);
                return true;
            }
            catch (Exception){
                return false;
                
            }
        }
        public string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Username already exists. Please enter a different user name.";
                case MembershipCreateStatus.DuplicateEmail:
                    return "A username for that e-mail address already exists. Please enter a different e-mail address.";
                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";
                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";
                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";
                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";
                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";
                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        public bool CreateRole(string roleName)
        {
            try
            {
                Roles.CreateRole(roleName);
                return true;
            }
            catch (Exception)
            {
                return false;
                //Log error here
            }
        }
        public bool AddUserToRole(string userName, string roleName)
        {
            try
            {
                Roles.AddUserToRole(userName, roleName);
                return true;
            }
            catch (Exception)
            {
                return false;
                //Log error here
            }
        }
        public string CreateMember(UserModel user)
        {
            MembershipCreateStatus status;
            Membership.CreateUser(user.UserName, user.Password, user.Email, user.PasswordQuestion, user.PasswordAnswer, user.IsApproved, null, out status);
            return status == MembershipCreateStatus.Success ? status.ToString() : ErrorCodeToString(status);

        }

        public aspnet_Membership GetMembershipById(Guid userGuid)
        {
            return _userService.GetMembershipById(userGuid);
        }
        public aspnet_Membership GetMembershipByName(string email)
        {
            return _userService.GetMembershipByName(email);
        }

        public List<MembershipUser> GetAllUsers()
        {
            var members = Membership.GetAllUsers();
            return members.Cast<MembershipUser>().ToList();
        }
        public List<MembershipUser> GetAllUsersByRole(string roleName)
        {
            var members = Membership.GetAllUsers();
            return (from MembershipUser member in members let rolesForUser = Roles.GetRolesForUser(member.UserName).FirstOrDefault() where rolesForUser == roleName select member).ToList();
        }
        public List<MembershipUser> GetAllRoles()
        {
            var members = Membership.GetAllUsers();
            return members.Cast<MembershipUser>().ToList();
        }
        public bool DeleteUser(string userName)
        {
            return Membership.DeleteUser(userName);
        }
        public bool Approve(UserModel user)
        {
            var usr = Membership.GetUser(user.UserName);
            if (usr == null)
            {
                return false;
            }
            usr.IsApproved = user.IsApproved;
            Membership.UpdateUser(usr);
            return true;
        }
        public bool UnlockUser(UserModel user)
        {
            var usr = Membership.GetUser(user.UserName);
            if (usr == null)
            {
                return false;
            }
            usr.UnlockUser();
            Membership.UpdateUser(usr);
            return true;
        }

        //public bool UpdateUser(UserModel user)
        //{
        //    var usr = Membership.GetUser(user.UserName);
        //    if (usr == null){
        //        return false;
        //    }
        //    var userHotel = _db.UserHotels.FirstOrDefault(u => u.UserId == user.UserName);
        //    if (userHotel != null){
        //        userHotel.HotelId = user.HotelId;
        //        try{
        //            _db.Entry(userHotel).State = EntityState.Modified;
        //            _db.SaveChanges();
        //        }
        //        catch (Exception){
        //            //Log error here
        //            return false;
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(user.Roles)){
        //        //Update role for user
        //        var roles = Roles.GetRolesForUser(user.UserName);
        //        if (roles.Any()){
        //            var role = roles[0];
        //            Roles.RemoveUserFromRole(user.UserName, role);
        //            Roles.AddUserToRole(user.UserName, user.Roles);
        //        }
        //    }
        //    usr.Email = user.Email;
        //    Membership.UpdateUser(usr);
        //    return true;
        //}
    }

}