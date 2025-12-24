using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface ITourDetailService : IService<Detail>
	{
		Detail Add(Detail detail);

		void DeleteTour(Detail detail);

		Detail GetDetail(int id);

		List<Detail> GetDetailTours();

		List<Detail> GetDetailToursByTourId(int id);
	}
}