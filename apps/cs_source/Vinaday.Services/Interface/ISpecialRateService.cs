using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface ISpecialRateService : IService<SpecialRate>
	{
		SpecialRate GetSpecialRate(int id, int type, DateTime date);

		SpecialRate GetSpecialRate(int id);

		SpecialRate GetSpecialRate(int id, int type);

		List<SpecialRate> GetSpecialRates();

		List<SpecialRate> GetSpecialRates(int tourId, int type);
	}
}