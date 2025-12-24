using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class aspnet_Applications
	{
		public Guid ApplicationId
		{
			get;
			set;
		}

		public string ApplicationName
		{
			get;
			set;
		}

		public virtual ICollection<Vinaday.Data.Models.aspnet_Membership> aspnet_Membership
		{
			get;
			set;
		}

		public virtual ICollection<Vinaday.Data.Models.aspnet_Paths> aspnet_Paths
		{
			get;
			set;
		}

		public virtual ICollection<Vinaday.Data.Models.aspnet_Roles> aspnet_Roles
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

		public string LoweredApplicationName
		{
			get;
			set;
		}

		public aspnet_Applications()
		{
			this.aspnet_Membership = new List<Vinaday.Data.Models.aspnet_Membership>();
			this.aspnet_Paths = new List<Vinaday.Data.Models.aspnet_Paths>();
			this.aspnet_Roles = new List<Vinaday.Data.Models.aspnet_Roles>();
			this.aspnet_Users = new List<Vinaday.Data.Models.aspnet_Users>();
		}
	}
}