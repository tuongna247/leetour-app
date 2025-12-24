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
	public class TourDetailService : Service<Detail>, ITourDetailService, IService<Detail>
	{
		private readonly IRepositoryAsync<Detail> _tourDetailRepository;

		public TourDetailService(IRepositoryAsync<Detail> tourDetailRepository) : base(tourDetailRepository)
		{
			this._tourDetailRepository = tourDetailRepository;
		}

		public Detail Add(Detail detail)
		{
			this._tourDetailRepository.Insert(detail);
			return detail;
		}

		public void DeleteTour(Detail detail)
		{
			this._tourDetailRepository.Delete(detail);
		}

		public Detail GetDetail(int id)
		{
			Detail detail = this._tourDetailRepository.GetTourDetails().FirstOrDefault<Detail>((Detail d) => d.Id == id);
			return detail;
		}

		public List<Detail> GetDetailTours()
		{
			return this._tourDetailRepository.GetTourDetails().ToList<Detail>();
		}

		public List<Detail> GetDetailToursByTourId(int id)
		{
			List<Detail> list = this._tourDetailRepository.GetTourDetails().Where<Detail>((Detail t) => {
				int? tourId = t.TourId;
				int num = id;
				return (tourId.GetValueOrDefault() == num ? tourId.HasValue : false);
			}).ToList<Detail>();
			return list;
		}
	}
}