using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class TipRepository
	{
		public static IEnumerable<Tip> GetTips(this IRepositoryAsync<Tip> repository)
		{
			return repository.Queryable().AsEnumerable<Tip>();
		}
	}
}