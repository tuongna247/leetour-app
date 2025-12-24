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
	public class TourRateService3 : Service<Rate3>, ITourRateService3, IService<Rate3>
	{
		private readonly IRepositoryAsync<Rate3> _tourRateRepository3;

		public TourRateService3(IRepositoryAsync<Rate3> tourRateRepository) : base(tourRateRepository)
		{
			this._tourRateRepository3 = tourRateRepository;
		}

		public List<Rate3> GetTourRates()
		{
			return this._tourRateRepository3.GetTourRates().ToList<Rate3>();
		}

		public List<Rate3> GetTourRatesById(int id)
		{
			List<Rate3> list = this._tourRateRepository3.GetTourRatesById(id).ToList<Rate3>();
			return list;
		}

		public Rate3 GetTourRatesByIdPersion(int id, int person)
		{
			return this._tourRateRepository3.GetTourRatesByIdPersion(id, person);
		}

		public List<Rate3> GetTourRatesByTourId(int id)
		{
			List<Rate3> list = (
				from r in this._tourRateRepository3.GetTourRates()
				where r.TourId == id
				orderby r.RetailRate
				select r).ToList<Rate3>();
			return list;
		}
	}
}