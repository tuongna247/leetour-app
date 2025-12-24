using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class DealService : Service<Deals>, IDealService, IService<Deals>
	{
		private readonly IRepositoryAsync<Deals> _dealRepository;

		public DealService(IRepositoryAsync<Deals> dealRepository) : base(dealRepository)
		{
			this._dealRepository = dealRepository;
		}

		public List<Deals> GetAllDeals()
		{
			return this._dealRepository.GetAlllDeals().ToList<Deals>();
		}

		public Deals GetDeal(int id)
		{
			return this._dealRepository.GetDeal(id);
		}

		public List<Deals> GetDeals()
		{
			return this._dealRepository.GetDeals().ToList<Deals>();
		}

		public List<Deals> GetDealsNotMain()
		{
			return this._dealRepository.GetDealsNotMain().ToList<Deals>();
		}

		public Deals GetMainDeal()
		{
			return this._dealRepository.GetMainDeal();
		}
	}
}