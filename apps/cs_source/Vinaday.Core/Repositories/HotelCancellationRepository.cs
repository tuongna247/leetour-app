using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class HotelCancellationRepository
	{
		public static HotelCancellation GetHotelCancellationPolicyById(this IRepositoryAsync<HotelCancellation> repository, int id, int status)
		{
			HotelCancellation hotelCancellation = repository.Queryable().FirstOrDefault<HotelCancellation>((HotelCancellation c) => c.HotelID == id && c.CheckInFrom.CompareTo(DateTime.Now) <= 0 && c.CheckOutTo.CompareTo(DateTime.Now) > 0 && c.Status == (int?)status);
			return hotelCancellation;
		}

		public static HotelCancellation GetHotelCancellationPolicyById(this IRepositoryAsync<HotelCancellation> repository, int id)
		{
			HotelCancellation hotelCancellation = repository.Queryable().FirstOrDefault<HotelCancellation>((HotelCancellation c) => c.ID == id);
			return hotelCancellation;
		}

		public static IEnumerable<HotelCancellation> GetHotelCancellationPolicys(this IRepositoryAsync<HotelCancellation> repository)
		{
			return repository.Queryable().AsEnumerable<HotelCancellation>();
		}

		public static IEnumerable<HotelCancellation> GetHotelCancellationPolicys(this IRepositoryAsync<HotelCancellation> repository, int id, int status)
		{
			IEnumerable<HotelCancellation> hotelCancellations = (
				from c in repository.Queryable()
				where c.HotelID == id && c.Status == (int?)status
				orderby c.ID descending
				select c).AsEnumerable<HotelCancellation>();
			return hotelCancellations;
		}
	}
}