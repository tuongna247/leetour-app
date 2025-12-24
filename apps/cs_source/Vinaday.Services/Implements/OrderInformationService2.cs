using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class OrderInformationService2 : Service<OrderInformation2s>, IOrderInformationService2, IService<OrderInformation2s>
	{
		private readonly IRepositoryAsync<OrderInformation2s> _orderInformationRepository;

		public OrderInformationService2(IRepositoryAsync<OrderInformation2s> orderInformationRepository) : base(orderInformationRepository)
		{
			this._orderInformationRepository = orderInformationRepository;
		}
	}
}