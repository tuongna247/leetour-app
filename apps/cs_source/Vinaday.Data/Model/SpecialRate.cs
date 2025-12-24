using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public sealed class SpecialRate : Entity
	{
		public DateTime BookingFrom
		{
			get;
			set;
		}

		public DateTime BookingTo
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

		public int? Status
		{
			get;
			set;
		}

		public int TourId
		{
			get;
			set;
		}

		public int? Type
		{
			get;
			set;
		}

		public SpecialRate()
		{
		}
	}
}