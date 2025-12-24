using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class Region : Entity
	{
		public virtual Country COUNTRY
		{
			get;
			set;
		}

		public int? CountryId
		{
			get;
			set;
		}

		public string DayTrip_Url
		{
			get;
			set;
		}

		public virtual ICollection<DAYTRIP> DAYTRIPs
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public int id
		{
			get;
			set;
		}

		public string ImageUrl
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string SEO_Description
		{
			get;
			set;
		}

		public string SEO_Keyword
		{
			get;
			set;
		}

		public string SEO_Title
		{
			get;
			set;
		}

		public virtual ICollection<TOUR> TOURs
		{
			get;
			set;
		}

		public string URL
		{
			get;
			set;
		}

		public Region()
		{
			this.DAYTRIPs = new List<DAYTRIP>();
			this.TOURs = new List<TOUR>();
		}
	}
}