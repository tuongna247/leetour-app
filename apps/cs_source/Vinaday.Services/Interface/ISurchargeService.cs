using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface ISurchargeService : IService<Surcharge>
	{
		Surcharge Add(Surcharge contact);

		Surcharge GetSurcharge(int id);

		List<Surcharge> GetSurchargesByRoomId(int id, int hotelId);

		List<Surcharge> GetSurchargesByRoomId(DateTime date, int id);
	}
}