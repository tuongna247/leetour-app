using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface IOrderDetailService2 : IService<OrderDetail2>
	{
		List<AccountantOrderDetails> GetAccountantOrderDetails(int id);

		OrderDetail2 GetOrderDetail(int id);

		OrderDetail2 GetOrderDetailByOrderId(int id);

		List<OrderDetail2> GetOrderDetails(int id);

		List<OrderInformations> GetOrderInformations(int id);
	}
}