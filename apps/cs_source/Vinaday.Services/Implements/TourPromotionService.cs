using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class TourPromotionService : Service<Tour_Promotion>, ITourPromotionService, IService<Tour_Promotion>
	{
		private readonly IRepositoryAsync<Tour_Promotion> _promotionRepository;

		public TourPromotionService(IRepositoryAsync<Tour_Promotion> promotionRepository) : base(promotionRepository)
		{
			this._promotionRepository = promotionRepository;
		}

		public List<Tour_Promotion> GetPromotionByDayTourId(DateTime date, int id)
		{
			return this._promotionRepository.GetPromotionByDayTourId(date, id);
		}

		public Tour_Promotion GetPromotionId(int id)
		{
			return this._promotionRepository.GetPromotionById(id);
		}

		public List<Tour_Promotion> GetPromotionsByTourId(int hotelId)
		{
			List<Tour_Promotion> list = this._promotionRepository.GetPromotionsByTourId(hotelId).ToList<Tour_Promotion>();
			return list;
		}
	}
}