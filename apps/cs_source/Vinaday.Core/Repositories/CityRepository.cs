using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class CityRepository
	{
		public static IEnumerable<City> GetCities(this IRepositoryAsync<City> repository)
		{
			IEnumerable<City> cities = (
				from c in repository.Queryable()
				orderby c.Priority
				select c).AsEnumerable<City>();
			return cities;
		}

		public static IEnumerable<City> GetCities(this IRepositoryAsync<City> repository, int id)
		{
			IEnumerable<City> cities = (
				from c in repository.Queryable()
				where c.CountryId == id
				orderby c.Priority
				select c).AsEnumerable<City>();
			return cities;
		}

		public static IEnumerable<City> GetCitiesByCountryId(this IRepositoryAsync<City> repository, int countryId)
		{
			IEnumerable<City> cities = (
				from c in repository.Queryable()
				where c.CountryId == countryId
				orderby c.Priority
				select c).AsEnumerable<City>();
			return cities;
		}

		public static City GetCityByName(this IRepositoryAsync<City> repository, string name)
		{
			City city = repository.Queryable().FirstOrDefault<City>((City c) => c.Description.Replace(" ", "").ToLower() == name);
			return city;
		}
	}
}