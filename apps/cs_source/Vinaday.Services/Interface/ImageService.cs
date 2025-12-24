using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class ImageService : Service<HotelImages>, IImageService, IService<HotelImages>
	{
		private readonly IRepositoryAsync<HotelImages> _imageRepository;

		public ImageService(IRepositoryAsync<HotelImages> imageRepository) : base(imageRepository)
		{
			this._imageRepository = imageRepository;
		}

		public List<HotelImages> GetHotelImages()
		{
			return this._imageRepository.GetHotelImages().ToList<HotelImages>();
		}

		public HotelImages GetHotelImageSetting()
		{
			return this._imageRepository.GetHotelImageSetting();
		}

		public List<HotelImages> GetImageListByHotelId(int hotelId)
		{
			List<HotelImages> list = this._imageRepository.GetImageListByHotelId(hotelId).ToList<HotelImages>();
			return list;
		}

		public HotelImages GetImageSingleByHotelId(int hotelId)
		{
			return this._imageRepository.GetImageSingleByHotelId(hotelId);
		}
	}
}