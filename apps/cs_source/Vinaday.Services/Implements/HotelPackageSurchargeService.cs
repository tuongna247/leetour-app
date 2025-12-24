using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class HotelPackageSurchargeService : Service<HotelPackage_Surcharge>
	{
		private readonly IRepositoryAsync<HotelPackage_Surcharge> _obj;

		public HotelPackageSurchargeService(IRepositoryAsync<HotelPackage_Surcharge> obj) : base(obj)
		{
			this._obj = obj;
		}

	}
}