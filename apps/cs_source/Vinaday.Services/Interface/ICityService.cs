using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface ICityService : IService<City>
	{
		List<City> GetCities();

		List<City> GetCitiesByCountryId(int countryId);

		City GetCity(string name);

		City GetCityById(int id);
	}
}