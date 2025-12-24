using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class PaymentOrderDetailRepository
	{
		public static PaymentOrderDetail GetPaymentOrderByPaymentId(this IRepositoryAsync<PaymentOrderDetail> repository, int paymentId)
		{
			PaymentOrderDetail paymentOrderDetail = repository.Queryable().FirstOrDefault<PaymentOrderDetail>((PaymentOrderDetail l) => l.PaymentId == paymentId);
			return paymentOrderDetail;
		}

		public static IEnumerable<PaymentOrderDetail> GetPaymentOrderDetail(this IRepositoryAsync<PaymentOrderDetail> repository, int id)
		{
			IEnumerable<PaymentOrderDetail> paymentOrderDetails = (
				from l in repository.Queryable()
				where l.Id == id
				select l).AsEnumerable<PaymentOrderDetail>();
			return paymentOrderDetails;
		}

		public static IEnumerable<PaymentOrderDetail> GetPaymentOrderDetails(this IRepositoryAsync<PaymentOrderDetail> repository)
		{
			return repository.Queryable().AsEnumerable<PaymentOrderDetail>();
		}

		public static IEnumerable<PaymentOrderDetail> GetPaymentOrdersByPaymentId(this IRepositoryAsync<PaymentOrderDetail> repository, int paymentId)
		{
			IEnumerable<PaymentOrderDetail> paymentOrderDetails = (
				from l in repository.Queryable()
				where l.PaymentId == paymentId
				select l).AsEnumerable<PaymentOrderDetail>();
			return paymentOrderDetails;
		}
	}
}