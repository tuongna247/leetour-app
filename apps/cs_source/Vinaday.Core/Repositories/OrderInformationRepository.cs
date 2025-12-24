using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class OrderInformationRepository
	{
		public static IEnumerable<OrderInformations> GetInformationOrderByOrderId(this IRepositoryAsync<OrderInformations> repository, int orderId)
		{
			IEnumerable<OrderInformations> orderInformations = (
				from o in repository.Queryable()
				where o.OrderId == orderId
				select o).AsEnumerable<OrderInformations>();
			return orderInformations;
		}

		public static IEnumerable<OrderInformations> GetOrderInformation(this IRepositoryAsync<OrderInformations> repository, int id)
		{
			IEnumerable<OrderInformations> orderInformations = (
				from o in repository.Queryable()
				where o.OrderId == id
				select o).AsEnumerable<OrderInformations>();
			return orderInformations;
		}

		public static IEnumerable<OrderInformations> GetOrders(this IRepositoryAsync<OrderInformations> repository)
		{
			return repository.Queryable().AsEnumerable<OrderInformations>();
		}
	}
}