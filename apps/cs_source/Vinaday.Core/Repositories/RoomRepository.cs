using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class RoomRepository
	{
		public static Room GetRoom(this IRepositoryAsync<Room> repository, int id)
		{
			Room room = repository.Queryable().FirstOrDefault<Room>((Room r) => r.Id == id && r.Status != (bool?)false);
			return room;
		}

		public static IEnumerable<Room> GetRoomAll(this IRepositoryAsync<Room> repository)
		{
			IEnumerable<Room> rooms = (
				from r in repository.Queryable()
				where r.Status != (bool?)false
				select r).AsEnumerable<Room>();
			return rooms;
		}

		public static IEnumerable<Room> GetRoomList(this IRepositoryAsync<Room> repository, int hotelId)
		{
			IEnumerable<Room> rooms = (
				from r in repository.Queryable()
				where r.HotelId == (int?)hotelId && r.Status != (bool?)false
				orderby r.Id descending
				select r).AsEnumerable<Room>();
			return rooms;
		}
	}
}