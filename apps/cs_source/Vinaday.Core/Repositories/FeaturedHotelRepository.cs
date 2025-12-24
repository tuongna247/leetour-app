using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class FeaturedHotelRepository
	{
		public static IEnumerable<HotelFeatureds> GetFeaturedAscendingList(this IRepositoryAsync<HotelFeatureds> repository)
		{
			IEnumerable<HotelFeatureds> hotelFeatureds = (
				from f in repository.Queryable()
				orderby f.Priority
				select f).AsEnumerable<HotelFeatureds>();
			return hotelFeatureds;
		}

		public static IEnumerable<HotelFeatureds> GetFeaturedHotels(this IRepositoryAsync<HotelFeatureds> repository)
		{
			return repository.Queryable().AsEnumerable<HotelFeatureds>();
		}
	}
}