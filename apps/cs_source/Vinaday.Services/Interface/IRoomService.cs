using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;
using Vinaday.Data.Models.Extention;

namespace Vinaday.Services
{
	public interface IRoomService : IService<Room>
	{
		Room AddRoom(Room room);

		List<RoomModel> GetPromotions(RoomModel room, int hotelId, int roomId, DateTime checkIn, DateTime checkOut);

		List<PromotionModel> GetPromotionsList(int id, int language);

		List<RoomModel> GetPromotionsVn(RoomModel room, int hotelId, int roomId, DateTime checkIn, DateTime checkOut);

		Room GetRoom(int id);

		List<Room> GetRoomList(int id);

		Room UpdateRoom(Room room);
	}
}