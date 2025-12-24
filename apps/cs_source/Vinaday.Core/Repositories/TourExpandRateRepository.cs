using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class TourExpandRateRepository
	{
		public static IEnumerable<ExpandRates> GetTourExpandRates(this IRepositoryAsync<ExpandRates> repository)
		{
			IEnumerable<ExpandRates> expandRates = (
				from r in repository.Queryable()
				orderby r.StartDate
				select r).AsEnumerable<ExpandRates>();
			return expandRates;
		}

		public static IEnumerable<ExpandRates> GetTourRatesExpandById(this IRepositoryAsync<ExpandRates> repository, int id)
		{
			IEnumerable<ExpandRates> expandRates = (
				from r in repository.Queryable()
				where r.TourId == id
				orderby r.StartDate
				select r).AsEnumerable<ExpandRates>();
			return expandRates;
		}
	}
}