using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class TourReviewService : Service<TourReview>
	{
        private readonly IRepositoryAsync<TourReview> _tourReviewRepository;

        public TourReviewService(IRepositoryAsync<TourReview> tourReviewRepository) : base(tourReviewRepository)
        {
            this._tourReviewRepository = tourReviewRepository;
        }

        //public List<TourReview> GetTourRates()
        //{
        //	return this._tourRateRepository.GetTourRates().ToList<Rate>();
        //}

        public List<TourReview> GetTourReviewByTourId(int id)
        {
            List<TourReview> list = this._tourReviewRepository.Queryable().Where(a=>a.TourId==id).ToList<TourReview>();
            return list;
        }

        //public Rate GetTourRatesByIdPersion(int id, int person)
        //{
        //	return this._tourRateRepository.GetTourRatesByIdPersion(id, person);
        //}

        //public List<TourReview> GetTourRatesByTourId(int id)
        //{
        //	List<TourReview> list = (
        //		from r in this._tourRateRepository.GetTourRates()
        //		where r.TourId == id
        //		orderby r.RetailRate
        //		select r).ToList<Rate>();
        //	return list;
        //}
    }
}