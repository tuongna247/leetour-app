using Service.Pattern;
using System;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface ISeoService : IService<Seo>
	{
		Seo Add(Seo seo);

		void DeleteSeo(Seo seo);

		Seo GetSeoEntityId(int id);
	}
}