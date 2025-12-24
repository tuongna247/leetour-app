using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class ExpandRatesModel
	{
		public List<Rate> ExpandRatePrices
		{
			get;
			set;
		}

		public Vinaday.Data.Models.ExpandRates ExpandRates
		{
			get;
			set;
		}

		public ExpandRatesModel()
		{
		}
	}
}