using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class Room : Entity
	{
		public int? AdultNumber
		{
			get;
			set;
		}

		public int? AvailableRoom
		{
			get;
			set;
		}

		public virtual ICollection<Booking> Bookings
		{
			get;
			set;
		}

		public bool? BreakfastInclude
		{
			get;
			set;
		}

		public string BreakfastSurcharge
		{
			get;
			set;
		}

		public string CancelPolicy
		{
			get;
			set;
		}

		public int? ChildrenAge
		{
			get;
			set;
		}

		public int? ChildrenNumber
		{
			get;
			set;
		}

		public string EnglishName
		{
			get;
			set;
		}

		public virtual ICollection<ExpiredRoom> ExpiredRooms
		{
			get;
			set;
		}

		public int? ExtraBed
		{
			get;
			set;
		}

		public int? ExtraBedPrice
		{
			get;
			set;
		}

		public virtual Vinaday.Data.Models.Hotel Hotel
		{
			get;
			set;
		}

		public virtual ICollection<HotelCancellation> HotelCancellations
		{
			get;
			set;
		}

		public int? HotelId
		{
			get;
			set;
		}

        public int? Sort
        {
            get;
            set;
        }

        public int? HotelPromotionId
		{
			get;
			set;
		}

		public virtual ICollection<HOTELPROMOTION> HotelPromotions
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

		public bool? IsExpire
		{
			get;
			set;
		}

		public string LocalName
		{
			get;
			set;
		}

		public int? MaxExtrabed
		{
			get;
			set;
		}

		public int? MaxOccupancy
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string PromotionType
		{
			get;
			set;
		}

		public decimal? RackRate
		{
			get;
			set;
		}

		public virtual ICollection<RateControl> RateControls
		{
			get;
			set;
		}

		public virtual ICollection<RoomControl> RoomControls
		{
			get;
			set;
		}

		public string RoomFacilities
		{
			get;
			set;
		}

		public int? RoomSize
		{
			get;
			set;
		}

		public int? SellingRate
		{
			get;
			set;
		}

		public decimal? SellingTa
		{
			get;
			set;
		}

		public bool? Status
		{
			get;
			set;
		}

		public decimal? TaRate
		{
			get;
			set;
		}

		public string View
		{
			get;
			set;
		}

		public Room()
		{
			this.Bookings = new List<Booking>();
			this.ExpiredRooms = new List<ExpiredRoom>();
			this.HotelCancellations = new List<HotelCancellation>();
			this.HotelPromotions = new List<HOTELPROMOTION>();
			this.RateControls = new List<RateControl>();
			this.RoomControls = new List<RoomControl>();
		}
	}
}