using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class UserService : Service<aspnet_Membership>, IUserService
	{
		private readonly IRepositoryAsync<aspnet_Membership> _membershipRepository;

		public UserService(IRepositoryAsync<aspnet_Membership> membershipRepository) : base(membershipRepository)
		{
			this._membershipRepository = membershipRepository;
		}

		public aspnet_Membership GetMembershipById(Guid userGuid)
		{
			return this._membershipRepository.GetMembershipById(userGuid);
		}

		public aspnet_Membership GetMembershipByName(string email)
		{
			return this._membershipRepository.GetMembershipByEmail(email);
		}
	}
}