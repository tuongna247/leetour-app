using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class TipService : Service<Tip>, ITipService, IService<Tip>
	{
		private readonly IRepositoryAsync<Tip> _seoRepository;

		public TipService(IRepositoryAsync<Tip> seoRepository) : base(seoRepository)
		{
			this._seoRepository = seoRepository;
		}

		public Tip GetTipByHotelId(int id)
		{
			Tip tip = this._seoRepository.GetTips().FirstOrDefault<Tip>((Tip t) => t.HotelId == id);
			return tip;
		}
	}
}