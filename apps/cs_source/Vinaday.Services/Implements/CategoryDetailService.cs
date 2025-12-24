using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class CategoryDetailService : Service<CategoryDetail>, ICategoryDetailService, IService<CategoryDetail>
	{
		private readonly IRepositoryAsync<CategoryDetail> _categoryDetailRepository;

		public CategoryDetailService(IRepositoryAsync<CategoryDetail> categoryDetailRepository) : base(categoryDetailRepository)
		{
			this._categoryDetailRepository = categoryDetailRepository;
		}

		public List<CategoryDetail> GetCategoriesDetail(int id)
		{
			List<CategoryDetail> list = (
				from c in this._categoryDetailRepository.GetCategoriesDetail()
				where (c.CategoryId != id ? false : c.Status)
				select c).ToList<CategoryDetail>();
			return list;
		}

		public CategoryDetail GetCategoryDetail(int id)
		{
			CategoryDetail categoryDetail = this._categoryDetailRepository.GetCategoriesDetail().FirstOrDefault<CategoryDetail>((CategoryDetail c) => (c.Id != id ? false : c.Status));
			return categoryDetail;
		}
	}
}