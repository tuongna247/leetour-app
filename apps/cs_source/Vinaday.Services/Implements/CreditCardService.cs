using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class CreditCardService : Service<CreditCard>, ICreditCardService, IService<CreditCard>
	{
		private readonly IRepositoryAsync<CreditCard> _creditCardRepository;

		public CreditCardService(IRepositoryAsync<CreditCard> creditCardRepository) : base(creditCardRepository)
		{
			this._creditCardRepository = creditCardRepository;
		}

		public CreditCard Add(CreditCard creditCard)
		{
			this._creditCardRepository.Insert(creditCard);
			return creditCard;
		}

		public CreditCard GetCreditCard(int id)
		{
			return this._creditCardRepository.GetCreditCard(id);
		}

		public List<CreditCard> GetCreditCards()
		{
			return this._creditCardRepository.GetCreditCards().ToList<CreditCard>();
		}
	}
}