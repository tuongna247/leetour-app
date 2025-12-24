using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface ICustomerService : IService<Customer>
	{
		Customer GetCustomerByEmail(string email);

		Customer GetCustomerById(int id);

		Customer GetCustomerByMemberId(string memberId);

		Customer GetCustomerByPhone(string phone);

		List<Customer> GetCustomerList();

		bool InsertCustomer(Customer customer);

		bool ValidateCustomer(string email, string userName);
	}
}