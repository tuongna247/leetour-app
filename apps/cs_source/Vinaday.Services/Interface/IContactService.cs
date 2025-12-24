using Service.Pattern;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface IContactService : IService<Contact>
	{
		Contact Add(Contact contact);
	}
}