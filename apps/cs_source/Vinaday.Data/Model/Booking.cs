using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public sealed class Booking : Entity
	{
		public bool? AmenBooking
		{
			get;
			set;
		}

		public ICollection<BOOKINGHISTORY> BookingHistories
		{
			get;
			set;
		}

		public DateTime? CheckIn
		{
			get;
			set;
		}

		public DateTime? CheckOut
		{
			get;
			set;
		}

		public Vinaday.Data.Models.Customer Customer
		{
			get;
			set;
		}

		public int? CustomerId
		{
			get;
			set;
		}

		public DateTime? Date
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public int? EditBy
		{
			get;
			set;
		}

		public decimal? FeeTax
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

		public HOTELPROMOTION HotelPromotion
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public string IpLocation
		{
			get;
			set;
		}

		public bool? IsRefund
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public Vinaday.Data.Models.Nationality Nationality
		{
			get;
			set;
		}

		public int? Night
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

		public string Pnr
		{
			get;
			set;
		}

		public int? PromotionId
		{
			get;
			set;
		}

		public string ReceiptId
		{
			get;
			set;
		}

		public decimal? RefundFee
		{
			get;
			set;
		}

		public Vinaday.Data.Models.Room Room
		{
			get;
			set;
		}

		public int? RoomId
		{
			get;
			set;
		}

		public decimal? RoomRate
		{
			get;
			set;
		}

		public int? Rooms
		{
			get;
			set;
		}

		public bool? SendReceipt
		{
			get;
			set;
		}

		public bool? SendVoucher
		{
			get;
			set;
		}

		public string SpecialRequest
		{
			get;
			set;
		}

		public decimal? Surcharge
		{
			get;
			set;
		}

		public string SurchargeName
		{
			get;
			set;
		}

		public decimal? Total
		{
			get;
			set;
		}

		public USER User
		{
			get;
			set;
		}

		public Booking()
		{
			this.BookingHistories = new List<BOOKINGHISTORY>();
		}
	}
}