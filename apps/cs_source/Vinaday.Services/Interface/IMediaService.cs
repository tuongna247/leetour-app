using Service.Pattern;
using System;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface IMediaService : IService<Medium>
	{
		Medium Add(Medium media);

		Medium GetMediaById(int id);

		Medium GetMediaByTourId(int id);
	}
}