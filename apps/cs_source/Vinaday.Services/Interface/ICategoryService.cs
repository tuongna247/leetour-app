using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;
using Vinaday.Data.Models.Extention;

namespace Vinaday.Services
{
	public interface ICategoryService : IService<Category>
	{
		List<Category> GetCategories();

		Category GetCategory(int id);

		CategoryModel GetCategoryById(int id);
	}
}