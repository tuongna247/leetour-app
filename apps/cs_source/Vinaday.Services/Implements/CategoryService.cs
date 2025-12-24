using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;
using Vinaday.Data.Models.Extention;

namespace Vinaday.Services
{
	public class CategoryService : Service<Category>, ICategoryService, IService<Category>
	{
		private readonly IRepositoryAsync<Category> _categoryRepository;

		private readonly IRepositoryAsync<CategoryDetail> _categoryDetailRepository;

		public CategoryService(IRepositoryAsync<Category> categoryRepository, IRepositoryAsync<CategoryDetail> categoryDetailRepository) : base(categoryRepository)
		{
			this._categoryRepository = categoryRepository;
			this._categoryDetailRepository = categoryDetailRepository;
		}

		public List<Category> GetCategories()
		{
			return this._categoryRepository.GetCategories().ToList<Category>();
		}

		public Category GetCategory(int id)
		{
			return this._categoryRepository.GetCategory(id);
		}

		public CategoryModel GetCategoryById(int id)
		{
			Category category = this._categoryRepository.GetCategory(id);
			List<CategoryDetail> list = this._categoryDetailRepository.GetCategoriesDetail(id).ToList<CategoryDetail>();
			return new CategoryModel()
			{
				Id = category.Id,
				Description = category.Description,
				Name = category.Name,
				CreatedDate = category.CreatedDate,
				KeyCode = category.KeyCode,
				ModifiedDate = category.ModifiedDate,
				Status = category.Status,
				CategoryDetails = list
			};
		}
	}
}