using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class CreditCard : Entity
	{
		public string Address
		{
			get;
			set;
		}

		public string CardNumber
		{
			get;
			set;
		}

		public DateTime CreatedDate
		{
			get;
			set;
		}

		public string Cvv
		{
			get;
			set;
		}

		public string ExpMonth
		{
			get;
			set;
		}

		public string ExpYear
		{
			get;
			set;
		}

		public string FirstName
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public bool? IsUse
		{
			get;
			set;
		}

		public string LastName
		{
			get;
			set;
		}

		public DateTime ModifiedDate
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public int? Status
		{
			get;
			set;
		}

		public int? Type
		{
			get;
			set;
		}

		public CreditCard()
		{
		}
	}
}