using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface ICountryService : IService<Country>
	{
		Country GetCountry(int id);

		List<Country> GetCountryList();
	}
}