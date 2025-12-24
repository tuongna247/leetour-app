using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class TourTopSiteService : Service<TourTopSite>, ITourTopSiteService, IService<TourTopSite>
	{
		private readonly IRepositoryAsync<TourTopSite> _tourSiteRepository;

		public TourTopSiteService(IRepositoryAsync<TourTopSite> featuredRepository) : base(featuredRepository)
		{
			this._tourSiteRepository = featuredRepository;
		}

        //public TourTopSite GetTourTopSite(int tourId)
        //{
        // return this._tourSiteRepository.ge
        //}

        //public List<TourTopSite> GetTourTopSites()
        //{
        //    throw new NotImplementedException();
        //}

        public TourTopSite GetTourTopSite(int tourId)
        {
            return this._tourSiteRepository.GetTourTopSite(tourId);
        }

        public List<TourTopSite> GetTourTopSites()
        {
            return this._tourSiteRepository.GetTourTopSites().ToList<TourTopSite>();
        }
    }
}