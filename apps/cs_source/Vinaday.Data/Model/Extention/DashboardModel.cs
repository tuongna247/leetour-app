using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Extention
{
	public class DashboardModel
	{
		public double AirportFee
		{
			get;
			set;
		}

		public string CheckOut
		{
			get;
			set;
		}

		public string City
		{
			get;
			set;
		}

		public int CityId
		{
			get;
			set;
		}

		public string Country
		{
			get;
			set;
		}

		public string CreateDate
		{
			get;
			set;
		}

		public string EarliestCheckIn
		{
			get;
			set;
		}

		public List<RoomReguest> Emails
		{
			get;
			set;
		}

		public List<CatDetail> Facilities
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public List<HotelImages> Images
		{
			get;
			set;
		}

		public string ImageUrl
		{
			get;
			set;
		}

		public bool IsWifi
		{
			get;
			set;
		}

		public double Latitude
		{
			get;
			set;
		}

		public double Longtitude
		{
			get;
			set;
		}

		public List<Notify> Notifies
		{
			get;
			set;
		}

		public int NumberOfRestaurants
		{
			get;
			set;
		}

		public List<Order> Orders
		{
			get;
			set;
		}

		public string Overview
		{
			get;
			set;
		}

		public bool ParkingAvailable
		{
			get;
			set;
		}

		public string Price
		{
			get;
			set;
		}

		public string PricePromotion
		{
			get;
			set;
		}

		public List<PromotionModel> ProductModels
		{
			get;
			set;
		}

		public int Rating
		{
			get;
			set;
		}

		public List<RoomReguest> RoomReguests
		{
			get;
			set;
		}

		public List<Room> Rooms
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

		public string SeoTitle
		{
			get;
			set;
		}

		public List<CatDetail> SportRecreations
		{
			get;
			set;
		}

		public int Star
		{
			get;
			set;
		}

		public string TimeToAirport
		{
			get;
			set;
		}

		public Vinaday.Data.Models.Tip Tip
		{
			get;
			set;
		}

		public int TotalEmail
		{
			get;
			set;
		}

		public int TotalEmailNoneRead
		{
			get;
			set;
		}

		public int TotalNotifyNoneRead
		{
			get;
			set;
		}

		public int TotalOrder
		{
			get;
			set;
		}

		public decimal TotalRevenue
		{
			get;
			set;
		}

		public string TotalRoom
		{
			get;
			set;
		}

		public int TotalUser
		{
			get;
			set;
		}

		public string YearHotelBuilt
		{
			get;
			set;
		}

		public string YearHotelLastRenovated
		{
			get;
			set;
		}

		public DashboardModel()
		{
		}
	}
}