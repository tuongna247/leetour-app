using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface ITourRateService2 : IService<Rate2>
	{
		List<Rate2> GetTourRates();

		List<Rate2> GetTourRatesById(int id);

		Rate2 GetTourRatesByIdPersion(int id, int person);

		List<Rate2> GetTourRatesByTourId(int id);
	}
}