using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class RateExchangeRepository
	{
		public static IEnumerable<RateExchange> GetRateExchanges(this IRepositoryAsync<RateExchange> repository)
		{
			return repository.Queryable().AsEnumerable<RateExchange>();
		}

		public static RateExchange GetRateExchangeSingle(this IRepositoryAsync<RateExchange> repository, int id)
		{
			RateExchange rateExchange = repository.Queryable().FirstOrDefault<RateExchange>((RateExchange e) => e.Id == id);
			return rateExchange;
		}
	}
}