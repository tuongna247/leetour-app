using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface IAccountantOrderService : IService<AccountantOrders>
	{
		AccountantOrders GetOrder(int parentId);

		List<AccountantOrders> GetOrders();
	}
}