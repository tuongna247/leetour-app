using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface IBusinessCardService : IService<BusinessCard>
	{
		BusinessCard GetBusinessCard(int id);

		List<BusinessCard> GetBusinessCards();

		List<BusinessCard> GetBusinessCardsByUser(string user);
	}
}