using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class TourExpandRateService : Service<ExpandRates>, ITourExpandRateService
	{
		private readonly IRepositoryAsync<ExpandRates> _tourExpandRateRepository;

		public TourExpandRateService(IRepositoryAsync<ExpandRates> tourExpandRateRepository) : base(tourExpandRateRepository)
		{
			this._tourExpandRateRepository = tourExpandRateRepository;
		}

		public List<ExpandRates> GetTourRates()
		{
			return this._tourExpandRateRepository.GetTourExpandRates().ToList<ExpandRates>();
		}

		public List<ExpandRates> GetTourRatesByTourId(int id)
		{
			List<ExpandRates> list = this._tourExpandRateRepository.GetTourRatesExpandById(id).ToList<ExpandRates>();
			return list;
		}
	}
}