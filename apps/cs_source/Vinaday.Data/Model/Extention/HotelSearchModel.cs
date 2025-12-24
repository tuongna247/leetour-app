using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Extention
{
    public class HotelSearchModel
    {
        public string Address { get; set; }
        public DateTime EndDate { get; set; }
        public int Quantity { get; set; }

        public double AirportFee { get; set; }

        public string CheckOut { get; set; }

        public string City { get; set; }
        public int CityId { get; set; }

        public Vinaday.Data.Models.City CityModel { get; set; }

        public int? CollectionValue { get; set; }

        public int Count { get; set; }

        public string Country { get; set; }

        public string CreateDate { get; set; }

        public string Description { get; set; }

        public string EarliestCheckIn { get; set; }

        public string Facilities { get; set; }

        public string HotelUrl { get; set; }

        public int Id { get; set; }

        public List<HotelImages> Images { get; set; }

        public string ImageUrl { get; set; }

        public bool IsBreakfast { get; set; }

        public bool IsWifi { get; set; }

        public double Latitude { get; set; }

        public double Longtitude { get; set; }

        public string Name { get; set; }

        public string NameVn { get; set; }

        public int NumberOfRestaurants { get; set; }

        public string Overview { get; set; }
        public bool ParkingAvailable { get; set; }
        public string Price { get; set; }

        public string PricePromotion { get; set; }

        public List<PromotionModel> ProductModels { get; set; }

        public int Rating { get; set; }

        public Vinaday.Data.Models.Extention.ReviewModel ReviewModel { get; set; }

        public List<Room> Rooms { get; set; }
        public string RoomService { get; set; }

        public string RoomVoltage { get; set; }

        public string SeoTitle { get; set; }
        public int Star { get; set; }

        public List<CategoryDetail> TheCollections { get; set; }

        public string TimeToAirport { get; set; }

        public Vinaday.Data.Models.Tip Tip { get; set; }
        public string TotalRoom { get; set; }
        public decimal DiscountPercent { get; set; }

        public string YearHotelBuilt { get; set; }

        public string Phone { get; set; }

        public string YearHotelLastRenovated { get; set; }

        public HotelSearchModel()
        {
        }
    }
}