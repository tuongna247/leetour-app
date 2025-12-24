using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models.Extention
{
	public class ProductDetailModel
	{
		public DateTime Date
		{
			get;
			set;
		}

		public string Night
		{
			get;
			set;
		}

		public decimal PriceDiscount
		{
			get;
			set;
		}

		public decimal PriceRoom
		{
			get;
			set;
		}

		public decimal PriceSurcharge
		{
			get;
			set;
		}

		public int Quantity
		{
			get;
			set;
		}

		public ProductDetailModel()
		{
		}
	}
}