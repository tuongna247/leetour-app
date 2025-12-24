using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class TOUR
	{
		public string Accommondation
		{
			get;
			set;
		}

		public int? AccountContactId
		{
			get;
			set;
		}

		public string Ages
		{
			get;
			set;
		}

		public int? CANCELPOLICY_FROMDAY
		{
			get;
			set;
		}

		public int? CANCELPOLICY_TODAY
		{
			get;
			set;
		}

		public int? CANCELPOLICYTYPE
		{
			get;
			set;
		}

		public string CANCELPOLICYVALUE1
		{
			get;
			set;
		}

		public string CANCELPOLICYVALUE1_VN { get; set; }

        public  string LinkRedirect { get; set; }


        public string CANCELPOLICYVALUE2
		{
			get;
			set;
		}

		public string CANCELPOLICYVALUE2_VN
		{
			get;
			set;
		}

		public string CarbonEmissionOffset
		{
			get;
			set;
		}

		public int? CommissionRate
		{
			get;
			set;
		}

		public virtual Vinaday.Data.Models.Contact Contact
		{
			get;
			set;
		}

		public virtual Vinaday.Data.Models.Contact Contact1
		{
			get;
			set;
		}

		public virtual Vinaday.Data.Models.Contact Contact2
		{
			get;
			set;
		}

		public virtual Vinaday.Data.Models.Contact Contact3
		{
			get;
			set;
		}

		public double? CostPerDay
		{
			get;
			set;
		}

		public string Country
		{
			get;
			set;
		}

		public int? CountryId
		{
			get;
			set;
		}

		public double? DepositRequired
		{
			get;
			set;
		}

		public string DESCRIPTION
		{
			get;
			set;
		}

		public string Duration
		{
			get;
			set;
		}

		public string FINISH
		{
			get;
			set;
		}

		public string GroupSize
		{
			get;
			set;
		}

		public string IMAGE
		{
			get;
			set;
		}

		public string IncludeActivity
		{
			get;
			set;
		}

		public string Location
		{
			get;
			set;
		}

		public int? LocationId
		{
			get;
			set;
		}

		public int? MainContactId
		{
			get;
			set;
		}

		public int? MarketingContactId
		{
			get;
			set;
		}

		public string Meals
		{
			get;
			set;
		}

		public string NAME
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

		public string Price
		{
			get;
			set;
		}

		public double? PRICE_FROM
		{
			get;
			set;
		}

		public virtual Vinaday.Data.Models.Region Region
		{
			get;
			set;
		}

		public int? ReserContactId
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

		public string START
		{
			get;
			set;
		}

		public int? START_RATING
		{
			get;
			set;
		}

		public int? Status
		{
			get;
			set;
		}

		public double? Total
		{
			get;
			set;
		}

		public virtual ICollection<TOURBOOKING> TOURBOOKINGs
		{
			get;
			set;
		}

		public int? TourDetailCount
		{
			get;
			set;
		}

		public virtual ICollection<TOURDETAIL> TOURDETAILs
		{
			get;
			set;
		}

		public int TOURID
		{
			get;
			set;
		}

		public virtual Vinaday.Data.Models.TourOperator TourOperator
		{
			get;
			set;
		}

		public virtual ICollection<TourRate> TourRates { get; set; }


		public string Transport
		{
			get;
			set;
		}

		public string TravelStyle
		{
			get;
			set;
		}

		public string URL
		{
			get;
			set;
		}

		public TOUR()
		{
			this.TOURBOOKINGs = new List<TOURBOOKING>();
			this.TOURDETAILs = new List<TOURDETAIL>();
			this.TourRates = new List<TourRate>();
		}
	}
}