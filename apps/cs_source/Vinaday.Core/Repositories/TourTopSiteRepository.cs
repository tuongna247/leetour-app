using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class TourTopSiteRepository
    {
		public static TourTopSite GetTourTopSite(this IRepositoryAsync<TourTopSite> repository, int tourId)
		{
			TourTopSite featured = repository.Queryable().FirstOrDefault<TourTopSite>((TourTopSite f) => f.TourId == tourId);
			return featured;
		}

		public static IEnumerable<TourTopSite> GetTourTopSites(this IRepositoryAsync<TourTopSite> repository)
		{
			return repository.Queryable().AsEnumerable<TourTopSite>();
		}
	}
}