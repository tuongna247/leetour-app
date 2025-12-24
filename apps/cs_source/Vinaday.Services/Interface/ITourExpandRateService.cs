using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface ITourExpandRateService : IService<ExpandRates>
	{
		List<ExpandRates> GetTourRates();

		List<ExpandRates> GetTourRatesByTourId(int id);
	}
}