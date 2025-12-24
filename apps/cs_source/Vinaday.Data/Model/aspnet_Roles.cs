using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class aspnet_Roles
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

		public virtual ICollection<Vinaday.Data.Models.aspnet_Users> aspnet_Users
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public string LoweredRoleName
		{
			get;
			set;
		}

		public Guid RoleId
		{
			get;
			set;
		}

		public string RoleName
		{
			get;
			set;
		}

		public aspnet_Roles()
		{
			this.aspnet_Users = new List<Vinaday.Data.Models.aspnet_Users>();
		}
	}
}