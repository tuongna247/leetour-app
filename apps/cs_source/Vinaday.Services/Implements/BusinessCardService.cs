using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class BusinessCardService : Service<BusinessCard>, IBusinessCardService, IService<BusinessCard>
	{
		private readonly IRepositoryAsync<BusinessCard> _businessCardRepository;

		public BusinessCardService(IRepositoryAsync<BusinessCard> businessCardRepository) : base(businessCardRepository)
		{
			this._businessCardRepository = businessCardRepository;
		}

		public BusinessCard GetBusinessCard(int id)
		{
			return this._businessCardRepository.GetBusinessCard(id);
		}

		public List<BusinessCard> GetBusinessCards()
		{
			return this._businessCardRepository.GetBusinessCards().ToList<BusinessCard>();
		}

		public List<BusinessCard> GetBusinessCardsByUser(string user)
		{
			List<BusinessCard> list = this._businessCardRepository.GetBusinessCards(user).ToList<BusinessCard>();
			return list;
		}
	}
}