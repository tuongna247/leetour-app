using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class CatDetailService : Service<CatDetail>, ICatDetailService, IService<CatDetail>
	{
		private readonly IRepositoryAsync<CatDetail> _categoryDetailRepository;

		public CatDetailService(IRepositoryAsync<CatDetail> categoryDetailRepository) : base(categoryDetailRepository)
		{
			this._categoryDetailRepository = categoryDetailRepository;
		}

		public List<CatDetail> CategoriesList(string hoteCategory, int categoryId)
		{
			IEnumerable<CatDetail> categoriesDetail = this._categoryDetailRepository.GetCategoriesDetail(categoryId);
			Dictionary<int, int> selectedItem = Utilities.GetSelectedItem(hoteCategory);
			CatDetail[] array = categoriesDetail as CatDetail[] ?? categoriesDetail.ToArray<CatDetail>();
			CatDetail[] catDetailArray = array;
			for (int i = 0; i < (int)catDetailArray.Length; i++)
			{
				CatDetail catDetail = catDetailArray[i];
				catDetail.CheckedItem = (selectedItem.ContainsKey(catDetail.Id) ? "true" : string.Empty);
			}
			return array.ToList<CatDetail>();
		}

		public List<CatDetail> GetCategoriesDetail(int id)
		{
			List<CatDetail> list = this._categoryDetailRepository.GetCategoriesDetail(id).ToList<CatDetail>();
			return list;
		}

		public List<CatDetail> GetCategoriesDetail()
		{
			return this._categoryDetailRepository.GetCategoriesDetail().ToList<CatDetail>();
		}

		public CatDetail GetCategoryDetail(int id)
		{
			return this._categoryDetailRepository.GetCategoryDetail(id);
		}
	}
}