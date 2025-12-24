using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface IPaymentOrderDetailService2 : IService<PaymentOrderDetail2>
	{
		PaymentOrderDetail2 GetPaymentOrderDetail(int paymentId);

		List<PaymentOrderDetail2> GetPaymentOrderDetails();

		List<PaymentOrderDetail2> GetPaymentOrderDetails(int paymentId);
	}
}