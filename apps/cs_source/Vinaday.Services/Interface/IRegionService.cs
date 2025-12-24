using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface IRegionService : IService<Region1>
	{
		Region1 GetRegion(int regionId);

		List<Region1> GetRegionList();
	}
}