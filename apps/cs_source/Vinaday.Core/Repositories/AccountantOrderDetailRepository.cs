using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class AccountantOrderDetailRepository
	{
		public static IEnumerable<AccountantOrderDetails> GetInformationOrderByOrderId(this IRepositoryAsync<AccountantOrderDetails> repository, int orderId)
		{
			IEnumerable<AccountantOrderDetails> accountantOrderDetails = (
				from o in repository.Queryable()
				where o.OrderId == orderId
				select o).AsEnumerable<AccountantOrderDetails>();
			return accountantOrderDetails;
		}

		public static IEnumerable<AccountantOrderDetails> GetOrderInformation(this IRepositoryAsync<AccountantOrderDetails> repository, int id)
		{
			IEnumerable<AccountantOrderDetails> accountantOrderDetails = (
				from o in repository.Queryable()
				where o.OrderId == id
				select o).AsEnumerable<AccountantOrderDetails>();
			return accountantOrderDetails;
		}

		public static IEnumerable<AccountantOrderDetails> GetOrders(this IRepositoryAsync<AccountantOrderDetails> repository)
		{
			return repository.Queryable().AsEnumerable<AccountantOrderDetails>();
		}
	}
}