using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class AccountantOrderService : Service<AccountantOrders>, IAccountantOrderService, IService<AccountantOrders>
	{
		private readonly IRepositoryAsync<AccountantOrders> _orderRepository;

		public AccountantOrderService(IRepositoryAsync<AccountantOrders> orderRepository) : base(orderRepository)
		{
			this._orderRepository = orderRepository;
		}

		public int Add(AccountantOrders order)
		{
//			this._orderRepository.Add(order);
			this._orderRepository.Insert(order);
			return order.Id;
		}

		public AccountantOrders GetOrder(int parentId)
		{
			return this._orderRepository.GetOrder(parentId);
		}

		public List<AccountantOrders> GetOrders()
		{
			return this._orderRepository.GetHotelOrders().ToList<AccountantOrders>();
		}
	}
}