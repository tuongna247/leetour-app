using Repository.Pattern.Repositories;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class UserRepository
	{
		public static aspnet_Membership GetMembershipByEmail(this IRepositoryAsync<aspnet_Membership> repository, string email)
		{
			aspnet_Membership aspnetMembership = repository.Queryable().FirstOrDefault<aspnet_Membership>((aspnet_Membership x) => x.aspnet_Users.UserName.Contains(email));
			return aspnetMembership;
		}

		public static aspnet_Membership GetMembershipById(this IRepositoryAsync<aspnet_Membership> repository, Guid userId)
		{
			aspnet_Membership aspnetMembership = repository.Queryable().FirstOrDefault<aspnet_Membership>((aspnet_Membership x) => x.UserId == userId);
			return aspnetMembership;
		}
	}
}