using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Extention
{
    public class HotelModel
    {
        public string Address { get; set; }

        public double AirportFee { get; set; }

        public string CheckOut { get; set; }
        public string HotelKeyword { get; set; }
        public bool ShowAirportTransferAvailable { get; set; }
        public bool ShowAirportTransferFee { get; set; }
        public bool? ShowBreakfastCharge { get; set; }
        public bool ShowDistanceFromManCity { get; set; }
        public double? AirportTransferFee { get; set; }
        public double? BreakfastCharge { get; set; }
        public bool ShowYearHotelBuilt { get; set; }

        public bool ShowEarliestCheckIn { get; set; }
        public bool ShowParkingAvailable { get; set; }
        public bool ShowRoomService { get; set; }
        public bool ShowYearHotelLastRenovated { get; set; }
        public string EarliestCheckIn { get; set; }
        public string City { get; set; }

        public int CityId { get; set; }

        public string Country { get; set; }

        public string CreateDate { get; set; }

        public string Description { get; set; }


        public List<CatDetail> Facilities { get; set; }

        public string HotelUrl { get; set; }

        public int Id { get; set; }

        public List<HotelImages> Images { get; set; }

        public string ImageUrl { get; set; }

        public bool IsBreakfast { get; set; }

        public bool IsWifi { get; set; }

        public double Latitude { get; set; }

        public double Longtitude { get; set; }
        public string Name { get; set; }
        public string ImportantNoteVn { get; set; }
        public string ImportantNoteEn { get; set; }

        public int NumberOfRestaurants { get; set; }

        public string Overview { get; set; }

        public bool ParkingAvailable { get; set; }
        public string Phone { get; set; }

        public string Price { get; set; }

        public string PricePromotion { get; set; }

        public List<PromotionModel> ProductModels { get; set; }

        public int Rating { get; set; }
        public decimal Discount { get; set; }

        public List<Room> Rooms { get; set; }

        public string RoomService { get; set; }

        public string RoomVoltage { get; set; }

        public string SeoTitle { get; set; }

        public List<CatDetail> SportRecreations { get; set; }

        public int Star { get; set; }

        public string StreetAddressEnglish { get; set; }
        public string HashTagVn { get; set; }
        public string HashTagEn { get; set; }
        public List<CategoryDetail> TheCollections { get; set; }

        public string TimeToAirport { get; set; }

        public Vinaday.Data.Models.Tip Tip { get; set; }

        public string TotalRoom { get; set; }
        public string YearHotelBuilt { get; set; }

        public string YearHotelLastRenovated { get; set; }

        public HotelModel()
        {
        }
    }
}