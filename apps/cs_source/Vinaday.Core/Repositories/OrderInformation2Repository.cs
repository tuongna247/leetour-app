using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class OrderInformation2Repository
	{
		public static IEnumerable<OrderInformation2s> GetInformationOrderByOrderId(this IRepositoryAsync<OrderInformation2s> repository, int orderId)
		{
			IEnumerable<OrderInformation2s> orderInformation2s = (
				from o in repository.Queryable()
				where o.OrderId == orderId
				select o).AsEnumerable<OrderInformation2s>();
			return orderInformation2s;
		}

		public static IEnumerable<OrderInformation2s> GetOrderInformation(this IRepositoryAsync<OrderInformation2s> repository, int id)
		{
			IEnumerable<OrderInformation2s> orderInformation2s = (
				from o in repository.Queryable()
				where o.OrderId == id
				select o).AsEnumerable<OrderInformation2s>();
			return orderInformation2s;
		}

		public static IEnumerable<OrderInformation2s> GetOrders(this IRepositoryAsync<OrderInformation2s> repository)
		{
			return repository.Queryable().AsEnumerable<OrderInformation2s>();
		}
	}
}