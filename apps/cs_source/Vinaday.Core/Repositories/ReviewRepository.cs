using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class ReviewRepository
	{
		public static Review GetReview(this IRepositoryAsync<Review> repository, int id)
		{
			Review review = repository.Queryable().FirstOrDefault<Review>((Review r) => r.Id == id);
			return review;
		}

		public static IEnumerable<Review> GetReviews(this IRepositoryAsync<Review> repository)
		{
			return repository.Queryable().AsEnumerable<Review>();
		}

		public static IEnumerable<Review> GetReviews(this IRepositoryAsync<Review> repository, int hotelId)
		{
			IEnumerable<Review> reviews = (
				from r in repository.Queryable()
				where r.HotelId == (int?)hotelId && r.Status
				select r).AsEnumerable<Review>();
			return reviews;
		}

		public static IEnumerable<Review> GetReviewsByCustomerId(this IRepositoryAsync<Review> repository, string customerId)
		{
			IEnumerable<Review> reviews = (
				from r in repository.Queryable()
				where r.CustomerId.Contains(customerId) && r.Status
				select r).AsEnumerable<Review>();
			return reviews;
		}

		public static bool IsReview(this IRepositoryAsync<Review> repository, string customerId, int productId)
		{
			bool flag = repository.Queryable().Any<Review>((Review r) => r.CustomerId.Contains(customerId) && r.HotelId == (int?)productId);
			return flag;
		}
	}
}