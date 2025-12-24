using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;
using Vinaday.Data.Models.Extention;

namespace Vinaday.Services
{
	public interface IOrderService : IService<Order>
	{
		int Add(Order order);

		DashboardModel GetDashboardItem();

		List<Order> GetHotelOrders();

		OrderModel GetOrder(int id);

		Order GetOrderById(int id);

		List<OrderModel> GetOrders();

		List<OrderModel> GetOrdersByCustomerId(string custId);

		List<OrderModel> GetOrdersByCustomerId(int custId);

		List<OrderModel> GetOrdersByDate(DateTime startDate, DateTime endDate, bool byCreatedDate);

		List<OrderModel> GetOrdersByUserName(string userName);

		List<OrderModel> GetOrdersByUserNameDate(string userName, DateTime startDate, DateTime endDate);

		List<Order> GetOrdersForTest();

		OrderModel GetOrderVietnam(int id);

		List<OrderModel> GetPaidOrders();

		List<Order> GetTourOrders();
	}
}