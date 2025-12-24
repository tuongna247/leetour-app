using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models.Extention
{
	public class RoomControlModel
	{
		public int AutoTopUp
		{
			get;
			set;
		}

		public bool CloseOutRegular
		{
			get;
			set;
		}

		public decimal CompulsoryMeal
		{
			get;
			set;
		}

		public DateTime Date
		{
			get;
			set;
		}

		public decimal FinalPrice
		{
			get;
			set;
		}

		public int Guaranteed
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public bool IsBreakfast
		{
			get;
			set;
		}

		public bool IsPast
		{
			get;
			set;
		}

		public decimal Profit
		{
			get;
			set;
		}

		public int Regular
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

		public decimal Surcharge1
		{
			get;
			set;
		}

		public decimal Surcharge2
		{
			get;
			set;
		}

		public string SurchargeList
		{
			get;
			set;
		}

		public decimal TaRate
		{
			get;
			set;
		}

		public int TotalAvailable
		{
			get;
			set;
		}

		public int UseGuaranteed
		{
			get;
			set;
		}

		public int UseRegular
		{
			get;
			set;
		}

		public RoomControlModel()
		{
		}
	}
}