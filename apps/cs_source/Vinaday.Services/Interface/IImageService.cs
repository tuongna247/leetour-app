using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface IImageService : IService<HotelImages>
	{
		List<HotelImages> GetHotelImages();

		HotelImages GetHotelImageSetting();

		List<HotelImages> GetImageListByHotelId(int hotelId);

		HotelImages GetImageSingleByHotelId(int hotelId);
	}
}