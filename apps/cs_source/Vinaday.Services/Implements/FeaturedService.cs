using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class FeaturedService : Service<Featured>, IFeaturedService, IService<Featured>
	{
		private readonly IRepositoryAsync<Featured> _featuredRepository;

		public FeaturedService(IRepositoryAsync<Featured> featuredRepository) : base(featuredRepository)
		{
			this._featuredRepository = featuredRepository;
		}

		public Featured GetFeatured(int tourId)
		{
			return this._featuredRepository.GetFeatured(tourId);
		}

		public List<Featured> GetFeatureds()
		{
			return this._featuredRepository.GetFeatureds().ToList<Featured>();
		}
	}
}