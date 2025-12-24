using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class vw_aspnet_Users
	{
		public Guid ApplicationId
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

		public vw_aspnet_Users()
		{
		}
	}
}