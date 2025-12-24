using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class RateControl
	{
		public decimal? CompulsoryMeal
		{
			get;
			set;
		}

		public DateTime DateRate
		{
			get;
			set;
		}

		public decimal FinalPrice
		{
			get;
			set;
		}

		public virtual Room HOTELDETAIL
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public decimal Profit
		{
			get;
			set;
		}

		public int RoomId
		{
			get;
			set;
		}

		public decimal SellingRate
		{
			get;
			set;
		}

		public decimal? Surcharge1
		{
			get;
			set;
		}

		public decimal? Surcharge2
		{
			get;
			set;
		}

		public decimal TARate
		{
			get;
			set;
		}

		public RateControl()
		{
		}
	}
}