using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class DAYTRIP
	{
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

		public string CANCELPOLICYVALUE1_VN
		{
			get;
			set;
		}

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

		public string City
		{
			get;
			set;
		}

		public int? CommissionRate
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

		public virtual ICollection<Vinaday.Data.Models.DAYTRIP_IMAGES> DAYTRIP_IMAGES
		{
			get;
			set;
		}

		public virtual ICollection<DAYTRIPBOOKING> DAYTRIPBOOKINGs
		{
			get;
			set;
		}

		public int DAYTRIPID
		{
			get;
			set;
		}

		public virtual ICollection<DayTripRate> DayTripRates
		{
			get;
			set;
		}

		public string DESCRIPTION
		{
			get;
			set;
		}

		public string DropOffPoint
		{
			get;
			set;
		}

		public string Duration
		{
			get;
			set;
		}

		public int? ENDBOOKING
		{
			get;
			set;
		}

		public string Exclude
		{
			get;
			set;
		}

		public string GroupSize
		{
			get;
			set;
		}

		public string HightLight
		{
			get;
			set;
		}

		public string IMAGE
		{
			get;
			set;
		}

		public string Include
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

		public string NAME
		{
			get;
			set;
		}

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

		public string OverView
		{
			get;
			set;
		}

		public string PickupPoint
		{
			get;
			set;
		}

		public decimal? PRICE_FROM
		{
			get;
			set;
		}

		public string ProgrameDetail
		{
			get;
			set;
		}

		public virtual Vinaday.Data.Models.Region Region
		{
			get;
			set;
		}

		public string SEO_DESCRIPTION
		{
			get;
			set;
		}

		public string SEO_Keyword
		{
			get;
			set;
		}

		public int? START_RATING
		{
			get;
			set;
		}

		public int? STARTBOOKING
		{
			get;
			set;
		}

		public string StartingTime
		{
			get;
			set;
		}

		public int? Status
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public virtual Vinaday.Data.Models.TourOperator TourOperator
		{
			get;
			set;
		}

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

		public DAYTRIP()
		{
			this.DAYTRIP_IMAGES = new List<Vinaday.Data.Models.DAYTRIP_IMAGES>();
			this.DAYTRIPBOOKINGs = new List<DAYTRIPBOOKING>();
			this.DayTripRates = new List<DayTripRate>();
		}
	}
}