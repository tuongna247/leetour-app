using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class MediaService : Service<Medium>, IMediaService, IService<Medium>
	{
		private readonly IRepositoryAsync<Medium> _mediaRepository;

		public MediaService(IRepositoryAsync<Medium> mediaRepository) : base(mediaRepository)
		{
			this._mediaRepository = mediaRepository;
		}

		public Medium Add(Medium media)
		{
			this._mediaRepository.Insert(media);
			return media;
		}

		public Medium GetMediaById(int id)
		{
			return this._mediaRepository.GetMediaByMediaId(id);
		}

		public Medium GetMediaByTourId(int id)
		{
			Medium medium = this._mediaRepository.GetMedium().FirstOrDefault<Medium>((Medium m) => m.OwnerId == id);
			return medium;
		}
	}
}