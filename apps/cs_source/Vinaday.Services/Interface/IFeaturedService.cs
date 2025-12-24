using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface IFeaturedService : IService<Featured>
	{
		Featured GetFeatured(int tourId);

		List<Featured> GetFeatureds();
	}
}