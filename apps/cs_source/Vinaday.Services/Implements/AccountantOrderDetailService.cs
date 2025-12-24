using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class AccountantOrderDetailService : Service<AccountantOrderDetails>, IAccountantOrderDetailService, IService<AccountantOrderDetails>
	{
		private readonly IRepositoryAsync<AccountantOrderDetails> _orderDetailRepository;

		public AccountantOrderDetailService(IRepositoryAsync<AccountantOrderDetails> orderDetailRepository) : base(orderDetailRepository)
		{
			this._orderDetailRepository = orderDetailRepository;
		}

		public int Add(AccountantOrderDetails order)
		{
			this._orderDetailRepository.Insert(order);
			return order.Id;
		}
	}
}