using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface ITourPromotionService : IService<Tour_Promotion>
	{
        List<Tour_Promotion> GetPromotionByDayTourId(DateTime date, int id);

		Tour_Promotion GetPromotionId(int id);

		List<Tour_Promotion> GetPromotionsByTourId(int hotelId);
	}
}