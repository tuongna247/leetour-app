using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class PaymentOrderService : Service<PaymentOrder>, IPaymentOrderService, IService<PaymentOrder>
	{
		private readonly IRepositoryAsync<PaymentOrder> _paymentOrderRepository;

		private readonly IRepositoryAsync<PaymentOrderDetail> _paymentOrderDetailRepository;

		public PaymentOrderService(IRepositoryAsync<PaymentOrder> paymentOrderRepository, IRepositoryAsync<PaymentOrderDetail> paymentOrderDetailRepository) : base(paymentOrderRepository)
		{
			this._paymentOrderRepository = paymentOrderRepository;
			this._paymentOrderDetailRepository = paymentOrderDetailRepository;
		}

		public PaymentOrder GetPaymentOrderByOrderId(int orderId)
		{
			return this._paymentOrderRepository.GetPaymentOrderByOrderId(orderId);
		}

		public List<PaymentOrder> GetPaymentOrders()
		{
			return this._paymentOrderRepository.GetPaymentOrders().ToList<PaymentOrder>();
		}
	}
}