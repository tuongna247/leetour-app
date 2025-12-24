using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class CustomerRepository
	{
		public static Customer GetCustomer(this IRepositoryAsync<Customer> repository, int id)
		{
			Customer customer = repository.Queryable().FirstOrDefault<Customer>((Customer c) => c.CustomerId == id);
			return customer;
		}

		public static Customer GetCustomerByMemberId(this IRepositoryAsync<Customer> repository, string memberId)
		{
			Customer customer = repository.Queryable().FirstOrDefault<Customer>((Customer c) => c.MemberId.Contains(memberId));
			return customer;
		}

		public static IEnumerable<Customer> GetCustomers(this IRepositoryAsync<Customer> repository)
		{
			return repository.Queryable().AsEnumerable<Customer>();
		}

		public static bool ValidateCustomer(this IRepositoryAsync<Customer> repository, string email, string userName)
		{
			IEnumerable<Customer> customers = (
				from c in repository.Queryable()
				where (c.Email == email || c.UserName == userName) && c.ISSENDMAIL == (bool?)true
				select c).AsEnumerable<Customer>();
			return !customers.Any<Customer>();
		}
	}
}