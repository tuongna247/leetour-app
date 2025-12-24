using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class OrderRepository
	{
		public static IEnumerable<Order> GetHotelOrders(this IRepositoryAsync<Order> repository)
		{
			IEnumerable<Order> orders = (
				from o in repository.Queryable()
				where o.Type == 1
				select o).AsEnumerable<Order>();
			return orders;
		}

		public static Order GetOrder(this IRepositoryAsync<Order> repository, int id)
		{
			Order order = repository.Queryable().FirstOrDefault<Order>((Order o) => o.Id == id);
			return order;
		}

		public static IEnumerable<Order> GetOrders(this IRepositoryAsync<Order> repository)
		{
			IEnumerable<Order> orders = (
				from o in repository.Queryable()
				orderby o.CreatedDate
				select o).AsEnumerable<Order>();
			return orders;
		}

		public static IEnumerable<Order> GetOrdersByCustomerId(this IRepositoryAsync<Order> repository, string custId)
		{
			IEnumerable<Order> orders = (
				from o in repository.Queryable()
				where o.MemberId.Contains(custId)
				orderby o.CreatedDate
				select o).AsEnumerable<Order>();
			return orders;
		}

		public static IEnumerable<Order> GetOrdersByCustomerId(this IRepositoryAsync<Order> repository, int custId)
		{
			IEnumerable<Order> orders = (
				from o in repository.Queryable()
				where o.CustomerId == (int?)custId
				orderby o.CreatedDate
				select o).AsEnumerable<Order>();
			return orders;
		}

		public static IEnumerable<Order> GetOrdersByDate(this IRepositoryAsync<Order> repository, int status, DateTime startDate, DateTime endDate)
		{
			IEnumerable<Order> orders = (
				from o in repository.Queryable()
				where o.Status == status && (o.CreatedDate > startDate) && (o.CreatedDate <= endDate)
				orderby o.Id descending
				select o).AsEnumerable<Order>();
			return orders;
		}

		public static IEnumerable<Order> GetOrdersByDate(this IRepositoryAsync<Order> repository, DateTime startDate, DateTime endDate, bool isByCreatedDate)
		{
			IEnumerable<Order> orders;
			orders = (!isByCreatedDate ? (
				from o in repository.Queryable()
				where (o.StartDate > startDate) && (o.StartDate <= endDate)
				orderby o.Id descending
				select o).AsEnumerable<Order>() : (
				from o in repository.Queryable()
				where (o.CreatedDate > startDate) && (o.CreatedDate <= endDate)
				orderby o.Id descending
				select o).AsEnumerable<Order>());
			return orders;
		}

		public static IEnumerable<Order> GetOrdersByStatus(this IRepositoryAsync<Order> repository, int status)
		{
			IEnumerable<Order> orders = (
				from o in repository.Queryable()
				where o.Status == status
				orderby o.Id descending
				select o).AsEnumerable<Order>();
			return orders;
		}

		public static IEnumerable<Order> GetOrdersByUserDate(this IRepositoryAsync<Order> repository, string userName, int status, DateTime startDate, DateTime endDate)
		{
			IEnumerable<Order> orders = (
				from o in repository.Queryable()
				where o.Management == userName && (o.CreatedDate >= startDate) && (o.CreatedDate <= endDate)
				orderby o.Id descending
				select o).AsEnumerable<Order>();
			return orders;
		}

		public static IEnumerable<Order> GetOrdersByUserName(this IRepositoryAsync<Order> repository, string userName)
		{
			IEnumerable<Order> orders = (
				from o in repository.Queryable()
				where o.Management == userName
				orderby o.CreatedDate
				select o).AsEnumerable<Order>();
			return orders;
		}

		public static IEnumerable<Order> GetOrdersMonthly(this IRepositoryAsync<Order> repository)
		{
			DateTime now = DateTime.Now;
			DateTime dateTime = new DateTime(now.Year, now.Month, 1);
			DateTime dateTime1 = dateTime.AddMonths(1).AddDays(-1);
			IEnumerable<Order> orders = (
				from o in repository.Queryable()
				where (o.CreatedDate >= dateTime) && (o.CreatedDate < dateTime1)
				orderby o.CreatedDate
				select o).AsEnumerable<Order>();
			return orders;
		}

		public static IEnumerable<Order> GetTourOrders(this IRepositoryAsync<Order> repository)
		{
			IEnumerable<Order> orders = (
				from o in repository.Queryable()
				where o.Type == 2 || o.Type == 3
				select o).AsEnumerable<Order>();
			return orders;
		}
	}
}