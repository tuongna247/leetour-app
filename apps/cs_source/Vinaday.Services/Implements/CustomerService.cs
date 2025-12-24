using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class CustomerService : Service<Customer>, ICustomerService, IService<Customer>
	{
		private readonly IRepositoryAsync<Customer> _customerRepository;

		public CustomerService(IRepositoryAsync<Customer> customerRepository) : base(customerRepository)
		{
			this._customerRepository = customerRepository;
		}

		public Customer GetCustomerByEmail(string email)
		{
			Customer customer = this._customerRepository.GetCustomers().FirstOrDefault<Customer>((Customer c) => c.Email == email);
			return customer;
		}

		public Customer GetCustomerById(int id)
		{
			Customer customer = this._customerRepository.GetCustomers().FirstOrDefault<Customer>((Customer c) => c.CustomerId == id);
			return customer;
		}

		public Customer GetCustomerByMemberId(string memberId)
		{
			return this._customerRepository.GetCustomerByMemberId(memberId);
		}

		public Customer GetCustomerByPhone(string phone)
		{
			Customer customer = this._customerRepository.GetCustomers().FirstOrDefault<Customer>((Customer c) => c.PhoneNumber == phone);
			return customer;
		}

		public List<Customer> GetCustomerList()
		{
			return this._customerRepository.GetCustomers().ToList<Customer>();
		}

		public bool InsertCustomer(Customer customer)
		{
			return true;
		}

		public bool ValidateCustomer(string email, string userName)
		{
			return this._customerRepository.ValidateCustomer(email, userName);
		}
	}
}