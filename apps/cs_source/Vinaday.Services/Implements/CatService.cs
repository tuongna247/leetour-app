using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class CatService : Service<Category1>, ICatService, IService<Category1>
	{
		private readonly IRepositoryAsync<Category1> _categoryRepository;

		public CatService(IRepositoryAsync<Category1> categoryRepository) : base(categoryRepository)
		{
			this._categoryRepository = categoryRepository;
		}

		public List<Category1> GetCategories()
		{
			return this._categoryRepository.GetCategories().ToList<Category1>();
		}
	}
}