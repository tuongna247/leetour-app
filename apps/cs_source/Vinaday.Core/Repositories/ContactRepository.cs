using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Core.Repositories
{
	public static class ContactRepository
	{
		public static Contact GetContact(this IRepositoryAsync<Contact> repository, int id)
		{
			Contact contact = repository.Queryable().FirstOrDefault<Contact>((Contact c) => c.CONTACTID == id);
			return contact;
		}

		public static IEnumerable<Contact> GetContacts(this IRepositoryAsync<Contact> repository)
		{
			return repository.Queryable().AsEnumerable<Contact>();
		}
	}
}