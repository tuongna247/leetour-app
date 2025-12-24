using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class LottoDetailRepository
	{
		public static bool CheckCode(this IRepositoryAsync<LottoDetail> repository, string code)
		{
			bool flag = repository.Queryable().FirstOrDefault<LottoDetail>((LottoDetail l) => l.Code == code && l.Status) != null;
			return flag;
		}

		public static IEnumerable<LottoDetail> GetLottoDetails(this IRepositoryAsync<LottoDetail> repository)
		{
			return repository.Queryable().AsEnumerable<LottoDetail>();
		}

		public static IEnumerable<LottoDetail> GetLottoDetailsByLottoId(this IRepositoryAsync<LottoDetail> repository, int id)
		{
			IEnumerable<LottoDetail> lottoDetails = (
				from l in repository.Queryable()
				where l.LottoId == id && l.Status
				select l).AsEnumerable<LottoDetail>();
			return lottoDetails;
		}
	}
}