using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class RoomReguestRepository
	{
		public static IEnumerable<RoomReguest> GetMonthlyReguests(this IRepositoryAsync<RoomReguest> repository)
		{
			DateTime now = DateTime.Now;
			DateTime dateTime = new DateTime(now.Year, now.Month, 1);
			DateTime dateTime1 = dateTime.AddMonths(1).AddDays(-1);
			IEnumerable<RoomReguest> roomReguests = (
				from o in repository.Queryable()
				where (o.CreateDate >= (DateTime?)dateTime) && (o.CreateDate < (DateTime?)dateTime1)
				select o into r
				orderby r.CreateDate descending
				select r).AsEnumerable<RoomReguest>();
			return roomReguests;
		}

		public static RoomReguest GetReguest(this IRepositoryAsync<RoomReguest> repository, int id)
		{
			RoomReguest roomReguest = repository.Queryable().FirstOrDefault<RoomReguest>((RoomReguest r) => r.Id == id);
			return roomReguest;
		}

		public static IEnumerable<RoomReguest> GetReguests(this IRepositoryAsync<RoomReguest> repository)
		{
			IEnumerable<RoomReguest> roomReguests = (
				from r in repository.Queryable()
				orderby r.CreateDate descending
				select r).AsEnumerable<RoomReguest>();
			return roomReguests;
		}

		public static IEnumerable<RoomReguest> GetRoomReguests(this IRepositoryAsync<RoomReguest> repository)
		{
			IEnumerable<RoomReguest> roomReguests = (
				from r in repository.Queryable()
				orderby r.CreateDate descending
				select r).AsEnumerable<RoomReguest>();
			return roomReguests;
		}

		public static IEnumerable<RoomReguest> GetRoomReguestsNoneRead(this IRepositoryAsync<RoomReguest> repository)
		{
			IEnumerable<RoomReguest> roomReguests = (
				from o in repository.Queryable()
				where !o.IsRead
				select o into r
				orderby r.CreateDate descending
				select r).AsEnumerable<RoomReguest>();
			return roomReguests;
		}
	}
}