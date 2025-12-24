using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Vinaday.Admin.Models;

namespace Vinaday.Admin.Controllers
{
	public static class IdentityExtensions
	{
		public static async Task<ApplicationUser> FindByNameOrEmailAsync(UserManager<ApplicationUser> userManager, string usernameOrEmail, string password)
		{
			ApplicationUser applicationUser;
			ApplicationUser applicationUser1;
			string userName = usernameOrEmail;
			if (usernameOrEmail.Contains("@"))
			{
				applicationUser1 = await userManager.FindByEmailAsync(usernameOrEmail);
				ApplicationUser applicationUser2 = applicationUser1;
				ApplicationUser applicationUser3 = applicationUser2;
				applicationUser2 = null;
				if (applicationUser3 != null)
				{
					userName = applicationUser3.UserName;
				}
				applicationUser1 = await userManager.FindAsync(userName, password);
				applicationUser = applicationUser1;
			}
			else
			{
				applicationUser1 = await userManager.FindAsync(userName, password);
				applicationUser = applicationUser1;
			}
			return applicationUser;
		}
	}
}