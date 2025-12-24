using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface IReviewService : IService<Review>
	{
		Review Add(Review review);

		Review GetReview(int id);

		List<Review> GetReviews(int hotelId);

		List<Review> GetReviewsByCustomerId(string customerid);
	}
}