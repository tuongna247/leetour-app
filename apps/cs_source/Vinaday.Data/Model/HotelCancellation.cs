using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class HotelCancellation : Entity
	{
		public int CancellationID
		{
			get;
			set;
		}

		public virtual Vinaday.Data.Models.CancellationPolicy CancellationPolicy
		{
			get;
			set;
		}

		public DateTime CheckInFrom
		{
			get;
			set;
		}

		public DateTime CheckOutTo
		{
			get;
			set;
		}

		public virtual Vinaday.Data.Models.Hotel Hotel
		{
			get;
			set;
		}

		public virtual Room HOTELDETAIL
		{
			get;
			set;
		}

		public int? HotelDetailId
		{
			get;
			set;
		}

		public int HotelID
		{
			get;
			set;
		}

		public int ID
		{
			get;
			set;
		}

		public int? Status
		{
			get;
			set;
		}

		public HotelCancellation()
		{
		}
	}
}