using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class CategoryDetailRepository
	{
		public static IEnumerable<CategoryDetail> GetCategoriesDetail(this IRepositoryAsync<CategoryDetail> repository)
		{
			return repository.Queryable().AsEnumerable<CategoryDetail>();
		}

		public static IEnumerable<CategoryDetail> GetCategoriesDetail(this IRepositoryAsync<CategoryDetail> repository, int id)
		{
			IEnumerable<CategoryDetail> categoryDetails = (
				from c in repository.Queryable()
				where c.CategoryId == id && c.Status
				select c).AsEnumerable<CategoryDetail>();
			return categoryDetails;
		}

		public static CategoryDetail GetCategoryDetail(this IRepositoryAsync<CategoryDetail> repository, int id)
		{
			CategoryDetail categoryDetail = repository.Queryable().FirstOrDefault<CategoryDetail>((CategoryDetail c) => c.Id == id && c.Status);
			return categoryDetail;
		}
	}
}