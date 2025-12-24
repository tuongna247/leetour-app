using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
    public class CouponCodeService : Service<CouponCode>
    {
        private readonly IRepositoryAsync<CouponCode> _couponCodeRepository;

        public CouponCodeService(IRepositoryAsync<CouponCode> couponCodeRepository) : base(couponCodeRepository)
        {
            this._couponCodeRepository = couponCodeRepository;
        }


    }

    public interface ICouponCodeService
    {

    }
}