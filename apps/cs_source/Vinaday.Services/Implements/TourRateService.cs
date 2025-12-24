using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class TourRateService : Service<Rate>, ITourRateService, IService<Rate>
	{
		private readonly IRepositoryAsync<Rate> _tourRateRepository;

		public TourRateService(IRepositoryAsync<Rate> tourRateRepository) : base(tourRateRepository)
		{
			this._tourRateRepository = tourRateRepository;
		}

		public List<Rate> GetTourRates()
		{
			return this._tourRateRepository.GetTourRates().ToList<Rate>();
		}

		public List<Rate> GetTourRatesById(int id)
		{
			List<Rate> list = this._tourRateRepository.GetTourRatesById(id).ToList<Rate>();
			return list;
		}

		public Rate GetTourRatesByIdPersion(int id, int person)
		{
			return this._tourRateRepository.GetTourRatesByIdPersion(id, person);
		}

		public List<Rate> GetTourRatesByTourId(int id)
		{
			List<Rate> list = (
				from r in this._tourRateRepository.GetTourRates()
				where r.TourId == id
				orderby r.RetailRate
				select r).ToList<Rate>();
			return list;
		}
	}
}