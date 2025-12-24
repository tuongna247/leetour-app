using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class ViewListTourService : Service<ViewListTour>
	{
		private readonly IRepositoryAsync<ViewListTour> _tourRepository;

		public ViewListTourService(IRepositoryAsync<ViewListTour> tourRepository) : base(tourRepository)
		{
			this._tourRepository = tourRepository;
		}
	}
}