using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface IReviewDetailService : IService<ReviewDetail>
	{
		ReviewDetail Add(ReviewDetail review);

		ReviewDetail GetReview(int id);

		List<ReviewDetail> GetReviews(int id);
	}
}