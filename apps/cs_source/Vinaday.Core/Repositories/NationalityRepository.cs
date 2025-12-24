using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class NationalityRepository
	{
		public static IEnumerable<Nationality> GetNationalities(this IRepositoryAsync<Nationality> repository)
		{
			IEnumerable<Nationality> nationalities = (
				from n in repository.Queryable()
				orderby n.Priority
				select n).AsEnumerable<Nationality>();
			return nationalities;
		}

		public static Nationality GetNationality(this IRepositoryAsync<Nationality> repository, int id)
		{
			Nationality nationality = repository.Queryable().FirstOrDefault<Nationality>((Nationality n) => n.ID == id);
			return nationality;
		}
	}
}