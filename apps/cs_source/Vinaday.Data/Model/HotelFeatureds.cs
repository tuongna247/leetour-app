using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class HotelFeatureds : Entity
	{
		public DateTime CreatedDate
		{
			get;
			set;
		}

		public int HotelId
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

		public HotelFeatureds()
		{
		}
	}
}