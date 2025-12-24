using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class vw_aspnet_Applications
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

		public vw_aspnet_Applications()
		{
		}
	}
}