using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class ContactService : Service<Contact>, IContactService, IService<Contact>
	{
		private readonly IRepositoryAsync<Contact> _contactRepository;

		public ContactService(IRepositoryAsync<Contact> contactRepository) : base(contactRepository)
		{
			this._contactRepository = contactRepository;
		}

		public Contact Add(Contact contact)
		{
			this._contactRepository.Insert(contact);
			return contact;
		}
	}
}