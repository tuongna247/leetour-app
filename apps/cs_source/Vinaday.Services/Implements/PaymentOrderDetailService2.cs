using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class PaymentOrderDetailService2 : Service<PaymentOrderDetail2>, IPaymentOrderDetailService2, IService<PaymentOrderDetail2>
	{
		private readonly IRepositoryAsync<PaymentOrderDetail2> _paymentOrderDetailRepository;

		public PaymentOrderDetailService2(IRepositoryAsync<PaymentOrderDetail2> paymentOrderDetailRepository) : base(paymentOrderDetailRepository)
		{
			this._paymentOrderDetailRepository = paymentOrderDetailRepository;
		}

		public PaymentOrderDetail2 GetPaymentOrderDetail(int paymentId)
		{
			return this._paymentOrderDetailRepository.GetPaymentOrderByPaymentId(paymentId);
		}

		public List<PaymentOrderDetail2> GetPaymentOrderDetails()
		{
			return this._paymentOrderDetailRepository.GetPaymentOrderDetails().ToList<PaymentOrderDetail2>();
		}

		public List<PaymentOrderDetail2> GetPaymentOrderDetails(int paymentId)
		{
			List<PaymentOrderDetail2> list = this._paymentOrderDetailRepository.GetPaymentOrdersByPaymentId(paymentId).ToList<PaymentOrderDetail2>();
			return list;
		}
	}
}