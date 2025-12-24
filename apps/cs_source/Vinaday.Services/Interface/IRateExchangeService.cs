using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface IRateExchangeService : IService<RateExchange>
	{
		RateExchange GetRateExchangeById(int id);

		List<RateExchange> GetRateExchanges();
	}
}