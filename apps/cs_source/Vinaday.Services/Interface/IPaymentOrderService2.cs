using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface IPaymentOrderService2 : IService<PaymentOrder2>
	{
		PaymentOrder2 GetPaymentOrderByOrderId(int orderId);

		List<PaymentOrder2> GetPaymentOrders();
	}
}