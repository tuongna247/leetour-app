using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class HOTELPROMOTION
	{
		public DateTime BOOKINGENDDATE
		{
			get;
			set;
		}

		public virtual ICollection<Booking> BOOKINGs
		{
			get;
			set;
		}

		public DateTime? BOOKINGSTARTDATE
		{
			get;
			set;
		}

		public int? CANCELPOLICY
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

		public string DESCRIPTION
		{
			get;
			set;
		}

		public DateTime? ENDDATE
		{
			get;
			set;
		}

		public int? FROMDATE
		{
			get;
			set;
		}

		public virtual Room HOTELDETAIL
		{
			get;
			set;
		}

		public int? HOTELDETAILID
		{
			get;
			set;
		}

		public int HOTELPROMOTIONID
		{
			get;
			set;
		}

		public bool? IsVietNamHotel
		{
			get;
			set;
		}

		public virtual Vinaday.Data.Models.PROMOTIONSTAYPAY PROMOTIONSTAYPAY
		{
			get;
			set;
		}

		public int? PROMOTIONSTAYPAYID
		{
			get;
			set;
		}

		public int? PROMOTIONTYPE
		{
			get;
			set;
		}

		public decimal? SELL_RATE
		{
			get;
			set;
		}

		public int? SELLING_TA
		{
			get;
			set;
		}

		public DateTime? STARTDATE
		{
			get;
			set;
		}

		public bool? STATUS
		{
			get;
			set;
		}

		public int? TA_RATE
		{
			get;
			set;
		}

		public string TITLE
		{
			get;
			set;
		}

		public int? TODATE
		{
			get;
			set;
		}

		public HOTELPROMOTION()
		{
			this.BOOKINGs = new List<Booking>();
		}
	}
}