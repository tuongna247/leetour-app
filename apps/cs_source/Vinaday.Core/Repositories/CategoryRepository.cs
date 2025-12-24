using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class CategoryRepository
	{
		public static IEnumerable<Category> GetCategories(this IRepositoryAsync<Category> repository)
		{
			return repository.Queryable().AsEnumerable<Category>();
		}

		public static Category GetCategory(this IRepositoryAsync<Category> repository, int id)
		{
			Category category = repository.Queryable().FirstOrDefault<Category>((Category c) => c.Id == id);
			return category;
		}
	}
}