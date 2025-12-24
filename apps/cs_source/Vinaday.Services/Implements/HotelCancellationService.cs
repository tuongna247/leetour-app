using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class HotelCancellationService : Service<HotelCancellation>, IHotelCancellationService, IService<HotelCancellation>
	{
		private readonly IRepositoryAsync<HotelCancellation> _cancellationHotelRepository;

		public HotelCancellationService(IRepositoryAsync<HotelCancellation> cancellationHotelRepository) : base(cancellationHotelRepository)
		{
			this._cancellationHotelRepository = cancellationHotelRepository;
		}

		public HotelCancellation GetCancellationHotel(int id)
		{
			HotelCancellation hotelCancellation = this._cancellationHotelRepository.GetHotelCancellationPolicys().FirstOrDefault<HotelCancellation>((HotelCancellation c) => c.HotelID == id);
			return hotelCancellation;
		}

		public HotelCancellation GetCancellationHotelById(int id)
		{
			return this._cancellationHotelRepository.GetHotelCancellationPolicyById(id);
		}

		public List<HotelCancellation> GetCancellationHotelList()
		{
			return this._cancellationHotelRepository.GetHotelCancellationPolicys().ToList<HotelCancellation>();
		}
	}
}