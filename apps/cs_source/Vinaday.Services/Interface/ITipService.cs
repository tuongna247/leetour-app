using Service.Pattern;
using System;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface ITipService : IService<Tip>
	{
		Tip GetTipByHotelId(int id);
	}
}