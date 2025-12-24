using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;
using Vinaday.Data.Models.Extention;

namespace Vinaday.Services
{
	public interface IHotelService : IService<Hotel>
	{
		Hotel Add(Hotel hotel);

		string GenerateSlugHotelKey(Hotel hotel);

		List<HotelSearchModel> GetAllHotelByCityId(int id);

		string GetCancellationHotel(int id);

		string GetCancellationHotelVn(int id);

		List<HotelSearchModel> GetCities();
        List<HotelSearchModel> GetCitieByLuxyHotels();

        List<City> GetCities(int id);

		List<HotelSearchModel> GetCitiesEn();

		City GetCityByName(string name);

		City GetCityIdByName(string name);

		Contact GetContact(int id);

		List<CancellationModel> GetHotelCancellationPoliciesByHotelId(int id);

		List<HotelModel> GetHotelFeatured();

		List<Hotel> GetHotels();

		Hotel GetHotelSingle(int id);

		Hotel GetHotelSingleByHotelName(string name);

		Hotel GetHotelSingleByHotelNameVn(string name);

		Hotel GetHotelSingleByRoomId(int id);

		HotelImages GetImage(int id);

		List<HotelImages> GetImagesesByHotelId(int id);

		ReviewModel GetReviewsByHotelId(int hotelId, int language);

		ReviewModel GetReviewsByHotelIdForSearch(int hotelId, int language);

		RoomModel GetRoom(int hotelId);

		List<Room> GetRoomByHotelId(int id);

		List<HotelImages> GetRoomImagesByHotelId(int id);

		List<HotelModel> GetSimilarHotel(int cityId, int hotelId, int status, int munberOfList);

		List<HotelModel> GetSimilarLuxuryHotel(int cityId, int hotelId, int status, int munberOfList);

		List<HotelModel> GetSimilarVietnamHotel(int cityId, int hotelId, int status, int munberOfList);

		List<HotelModel> GetVietnamHotelFeatured();

		List<HotelModel> HotelBookingRecently(int munberOfList);

		Contact InsertContact(Contact contact);

		HotelSearchModel MapHotelToHotelModel(HotelSearchModel hotelModel);

		Contact UpdateContact(Contact contact);

		List<HotelModel> VietnamHotelBookingRecently(int munberOfList);
	}
}