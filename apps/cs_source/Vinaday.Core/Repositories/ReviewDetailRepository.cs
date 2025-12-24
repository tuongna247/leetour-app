using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class ReviewDetailRepository
	{
		public static ReviewDetail GetReview(this IRepositoryAsync<ReviewDetail> repository, int id)
		{
			ReviewDetail reviewDetail = repository.Queryable().FirstOrDefault<ReviewDetail>((ReviewDetail r) => r.Id == id);
			return reviewDetail;
		}

		public static IEnumerable<ReviewDetail> GetReviews(this IRepositoryAsync<ReviewDetail> repository)
		{
			return repository.Queryable().AsEnumerable<ReviewDetail>();
		}

		public static IEnumerable<ReviewDetail> GetReviews(this IRepositoryAsync<ReviewDetail> repository, int id)
		{
			IEnumerable<ReviewDetail> reviewDetails = (
				from r in repository.Queryable()
				where r.ReviewId == id
				select r).AsEnumerable<ReviewDetail>();
			return reviewDetails;
		}
	}
}