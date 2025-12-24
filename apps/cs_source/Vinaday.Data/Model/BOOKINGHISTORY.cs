using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class BOOKINGHISTORY
	{
		public virtual Booking BOOKING
		{
			get;
			set;
		}

		public DateTime? BookingArrive
		{
			get;
			set;
		}

		public DateTime? BookingDepart
		{
			get;
			set;
		}

		public int? BookingId
		{
			get;
			set;
		}

		public string CancellationReason
		{
			get;
			set;
		}

		public int? CancellationType
		{
			get;
			set;
		}

		public DateTime Createdate
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

		public bool? IsSendReceipt
		{
			get;
			set;
		}

		public bool? IsSendVoucher
		{
			get;
			set;
		}

		public int? STATUS
		{
			get;
			set;
		}

		public BOOKINGHISTORY()
		{
		}
	}
}