using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
    public class TourRateOptionsService : Service<TourRateOptions>
    {
        private readonly IRepositoryAsync<TourRateOptions> _Repository;

        public TourRateOptionsService(IRepositoryAsync<TourRateOptions> repository) : base(repository)
        {
            this._Repository = repository;
        }


    }

    public interface ITourRateOptionsService
    {

    }
}