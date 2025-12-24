using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class TourSurchargeService : Service<Tour_Surcharge>, ITourSurchargeService, IService<Tour_Surcharge>
	{
		private readonly IRepositoryAsync<Tour_Surcharge> _surchargeRepository;

		private readonly IRepositoryAsync<RoomControl> _roomControlRepository;

		public TourSurchargeService(IRepositoryAsync<Tour_Surcharge> surchargeRepository, IRepositoryAsync<RoomControl> roomControlRepository) : base(surchargeRepository)
		{
			this._surchargeRepository = surchargeRepository;
			this._roomControlRepository = roomControlRepository;
		}

		public Vinaday.Data.Models.Tour_Surcharge Add(Vinaday.Data.Models.Tour_Surcharge Tour_Surcharge)
		{
			this._surchargeRepository.Insert(Tour_Surcharge);
			return Tour_Surcharge;
		}

		public Tour_Surcharge GetSurcharge(int id)
		{
			return this._surchargeRepository.GetSurcharge(id);
		}

		public List<Tour_Surcharge> GetSurchargesByDayTourId(DateTime date, int id)
		{
			List<Tour_Surcharge> list = this._surchargeRepository.GetSurchargesByTourIdDate(date, id).ToList<Tour_Surcharge>();
			return list;
		}

		public List<Tour_Surcharge> GetSurchargesByTourId(int id)
		{
			List<Tour_Surcharge> list = this._surchargeRepository.GetSurchargesByTourId(id).ToList<Tour_Surcharge>();
			return list;
		}
	}
}