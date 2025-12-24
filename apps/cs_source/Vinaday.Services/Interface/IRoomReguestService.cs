using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface IRoomReguestService : IService<RoomReguest>
	{
		RoomReguest GetReguest(int id);

		List<RoomReguest> GetRoomReguests();
	}
}