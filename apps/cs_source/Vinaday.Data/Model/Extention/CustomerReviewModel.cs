using System;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Extention
{
	public class CustomerReviewModel
	{
		public Vinaday.Data.Models.Review Review
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public CustomerReviewModel()
		{
		}
	}
}