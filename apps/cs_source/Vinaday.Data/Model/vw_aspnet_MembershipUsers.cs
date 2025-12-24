using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class vw_aspnet_MembershipUsers
	{
		public Guid ApplicationId
		{
			get;
			set;
		}

		public string Comment
		{
			get;
			set;
		}

		public DateTime CreateDate
		{
			get;
			set;
		}

		public string Email
		{
			get;
			set;
		}

		public int FailedPasswordAnswerAttemptCount
		{
			get;
			set;
		}

		public DateTime FailedPasswordAnswerAttemptWindowStart
		{
			get;
			set;
		}

		public int FailedPasswordAttemptCount
		{
			get;
			set;
		}

		public DateTime FailedPasswordAttemptWindowStart
		{
			get;
			set;
		}

		public bool IsAnonymous
		{
			get;
			set;
		}

		public bool IsApproved
		{
			get;
			set;
		}

		public bool IsLockedOut
		{
			get;
			set;
		}

		public DateTime LastActivityDate
		{
			get;
			set;
		}

		public DateTime LastLockoutDate
		{
			get;
			set;
		}

		public DateTime LastLoginDate
		{
			get;
			set;
		}

		public DateTime LastPasswordChangedDate
		{
			get;
			set;
		}

		public string LoweredEmail
		{
			get;
			set;
		}

		public string MobileAlias
		{
			get;
			set;
		}

		public string MobilePIN
		{
			get;
			set;
		}

		public string PasswordAnswer
		{
			get;
			set;
		}

		public int PasswordFormat
		{
			get;
			set;
		}

		public string PasswordQuestion
		{
			get;
			set;
		}

		public Guid UserId
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public vw_aspnet_MembershipUsers()
		{
		}
	}
}