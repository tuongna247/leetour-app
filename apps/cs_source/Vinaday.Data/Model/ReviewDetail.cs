using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class ReviewDetail : Entity
	{
		public virtual Vinaday.Data.Models.CatDetail CatDetail
		{
			get;
			set;
		}

		public int CategoryDetailId
		{
			get;
			set;
		}

		public DateTime CreateDate
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public DateTime? ModifiedDate
		{
			get;
			set;
		}

		public virtual Vinaday.Data.Models.Review Review
		{
			get;
			set;
		}

		public int ReviewId
		{
			get;
			set;
		}

		public bool Status
		{
			get;
			set;
		}

		public int Value
		{
			get;
			set;
		}

		public ReviewDetail()
		{
		}
	}
}