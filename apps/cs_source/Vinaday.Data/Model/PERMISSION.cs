using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class PERMISSION
	{
		public string DESCRIPTION
		{
			get;
			set;
		}

		public string NAME
		{
			get;
			set;
		}

		public int PERMISSIONID
		{
			get;
			set;
		}

		public virtual Vinaday.Data.Models.ROLE ROLE
		{
			get;
			set;
		}

		public int? ROLEID
		{
			get;
			set;
		}

		public PERMISSION()
		{
		}
	}
}