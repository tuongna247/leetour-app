using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class RoomReguestService : Service<RoomReguest>, IRoomReguestService, IService<RoomReguest>
	{
		private readonly IRepositoryAsync<RoomReguest> _roomReguestRepository;

		public RoomReguestService(IRepositoryAsync<RoomReguest> roomReguestRepository) : base(roomReguestRepository)
		{
			this._roomReguestRepository = roomReguestRepository;
		}

		public RoomReguest GetReguest(int id)
		{
			return this._roomReguestRepository.GetReguest(id);
		}

		public List<RoomReguest> GetRoomReguests()
		{
			return this._roomReguestRepository.GetRoomReguests().ToList<RoomReguest>();
		}
	}
}