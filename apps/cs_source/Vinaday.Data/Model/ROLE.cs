using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class ROLE
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

		public virtual ICollection<PERMISSION> PERMISSIONs
		{
			get;
			set;
		}

		public int ROLEID
		{
			get;
			set;
		}

		public virtual ICollection<USER> USERs
		{
			get;
			set;
		}

		public ROLE()
		{
			this.PERMISSIONs = new List<PERMISSION>();
			this.USERs = new List<USER>();
		}
	}
}