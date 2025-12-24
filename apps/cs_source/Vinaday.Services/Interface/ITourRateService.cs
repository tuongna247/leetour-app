using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface ITourRateService : IService<Rate>
	{
		List<Rate> GetTourRates();

		List<Rate> GetTourRatesById(int id);

		Rate GetTourRatesByIdPersion(int id, int person);

		List<Rate> GetTourRatesByTourId(int id);
	}
}