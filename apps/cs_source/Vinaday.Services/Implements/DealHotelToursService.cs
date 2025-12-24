using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
    public class DealHotelToursService : Service<DealHotelTours>
    {
        private readonly IRepositoryAsync<DealHotelTours> _dealhotelTourRepository;

        public DealHotelToursService(IRepositoryAsync<DealHotelTours> dealhotelTourRepository) : base(dealhotelTourRepository)
        {
            this._dealhotelTourRepository = dealhotelTourRepository;
        }


    }

    public interface IDealHotelToursService
    {

    }

    
}