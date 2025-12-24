using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class PaymentOrderDetailRepository2
	{
		public static PaymentOrderDetail2 GetPaymentOrderByPaymentId(this IRepositoryAsync<PaymentOrderDetail2> repository, int paymentId)
		{
			PaymentOrderDetail2 paymentOrderDetail2 = repository.Queryable().FirstOrDefault<PaymentOrderDetail2>((PaymentOrderDetail2 l) => l.PaymentId == paymentId);
			return paymentOrderDetail2;
		}

		public static IEnumerable<PaymentOrderDetail2> GetPaymentOrderDetail(this IRepositoryAsync<PaymentOrderDetail2> repository, int id)
		{
			IEnumerable<PaymentOrderDetail2> paymentOrderDetail2s = (
				from l in repository.Queryable()
				where l.Id == id
				select l).AsEnumerable<PaymentOrderDetail2>();
			return paymentOrderDetail2s;
		}

		public static IEnumerable<PaymentOrderDetail2> GetPaymentOrderDetails(this IRepositoryAsync<PaymentOrderDetail2> repository)
		{
			return repository.Queryable().AsEnumerable<PaymentOrderDetail2>();
		}

		public static IEnumerable<PaymentOrderDetail2> GetPaymentOrdersByPaymentId(this IRepositoryAsync<PaymentOrderDetail2> repository, int paymentId)
		{
			IEnumerable<PaymentOrderDetail2> paymentOrderDetail2s = (
				from l in repository.Queryable()
				where l.PaymentId == paymentId
				select l).AsEnumerable<PaymentOrderDetail2>();
			return paymentOrderDetail2s;
		}
	}
}