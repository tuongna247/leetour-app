using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class TourRate3Repository
    {
		public static IEnumerable<Rate3> GetTourRates(this IRepositoryAsync<Rate3> repository)
		{
			return repository.Queryable().AsEnumerable<Rate3>();
		}

		public static IEnumerable<Rate3> GetTourRatesById(this IRepositoryAsync<Rate3> repository, int id)
		{
			IEnumerable<Rate3> rates = (
				from r in repository.Queryable()
				where r.TourId == id
				orderby r.RetailRate descending
				select r).AsEnumerable<Rate3>();
			return rates;
		}

		public static Rate3 GetTourRatesByIdPersion(this IRepositoryAsync<Rate3> repository, int id, int person)
		{
			Rate3 rate = repository.Queryable().FirstOrDefault<Rate3>((Rate3 r) => r.TourId == id && r.PersonNo == (int?)person);
			return rate;
		}
	}
}