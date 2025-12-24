using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class NationalityService : Service<Nationality>, INationalityService
	{
		private readonly IRepositoryAsync<Nationality> _nationalityRepository;

		public NationalityService(IRepositoryAsync<Nationality> nationalityRepository) : base(nationalityRepository)
		{
			this._nationalityRepository = nationalityRepository;
		}

		public Nationality GetNationality(int id)
		{
			return this._nationalityRepository.GetNationality(id);
		}

		public List<Nationality> GetNationalityList()
		{
			return this._nationalityRepository.GetNationalities().ToList<Nationality>();
		}
	}
}