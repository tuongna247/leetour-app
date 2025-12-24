using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class HotelPackageService : Service<HotelPackage>
	{
		private readonly IRepositoryAsync<HotelPackage> _hotelPackageRepository;

		public HotelPackageService(IRepositoryAsync<HotelPackage> hotelPackageRespo) : base(hotelPackageRespo)
		{
			this._hotelPackageRepository = hotelPackageRespo;
		}

	}
}