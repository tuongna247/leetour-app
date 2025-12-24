using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface IVoucherService : IService<Voucher>
	{
		Voucher GetVoucher(int id);

		Voucher GetVoucherByBookingId(int id);

		List<Voucher> GetVouchers();
	}
}