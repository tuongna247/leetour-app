using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class LottoRepository
	{
		public static Lotto GetLotto(this IRepositoryAsync<Lotto> repository, int id)
		{
			Lotto lotto = repository.Queryable().FirstOrDefault<Lotto>((Lotto l) => l.Id == id);
			return lotto;
		}

		public static Lotto GetLottoByName(this IRepositoryAsync<Lotto> repository, string name)
		{
			Lotto lotto = repository.Queryable().FirstOrDefault<Lotto>((Lotto l) => l.HashLink.Contains(name) && l.Status);
			return lotto;
		}

		public static IEnumerable<Lotto> GetLottos(this IRepositoryAsync<Lotto> repository)
		{
			return repository.Queryable().AsEnumerable<Lotto>();
		}
	}
}