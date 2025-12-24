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
	public class CountryService : Service<Country>, ICountryService, IService<Country>
	{
		private readonly IRepositoryAsync<Country> _countryRepository;

		public CountryService(IRepositoryAsync<Country> countryRepository) : base(countryRepository)
		{
			this._countryRepository = countryRepository;
		}

		public Country GetCountry(int id)
		{
			Country country = this._countryRepository.GetCountries().FirstOrDefault<Country>((Country c) => c.CountryId == id);
			return country;
		}

		public List<Country> GetCountryList()
		{
			return this._countryRepository.GetCountries().ToList<Country>();
		}
	}
}