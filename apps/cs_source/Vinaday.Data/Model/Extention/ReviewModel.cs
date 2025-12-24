using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Extention
{
	public class ReviewModel
	{
		public string Content
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public ReviewDetailModel ReviewDetail
		{
			get;
			set;
		}

		public List<Review> Reviews
		{
			get;
			set;
		}

		public string Score
		{
			get;
			set;
		}

		public string ScoreText
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public ReviewModel()
		{
		}
	}
}