using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class Contact : Entity
	{
		public string COMPAREEMAILADDRESS
		{
			get;
			set;
		}

		public int CONTACTID
		{
			get;
			set;
		}

		public string EMAILADDRESS
		{
			get;
			set;
		}

		public string FAX
		{
			get;
			set;
		}

		public string FIRSTNAME
		{
			get;
			set;
		}

		public virtual ICollection<Hotel> Hotels
		{
			get;
			set;
		}

		public virtual ICollection<Hotel> Hotels1
		{
			get;
			set;
		}

		public virtual ICollection<Hotel> Hotels2
		{
			get;
			set;
		}

		public virtual ICollection<Hotel> Hotels3
		{
			get;
			set;
		}

		public string LASTNAME
		{
			get;
			set;
		}

		public string PHONE
		{
			get;
			set;
		}

		public string SALUTATION
		{
			get;
			set;
		}

		public bool? STATUS
		{
			get;
			set;
		}

		public virtual ICollection<TOUR> TOURs
		{
			get;
			set;
		}

		public virtual ICollection<TOUR> TOURs1
		{
			get;
			set;
		}

		public virtual ICollection<TOUR> TOURs2
		{
			get;
			set;
		}

		public virtual ICollection<TOUR> TOURs3
		{
			get;
			set;
		}

		public Contact()
		{
			this.Hotels = new List<Hotel>();
			this.Hotels1 = new List<Hotel>();
			this.Hotels2 = new List<Hotel>();
			this.Hotels3 = new List<Hotel>();
			this.TOURs = new List<TOUR>();
			this.TOURs1 = new List<TOUR>();
			this.TOURs2 = new List<TOUR>();
			this.TOURs3 = new List<TOUR>();
		}
	}
}