using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface ICancellationService : IService<CancellationPolicy>
	{
		CancellationPolicy GetCancellation(int id);

		List<CancellationPolicy> GetCancellationList();
	}
}