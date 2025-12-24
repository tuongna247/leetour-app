using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class HotelImageRepository
	{
		public static IEnumerable<HotelImages> GetHotelImages(this IRepositoryAsync<HotelImages> repository)
		{
			return repository.Queryable().AsEnumerable<HotelImages>();
		}

		public static HotelImages GetHotelImageSetting(this IRepositoryAsync<HotelImages> repository)
		{
			HotelImages hotelImage = repository.Queryable().FirstOrDefault<HotelImages>((HotelImages i) => i.ImageQuanlity != null);
			return hotelImage;
		}

		public static IEnumerable<HotelImages> GetImageListByHotelId(this IRepositoryAsync<HotelImages> repository, int hotelId)
		{
			IEnumerable<HotelImages> hotelImages = (
				from i in repository.Queryable()
				where i.HotelId == hotelId
				orderby i.Id descending
				select i).AsEnumerable<HotelImages>();
			return hotelImages;
		}

		public static HotelImages GetImageSingle(this IRepositoryAsync<HotelImages> repository, int id)
		{
			HotelImages hotelImage = repository.Queryable().FirstOrDefault<HotelImages>((HotelImages i) => i.Id == id);
			return hotelImage;
		}

		public static HotelImages GetImageSingleByHotelId(this IRepositoryAsync<HotelImages> repository, int hotelId)
		{
			HotelImages hotelImage;
			IQueryable<HotelImages> hotelImages = 
				from i in repository.Queryable()
				where i.HotelId == hotelId
				select i;
			if (hotelImages.Count<HotelImages>() <= 0)
			{
				hotelImage = null;
			}
			else
			{
				HotelImages hotelImage1 = hotelImages.FirstOrDefault<HotelImages>((HotelImages a) => a.PictureType == "MainPhoto");
				hotelImage = (hotelImage1 == null ? hotelImages.FirstOrDefault<HotelImages>() : hotelImage1);
			}
			return hotelImage;
		}

        public static HotelImages GetImageSingleByRoomId(this IRepositoryAsync<HotelImages> repository, int hotelId, int roomId)
        {
            HotelImages hotelImage;
            IQueryable<HotelImages> hotelImages =
                from i in repository.Queryable()
                where i.HotelId == hotelId 
                select i;
            if (hotelImages.Count<HotelImages>() <= 0)
            {
                hotelImage = null;
            }
            else
            {
                HotelImages hotelImage1 = hotelImages.FirstOrDefault<HotelImages>((HotelImages a) => a.PictureType == "MainPhoto");
                hotelImage = (hotelImage1 == null ? hotelImages.FirstOrDefault<HotelImages>() : hotelImage1);
            }
            return hotelImage;
        }

        public static IEnumerable<HotelImages> GetRoomImageListByHotelId(this IRepositoryAsync<HotelImages> repository, int hotelId)
		{
			IEnumerable<HotelImages> hotelImages = (
				from i in repository.Queryable()
				where i.HotelId == hotelId && i.PictureType == "RoomType"
				orderby i.Id descending
				select i).AsEnumerable<HotelImages>();
			return hotelImages;
		}
	}
}