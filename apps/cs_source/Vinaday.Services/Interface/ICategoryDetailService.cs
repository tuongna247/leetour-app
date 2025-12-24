using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface ICategoryDetailService : IService<CategoryDetail>
	{
		List<CategoryDetail> GetCategoriesDetail(int id);

		CategoryDetail GetCategoryDetail(int id);
	}
}