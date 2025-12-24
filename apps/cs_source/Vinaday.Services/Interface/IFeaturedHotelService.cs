using Service.Pattern;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface IFeaturedHotelService : IService<HotelFeatureds>
	{
		List<HotelFeatureds> GetFeaturedAscendingList();

		List<HotelFeatureds> GetHotelFeatureds();
	}
}