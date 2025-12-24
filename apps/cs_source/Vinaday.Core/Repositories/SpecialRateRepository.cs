using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class SpecialRateRepository
	{
		public static SpecialRate GetSpecialRate(this IRepositoryAsync<SpecialRate> repository, int id, int type)
		{
			SpecialRate specialRate = repository.Queryable().FirstOrDefault<SpecialRate>((SpecialRate s) => s.Id == id && s.Type == (int?)type);
			return specialRate;
		}

		public static SpecialRate GetSpecialRate(this IRepositoryAsync<SpecialRate> repository, int id, int type, DateTime date)
		{
			SpecialRate specialRate = repository.Queryable().FirstOrDefault<SpecialRate>((SpecialRate s) => s.TourId == id && s.Type == (int?)type && (s.BookingFrom < date) && (s.BookingTo >= date));
			return specialRate;
		}

		public static SpecialRate GetSpecialRate(this IRepositoryAsync<SpecialRate> repository, int tourId)
		{
			SpecialRate specialRate = repository.Queryable().FirstOrDefault<SpecialRate>((SpecialRate s) => s.Id == tourId);
			return specialRate;
		}

		public static IEnumerable<SpecialRate> GetSpecialRates(this IRepositoryAsync<SpecialRate> repository)
		{
			return repository.Queryable().AsEnumerable<SpecialRate>();
		}

		public static IEnumerable<SpecialRate> GetSpecialRates(this IRepositoryAsync<SpecialRate> repository, int tourId, int type)
		{
			IEnumerable<SpecialRate> specialRates = (
				from s in repository.Queryable()
				where s.TourId == tourId && s.Type == (int?)type && (DateTime.Now <= s.BookingTo)
				select s).AsEnumerable<SpecialRate>();
			return specialRates;
		}
	}
}