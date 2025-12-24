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
    public class TourRateService2 : Service<Rate2>, ITourRateService2, IService<Rate2>
    {
        private readonly IRepositoryAsync<Rate2> _tourRateRepository2;

        public TourRateService2(IRepositoryAsync<Rate2> tourRateRepository) : base(tourRateRepository)
        {
            _tourRateRepository2 = tourRateRepository;
        }


        public List<Rate2> GetTourRates()
        {
            return this._tourRateRepository2.GetTourRates().ToList<Rate2>();
        }

        public List<Rate2> GetTourRatesById(int id)
        {
            List<Rate2> list = this._tourRateRepository2.GetTourRatesById(id).ToList<Rate2>();
            return list;
        }

        public Rate2 GetTourRatesByIdPersion(int id, int person)
        {
            return this._tourRateRepository2.GetTourRatesByIdPersion(id, person);
        }

        public List<Rate2> GetTourRatesByTourId(int id)
        {
            List<Rate2> list = (
                from r in this._tourRateRepository2.GetTourRates()
                where r.TourId == id
                orderby r.RetailRate
                select r).ToList<Rate2>();
            return list;
        }

    }
}