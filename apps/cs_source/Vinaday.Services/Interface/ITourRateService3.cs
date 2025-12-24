using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface ITourRateService3 : IService<Rate3>
	{
		List<Rate3> GetTourRates();

		List<Rate3> GetTourRatesById(int id);

		Rate3 GetTourRatesByIdPersion(int id, int person);

		List<Rate3> GetTourRatesByTourId(int id);
	}
}