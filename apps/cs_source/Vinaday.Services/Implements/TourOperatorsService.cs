using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class TourOperatorsService : Service<TourOperators>, ITourOperatorsService
    {
		private readonly IRepositoryAsync<TourOperators> _tourOpertorRepository;

		public TourOperatorsService(IRepositoryAsync<TourOperators> tourOpertorRepository) : base(tourOpertorRepository)
		{
			this._tourOpertorRepository = tourOpertorRepository;
		}

      
    }
}