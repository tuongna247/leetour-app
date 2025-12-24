using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class RegionService : Service<Region1>, IRegionService, IService<Region1>
	{
		private readonly IRepositoryAsync<Region1> _regionRepository;

		public RegionService(IRepositoryAsync<Region1> regionRepository) : base(regionRepository)
		{
			this._regionRepository = regionRepository;
		}

		public Region1 GetRegion(int regionId)
		{
			return this._regionRepository.GetRegion(regionId);
		}

		public List<Region1> GetRegionList()
		{
			return this._regionRepository.GetRegions().ToList<Region1>();
		}
	}
}