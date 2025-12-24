using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public sealed class Tour : Entity
	{
		public string Accommondation
		{
			get;
			set;
		}

		public string Ages
		{
			get;
			set;
		}

		public int? CancelationPolicy
		{
			get;
			set;
		}

		public string CarbonEmissionOffset
		{
			get;
			set;
		}

		public string Cities
		{
			get;
			set;
		}

		public int? CommissionRate
		{
			get;
			set;
		}

		public double? CostPerDay
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

		public double? DepositRequired
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public ICollection<Detail> Details
		{
			get;
			set;
		}

		public int? Discount
		{
			get;
			set;
		}

		public string Duration
		{
			get;
			set;
		}

		public string ExcludeActivity
		{
			get;
			set;
		}

		public ICollection<Featured> Featureds
		{
			get;
			set;
		}

        public ICollection<TourTopSite> TourTopSites
        {
            get;
            set;
        }

        public string Filter
		{
			get;
			set;
		}

		public string Finish
		{
			get;
			set;
		}

		public string GroupSize
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public string IncludeActivity
		{
			get;
			set;
		}

		public string InclusiveBenefit
		{
			get;
			set;
		}

		public int? Language
		{
			get;
			set;
		}

		public string Location
		{
			get;
			set;
		}

		public string Meals
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
        public  string TourTitle { get; set; }

		public string Notes
		{
			get;
			set;
		}

		public int? OperatorId
		{
			get;
			set;
		}

		public string Overview
		{
			get;
			set;
		}

		public int? ParentId
		{
			get;
			set;
		}

		public string Price
		{
			get;
			set;
		}

		public double? PriceFrom
		{
			get;
			set;
		}

		public Vinaday.Data.Models.ProfileCompany ProfileCompany
		{
			get;
			set;
		}

		public ICollection<Rate> Rates { get; set; }
        public ICollection<Rate2> Rate2s { get; set; }
        public ICollection<Rate3> Rate3s { get; set; }

        public Region1 Region
		{
			get;
			set;
		}

		public int? RegionId
		{
			get;
			set;
		}

		public string SearchKey
		{
			get;
			set;
		}

		public string Start
		{
			get;
			set;
		}

		public int? Status
		{
			get;
			set;
		}

		public string TourCode
		{
			get;
			set;
		}

		public int? TourDetailCount
		{
			get;
			set;
		}

		public string Transport
		{
			get;
			set;
		}

		public string TravelStyle { get; set; }
        public string SEO_Description { get; set; }
	    public string SEO_Meta { get; set; }
	    public string SEO_Title { get; set; }
        public string DepartureOption1 { get; set; }
        public string DepartureOption2 { get; set; }
        public string VideoLink { get; set; }
        public string DepartureOption3 { get; set; }

        public int Type
		{
			get;
			set;
		}

		public int? YourSave { get; set; }
        public string TourGroup1 { get; set; }
        public string TourGroup2 { get; set; }
        public string TourGroup3 { get; set; }
        public string TourGroup1Include { get; set; }
        public string TourGroup2Include { get; set; }
        public string TourGroup3Include { get; set; }

        public Tour()
		{
			this.Details = new List<Detail>();
			this.Featureds = new List<Featured>();
			this.Rates = new List<Rate>();
            this.Rate2s = new List<Rate2>();
            this.Rate3s = new List<Rate3>();
        }
	}
}