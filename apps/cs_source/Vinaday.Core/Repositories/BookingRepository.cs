using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class BookingRepository
	{
		public static IEnumerable<Booking> GetBookings(this IRepositoryAsync<Booking> repository)
		{
			return repository.Queryable().AsEnumerable<Booking>();
		}

		public static IEnumerable<Booking> GetBookingsByPnr(this IRepositoryAsync<Booking> repository, string pnr)
		{
			IEnumerable<Booking> list = (
				from b in repository.Queryable()
				where b.Pnr == pnr
				select b).ToList<Booking>();
			return list;
		}

		public static IEnumerable<Booking> GetRecentlyBookings(this IRepositoryAsync<Booking> repository)
		{
			IEnumerable<Booking> list = (
				from p in repository.Queryable()
				orderby p.Date descending
				select p into g
				group g by g.Room into p
				select p.FirstOrDefault<Booking>()).Take<Booking>(20).ToList<Booking>();
			return list;
		}
	}
}