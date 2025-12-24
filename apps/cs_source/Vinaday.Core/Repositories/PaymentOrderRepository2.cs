using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class PaymentOrderRepository2
	{
		public static IEnumerable<PaymentOrder2> GetPaymentOrder(this IRepositoryAsync<PaymentOrder2> repository, int id)
		{
			IEnumerable<PaymentOrder2> paymentOrder2s = (
				from l in repository.Queryable()
				where l.Id == id
				select l).AsEnumerable<PaymentOrder2>();
			return paymentOrder2s;
		}

		public static PaymentOrder2 GetPaymentOrderByOrderId(this IRepositoryAsync<PaymentOrder2> repository, int orderId)
		{
			PaymentOrder2 paymentOrder2 = repository.Queryable().FirstOrDefault<PaymentOrder2>((PaymentOrder2 l) => l.OrderId == orderId);
			return paymentOrder2;
		}

		public static IEnumerable<PaymentOrder2> GetPaymentOrders(this IRepositoryAsync<PaymentOrder2> repository)
		{
			return repository.Queryable().AsEnumerable<PaymentOrder2>();
		}

		public static IEnumerable<PaymentOrder2> GetPaymentOrdersByOrderId(this IRepositoryAsync<PaymentOrder2> repository, int orderId)
		{
			IEnumerable<PaymentOrder2> paymentOrder2s = (
				from l in repository.Queryable()
				where l.OrderId == orderId
				select l).AsEnumerable<PaymentOrder2>();
			return paymentOrder2s;
		}
	}
}