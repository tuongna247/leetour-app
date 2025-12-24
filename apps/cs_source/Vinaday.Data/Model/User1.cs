using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class User1
	{
		public DateTime CreationDate
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public string Password
		{
			get;
			set;
		}

		public virtual Role1 Role
		{
			get;
			set;
		}

		public int RoleId
		{
			get;
			set;
		}

		public string Username
		{
			get;
			set;
		}

		public User1()
		{
		}
	}
}