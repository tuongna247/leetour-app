using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class Detail : Entity
	{
		public string Content
		{
			get;
			set;
		}

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

		public string ImageId
		{
			get;
			set;
		}

		public string Itininary
		{
			get;
			set;
		}

		public string Meal
		{
			get;
			set;
		}
        public  string ImageDetail { get; set; }
        public  string ImageDetailDescription { get; set; }

        public  string OverNight { get; set; }

		public DateTime ModifiedDate
		{
			get;
			set;
		}

		public int? SortOrder
		{
			get;
			set;
		}

		public int Status
		{
			get;
			set;
		}

		public virtual Vinaday.Data.Models.Tour Tour
		{
			get;
			set;
		}

		public int? TourId
		{
			get;
			set;
		}

		public string Transport
		{
			get;
			set;
		}

		public Detail()
		{
		}
	}
}