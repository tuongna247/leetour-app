using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class CustomerCall
	{
		public DateTime CallDate
		{
			get;
			set;
		}

		public int CustomerId
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public string Note
		{
			get;
			set;
		}

		public CustomerCall()
		{
		}
	}
}