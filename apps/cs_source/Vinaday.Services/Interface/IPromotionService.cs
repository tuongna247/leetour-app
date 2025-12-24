using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface IPromotionService : IService<Promotion>
	{
		List<Promotion> GetPromotionByHotelId(int hotelId);

		Promotion GetPromotionId(int id);
	}
}