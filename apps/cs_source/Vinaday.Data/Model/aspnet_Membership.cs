using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class aspnet_Membership : Entity
	{
		public Guid ApplicationId
		{
			get;
			set;
		}

		public virtual Vinaday.Data.Models.aspnet_Applications aspnet_Applications
		{
			get;
			set;
		}

		public virtual Vinaday.Data.Models.aspnet_Users aspnet_Users
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

		public string MobilePIN
		{
			get;
			set;
		}

		public string Password
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

		public string PasswordSalt
		{
			get;
			set;
		}

		public Guid UserId
		{
			get;
			set;
		}

		public aspnet_Membership()
		{
		}
	}
}