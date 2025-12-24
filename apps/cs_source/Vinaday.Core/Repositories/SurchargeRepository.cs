using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class SurchargeRepository
	{
		public static Surcharge GetSurcharge(this IRepositoryAsync<Surcharge> repository, int id)
		{
			Surcharge surcharge = repository.Queryable().FirstOrDefault<Surcharge>((Surcharge s) => s.Id == id);
			return surcharge;
		}

		public static IEnumerable<Surcharge> GetSurcharges(this IRepositoryAsync<Surcharge> repository)
		{
			return repository.Queryable().AsEnumerable<Surcharge>();
		}

		public static IEnumerable<Surcharge> GetSurchargesByHotelId(this IRepositoryAsync<Surcharge> repository, DateTime date, int hotelId)
		{
			IEnumerable<Surcharge> surcharges = (
				from a in repository.Queryable()
				where (a.StayDateTo > date) && (a.StayDateFrom <= date) && a.HotelId == hotelId
				select a).AsEnumerable<Surcharge>();
			return surcharges;
		}

		public static IEnumerable<Surcharge> GetSurchargesByRoomId(this IRepositoryAsync<Surcharge> repository, DateTime date, int roomId, int hotelId)
		{
			IEnumerable<Surcharge> surcharges = (
				from a in repository.Queryable()
				where (a.StayDateTo > date) && (a.StayDateFrom <= date) && a.HotelId == hotelId && (a.RoomId == roomId || a.RoomId == -1)
				select a).AsEnumerable<Surcharge>();
			return surcharges;
		}

		public static IEnumerable<Surcharge> GetSurchargesByRoomId(this IRepositoryAsync<Surcharge> repository, int id, int hotelId)
		{
			IEnumerable<Surcharge> surcharges;
			if (id == -1)
			{
				IEnumerable<Surcharge> surcharges1 = 
					from s in repository.Queryable()
					where s.HotelId == hotelId
					select s;
				surcharges = surcharges1;
			}
			else
			{
				surcharges = (
					from s in repository.Queryable()
					where s.HotelId == hotelId && (s.RoomId == id || s.RoomId == -1)
					select s).AsEnumerable<Surcharge>();
			}
			return surcharges;
		}

		public static IEnumerable<Surcharge> GetSurchargesByRoomId(this IRepositoryAsync<Surcharge> repository, DateTime date, int id)
		{
			IEnumerable<Surcharge> surcharges = (
				from s in repository.Queryable()
				where s.RoomId == id && (s.StayDateTo > date) && (s.StayDateFrom <= date)
				select s).AsEnumerable<Surcharge>();
			return surcharges;
		}
	}
}