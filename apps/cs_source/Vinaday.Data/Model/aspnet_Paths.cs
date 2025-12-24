using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class aspnet_Paths
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

		public virtual Vinaday.Data.Models.aspnet_PersonalizationAllUsers aspnet_PersonalizationAllUsers
		{
			get;
			set;
		}

		public virtual ICollection<Vinaday.Data.Models.aspnet_PersonalizationPerUser> aspnet_PersonalizationPerUser
		{
			get;
			set;
		}

		public string LoweredPath
		{
			get;
			set;
		}

		public string Path
		{
			get;
			set;
		}

		public Guid PathId
		{
			get;
			set;
		}

		public aspnet_Paths()
		{
			this.aspnet_PersonalizationPerUser = new List<Vinaday.Data.Models.aspnet_PersonalizationPerUser>();
		}
	}
}