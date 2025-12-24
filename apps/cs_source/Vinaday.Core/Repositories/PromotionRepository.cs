using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class PromotionRepository
	{
		public static Promotion GetPromotionById(this IRepositoryAsync<Promotion> repository, int id)
		{
			Promotion promotion = repository.Queryable().FirstOrDefault<Promotion>((Promotion p) => p.Id == id && p.Status == (bool?)true);
			return promotion;
		}

		public static IEnumerable<Promotion> GetPromotions(this IRepositoryAsync<Promotion> repository)
		{
			return repository.Queryable().AsEnumerable<Promotion>();
		}

		public static IEnumerable<Promotion> GetPromotions(this IRepositoryAsync<Promotion> repository, int hotelId, int roomId, DateTime checkIn, DateTime checkOut, string date)
		{
			double totalDays = (checkOut - checkIn).TotalDays;
			IEnumerable<Promotion> promotions = (
				from p in repository.Queryable()
				where p.HotelId == hotelId && p.Status == (bool?)true && (p.RoomId == -1 || p.RoomId == roomId) && (p.CheckIn <= (DateTime?)checkIn) && (p.CheckOut >= (DateTime?)checkOut) && (p.Language == (int?)-1 || p.Language == (int?)2) && (double?)p.MinimumStay <= (double?)totalDays && p.DateOfWeek.Contains(date)
				select p).AsEnumerable<Promotion>();
			return promotions;
		}

		public static IEnumerable<Promotion> GetPromotionsByHotelId(this IRepositoryAsync<Promotion> repository, int hotelId)
		{
			IEnumerable<Promotion> promotions = (
				from p in repository.Queryable()
				where p.HotelId == hotelId
				select p).AsEnumerable<Promotion>();
			return promotions;
		}

		public static IEnumerable<Promotion> GetPromotionsByHotelIdOrRoomId(this IRepositoryAsync<Promotion> repository, int hotelId, int roomId)
		{
			IEnumerable<Promotion> promotions = (
				from p in repository.Queryable()
				where p.HotelId == hotelId && (p.RoomId == roomId || p.RoomId == -1) && (p.CheckIn <= (DateTime?)DateTime.Now)
				select p).AsEnumerable<Promotion>();
			return promotions;
		}

		public static IEnumerable<Promotion> GetVnPromotions(this IRepositoryAsync<Promotion> repository, int hotelId, int roomId, DateTime checkIn, DateTime checkOut, string date)
		{
			double totalDays = (checkOut - checkIn).TotalDays;
			IEnumerable<Promotion> promotions = (
				from p in repository.Queryable()
				where p.HotelId == hotelId && p.Status == (bool?)true && (p.RoomId == -1 || p.RoomId == roomId) && (p.CheckIn <= (DateTime?)checkIn) && (p.CheckOut > (DateTime?)checkOut) && (p.Language == (int?)-1 || p.Language == (int?)1) && (double?)p.MinimumStay <= (double?)totalDays && p.DateOfWeek.Contains(date)
				select p).AsEnumerable<Promotion>();
			return promotions;
		}
	}
}