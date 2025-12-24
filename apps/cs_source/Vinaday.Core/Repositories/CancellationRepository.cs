using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class CancellationRepository
	{
		public static CancellationPolicy GetCancellationPolicyById(this IRepositoryAsync<CancellationPolicy> repository, int id)
		{
			CancellationPolicy cancellationPolicy = repository.Queryable().FirstOrDefault<CancellationPolicy>((CancellationPolicy c) => c.Id == id);
			return cancellationPolicy;
		}

		public static IEnumerable<CancellationPolicy> GetCancellationPolicys(this IRepositoryAsync<CancellationPolicy> repository)
		{
			return repository.Queryable().AsEnumerable<CancellationPolicy>();
		}
	}
}