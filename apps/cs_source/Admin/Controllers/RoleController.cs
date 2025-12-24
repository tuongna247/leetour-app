using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Vinaday.Admin.Models;
using Vinaday.Data.Models;
using Vinaday.Web.Framework;
using Vinaday.Web.Framework.Services;

namespace Vinaday.Admin.Controllers
{
  //  [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        //public RoleController()
        //    : this(new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new SecurityContext()))) { }
        //public RoleController(RoleManager<IdentityRole> roleManager)
        //{
        //    RoleManager = roleManager;

        //}
        //public RoleManager<IdentityRole> RoleManager { get; private set; }
        public ActionResult Index()
        {
            var context = new SecurityContext();
            var roles = context.Roles.OrderBy(x => x.Name);
            return View(roles);
        }
        //
        // POST: /Roles/Create
        [HttpPost]
        public ActionResult Create(RoleViewModel roleViewModel)
        {
            var objectModel = new ObjectModel();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new SecurityContext()));


            if (!roleManager.RoleExists(roleViewModel.Name))
            {
                var role = new IdentityRole { Name = roleViewModel.Name };
                roleManager.Create(role);
                objectModel.Status = (int)Utilities.Status.Active;
                objectModel.Url = string.Format("~/Role/Index");
            }
            else
            {
                objectModel.Status = (int)Utilities.Status.Inactive;
                objectModel.Message = string.Format("Insert category detail is error!");
            }
            return Json(objectModel);
        }

    }
}