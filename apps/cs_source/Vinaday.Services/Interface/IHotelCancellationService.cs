using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface IHotelCancellationService : IService<HotelCancellation>
	{
		HotelCancellation GetCancellationHotel(int id);

		HotelCancellation GetCancellationHotelById(int id);

		List<HotelCancellation> GetCancellationHotelList();
	}
}