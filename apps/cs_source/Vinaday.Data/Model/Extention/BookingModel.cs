using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Extention
{
	public class BookingModel
	{
		public int Children { get; set; }
        public int Adult { get; set; }
        public string StartDate { get; set; }
        public int PriceType { get; set; }
        public string CouponCode { get; set; }
        public string DepartureOption { get; set; }
        public string RoomOptionId { get; set; }
        public string RoomOptionName { get; set; }
        public string RoomOptionRate { get; set; }
        public string GroupType { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal TourPriceOption { get; set; }
        public int RateType { get; set; }
        public decimal FinalRate { get; set; }

        public Vinaday.Data.Models.Customer Customer { get; set; }

        public Medium Image { get; set; }

        public Vinaday.Data.Models.Order Order { get; set; }

        public Tour Product { get; set; }

        public SpecialRate Promotion { get; set; }

        public Tour_Promotion Promotion2 { get; set; }

        public Vinaday.Data.Models.Rate Rate { get; set; }

        public SpecialRate Surcharge { get; set; }

        public List<Tour_Surcharge> Surcharge2 { get; set; }

        public BookingModel()
		{
		}
	}
}