using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class CouponCode : Entity
	{
        public int Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
        public string Description { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public double DiscountPercent { get; set; }
        public double DiscountValue { get; set; }
        public int DiscountType { get; set; }
        public int TourId { get; set; }
        public string TourName { get; set; }
        public int Person { get; set; }

        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; }

        public int CityId { get; set; }
        public string CityName { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public int Type { get; set; }
        public int Quantity { get; set; }

        public bool ApplyTour { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string WID { get; set; }
        public string UID { get; set; }

    }
}