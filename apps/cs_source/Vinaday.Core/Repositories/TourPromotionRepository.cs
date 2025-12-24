using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class TourPromotionRepository
	{
		public static List<Tour_Promotion> GetPromotionByDayTourId(this IRepositoryAsync<Tour_Promotion> repository, DateTime date, int id)
		{
            List<Tour_Promotion> tourPromotion = repository.Queryable().Where(p => p.TourId == id && p.Status == (bool?)true && (p.CheckIn <= (DateTime?)date) && (p.CheckOut >= (DateTime?)date)).ToList();
			return tourPromotion;
		}

		public static Tour_Promotion GetPromotionById(this IRepositoryAsync<Tour_Promotion> repository, int id)
		{
			Tour_Promotion tourPromotion = repository.Queryable().FirstOrDefault<Tour_Promotion>((Tour_Promotion p) => p.Id == id && p.Status == (bool?)true);
			return tourPromotion;
		}

		public static IEnumerable<Tour_Promotion> GetPromotions(this IRepositoryAsync<Tour_Promotion> repository)
		{
			return repository.Queryable().AsEnumerable<Tour_Promotion>();
		}

		public static IEnumerable<Tour_Promotion> GetPromotions(this IRepositoryAsync<Tour_Promotion> repository, int tourId, int roomId, DateTime checkIn, DateTime checkOut, string date)
		{
			double totalDays = (checkOut - checkIn).TotalDays;
			IEnumerable<Tour_Promotion> tourPromotions = (
				from p in repository.Queryable()
				where p.TourId == tourId && p.Status == (bool?)true && (p.CheckIn <= (DateTime?)checkIn) && (p.CheckOut >= (DateTime?)checkOut) && (p.Language == (int?)-1 || p.Language == (int?)2) && (double?)p.MinimumStay <= (double?)totalDays && p.DateOfWeek.Contains(date)
				select p).AsEnumerable<Tour_Promotion>();
			return tourPromotions;
		}

		public static IEnumerable<Tour_Promotion> GetPromotionsByTourId(this IRepositoryAsync<Tour_Promotion> repository, int tourId)
		{
			IEnumerable<Tour_Promotion> tourPromotions = (
				from p in repository.Queryable()
				where p.TourId == tourId
				select p).AsEnumerable<Tour_Promotion>();
			return tourPromotions;
		}

		public static IEnumerable<Tour_Promotion> GetVnPromotions(this IRepositoryAsync<Tour_Promotion> repository, int tourId, DateTime checkIn, DateTime checkOut, string date)
		{
			double totalDays = (checkOut - checkIn).TotalDays;
			IEnumerable<Tour_Promotion> tourPromotions = (
				from p in repository.Queryable()
				where p.TourId == tourId && p.Status == (bool?)true && (p.CheckIn <= (DateTime?)checkIn) && (p.CheckOut > (DateTime?)checkOut) && (p.Language == (int?)-1 || p.Language == (int?)1) && (double?)p.MinimumStay <= (double?)totalDays && p.DateOfWeek.Contains(date)
				select p).AsEnumerable<Tour_Promotion>();
			return tourPromotions;
		}
	}
}