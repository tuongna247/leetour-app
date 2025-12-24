using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class RateExchange : Entity
	{
		public decimal? CurrentPrice
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public virtual ICollection<Hotel> Hotels
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public decimal? Money
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public RateExchange()
		{
			this.Hotels = new List<Hotel>();
		}
	}
}