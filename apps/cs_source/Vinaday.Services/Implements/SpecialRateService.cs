using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class SpecialRateService : Service<SpecialRate>, ISpecialRateService, IService<SpecialRate>
	{
		private readonly IRepositoryAsync<SpecialRate> _specialRateRepository;

		public SpecialRateService(IRepositoryAsync<SpecialRate> specialRateRepository) : base(specialRateRepository)
		{
			this._specialRateRepository = specialRateRepository;
		}

		public SpecialRate GetSpecialRate(int id, int type)
		{
			return this._specialRateRepository.GetSpecialRate(id, type);
		}

		public SpecialRate GetSpecialRate(int id)
		{
			return this._specialRateRepository.GetSpecialRate(id);
		}

		public SpecialRate GetSpecialRate(int id, int type, DateTime date)
		{
			return this._specialRateRepository.GetSpecialRate(id, type, date);
		}

		public List<SpecialRate> GetSpecialRates()
		{
			return this._specialRateRepository.GetSpecialRates().ToList<SpecialRate>();
		}

		public List<SpecialRate> GetSpecialRates(int tourId, int type)
		{
			List<SpecialRate> list = this._specialRateRepository.GetSpecialRates(tourId, type).ToList<SpecialRate>();
			return list;
		}
	}
}