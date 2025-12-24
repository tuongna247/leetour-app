using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class Featured : Entity
	{
		public DateTime CreatedDate
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public int ImageUrl
		{
			get;
			set;
		}

		public DateTime ModifiedDate
		{
			get;
			set;
		}

		public int Priority
		{
			get;
			set;
		}

		public virtual Vinaday.Data.Models.Tour Tour
		{
			get;
			set;
		}

		public int TourId
		{
			get;
			set;
		}

		public Featured()
		{
		}
	}
}