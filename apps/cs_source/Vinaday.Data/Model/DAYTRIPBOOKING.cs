using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class DAYTRIPBOOKING
	{
		public bool? AMENBOOKING
		{
			get;
			set;
		}

		public DateTime? CHECK_IN
		{
			get;
			set;
		}

		public DateTime? CHECK_OUT
		{
			get;
			set;
		}

		public virtual Vinaday.Data.Models.Customer Customer
		{
			get;
			set;
		}

		public int? CUSTOMERID
		{
			get;
			set;
		}

		public DateTime? Date
		{
			get;
			set;
		}

		public int? DAY
		{
			get;
			set;
		}

		public virtual Vinaday.Data.Models.DAYTRIP DAYTRIP
		{
			get;
			set;
		}

		public virtual ICollection<DAYTRIPBOOKINGHISTORY> DAYTRIPBOOKINGHISTORies
		{
			get;
			set;
		}

		public int? DaytripID
		{
			get;
			set;
		}

		public string DESCRIPTION
		{
			get;
			set;
		}

		public int? EDITBY
		{
			get;
			set;
		}

		public decimal? FEE_TAX
		{
			get;
			set;
		}

		public string GuestFirstName
		{
			get;
			set;
		}

		public string GuestLastName
		{
			get;
			set;
		}

		public int? GuestNationality
		{
			get;
			set;
		}

		public int ID
		{
			get;
			set;
		}

		public string IPLOCATION
		{
			get;
			set;
		}

		public bool? ISREFUND
		{
			get;
			set;
		}

		public string NAME
		{
			get;
			set;
		}

		public bool? OwnerNotStayAtHotel
		{
			get;
			set;
		}

		public int? PaymentStatus
		{
			get;
			set;
		}

		public int? PaymentType
		{
			get;
			set;
		}

		public int? Person
		{
			get;
			set;
		}

		public string RECEIPTID
		{
			get;
			set;
		}

		public decimal? RefundFee
		{
			get;
			set;
		}

		public decimal? ROOM_RATE
		{
			get;
			set;
		}

		public int? ROOMS
		{
			get;
			set;
		}

		public bool? SENDRECEIPT
		{
			get;
			set;
		}

		public bool? SENDVOUCHER
		{
			get;
			set;
		}

		public string SpecialRequest
		{
			get;
			set;
		}

		public string STARTTIME
		{
			get;
			set;
		}

		public decimal? SURCHARGE
		{
			get;
			set;
		}

		public string SURCHARGENAME
		{
			get;
			set;
		}

		public decimal? TOTAL
		{
			get;
			set;
		}

		public DAYTRIPBOOKING()
		{
			this.DAYTRIPBOOKINGHISTORies = new List<DAYTRIPBOOKINGHISTORY>();
		}
	}
}