using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class aspnet_PersonalizationPerUser
	{
		public virtual Vinaday.Data.Models.aspnet_Paths aspnet_Paths
		{
			get;
			set;
		}

		public virtual Vinaday.Data.Models.aspnet_Users aspnet_Users
		{
			get;
			set;
		}

		public Guid Id
		{
			get;
			set;
		}

		public DateTime LastUpdatedDate
		{
			get;
			set;
		}

		public byte[] PageSettings
		{
			get;
			set;
		}

		public Guid? PathId
		{
			get;
			set;
		}

		public Guid? UserId
		{
			get;
			set;
		}

		public aspnet_PersonalizationPerUser()
		{
		}
	}
}