using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface ITourTopSiteService : IService<TourTopSite>
    {
        TourTopSite GetTourTopSite(int tourId);

        List<TourTopSite> GetTourTopSites();
    }
}