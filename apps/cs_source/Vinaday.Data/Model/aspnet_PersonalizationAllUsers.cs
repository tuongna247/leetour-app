using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class aspnet_PersonalizationAllUsers
	{
		public virtual Vinaday.Data.Models.aspnet_Paths aspnet_Paths
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

		public Guid PathId
		{
			get;
			set;
		}

		public aspnet_PersonalizationAllUsers()
		{
		}
	}
}