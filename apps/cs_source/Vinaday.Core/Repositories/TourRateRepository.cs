using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class TourRateRepository
	{
		public static IEnumerable<Rate> GetTourRates(this IRepositoryAsync<Rate> repository)
		{
			return repository.Queryable().AsEnumerable<Rate>();
		}

		public static IEnumerable<Rate> GetTourRatesById(this IRepositoryAsync<Rate> repository, int id)
		{
			IEnumerable<Rate> rates = (
				from r in repository.Queryable()
				where r.TourId == id
				orderby r.RetailRate descending
				select r).AsEnumerable<Rate>();
			return rates;
		}

		public static Rate GetTourRatesByIdPersion(this IRepositoryAsync<Rate> repository, int id, int person)
		{
			Rate rate = repository.Queryable().FirstOrDefault<Rate>((Rate r) => r.TourId == id && r.PersonNo == (int?)person);
			return rate;
		}
	}
}