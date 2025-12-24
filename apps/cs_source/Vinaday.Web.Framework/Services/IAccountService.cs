using System;
using System.Collections.Generic;
using System.Web.Security;
using Vinaday.Data.Models;
using Vinaday.Data.Models.Extention;

namespace Vinaday.Web.Framework.Services
{
	public interface IAccountService
	{
		bool AddUserToRole(string userName, string roleName);

		bool Approve(UserModel user);

		bool ChangePassword(string useName, string newPassword);

		string CreateMember(UserModel user);

		bool CreateRole(string roleName);

		bool DeleteUser(string userName);

		string ErrorCodeToString(MembershipCreateStatus createStatus);

		List<MembershipUser> GetAllUsers();

		List<MembershipUser> GetAllUsersByRole(string roleName);

		aspnet_Membership GetMembershipById(Guid userGuid);

		aspnet_Membership GetMembershipByName(string email);

		bool Login(string userName, string password);

		void Logout();

		string ResetPassword(string userName, bool notify = true);

		bool UnlockUser(UserModel user);
	}
}