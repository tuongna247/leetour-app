using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class TourDetailRepository
	{
		public static Detail GetTourDetail(this IRepositoryAsync<Detail> repository, int id)
		{
			Detail detail = repository.Queryable().FirstOrDefault<Detail>((Detail d) => d.Id == id);
			return detail;
		}

		public static IEnumerable<Detail> GetTourDetails(this IRepositoryAsync<Detail> repository)
		{
			return repository.Queryable().AsEnumerable<Detail>();
		}
	}
}