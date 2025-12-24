using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class OrderInformationService : Service<OrderInformations>, IOrderInformationService, IService<OrderInformations>
	{
		private readonly IRepositoryAsync<OrderInformations> _orderInformationRepository;

		public OrderInformationService(IRepositoryAsync<OrderInformations> orderInformationRepository) : base(orderInformationRepository)
		{
			this._orderInformationRepository = orderInformationRepository;
		}
	}
}