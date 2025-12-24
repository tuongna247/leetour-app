using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class TourRate2Repository
    {
		public static IEnumerable<Rate2> GetTourRates(this IRepositoryAsync<Rate2> repository)
		{
			return repository.Queryable().AsEnumerable<Rate2>();
		}

		public static IEnumerable<Rate2> GetTourRatesById(this IRepositoryAsync<Rate2> repository, int id)
		{
			IEnumerable<Rate2> rates = (
				from r in repository.Queryable()
				where r.TourId == id
				orderby r.RetailRate descending
				select r).AsEnumerable<Rate2>();
			return rates;
		}

		public static Rate2 GetTourRatesByIdPersion(this IRepositoryAsync<Rate2> repository, int id, int person)
		{
			Rate2 rate = repository.Queryable().FirstOrDefault<Rate2>((Rate2 r) => r.TourId == id && r.PersonNo == (int?)person);
			return rate;
		}
	}
}