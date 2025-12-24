using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class VoucherRepository
	{
		public static Voucher GetVoucher(this IRepositoryAsync<Voucher> repository, int id)
		{
			Voucher voucher = repository.Queryable().FirstOrDefault<Voucher>((Voucher t) => t.Id == id);
			return voucher;
		}

		public static Voucher GetVoucherByBookingId(this IRepositoryAsync<Voucher> repository, int id)
		{
			Voucher voucher = repository.Queryable().FirstOrDefault<Voucher>((Voucher t) => t.BookingId == id);
			return voucher;
		}

		public static IEnumerable<Voucher> GetVouchers(this IRepositoryAsync<Voucher> repository)
		{
			return repository.Queryable().AsEnumerable<Voucher>();
		}
	}
}