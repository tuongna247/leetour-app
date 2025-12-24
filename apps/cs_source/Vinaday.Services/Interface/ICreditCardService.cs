using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface ICreditCardService : IService<CreditCard>
	{
		CreditCard Add(CreditCard creditCard);

		CreditCard GetCreditCard(int id);

		List<CreditCard> GetCreditCards();
	}
}