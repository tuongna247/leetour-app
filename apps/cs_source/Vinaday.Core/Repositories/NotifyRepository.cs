using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class NotifyRepository
	{
		public static IEnumerable<Notify> GetNotifies(this IRepositoryAsync<Notify> repository)
		{
			IEnumerable<Notify> notifies = (
				from n in repository.Queryable()
				orderby n.CreateDate descending
				select n).AsEnumerable<Notify>();
			return notifies;
		}
	}
}