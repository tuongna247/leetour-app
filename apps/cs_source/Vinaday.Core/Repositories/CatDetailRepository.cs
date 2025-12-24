using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class CatDetailRepository
	{
		public static IEnumerable<CatDetail> GetCategoriesDetail(this IRepositoryAsync<CatDetail> repository)
		{
			IEnumerable<CatDetail> catDetails = (
				from c in repository.Queryable()
				where c.Status
				select c).AsEnumerable<CatDetail>();
			return catDetails;
		}

		public static IEnumerable<CatDetail> GetCategoriesDetail(this IRepositoryAsync<CatDetail> repository, int id)
		{
			IEnumerable<CatDetail> catDetails = (
				from c in repository.Queryable()
				where c.CatId == id && c.Status
				select c).AsEnumerable<CatDetail>();
			return catDetails;
		}

		public static CatDetail GetCategoryDetail(this IRepositoryAsync<CatDetail> repository, int id)
		{
			CatDetail catDetail = repository.Queryable().FirstOrDefault<CatDetail>((CatDetail c) => c.Id == id && c.Status);
			return catDetail;
		}
	}
}