using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface INationalityService
	{
		Nationality GetNationality(int id);

		List<Nationality> GetNationalityList();
	}
}