using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class BusinessCardRepository
	{
		public static BusinessCard GetBusinessCard(this IRepositoryAsync<BusinessCard> repository, int id)
		{
			BusinessCard businessCard = repository.Queryable().FirstOrDefault<BusinessCard>((BusinessCard c) => c.Id == id);
			return businessCard;
		}

		public static IEnumerable<BusinessCard> GetBusinessCards(this IRepositoryAsync<BusinessCard> repository)
		{
			IEnumerable<BusinessCard> businessCards = (
				from c in repository.Queryable()
				orderby c.IsCall descending
				select c).AsEnumerable<BusinessCard>();
			return businessCards;
		}

		public static IEnumerable<BusinessCard> GetBusinessCards(this IRepositoryAsync<BusinessCard> repository, string user)
		{
			IEnumerable<BusinessCard> businessCards = (
				from c in repository.Queryable()
				where c.UserAssignmentId == user
				select c).AsEnumerable<BusinessCard>();
			return businessCards;
		}
	}
}