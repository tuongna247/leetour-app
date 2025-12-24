using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;
using Vinaday.Data.Models.Extention;

namespace Vinaday.Services
{
	public interface IBookingService : IService<Booking>
	{
		string GeneratePnrNumber();

		List<Booking> GetGetBookings();

		ProductModel GetHotelProduct(int type, int id, string strCheckIn, string strCheckOut, int total, int promotion);
		ProductModel GetHotelPackage(int id, string strCheckIn, int totalRoom);
        ProductModel GetHotelPackageVN(int id, string strCheckIn, int totalRoom);

        ProductModel GetHotelCoupon(int id,  int totalRoom);

        ProductModel GetHotelProductVn(int type, int id, string strCheckIn, string strCheckOut, int total, int promotion);

		ProductModel GetOtherServices(int type, int id, string strCheckIn, string strCheckOut, int qty, decimal price, decimal total, decimal discount, string cancellation, string name, string description);
	}
}