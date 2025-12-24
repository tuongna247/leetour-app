using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface IOrderDetailService : IService<OrderDetail>
	{
		List<AccountantOrderDetails> GetAccountantOrderDetails(int id);

		OrderDetail GetOrderDetail(int id);

		OrderDetail GetOrderDetailByOrderId(int id);

		List<OrderDetail> GetOrderDetails(int id);

		List<OrderInformations> GetOrderInformations(int id);
	}
}