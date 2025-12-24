using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class OrderDetailRepository
	{
		public static OrderDetail GetOrderDetailByOrderId(this IRepositoryAsync<OrderDetail> repository, int id)
		{
			OrderDetail orderDetail = repository.Queryable().FirstOrDefault<OrderDetail>((OrderDetail o) => o.OrderId == id);
			return orderDetail;
		}

		public static IEnumerable<OrderDetail> GetOrderDetails(this IRepositoryAsync<OrderDetail> repository)
		{
			return repository.Queryable().AsEnumerable<OrderDetail>();
		}

		public static IEnumerable<OrderDetail> GetOrderDetails(this IRepositoryAsync<OrderDetail> repository, int id)
		{
			IEnumerable<OrderDetail> orderDetails = (
				from o in repository.Queryable()
				where o.OrderId == id
				orderby o.CreatedDate descending
				select o).AsEnumerable<OrderDetail>();
			return orderDetails;
		}
	}
}