using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class PaymentOrderDetailService : Service<PaymentOrderDetail>, IPaymentOrderDetailService, IService<PaymentOrderDetail>
	{
		private readonly IRepositoryAsync<PaymentOrderDetail> _paymentOrderDetailRepository;

		public PaymentOrderDetailService(IRepositoryAsync<PaymentOrderDetail> paymentOrderDetailRepository) : base(paymentOrderDetailRepository)
		{
			this._paymentOrderDetailRepository = paymentOrderDetailRepository;
		}

		public PaymentOrderDetail GetPaymentOrderDetail(int paymentId)
		{
			return this._paymentOrderDetailRepository.GetPaymentOrderByPaymentId(paymentId);
		}

		public List<PaymentOrderDetail> GetPaymentOrderDetails()
		{
			return this._paymentOrderDetailRepository.GetPaymentOrderDetails().ToList<PaymentOrderDetail>();
		}

		public List<PaymentOrderDetail> GetPaymentOrderDetails(int paymentId)
		{
			List<PaymentOrderDetail> list = this._paymentOrderDetailRepository.GetPaymentOrdersByPaymentId(paymentId).ToList<PaymentOrderDetail>();
			return list;
		}
	}
}