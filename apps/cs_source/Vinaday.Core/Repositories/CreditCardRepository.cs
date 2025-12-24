using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class CreditCardRepository
	{
		public static CreditCard GetCreditCard(this IRepositoryAsync<CreditCard> repository, int id)
		{
			CreditCard creditCard = repository.Queryable().FirstOrDefault<CreditCard>((CreditCard c) => c.Id == id);
			return creditCard;
		}

		public static IEnumerable<CreditCard> GetCreditCards(this IRepositoryAsync<CreditCard> repository)
		{
			return repository.Queryable().AsEnumerable<CreditCard>();
		}
	}
}