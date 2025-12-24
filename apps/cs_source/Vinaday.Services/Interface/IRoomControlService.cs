using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;
using Vinaday.Data.Models.Extention;

namespace Vinaday.Services
{
	public interface IRoomControlService : IService<RoomControl>
	{
		RoomControl GetRoomControlByRoomId(int roomId);

		List<RoomControlModel> GetRoomControlByRoomId(int roomId, DateTime checkIn, DateTime checkOut, int hotelId);

		RoomControl GetRoomControlByRoomIdRoomDate(int roomId, DateTime date);

		RoomControl GetRoomControlSingleByIdOrderBySellingRate(int roomId);

		List<RoomControl> GetRoomListCheckInOut(int roomId, DateTime fromDate, DateTime toDate);

		RoomControl GetSingleRoomCheckInOut(int id, DateTime checkIn, DateTime checkOut);

		RoomControl GetSingleRoomControlByDateRate(int id, DateTime dt);
	}
}