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
	public class CityService : Service<City>, ICityService, IService<City>
	{
		private readonly IRepositoryAsync<City> _cityRepository;

		public CityService(IRepositoryAsync<City> cityRepository) : base(cityRepository)
		{
			this._cityRepository = cityRepository;
		}

		public List<City> GetCities()
		{
			return this._cityRepository.GetCities().ToList<City>();
		}

		public List<City> GetCitiesByCountryId(int countryId)
		{
			List<City> list = this._cityRepository.GetCitiesByCountryId(countryId).ToList<City>();
			return list;
		}

		public City GetCity(string name)
		{
			return this._cityRepository.GetCityByName(name);
		}

		public City GetCityById(int id)
		{
			City city = this._cityRepository.GetCities().FirstOrDefault<City>((City c) => c.CityId == id);
			return city;
		}
	}
}