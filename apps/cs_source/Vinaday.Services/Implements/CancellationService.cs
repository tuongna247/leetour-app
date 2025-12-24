using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class CancellationService : Service<CancellationPolicy>, ICancellationService, IService<CancellationPolicy>
	{
		private readonly IRepositoryAsync<CancellationPolicy> _cancellationRepository;

		public CancellationService(IRepositoryAsync<CancellationPolicy> cancellationRepository) : base(cancellationRepository)
		{
			this._cancellationRepository = cancellationRepository;
		}

		public CancellationPolicy GetCancellation(int id)
		{
			return this._cancellationRepository.GetCancellationPolicyById(id);
		}

		public List<CancellationPolicy> GetCancellationList()
		{
			return this._cancellationRepository.GetCancellationPolicys().ToList<CancellationPolicy>();
		}
	}
}