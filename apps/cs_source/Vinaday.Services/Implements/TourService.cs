using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class TourService : Service<Tour>, ITourService, IService<Tour>
	{
		private readonly IRepositoryAsync<Tour> _tourRepository;

		public TourService(IRepositoryAsync<Tour> tourRepository) : base(tourRepository)
		{
			this._tourRepository = tourRepository;
		}

		public Tour Add(Tour tour)
		{
            
			this._tourRepository.Insert(tour);
			return tour;
		}

		public Task<Tour> FindAsync(string id)
		{
			return this._tourRepository.FindAsync(new object[] { id });
		}

		public List<Tour> GetSimilarTours(int tourId, string cityId, int status, int munberOfList)
		{
			List<Tour> list = this._tourRepository.GetSimilarTours(tourId, cityId, status, munberOfList).ToList<Tour>();
			return list;
		}

		public List<Tour> GetSimilarToursByLocation(int tourId, string localtion, int status, int munberOfList)
		{
			List<Tour> list = this._tourRepository.GetSimilarToursByLocation(tourId, localtion, status, munberOfList).ToList<Tour>();
			return list;
		}
		public List<Tour> GetSimilarToursByKey(string cityId, int status, string filterKey, int munberOfList)
		{
			List<Tour> list = this._tourRepository.GetSimilarToursByKey(cityId, status, filterKey, munberOfList).ToList<Tour>();
			return list;
		}

		public Tour GetTourById(int id)
		{
			return this._tourRepository.GetTour(id);
		}

		public Tour GetTourById(int id, int language)
		{
			return this._tourRepository.GetTour(id, language);
		}

		public Tour GetTourOrtherId(int id)
		{
			return this._tourRepository.GetTourOrtherId(id);
		}

		public List<Tour> GetTours()
		{
			return this._tourRepository.GetTours().ToList<Tour>();
		}

		public List<Tour> GetTours(int language)
		{
			List<Tour> list = this._tourRepository.GetTours(language).Where(a=>a.Status == (int)Utilities.Status.Active).ToList<Tour>();
			return list;
		}

		public List<Tour> GetToursByFilterKey(string city, string[] filterKey)
		{
			List<Tour> list = this._tourRepository.GetToursByFilterKey(city, filterKey).ToList<Tour>();
			return list;
		}

		public List<Tour> GetToursByFilterKey(string city, string[] filterKey, int language)
		{
			List<Tour> list = this._tourRepository.GetToursByFilterKey(city, filterKey, language).ToList<Tour>();
			return list;
		}

		public List<Tour> GetToursByKey(string key)
		{
			List<Tour> list = this._tourRepository.GetToursByKey(key).ToList<Tour>();
			return list;
		}

		public List<Tour> GetToursViet()
		{
			return this._tourRepository.GetToursViet().ToList<Tour>();
		}

		public List<Tour> GetToursVietByFilterKey(string city, string[] filterKey)
		{
			List<Tour> list = this._tourRepository.GetToursVietByFilterKey(city, filterKey).ToList<Tour>();
			return list;
		}

		public void Remove(string id)
		{
			this._tourRepository.Delete(id);
		}
	}
}