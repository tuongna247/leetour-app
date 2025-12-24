using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class DealsRepository
	{
		public static IEnumerable<Deals> GetAlllDeals(this IRepositoryAsync<Deals> repository)
		{
			return repository.Queryable().AsEnumerable<Deals>();
		}

		public static Deals GetDeal(this IRepositoryAsync<Deals> repository, int id)
		{
			Deals deal = repository.Queryable().FirstOrDefault<Deals>((Deals t) => t.Id == id);
			return deal;
		}

		public static IEnumerable<Deals> GetDeals(this IRepositoryAsync<Deals> repository)
		{
			IEnumerable<Deals> deals = (
				from t in repository.Queryable()
				where t.Type == 1 && t.Status == 2
				select t).AsEnumerable<Deals>();
			return deals;
		}

		public static IEnumerable<Deals> GetDealsNotMain(this IRepositoryAsync<Deals> repository)
		{
			IEnumerable<Deals> deals = (
				from t in repository.Queryable()
				where t.Priority != (int?)1 && t.Type == 1 && t.Status == 2
				select t).AsEnumerable<Deals>();
			return deals;
		}

		public static Deals GetMainDeal(this IRepositoryAsync<Deals> repository)
		{
			Deals deal = repository.Queryable().FirstOrDefault<Deals>((Deals t) => t.Priority == (int?)1 && t.Type == 1 && t.Status == 2);
			return deal;
		}
	}
}