using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface ICatDetailService : IService<CatDetail>
	{
		List<CatDetail> CategoriesList(string hoteCategory, int categoryId);

		List<CatDetail> GetCategoriesDetail(int id);

		List<CatDetail> GetCategoriesDetail();

		CatDetail GetCategoryDetail(int id);
	}
}