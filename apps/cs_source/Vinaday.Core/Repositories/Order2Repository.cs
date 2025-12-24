using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class Order2Repository
	{
		public static IEnumerable<Order2> GetHotelOrders(this IRepositoryAsync<Order2> repository)
		{
			IEnumerable<Order2> order2s = (
				from o in repository.Queryable()
				where o.Type == 1
				select o).AsEnumerable<Order2>();
			return order2s;
		}

		public static Order2 GetOrder(this IRepositoryAsync<Order2> repository, int id)
		{
			Order2 order2 = repository.Queryable().FirstOrDefault<Order2>((Order2 o) => o.Id == id);
			return order2;
		}

		public static Order2 GetOrderByOrderId(this IRepositoryAsync<Order2> repository, int id)
		{
			Order2 order2 = repository.Queryable().FirstOrDefault<Order2>((Order2 o) => o.TourOrderId == id);
			return order2;
		}

		public static IEnumerable<Order2> GetOrders(this IRepositoryAsync<Order2> repository)
		{
			IEnumerable<Order2> order2s = (
				from o in repository.Queryable()
				orderby o.CreatedDate
				select o).AsEnumerable<Order2>();
			return order2s;
		}

		public static IEnumerable<Order2> GetOrdersByCustomerId(this IRepositoryAsync<Order2> repository, string custId)
		{
			IEnumerable<Order2> order2s = (
				from o in repository.Queryable()
				where o.MemberId.Contains(custId)
				orderby o.CreatedDate
				select o).AsEnumerable<Order2>();
			return order2s;
		}

		public static IEnumerable<Order2> GetOrdersByCustomerId(this IRepositoryAsync<Order2> repository, int custId)
		{
			IEnumerable<Order2> order2s = (
				from o in repository.Queryable()
				where o.CustomerId == (int?)custId
				orderby o.CreatedDate
				select o).AsEnumerable<Order2>();
			return order2s;
		}

		public static IEnumerable<Order2> GetOrdersByDate(this IRepositoryAsync<Order2> repository, int status, DateTime startDate, DateTime endDate)
		{
			IEnumerable<Order2> order2s = (
				from o in repository.Queryable()
				where o.Status == status && (o.CreatedDate > startDate) && (o.CreatedDate <= endDate)
				orderby o.Id descending
				select o).AsEnumerable<Order2>();
			return order2s;
		}

		public static IEnumerable<Order2> GetOrdersByDate(this IRepositoryAsync<Order2> repository, DateTime startDate, DateTime endDate)
		{
			IEnumerable<Order2> order2s = (
				from o in repository.Queryable()
				where (o.CreatedDate > startDate) && (o.CreatedDate <= endDate)
				orderby o.Id descending
				select o).AsEnumerable<Order2>();
			return order2s;
		}

		public static IEnumerable<Order2> GetOrdersByStatus(this IRepositoryAsync<Order2> repository, int status)
		{
			IEnumerable<Order2> order2s = (
				from o in repository.Queryable()
				where o.Status == status
				orderby o.Id descending
				select o).AsEnumerable<Order2>();
			return order2s;
		}

		public static IEnumerable<Order2> GetOrdersByUserDate(this IRepositoryAsync<Order2> repository, string userName, int status, DateTime startDate, DateTime endDate)
		{
			IEnumerable<Order2> order2s = (
				from o in repository.Queryable()
				where o.Management == userName && o.Status == status && (o.CreatedDate > startDate) && (o.CreatedDate <= endDate)
				orderby o.Id descending
				select o).AsEnumerable<Order2>();
			return order2s;
		}

		public static IEnumerable<Order2> GetOrdersByUserName(this IRepositoryAsync<Order2> repository, string userName)
		{
			IEnumerable<Order2> order2s = (
				from o in repository.Queryable()
				where o.Management == userName
				orderby o.CreatedDate
				select o).AsEnumerable<Order2>();
			return order2s;
		}

		public static IEnumerable<Order2> GetOrdersMonthly(this IRepositoryAsync<Order2> repository)
		{
			DateTime now = DateTime.Now;
			DateTime dateTime = new DateTime(now.Year, now.Month, 1);
			DateTime dateTime1 = dateTime.AddMonths(1).AddDays(-1);
			IEnumerable<Order2> order2s = (
				from o in repository.Queryable()
				where (o.CreatedDate >= dateTime) && (o.CreatedDate < dateTime1)
				orderby o.CreatedDate
				select o).AsEnumerable<Order2>();
			return order2s;
		}

		public static IEnumerable<Order2> GetTourOrders(this IRepositoryAsync<Order2> repository)
		{
			IEnumerable<Order2> order2s = (
				from o in repository.Queryable()
				where o.Type == 2 || o.Type == 3
				select o).AsEnumerable<Order2>();
			return order2s;
		}
	}
}