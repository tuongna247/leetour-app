using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public sealed class ProfileCompany
	{
		public string Address
		{
			get;
			set;
		}

		public string CardNo
		{
			get;
			set;
		}

		public string Contacts
		{
			get;
			set;
		}

		public int? CountryId
		{
			get;
			set;
		}

		public DateTime CreatedDate
		{
			get;
			set;
		}

		public string Email
		{
			get;
			set;
		}

		public int Id
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

		public string Tel
		{
			get;
			set;
		}

		public ICollection<Tour> Tours
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public ProfileCompany()
		{
			this.Tours = new List<Tour>();
		}
	}
}