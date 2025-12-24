using Service.Pattern;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface ICatService : IService<Category1>
	{
		List<Category1> GetCategories();
	}
}