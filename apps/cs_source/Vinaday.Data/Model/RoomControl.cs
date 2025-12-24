using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class RoomControl : Entity
	{
		public int? AutoTopUp
		{
			get;
			set;
		}

		public bool? Breakfast
		{
			get;
			set;
		}

		public bool CloseOutRegular
		{
			get;
			set;
		}

		public decimal? CompulsoryMeal
		{
			get;
			set;
		}

		public decimal? FinalPrice
		{
			get;
			set;
		}

		public int? Guaranteed
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public decimal? Profit
		{
			get;
			set;
		}

		public int Regular
		{
			get;
			set;
		}

		public virtual Vinaday.Data.Models.Room Room
		{
			get;
			set;
		}

		public DateTime RoomDate
		{
			get;
			set;
		}

		public int RoomId
		{
			get;
			set;
		}

		public decimal? SellingRate
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

		public decimal? TaRate
		{
			get;
			set;
		}

		public int TotalAvailable
		{
			get;
			set;
		}

		public int? UseGuaranteed
		{
			get;
			set;
		}

		public int? UseRegular
		{
			get;
			set;
		}

		public RoomControl()
		{
		}
	}
}