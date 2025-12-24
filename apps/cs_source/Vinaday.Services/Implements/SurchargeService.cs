using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class SurchargeService : Service<Surcharge>, ISurchargeService, IService<Surcharge>
	{
		private readonly IRepositoryAsync<Surcharge> _surchargeRepository;

		private readonly IRepositoryAsync<RoomControl> _roomControlRepository;

		public SurchargeService(IRepositoryAsync<Surcharge> surchargeRepository, IRepositoryAsync<RoomControl> roomControlRepository) : base(surchargeRepository)
		{
			this._surchargeRepository = surchargeRepository;
			this._roomControlRepository = roomControlRepository;
		}

		public Surcharge Add(Surcharge surcharge)
		{
			this._surchargeRepository.Insert(surcharge);
			return surcharge;
		}

		public Surcharge GetSurcharge(int id)
		{
			return this._surchargeRepository.GetSurcharge(id);
		}

		public List<Surcharge> GetSurchargesByRoomId(int id, int hotelId)
		{
			List<Surcharge> list = this._surchargeRepository.GetSurchargesByRoomId(id, hotelId).ToList<Surcharge>();
			return list;
		}

		public List<Surcharge> GetSurchargesByRoomId(DateTime date, int id)
		{
			List<Surcharge> list = this._surchargeRepository.GetSurchargesByRoomId(date, id).ToList<Surcharge>();
			return list;
		}
	}
}