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
	public class OrderDetailService : Service<OrderDetail>, IOrderDetailService, IService<OrderDetail>
	{
		private readonly IRepositoryAsync<OrderDetail> _orderDetailRepository;

		private readonly IRepositoryAsync<AccountantOrderDetails> _accountantOrderDetailsRepository;

		private readonly IRepositoryAsync<OrderInformations> _orderInformationRepository;

		public OrderDetailService(IRepositoryAsync<OrderDetail> orderDetailRepository, IRepositoryAsync<OrderInformations> orderInformationRepository, IRepositoryAsync<AccountantOrderDetails> accountantOrderDetailsRepository) : base(orderDetailRepository)
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

		public OrderDetail GetOrderDetail(int id)
		{
			OrderDetail orderDetail = this._orderDetailRepository.GetOrderDetails().FirstOrDefault<OrderDetail>((OrderDetail o) => o.Id == id);
			return orderDetail;
		}

		public OrderDetail GetOrderDetailByOrderId(int id)
		{
			return this._orderDetailRepository.GetOrderDetailByOrderId(id);
		}

		public List<OrderDetail> GetOrderDetails(int id)
		{
			List<OrderDetail> list = (
				from o in this._orderDetailRepository.GetOrderDetails()
				where o.OrderId == id
				orderby o.CreatedDate descending
				select o).ToList<OrderDetail>();
			return list;
		}

		public List<OrderInformations> GetOrderInformations(int id)
		{
			List<OrderInformations> list = this._orderInformationRepository.GetInformationOrderByOrderId(id).ToList<OrderInformations>();
			return list;
		}
	}
}