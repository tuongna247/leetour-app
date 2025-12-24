using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class DayTripRate
	{
		public int? AgeFrom
		{
			get;
			set;
		}

		public int? AgeTo
		{
			get;
			set;
		}

		public virtual Vinaday.Data.Models.DAYTRIP DAYTRIP
		{
			get;
			set;
		}

		public int DaytripId
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public int id
		{
			get;
			set;
		}

		public decimal? NetRate
		{
			get;
			set;
		}

		public int? persons
		{
			get;
			set;
		}

		public decimal? RetailRate
		{
			get;
			set;
		}

		public DayTripRate()
		{
		}
	}
}