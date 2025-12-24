using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class MediaRepository
	{
		public static Medium GetMediaById(this IRepositoryAsync<Medium> repository, int id)
		{
			Medium medium = repository.Queryable().FirstOrDefault<Medium>((Medium m) => m.OwnerId == id);
			return medium;
		}

		public static Medium GetMediaByMediaId(this IRepositoryAsync<Medium> repository, int id)
		{
			Medium medium = repository.Queryable().FirstOrDefault<Medium>((Medium m) => m.Id == id);
			return medium;
		}

		public static IEnumerable<Medium> GetMedium(this IRepositoryAsync<Medium> repository)
		{
			return repository.Queryable().AsEnumerable<Medium>();
		}
	}
}