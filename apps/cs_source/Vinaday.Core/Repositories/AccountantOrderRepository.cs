using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class AccountantOrderRepository
	{
		public static IEnumerable<AccountantOrders> GetHotelOrders(this IRepositoryAsync<AccountantOrders> repository)
		{
			IEnumerable<AccountantOrders> accountantOrders = (
				from o in repository.Queryable()
				where o.Type == 1
				select o).AsEnumerable<AccountantOrders>();
			return accountantOrders;
		}

		public static AccountantOrders GetOrder(this IRepositoryAsync<AccountantOrders> repository, int parentId)
		{
			AccountantOrders accountantOrder = repository.Queryable().FirstOrDefault<AccountantOrders>((AccountantOrders o) => o.ParentId == parentId);
			return accountantOrder;
		}

		public static IEnumerable<AccountantOrders> GetTourOrders(this IRepositoryAsync<AccountantOrders> repository)
		{
			IEnumerable<AccountantOrders> accountantOrders = (
				from o in repository.Queryable()
				where o.Type == 2 || o.Type == 3
				select o).AsEnumerable<AccountantOrders>();
			return accountantOrders;
		}
	}
}