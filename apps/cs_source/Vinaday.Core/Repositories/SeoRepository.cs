using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class SeoRepository
	{
		public static Seo GetSeo(this IRepositoryAsync<Seo> repository, int id)
		{
			Seo seo = repository.Queryable().FirstOrDefault<Seo>((Seo s) => s.EntityId == id && s.IsActive);
			return seo;
		}

		public static IEnumerable<Seo> GetSeos(this IRepositoryAsync<Seo> repository)
		{
			return repository.Queryable().AsEnumerable<Seo>();
		}
	}
}