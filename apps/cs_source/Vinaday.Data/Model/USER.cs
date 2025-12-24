using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class USER
	{
		public virtual ICollection<Booking> BOOKINGs
		{
			get;
			set;
		}

		public string DESCRIPTION
		{
			get;
			set;
		}

		public int? HotelID
		{
			get;
			set;
		}

		public string PASSWORD
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

		public virtual ICollection<TourOperator> TourOperators
		{
			get;
			set;
		}

		public int USERID
		{
			get;
			set;
		}

		public string USERNAME
		{
			get;
			set;
		}

		public USER()
		{
			this.BOOKINGs = new List<Booking>();
			this.TourOperators = new List<TourOperator>();
		}
	}
}