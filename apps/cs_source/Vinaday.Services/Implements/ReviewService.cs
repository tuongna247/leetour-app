using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class ReviewService : Service<Review>, IReviewService, IService<Review>
	{
		private readonly IRepositoryAsync<Review> _reviewRepository;

		public ReviewService(IRepositoryAsync<Review> reviewRepository) : base(reviewRepository)
		{
			this._reviewRepository = reviewRepository;
		}

		public Review Add(Review review)
		{
			this._reviewRepository.Insert(review);
			return review;
		}

		public Review GetReview(int id)
		{
			return this._reviewRepository.GetReview(id);
		}

		public List<Review> GetReviews(int hotelId)
		{
			List<Review> list = this._reviewRepository.GetReviews(hotelId).ToList<Review>();
			return list;
		}

		public List<Review> GetReviewsByCustomerId(string customerid)
		{
			List<Review> list = this._reviewRepository.GetReviewsByCustomerId(customerid).ToList<Review>();
			return list;
		}
	}
}