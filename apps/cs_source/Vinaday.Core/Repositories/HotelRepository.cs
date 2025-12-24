using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class HotelRepository
	{
		public static int CountHotelByCityId(this IRepositoryAsync<Hotel> repository, int cityId, int status)
		{
			int num = (
				from a in repository.Queryable()
				where a.CityId == (int?)cityId && a.Status == (int?)status
				select a).AsEnumerable<Hotel>().Count<Hotel>();
			return num;
		}

        public static int CountLuxuryHotelByCityId(this IRepositoryAsync<Hotel> repository, int cityId, int status)
        {
            int num = (
                from a in repository.Queryable()
                where a.CityId == (int?)cityId && a.CollectionValue.HasValue && a.CollectionValue.Value>0 && a.Status == (int?)status
                select a).AsEnumerable<Hotel>().Count<Hotel>();
            return num;
        }


        public static IEnumerable<Hotel> GetHotelByCityId(this IRepositoryAsync<Hotel> repository, int cityId, int status)
		{
			IEnumerable<Hotel> hotels = (
				from a in repository.Queryable()
				where a.CityId == (int?)cityId && a.Status == (int?)status
				select a).AsEnumerable<Hotel>();
			return hotels;
		}

		public static IEnumerable<Hotel> GetHotelLuxurySimilars(this IRepositoryAsync<Hotel> repository, int cityId, int hotelId, int status, int munberOfList)
		{
			IEnumerable<Hotel> hotels = (
				from a in repository.Queryable()
				where a.CityId == (int?)cityId && a.Id != hotelId && a.Status == (int?)status && a.StartRating > (int?)8
				select a).Take<Hotel>(munberOfList).AsEnumerable<Hotel>();
			return hotels;
		}

		public static IEnumerable<Hotel> GetHotels(this IRepositoryAsync<Hotel> repository, int status)
		{
			IEnumerable<Hotel> hotels = (
				from h in repository.Queryable()
				where h.Status == (int?)status
				orderby h.Name
				select h).AsEnumerable<Hotel>();
			return hotels;
		}

		public static IEnumerable<Hotel> GetHotelSimilars(this IRepositoryAsync<Hotel> repository, int cityId, int hotelId, int status, int munberOfList)
		{
			IEnumerable<Hotel> hotels = (
				from a in repository.Queryable()
				where a.CityId == (int?)cityId && a.Id != hotelId && a.Status == (int?)status && a.StartRating <= (int?)8
				select a).Take<Hotel>(munberOfList).AsEnumerable<Hotel>();
			return hotels;
		}

		public static Hotel GetHotelSingle(this IRepositoryAsync<Hotel> repository, int id)
		{
			Hotel hotel = repository.Queryable().FirstOrDefault<Hotel>((Hotel h) => h.Id == id);
			return hotel;
		}

		public static Hotel GetHotelSingleByHotelName(this IRepositoryAsync<Hotel> repository, string name)
		{
			Hotel hotel = repository.Queryable().FirstOrDefault<Hotel>((Hotel h) => h.Name.ToLower().Trim() == name.ToLower().Trim());
			return hotel;
		}

		public static Hotel GetHotelSingleByHotelNameVn(this IRepositoryAsync<Hotel> repository, string name)
		{
			Hotel hotel = repository.Queryable().FirstOrDefault<Hotel>((Hotel h) => h.HotelNameLocal.ToLower().Trim() == name.ToLower().Trim());
			return hotel;
		}
	}
}