using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class ReviewDetailService : Service<ReviewDetail>, IReviewDetailService, IService<ReviewDetail>
	{
		private readonly IRepositoryAsync<ReviewDetail> _reviewDetailRepository;

		public ReviewDetailService(IRepositoryAsync<ReviewDetail> reviewDetailRepository) : base(reviewDetailRepository)
		{
			this._reviewDetailRepository = reviewDetailRepository;
		}

		public ReviewDetail Add(ReviewDetail review)
		{
			this._reviewDetailRepository.Insert(review);
			return review;
		}

		public ReviewDetail GetReview(int id)
		{
			return this._reviewDetailRepository.GetReview(id);
		}

		public List<ReviewDetail> GetReviews(int id)
		{
			List<ReviewDetail> list = this._reviewDetailRepository.GetReviews(id).ToList<ReviewDetail>();
			return list;
		}
	}
}