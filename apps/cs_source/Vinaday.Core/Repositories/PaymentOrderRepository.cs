using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class PaymentOrderRepository
	{
		public static IEnumerable<PaymentOrder> GetPaymentOrder(this IRepositoryAsync<PaymentOrder> repository, int id)
		{
			IEnumerable<PaymentOrder> paymentOrders = (
				from l in repository.Queryable()
				where l.Id == id
				select l).AsEnumerable<PaymentOrder>();
			return paymentOrders;
		}

		public static PaymentOrder GetPaymentOrderByOrderId(this IRepositoryAsync<PaymentOrder> repository, int orderId)
		{
			PaymentOrder paymentOrder = repository.Queryable().FirstOrDefault<PaymentOrder>((PaymentOrder l) => l.OrderId == orderId);
			return paymentOrder;
		}

		public static IEnumerable<PaymentOrder> GetPaymentOrders(this IRepositoryAsync<PaymentOrder> repository)
		{
			return repository.Queryable().AsEnumerable<PaymentOrder>();
		}

		public static IEnumerable<PaymentOrder> GetPaymentOrdersByOrderId(this IRepositoryAsync<PaymentOrder> repository, int orderId)
		{
			IEnumerable<PaymentOrder> paymentOrders = (
				from l in repository.Queryable()
				where l.OrderId == orderId
				select l).AsEnumerable<PaymentOrder>();
			return paymentOrders;
		}
	}
}