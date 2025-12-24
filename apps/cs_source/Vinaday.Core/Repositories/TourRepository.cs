using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class TourRepository
	{
		public static IEnumerable<Tour> GetSimilarTours(this IRepositoryAsync<Tour> repository, int tourId, string cityId, int status, int munberOfList)
		{
			IEnumerable<Tour> tours = (
				from a in repository.Queryable()
				where a.Cities.Contains(cityId) && a.Id != tourId && a.Status == (int?)status
				select a into x
				orderby Guid.NewGuid()
				select x).Take<Tour>(munberOfList).AsEnumerable<Tour>();
			return tours;
		}

		public static IEnumerable<Tour> GetSimilarToursByLocation(this IRepositoryAsync<Tour> repository, int tourId, string location, int status, int munberOfList)
		{
			IEnumerable<Tour> tours = (
				from a in repository.Queryable()
				where a.Location.Contains(location) && a.Id != tourId && a.Status == (int?)status
				select a into x
				orderby Guid.NewGuid()
				select x).Take<Tour>(munberOfList).AsEnumerable<Tour>();
			return tours;
		}

		public static IEnumerable<Tour> GetSimilarToursByKey(this IRepositoryAsync<Tour> repository, string cityId, int status, string filterKey, int munberOfList)
		{
			IEnumerable<Tour> tours = (
				from a in repository.Queryable()
				where a.Cities.Contains(cityId) && a.Filter.Contains(filterKey) && a.Status == (int?)status
				select a into x
				orderby Guid.NewGuid()
				select x).Take<Tour>(munberOfList).AsEnumerable<Tour>();
			return tours;
		}

		public static Tour GetTour(this IRepositoryAsync<Tour> repository, int id)
		{
			Tour tour = repository.Queryable().FirstOrDefault<Tour>((Tour t) => t.Id == id);
			return tour;
		}

		public static Tour GetTour(this IRepositoryAsync<Tour> repository, int id, int language)
		{
			Tour tour = repository.Queryable().FirstOrDefault<Tour>((Tour t) => t.Id == id && t.Language == (int?)language);
			return tour;
		}

		public static Tour GetTourOrtherId(this IRepositoryAsync<Tour> repository, int id)
		{
			Tour tour = repository.Queryable().FirstOrDefault<Tour>((Tour t) => t.Id != id);
			return tour;
		}

		public static IEnumerable<Tour> GetTours(this IRepositoryAsync<Tour> repository)
		{
			return repository.Queryable().AsEnumerable<Tour>();
		}

		public static IEnumerable<Tour> GetTours(this IRepositoryAsync<Tour> repository, int language)
		{
			IEnumerable<Tour> tours = (
				from t in repository.Queryable()
				where t.Language == (int?)language
				select t).AsEnumerable<Tour>();
			return tours;
		}

		public static IEnumerable<Tour> GetToursByFilterKey(this IRepositoryAsync<Tour> repository, string city, string[] filterKey)
		{
			IEnumerable<Tour> tours;
			Predicate<string> predicate = null;
			List<Tour> tours1 = new List<Tour>();
			foreach (Tour list in repository.Queryable().ToList<Tour>())
			{
				if (list != null)
				{
					string cities = list.Cities;
					string[] strArrays = new string[0];
					if (cities != null)
					{
						string[] strArrays1 = cities.Split(new char[] { ',' });
						Predicate<string> predicate1 = predicate;
						if (predicate1 == null)
						{
							Predicate<string> predicate2 = (string s) => s.Equals(city);
							Predicate<string> predicate3 = predicate2;
							predicate = predicate2;
							predicate1 = predicate3;
						}
						strArrays = Array.FindAll<string>(strArrays1, predicate1);
					}
					if (strArrays.Any<string>())
					{
						tours1.Add(list);
					}
				}
			}
			if ((filterKey.Count<string>() != 1 ? true : !filterKey[0].Contains("[all]")))
			{
				tours = 
					from f in tours1
					where filterKey.Any<string>((string filter) => (filter.Equals(f.Filter) ? true : f.Filter.Contains(filter)))
					select f;
			}
			else
			{
				tours = tours1;
			}
			return tours;
		}

		public static IEnumerable<Tour> GetToursByFilterKey(this IRepositoryAsync<Tour> repository, string city, string[] filterKey, int language)
		{
			IEnumerable<Tour> tours;
			Predicate<string> predicate = null;
			var tours1 = new List<Tour>();
			foreach (var list in (
			    repository.Queryable().Where(t => t.Language == language )).ToList())
			{
			    if (list == null) continue;
			    var cities = list.Cities;
			    var strArrays = new string[0];
			    if (cities != null)
			    {
			        var strArrays1 = cities.Split(',');
			        var predicate1 = predicate;
			        if (predicate1 == null)
			        {
			            Predicate<string> predicate2 = s => s.Equals(city);
			            var predicate3 = predicate2;
			            predicate = predicate2;
			            predicate1 = predicate3;
			        }
			        strArrays = Array.FindAll(strArrays1, predicate1);
			    }
			    if (strArrays.Any())
			    {
			        tours1.Add(list);
			    }
			}
			if (filterKey.Length != 1 || !filterKey[0].Contains("[all]"))
			{
				tours = 
					from tour in tours1
					where filterKey.Any(a => a != null && (a.Equals(tour.Filter) || (tour.Filter !=null && tour.Filter.Contains(a))))
					select tour;
			}
			else
			{
				tours = tours1;
			}
			return tours;
		}

		public static IEnumerable<Tour> GetToursByKey(this IRepositoryAsync<Tour> repository, string key)
		{
			IEnumerable<Tour> tours = (
				from t in repository.Queryable()
				where t.Name.Replace(" ", "-").Contains(key)
				select t).AsEnumerable<Tour>();
			return tours;
		}

		public static IEnumerable<Tour> GetToursViet(this IRepositoryAsync<Tour> repository)
		{
			IEnumerable<Tour> tours = (
				from t in repository.Queryable()
				where t.Language == (int?)1
				select t).AsEnumerable<Tour>();
			return tours;
		}

		public static IEnumerable<Tour> GetToursVietByFilterKey(this IRepositoryAsync<Tour> repository, string city, string[] filterKey)
		{
			IEnumerable<Tour> tours;
			Predicate<string> predicate = null;
			List<Tour> tours1 = new List<Tour>();
			foreach (Tour list in (
				from t in repository.Queryable()
				where t.Language == (int?)1
				select t).ToList<Tour>())
			{
				if (list != null)
				{
					string cities = list.Cities;
					string[] strArrays = new string[0];
					if (cities != null)
					{
						string[] strArrays1 = cities.Split(new char[] { ',' });
						Predicate<string> predicate1 = predicate;
						if (predicate1 == null)
						{
							Predicate<string> predicate2 = (string s) => s.Equals(city);
							Predicate<string> predicate3 = predicate2;
							predicate = predicate2;
							predicate1 = predicate3;
						}
						strArrays = Array.FindAll<string>(strArrays1, predicate1);
					}
					if (strArrays.Any<string>())
					{
						tours1.Add(list);
					}
				}
			}
			if ((filterKey.Count<string>() != 1 ? true : !filterKey[0].Contains("[all]")))
			{
				tours = 
					from f in tours1
					where filterKey.Any<string>((string filter) => (filter.Equals(f.Filter) ? true : f.Filter.Contains(filter)))
					select f;
			}
			else
			{
				tours = tours1;
			}
			return tours;
		}
	}
}