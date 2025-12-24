using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface ITourService : IService<Tour>
	{
		Tour Add(Tour tour);

		Task<Tour> FindAsync(string id);

		List<Tour> GetSimilarTours(int tourId, string cityId, int status, int munberOfList);
		List<Tour> GetSimilarToursByLocation(int tourId, string location, int status, int munberOfList);
		
		List<Tour> GetSimilarToursByKey(string cityId, int status, string filterKey, int munberOfList);

		Tour GetTourById(int id);

		Tour GetTourById(int id, int language);

		Tour GetTourOrtherId(int id);

		List<Tour> GetTours();

		List<Tour> GetTours(int language);

		List<Tour> GetToursByFilterKey(string city, string[] filterKey);

		List<Tour> GetToursByFilterKey(string city, string[] filterKey, int language);

		List<Tour> GetToursByKey(string key);

		List<Tour> GetToursViet();

		List<Tour> GetToursVietByFilterKey(string city, string[] filterKey);

		void Remove(string id);
	}
}