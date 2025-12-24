using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class CatRepository
	{
		public static IEnumerable<Category1> GetCategories(this IRepositoryAsync<Category1> repository)
		{
			return repository.Queryable().AsEnumerable<Category1>();
		}
	}
}