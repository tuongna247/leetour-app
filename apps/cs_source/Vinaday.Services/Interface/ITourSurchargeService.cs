using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface ITourSurchargeService : IService<Tour_Surcharge>
	{
		Tour_Surcharge Add(Tour_Surcharge contact);

		Tour_Surcharge GetSurcharge(int id);

		List<Tour_Surcharge> GetSurchargesByDayTourId(DateTime date, int id);

		List<Tour_Surcharge> GetSurchargesByTourId(int id);
	}
}