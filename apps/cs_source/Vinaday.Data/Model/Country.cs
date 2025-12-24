using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public sealed class Country : Entity
	{
		public string Capital
		{
			get;
			set;
		}

		public ICollection<City> Cities
		{
			get;
			set;
		}

		public int CountryId
		{
			get;
			set;
		}

		public string DaytripDescription
		{
			get;
			set;
		}

		public string DaytripImage
		{
			get;
			set;
		}

		public int? DayTrips
		{
			get;
			set;
		}

		public string DaytripSeoTitle
		{
			get;
			set;
		}

		public string DaytripUrl
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public int? Hotels
		{
			get;
			set;
		}

		public ICollection<Hotel> Hotels1
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

		public string NameVn
		{
			get;
			set;
		}

		public ICollection<Region> Regions
		{
			get;
			set;
		}

		public string SeoDescription
		{
			get;
			set;
		}

		public string SeoKeyword
		{
			get;
			set;
		}

		public string SeoMeta
		{
			get;
			set;
		}

		public string SeoTitle
		{
			get;
			set;
		}

		public string ShortName
		{
			get;
			set;
		}

		public bool Status
		{
			get;
			set;
		}

		public string TourDescription
		{
			get;
			set;
		}

		public string TourImage
		{
			get;
			set;
		}

		public int? Tours
		{
			get;
			set;
		}

		public string TourSeoTitle
		{
			get;
			set;
		}

		public string TourUrl
		{
			get;
			set;
		}

		public string Url
		{
			get;
			set;
		}

		public string VietnamSeoTitle
		{
			get;
			set;
		}

		public Country()
		{
			this.Cities = new List<City>();
			this.Regions = new List<Region>();
			this.Hotels1 = new List<Hotel>();
		}
	}
}