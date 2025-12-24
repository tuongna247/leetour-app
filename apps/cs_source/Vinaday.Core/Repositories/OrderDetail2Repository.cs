using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class OrderDetail2Repository
	{
		public static OrderDetail2 GetOrderDetailByOrderId(this IRepositoryAsync<OrderDetail2> repository, int id)
		{
			OrderDetail2 orderDetail2 = repository.Queryable().FirstOrDefault<OrderDetail2>((OrderDetail2 o) => o.OrderId == id);
			return orderDetail2;
		}

		public static IEnumerable<OrderDetail2> GetOrderDetails(this IRepositoryAsync<OrderDetail2> repository)
		{
			return repository.Queryable().AsEnumerable<OrderDetail2>();
		}

		public static IEnumerable<OrderDetail2> GetOrderDetails(this IRepositoryAsync<OrderDetail2> repository, int id)
		{
			IEnumerable<OrderDetail2> orderDetail2s = (
				from o in repository.Queryable()
				where o.OrderId == id
				orderby o.CreatedDate descending
				select o).AsEnumerable<OrderDetail2>();
			return orderDetail2s;
		}
	}
}