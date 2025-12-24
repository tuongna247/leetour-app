using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class RateExchangeService : Service<RateExchange>, IRateExchangeService, IService<RateExchange>
	{
		private readonly IRepositoryAsync<RateExchange> _rateExchangeRepository;

		public RateExchangeService(IRepositoryAsync<RateExchange> rateExchangeRepository) : base(rateExchangeRepository)
		{
			this._rateExchangeRepository = rateExchangeRepository;
		}

		public RateExchange GetRateExchangeById(int id)
		{
			return this._rateExchangeRepository.GetRateExchangeSingle(id);
		}

		public List<RateExchange> GetRateExchanges()
		{
			return this._rateExchangeRepository.GetRateExchanges().ToList<RateExchange>();
		}
	}
}