using System;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface IUserService
	{
		aspnet_Membership GetMembershipById(Guid userGuid);

		aspnet_Membership GetMembershipByName(string email);
	}
}