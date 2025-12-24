using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class PromotionService : Service<Promotion>, IPromotionService, IService<Promotion>
	{
		private readonly IRepositoryAsync<Promotion> _promotionRepository;

		public PromotionService(IRepositoryAsync<Promotion> promotionRepository) : base(promotionRepository)
		{
			this._promotionRepository = promotionRepository;
		}

		public List<Promotion> GetPromotionByHotelId(int hotelId)
		{
			List<Promotion> list = this._promotionRepository.GetPromotionsByHotelId(hotelId).ToList<Promotion>();
			return list;
		}

		public Promotion GetPromotionId(int id)
		{
			return this._promotionRepository.GetPromotionById(id);
		}
	}
}