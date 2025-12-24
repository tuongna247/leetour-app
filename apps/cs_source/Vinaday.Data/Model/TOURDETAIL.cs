using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class TOURDETAIL
	{
		public string Content
		{
			get;
			set;
		}

		public string ImageName
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

		public virtual Vinaday.Data.Models.TOUR TOUR
		{
			get;
			set;
		}

		public int TOURDETAILID
		{
			get;
			set;
		}

		public int? TOURID
		{
			get;
			set;
		}

		public string Transport
		{
			get;
			set;
		}

        public  string OverNight { get; set; }

        public TOURDETAIL()
		{
		}
	}
}