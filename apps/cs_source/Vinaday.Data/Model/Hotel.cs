using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public sealed class Hotel : Entity
	{
		public string AccommodationtypeId
		{
			get;
			set;
		}

		public int? AccountcontractId
		{
			get;
			set;
		}

		public bool? AirportTransferAvailable
		{
			get;
			set;
		}

		public double? AirportTransferFee
		{
			get;
			set;
		}

		public double? BreakfastCharge
		{
			get;
			set;
		}

		public int? CancelpolicyFromDay
		{
			get;
			set;
		}

		public int? CancelpolicyToday
		{
			get;
			set;
		}

		public int? CancelPolicyType
		{
			get;
			set;
		}

		public string CancelPolicyValue1
		{
			get;
			set;
		}

		public string CancelPolicyvalue1Vn
		{
			get;
			set;
		}

		public string CancelPolicyValue2
		{
			get;
			set;
		}

		public string CancelPolicyvalue2Vn
		{
			get;
			set;
		}

		public string CheckIn
		{
			get;
			set;
		}

		public string CheckOut
		{
			get;
			set;
		}

		public string CITY
		{
			get;
			set;
		}

		public int? CityId
		{
			get;
			set;
		}

		public string ClubContent
		{
			get;
			set;
		}

		public string ClubIncentives
		{
			get;
			set;
		}

		public int? CollectionValue
		{
			get;
			set;
		}

		public ICollection<COMMENT> Comments
		{
			get;
			set;
		}

		public Vinaday.Data.Models.Contact Contact
		{
			get;
			set;
		}

		public Vinaday.Data.Models.Contact Contact1
		{
			get;
			set;
		}

		public Vinaday.Data.Models.Contact Contact2
		{
			get;
			set;
		}

		public Vinaday.Data.Models.Contact Contact3
		{
			get;
			set;
		}

		public string Country
		{
			get;
			set;
		}

		public Vinaday.Data.Models.Country Country1
		{
			get;
			set;
		}

		public int? CountryId
		{
			get;
			set;
		}

		public int? Currency
		{
			get;
			set;
		}

		public string Description { get; set; }
        public string ImportantNoteEn { get; set; }
        public string ImportantNoteVn { get; set; }

        public string DistanceFromMainCity
		{
			get;
			set;
		}

		public string EarliestCheckIn
		{
			get;
			set;
		}

		public string Fax
		{
			get;
			set;
		}

		public string HotelAbbrName
		{
			get;
			set;
		}

		public string HotelAnotherName
		{
			get;
			set;
		}

		public string HotelBrand
		{
			get;
			set;
		}

		public ICollection<HotelCancellation> HotelCancellations
		{
			get;
			set;
		}

		public string HotelChain
		{
			get;
			set;
		}

		public ICollection<Room> HotelDetails
		{
			get;
			set;
		}

		public string HotelFacilities
		{
			get;
			set;
		}

		public ICollection<Vinaday.Data.Models.HotelImages> HotelImages
		{
			get;
			set;
		}

		public string HotelKeyword
		{
			get;
			set;
		}

		public string HotelName
		{
			get;
			set;
		}

		public string HotelNameDesc
		{
			get;
			set;
		}

		public string HotelNameLocal
		{
			get;
			set;
		}

		public string HotelNameLocalDesc
		{
			get;
			set;
		}

		public string HotelNameLocalDesc2
		{
			get;
			set;
		}

		public string HotelOtherName
		{
			get;
			set;
		}

		public string HotelPicture1
		{
			get;
			set;
		}

		public string HotelPicture2
		{
			get;
			set;
		}

		public string HotelPicture3
		{
			get;
			set;
		}

		public string HotelPicture4
		{
			get;
			set;
		}

		public string HotelPicture5
		{
			get;
			set;
		}

		public string HotelPolicy
		{
			get;
			set;
		}

		public string HotelRoomFacilities
		{
			get;
			set;
		}

		public string HotelSportRecreation
		{
			get;
			set;
		}

		public string HotelStyle
		{
			get;
			set;
		}

		public string HotelStyleName
		{
			get;
			set;
		}

		public ICollection<HOTELSURCHARGE> HotelSurcharges
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public IMAGE Image
		{
			get;
			set;
		}

		public int? ImageId
		{
			get;
			set;
		}

		public bool? IsClub
		{
			get;
			set;
		}

		public bool? IsSeo
		{
			get;
			set;
		}

		public string KeyValue
		{
			get;
			set;
		}

		public string KeywordVn
		{
			get;
			set;
		}

		public string KeywordVn2
		{
			get;
			set;
		}

		public double? Latitude
		{
			get;
			set;
		}

		public double? Longtitude
		{
			get;
			set;
		}

		public int MaincontractId
		{
			get;
			set;
		}

		public int? MarketingcontractId
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public int? NumberOfFloors
		{
			get;
			set;
		}

		public int? NumberOfRestaurants
		{
			get;
			set;
		}

		public int? NumberOfReview
		{
			get;
			set;
		}

		public int? NumberOfRoom
		{
			get;
			set;
		}

		public string Overview
		{
			get;
			set;
		}

		public bool? ParkingAvailable
		{
			get;
			set;
		}

		public string Phone
		{
			get;
			set;
		}

		public string PostCode
		{
			get;
			set;
		}

		public RateExchange RateOfExchange
		{
			get;
			set;
		}

		public string RatesCurrency
		{
			get;
			set;
		}

		public double? RatesFrom
		{
			get;
			set;
		}

		public double? RatingAverage
		{
			get;
			set;
		}

		public int? RegionId
		{
			get;
			set;
		}

		public string RegisterFormComplete
		{
			get;
			set;
		}

		public int? RegisterPercentage
		{
			get;
			set;
		}

		public int? ReservationcontractId
		{
			get;
			set;
		}

		public string RoomService
		{
			get;
			set;
		}

		public string RoomVoltage
		{
			get;
			set;
		}

		public string SeoDesc
		{
			get;
			set;
		}

		public string SeoDescVn
		{
			get;
			set;
		}

		public string SeoDescVn2
		{
			get;
			set;
		}

		public string SeoTitle
		{
			get;
			set;
		}

		public string SeoTitleVn
		{
			get;
			set;
		}

		public string SeoTitleVn2
		{
			get;
			set;
		}

		public bool? ShowAirportTransferAvailable
		{
			get;
			set;
		}

		public bool? ShowAirportTransferFee
		{
			get;
			set;
		}

		public bool? ShowBreakfastCharge
		{
			get;
			set;
		}

		public bool? ShowCheckIn
		{
			get;
			set;
		}

		public bool? ShowCheckOut
		{
			get;
			set;
		}

		public bool? ShowDistanceFromManCity
		{
			get;
			set;
		}

		public bool? ShowEarliestCheckIn
		{
			get;
			set;
		}

		public bool? ShowNumberOfRestaurants
		{
			get;
			set;
		}

		public bool? ShowParkingAvailable
		{
			get;
			set;
		}

		public bool? ShowRoomService
		{
			get;
			set;
		}

		public bool? ShowRoomVoltage
		{
			get;
			set;
		}

		public bool? ShowTimeToAirport
		{
			get;
			set;
		}

		public bool? ShowYearHotelBuilt
		{
			get;
			set;
		}

		public bool? ShowYearHotelLastRenovated
		{
			get;
			set;
		}

		public int? StartRating
		{
			get;
			set;
		}

		public string StateProvince
		{
			get;
			set;
		}

		public int? Status
		{
			get;
			set;
		}

		public string StreetAddressEnglish
		{
			get;
			set;
		}

		public string StreetAddressLocal
		{
			get;
			set;
		}

		public string TimeToAirport
		{
			get;
			set;
		}

		public string Url
		{
			get;
			set;
		}

		public string VnUrl
		{
			get;
			set;
		}

		public string Website
		{
			get;
			set;
		}

		public string YearHotelBuilt
		{
			get;
			set;
		}

        public string HashTagEn { get; set; }
        public string HashTagVn { get; set; }


        public string YearHotelLastRenovated
		{
			get;
			set;
		}

		public Hotel()
		{
			this.Comments = new List<COMMENT>();
			this.HotelImages = new List<Vinaday.Data.Models.HotelImages>();
			this.HotelCancellations = new List<HotelCancellation>();
			this.HotelDetails = new List<Room>();
			this.HotelSurcharges = new List<HOTELSURCHARGE>();
		}
	}
}