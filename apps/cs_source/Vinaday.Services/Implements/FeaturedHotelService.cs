using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class FeaturedHotelService : Service<HotelFeatureds>, IFeaturedHotelService, IService<HotelFeatureds>
	{
		private readonly IRepositoryAsync<HotelFeatureds> _featuredHotelRepository;

		public FeaturedHotelService(IRepositoryAsync<HotelFeatureds> featuredHotelRepository) : base(featuredHotelRepository)
		{
			this._featuredHotelRepository = featuredHotelRepository;
		}

		public List<HotelFeatureds> GetFeaturedAscendingList()
		{
			return this._featuredHotelRepository.GetFeaturedAscendingList().ToList<HotelFeatureds>();
		}

		public List<HotelFeatureds> GetHotelFeatureds()
		{
			return this._featuredHotelRepository.GetFeaturedHotels().ToList<HotelFeatureds>();
		}
	}
}