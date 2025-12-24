using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface IDealService : IService<Deals>
	{
		List<Deals> GetAllDeals();

		Deals GetDeal(int id);

		List<Deals> GetDeals();

		List<Deals> GetDealsNotMain();

		Deals GetMainDeal();
	}
}