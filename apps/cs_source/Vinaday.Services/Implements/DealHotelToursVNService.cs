using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
    public class DealHotelToursVNService : Service<DealHotelToursVN>
    {
        private readonly IRepositoryAsync<DealHotelToursVN> _dealhotelTourRepository;

        public DealHotelToursVNService(IRepositoryAsync<DealHotelToursVN> dealhotelTourRepository) : base(dealhotelTourRepository)
        {
            this._dealhotelTourRepository = dealhotelTourRepository;
        }


    }

    public interface IDealHotelToursVNService
    {

    }

    
}