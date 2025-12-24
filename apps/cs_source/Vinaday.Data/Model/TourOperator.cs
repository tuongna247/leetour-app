using System;
using System.Collections.Generic;


namespace Vinaday.Data.Models
{
	public class TourOperator
	{
		public string Address
		{
			get;
			set;
		}

		public string ContactType
		{
			get;
			set;
		}

		public int? CountryId
		{
			get;
			set;
		}

		public int CreateBy
		{
			get;
			set;
		}

		public DateTime CreateDate
		{
			get;
			set;
		}

		public virtual ICollection<DAYTRIP> DAYTRIPs
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

		public string IDCardNo
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public virtual Vinaday.Data.Models.Nationality Nationality
		{
			get;
			set;
		}

		public string Password
		{
			get;
			set;
		}

		public string Tel
		{
			get;
			set;
		}

		public virtual ICollection<TOUR> TOURs
		{
			get;
			set;
		}

		public DateTime? UpdateDate
		{
			get;
			set;
		}

		public virtual Vinaday.Data.Models.USER USER
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public TourOperator()
		{
			this.DAYTRIPs = new List<DAYTRIP>();
			this.TOURs = new List<TOUR>();
		}
	}
}