using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class TourSurchargeRepository
	{
		public static Tour_Surcharge GetSurcharge(this IRepositoryAsync<Tour_Surcharge> repository, int id)
		{
			Tour_Surcharge tourSurcharge = repository.Queryable().FirstOrDefault<Tour_Surcharge>((Tour_Surcharge s) => s.Id == id);
			return tourSurcharge;
		}

		public static IEnumerable<Tour_Surcharge> GetSurcharges(this IRepositoryAsync<Tour_Surcharge> repository)
		{
			return repository.Queryable().AsEnumerable<Tour_Surcharge>();
		}

		public static IEnumerable<Tour_Surcharge> GetSurchargesByTourId(this IRepositoryAsync<Tour_Surcharge> repository, int tourId)
		{
			IEnumerable<Tour_Surcharge> tourSurcharges = (
				from a in repository.Queryable()
				where a.TourId == tourId
				select a).AsEnumerable<Tour_Surcharge>();
			return tourSurcharges;
		}

		public static IEnumerable<Tour_Surcharge> GetSurchargesByTourIdDate(this IRepositoryAsync<Tour_Surcharge> repository, DateTime date, int tourId)
		{
			IEnumerable<Tour_Surcharge> tourSurcharges = (
				from a in repository.Queryable()
				where (a.StayDateTo >= date) && (a.StayDateFrom <= date) && a.TourId == tourId
				select a).AsEnumerable<Tour_Surcharge>();
			return tourSurcharges;
		}
	}
}