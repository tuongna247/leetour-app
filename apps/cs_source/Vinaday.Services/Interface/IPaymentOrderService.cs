using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface IPaymentOrderService : IService<PaymentOrder>
	{
		PaymentOrder GetPaymentOrderByOrderId(int orderId);

		List<PaymentOrder> GetPaymentOrders();
	}
}