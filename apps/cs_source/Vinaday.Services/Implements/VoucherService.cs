using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class VoucherService : Service<Voucher>, IVoucherService, IService<Voucher>
	{
		private readonly IRepositoryAsync<Voucher> _voucherRepository;

		public VoucherService(IRepositoryAsync<Voucher> voucherRepository) : base(voucherRepository)
		{
			this._voucherRepository = voucherRepository;
		}

		public Voucher GetVoucher(int id)
		{
			return this._voucherRepository.GetVoucher(id);
		}

		public Voucher GetVoucherByBookingId(int id)
		{
			return this._voucherRepository.GetVoucherByBookingId(id);
		}

		public List<Voucher> GetVouchers()
		{
			return this._voucherRepository.GetVouchers().ToList<Voucher>();
		}
	}
}