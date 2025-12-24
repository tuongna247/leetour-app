using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
    public class HotelCouponService : Service<HotelCoupon>
    {
        private readonly IRepositoryAsync<HotelCoupon> _hotelCouponRepository;

        public HotelCouponService(IRepositoryAsync<HotelCoupon> hotelCouponRepository) : base(hotelCouponRepository)
        {
            this._hotelCouponRepository = hotelCouponRepository;
        }


    }

    public interface IHotelCouponService
    {

    }

    
}