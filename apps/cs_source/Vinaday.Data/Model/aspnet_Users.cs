using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class aspnet_Users
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

		public virtual Vinaday.Data.Models.aspnet_Membership aspnet_Membership
		{
			get;
			set;
		}

		public virtual ICollection<Vinaday.Data.Models.aspnet_PersonalizationPerUser> aspnet_PersonalizationPerUser
		{
			get;
			set;
		}

		public virtual Vinaday.Data.Models.aspnet_Profile aspnet_Profile
		{
			get;
			set;
		}

		public virtual ICollection<Vinaday.Data.Models.aspnet_Roles> aspnet_Roles
		{
			get;
			set;
		}

		public bool IsAnonymous
		{
			get;
			set;
		}

		public DateTime LastActivityDate
		{
			get;
			set;
		}

		public string LoweredUserName
		{
			get;
			set;
		}

		public string MobileAlias
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

		public aspnet_Users()
		{
		}
	}
}