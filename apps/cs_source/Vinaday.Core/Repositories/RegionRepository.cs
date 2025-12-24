using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class RegionRepository
	{
		public static Region1 GetRegion(this IRepositoryAsync<Region1> repository, int id)
		{
			Region1 region1 = repository.Queryable().FirstOrDefault<Region1>((Region1 r) => r.Id == id);
			return region1;
		}

		public static IEnumerable<Region1> GetRegions(this IRepositoryAsync<Region1> repository)
		{
			return repository.Queryable().AsEnumerable<Region1>();
		}

		public static IEnumerable<Region1> GetRegions(this IRepositoryAsync<Region1> repository, int id)
		{
			IEnumerable<Region1> region1s = (
				from r in repository.Queryable()
				where r.Id == id
				select r).AsEnumerable<Region1>();
			return region1s;
		}
	}
}