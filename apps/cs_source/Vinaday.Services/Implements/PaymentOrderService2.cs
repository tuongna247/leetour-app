using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class PaymentOrderService2 : Service<PaymentOrder2>, IPaymentOrderService2, IService<PaymentOrder2>
	{
		private readonly IRepositoryAsync<PaymentOrder2> _paymentOrderRepository;

		private readonly IRepositoryAsync<PaymentOrderDetail2> _paymentOrderDetailRepository;

		public PaymentOrderService2(IRepositoryAsync<PaymentOrder2> paymentOrderRepository, IRepositoryAsync<PaymentOrderDetail2> paymentOrderDetailRepository) : base(paymentOrderRepository)
		{
			this._paymentOrderRepository = paymentOrderRepository;
			this._paymentOrderDetailRepository = paymentOrderDetailRepository;
		}

		public PaymentOrder2 GetPaymentOrderByOrderId(int orderId)
		{
			PaymentOrder2 paymentOrderByOrderId = this._paymentOrderRepository.GetPaymentOrderByOrderId(orderId);
			return (paymentOrderByOrderId != null ? paymentOrderByOrderId : new PaymentOrder2());
		}

		public List<PaymentOrder2> GetPaymentOrders()
		{
			return this._paymentOrderRepository.GetPaymentOrders().ToList<PaymentOrder2>();
		}
	}
}