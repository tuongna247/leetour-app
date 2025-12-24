using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class aspnet_Profile
	{
		public virtual Vinaday.Data.Models.aspnet_Users aspnet_Users
		{
			get;
			set;
		}

		public DateTime LastUpdatedDate
		{
			get;
			set;
		}

		public string PropertyNames
		{
			get;
			set;
		}

		public byte[] PropertyValuesBinary
		{
			get;
			set;
		}

		public string PropertyValuesString
		{
			get;
			set;
		}

		public Guid UserId
		{
			get;
			set;
		}

		public aspnet_Profile()
		{
		}
	}
}