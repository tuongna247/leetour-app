using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class Role1
	{
		public int Id
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public virtual ICollection<User1> Users
		{
			get;
			set;
		}

		public Role1()
		{
			this.Users = new List<User1>();
		}
	}
}