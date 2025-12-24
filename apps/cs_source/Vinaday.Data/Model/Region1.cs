using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public sealed class Region1 : Entity
	{
		public Vinaday.Data.Models.City City
		{
			get;
			set;
		}

		public int? CityId
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public string DescriptionVn
		{
			get;
			set;
		}

		public int Id
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

		public string SeoDescription
		{
			get;
			set;
		}

		public string SeoDescriptionVn
		{
			get;
			set;
		}

		public string SeoKeyword
		{
			get;
			set;
		}

		public string SeoKeywordVn
		{
			get;
			set;
		}

		public string SeoTitle
		{
			get;
			set;
		}

		public string SeoTitleVn
		{
			get;
			set;
		}

		public string Slug
		{
			get;
			set;
		}

		public int? Status
		{
			get;
			set;
		}

		public ICollection<Tour> Tours
		{
			get;
			set;
		}

		public int? Type
		{
			get;
			set;
		}

		public Region1()
		{
			this.Tours = new List<Tour>();
		}
	}
}