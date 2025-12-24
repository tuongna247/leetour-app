using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface IPaymentOrderDetailService : IService<PaymentOrderDetail>
	{
		PaymentOrderDetail GetPaymentOrderDetail(int paymentId);

		List<PaymentOrderDetail> GetPaymentOrderDetails();

		List<PaymentOrderDetail> GetPaymentOrderDetails(int paymentId);
	}
}