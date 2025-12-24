using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public sealed class CancellationPolicy : Entity
	{
		public string Code
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public string DescriptionVn
		{
			get;
			set;
		}

		public ICollection<HotelCancellation> HotelCancellations
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public int? Status
		{
			get;
			set;
		}

		public CancellationPolicy()
		{
			this.HotelCancellations = new List<HotelCancellation>();
		}
	}
}