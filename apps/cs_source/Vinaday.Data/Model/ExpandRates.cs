using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class ExpandRates : Entity
	{
		public DateTime CreatedDate
		{
			get;
			set;
		}

		public DateTime EndDate
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public DateTime ModifiedDate
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public decimal? Price
		{
			get;
			set;
		}

		public DateTime StartDate
		{
			get;
			set;
		}

		public bool? Status
		{
			get;
			set;
		}

		public int TourId
		{
			get;
			set;
		}

		public int Type
		{
			get;
			set;
		}

		public ExpandRates()
		{
		}
	}
}