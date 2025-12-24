using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class OrderInformations : Entity
	{
		public decimal? Amount
		{
			get;
			set;
		}

		public DateTime CreatedDate
		{
			get;
			set;
		}

		public string Date
		{
			get;
			set;
		}

		public decimal? Discount
		{
			get;
			set;
		}

		public string DiscountName
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

		public int OrderId
		{
			get;
			set;
		}

		public decimal? Price
		{
			get;
			set;
		}

		public int Quantity
		{
			get;
			set;
		}

		public int Status
		{
			get;
			set;
		}

		public decimal? Surcharge
		{
			get;
			set;
		}

		public string SurchargeName
		{
			get;
			set;
		}

		public OrderInformations()
		{
		}
	}
}