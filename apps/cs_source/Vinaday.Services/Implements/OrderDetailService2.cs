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
	public class OrderDetailService2 : Service<OrderDetail2>, IOrderDetailService2, IService<OrderDetail2>
	{
		private readonly IRepositoryAsync<OrderDetail2> _orderDetailRepository;

		private readonly IRepositoryAsync<AccountantOrderDetails> _accountantOrderDetailsRepository;

		private readonly IRepositoryAsync<OrderInformations> _orderInformationRepository;

		public OrderDetailService2(IRepositoryAsync<OrderDetail2> orderDetailRepository, IRepositoryAsync<OrderInformations> orderInformationRepository, IRepositoryAsync<AccountantOrderDetails> accountantOrderDetailsRepository) : base(orderDetailRepository)
		{
			this._orderDetailRepository = orderDetailRepository;
			this._orderInformationRepository = orderInformationRepository;
			this._accountantOrderDetailsRepository = accountantOrderDetailsRepository;
		}

		public List<AccountantOrderDetails> GetAccountantOrderDetails(int id)
		{
			List<AccountantOrderDetails> list = this._accountantOrderDetailsRepository.GetInformationOrderByOrderId(id).ToList<AccountantOrderDetails>();
			return list;
		}

		public OrderDetail2 GetOrderDetail(int id)
		{
			OrderDetail2 orderDetail2 = this._orderDetailRepository.GetOrderDetails().FirstOrDefault<OrderDetail2>((OrderDetail2 o) => o.Id == id);
			return orderDetail2;
		}

		public OrderDetail2 GetOrderDetailByOrderId(int id)
		{
			return this._orderDetailRepository.GetOrderDetailByOrderId(id);
		}

		public List<OrderDetail2> GetOrderDetails(int id)
		{
			List<OrderDetail2> list = (
				from o in this._orderDetailRepository.GetOrderDetails()
				where o.OrderId == id
				orderby o.CreatedDate descending
				select o).ToList<OrderDetail2>();
			return list;
		}

		public List<OrderInformations> GetOrderInformations(int id)
		{
			List<OrderInformations> list = this._orderInformationRepository.GetInformationOrderByOrderId(id).ToList<OrderInformations>();
			return list;
		}
	}
}