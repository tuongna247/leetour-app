using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class FeaturedRepository
	{
		public static Featured GetFeatured(this IRepositoryAsync<Featured> repository, int tourId)
		{
			Featured featured = repository.Queryable().FirstOrDefault<Featured>((Featured f) => f.TourId == tourId);
			return featured;
		}

		public static IEnumerable<Featured> GetFeatureds(this IRepositoryAsync<Featured> repository)
		{
			return repository.Queryable().AsEnumerable<Featured>();
		}
	}
}