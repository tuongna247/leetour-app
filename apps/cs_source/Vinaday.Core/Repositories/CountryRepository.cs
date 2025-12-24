using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class CountryRepository
	{
		public static IEnumerable<Country> GetCountries(this IRepositoryAsync<Country> repository)
		{
			return repository.Queryable().AsEnumerable<Country>();
		}

		public static Country GetCountry(this IRepositoryAsync<Country> repository, int id)
		{
			Country country = repository.Queryable().FirstOrDefault<Country>((Country c) => c.CountryId == id);
			return country;
		}
	}
}