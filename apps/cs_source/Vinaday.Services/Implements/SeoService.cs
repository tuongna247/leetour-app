using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class SeoService : Service<Seo>, ISeoService, IService<Seo>
	{
		private readonly IRepositoryAsync<Seo> _seoRepository;

		public SeoService(IRepositoryAsync<Seo> seoRepository) : base(seoRepository)
		{
			this._seoRepository = seoRepository;
		}

		public Seo Add(Seo seo)
		{
			this._seoRepository.Insert(seo);
			return seo;
		}

		public void DeleteSeo(Seo seo)
		{
			this._seoRepository.Delete(seo);
		}

		public Seo GetSeoEntityId(int id)
		{
			Seo seo = this._seoRepository.GetSeos().FirstOrDefault<Seo>((Seo s) => (s.EntityId != id ? false : s.IsActive));
			return seo;
		}
	}
}