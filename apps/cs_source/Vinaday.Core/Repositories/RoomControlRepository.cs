using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class RoomControlRepository
	{
		public static RoomControl GetRoomControlById(this IRepositoryAsync<RoomControl> repository, int roomId)
		{
			RoomControl roomControl = repository.Queryable().FirstOrDefault<RoomControl>((RoomControl a) => a.Id == roomId);
			return roomControl;
		}

		public static IEnumerable<RoomControl> GetRoomControls(this IRepositoryAsync<RoomControl> repository)
		{
			return repository.Queryable().AsEnumerable<RoomControl>();
		}

		public static IEnumerable<RoomControl> GetRoomControls(this IRepositoryAsync<RoomControl> repository, int roomId)
		{
			return repository.Queryable().AsEnumerable<RoomControl>();
		}

		public static RoomControl GetRoomControlSingleByIdOrderBySellingRate(this IRepositoryAsync<RoomControl> repository, int roomId)
		{
			RoomControl roomControl = (
				from a in repository.Queryable()
				where a.RoomId == roomId && a.SellingRate > (decimal?)((decimal)0)
				select a into r
				orderby r.SellingRate
				select r).FirstOrDefault<RoomControl>();
			return roomControl;
		}

		public static List<RoomControl> GetRoomListCheckInOut(this IRepositoryAsync<RoomControl> repository, int roomId, DateTime checkIn, DateTime checkOut)
		{
			List<RoomControl> list = (
				from a in repository.Queryable()
				where (a.RoomDate >= checkIn) && (a.RoomDate < checkOut) && a.RoomId == roomId
				select a into r
				orderby r.TotalAvailable
				select r).ToList<RoomControl>();
			return list;
		}

		public static IEnumerable<RoomControl> GetRoomsControlById(this IRepositoryAsync<RoomControl> repository, int roomId)
		{
			IEnumerable<RoomControl> roomControls = (
				from a in repository.Queryable()
				where a.RoomId == roomId
				select a).AsEnumerable<RoomControl>();
			return roomControls;
		}

		public static RoomControl GetRoomSingleByIdDate(this IRepositoryAsync<RoomControl> repository, int roomId, DateTime roomDate)
		{
			RoomControl roomControl = repository.Queryable().FirstOrDefault<RoomControl>((RoomControl a) => (a.RoomDate == roomDate) && a.RoomId == roomId && a.SellingRate != null);
			return roomControl;
		}

		public static RoomControl GetRoomSingleCheckInOut(this IRepositoryAsync<RoomControl> repository, int roomId, DateTime checkIn, DateTime checkOut)
		{
			RoomControl roomControl = (
				from a in repository.Queryable()
				where (a.RoomDate >= checkIn) && (a.RoomDate < checkOut) && a.RoomId == roomId
				select a into r
				orderby r.TotalAvailable
				select r).FirstOrDefault<RoomControl>();
			return roomControl;
		}

		public static decimal GetTotalPrice(this IRepositoryAsync<RoomControl> repository, int roomId, DateTime checkIn, DateTime checkOut)
		{
			decimal num = (
				from a in repository.Queryable()
				where (a.RoomDate >= checkIn) && (a.RoomDate < checkOut) && a.RoomId == roomId
				select a).Sum<RoomControl>((RoomControl r) => r.FinalPrice ?? new decimal(0));
			return num;
		}
	}
}