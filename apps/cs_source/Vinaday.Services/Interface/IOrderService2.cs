using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;
using Vinaday.Data.Models.Extention;

namespace Vinaday.Services
{
	public interface IOrderService2 : IService<Order2>
	{
		int Add(Vinaday.Data.Models.Order2 Order2);

		DashboardModel GetDashboardItem();

		List<Order2> GetHotelOrders();

		OrderModel GetOrder2(int id);

		Order2 GetOrderById(int id);

		Order2 GetOrderByOrderId(int id);

		List<OrderModel> GetOrders();

		List<OrderModel> GetOrdersByCustomerId(string custId);

		List<OrderModel> GetOrdersByCustomerId(int custId);

		List<OrderModel> GetOrdersByDate(DateTime startDate, DateTime endDate);

		List<OrderModel> GetOrdersByUserName(string userName);

		List<OrderModel> GetOrdersByUserNameDate(string userName, DateTime startDate, DateTime endDate);

		List<Order2> GetOrdersForTest();

		OrderModel GetOrderVietnam(int id);

		List<OrderModel> GetPaidOrders();

		List<Order2> GetTourOrders();
	}
}