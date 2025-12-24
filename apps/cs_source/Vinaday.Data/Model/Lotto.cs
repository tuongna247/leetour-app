using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class Lotto : Entity
	{
		public DateTime Created
		{
			get;
			set;
		}

		public string HashLink
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public string ImageLink
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public bool Status
		{
			get;
			set;
		}

		public int Total
		{
			get;
			set;
		}

		public int Type
		{
			get;
			set;
		}

		public Lotto()
		{
		}
	}
}