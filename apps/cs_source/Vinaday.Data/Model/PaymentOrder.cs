using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class PaymentOrder : Entity
	{
		public string CreatedBy
		{
			get;
			set;
		}

		public DateTime CreatedDate
		{
			get;
			set;
		}

		public decimal Discount
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public decimal Income
		{
			get;
			set;
		}

		public string IssueBy
		{
			get;
			set;
		}

		public DateTime ModifiedDate
		{
			get;
			set;
		}

		public DateTime OrderDate
		{
			get;
			set;
		}

		public int OrderId
		{
			get;
			set;
		}

		public decimal Outcome
		{
			get;
			set;
		}

		public decimal Profit
		{
			get;
			set;
		}

		public decimal Ratio
		{
			get;
			set;
		}

		public int Status
		{
			get;
			set;
		}

		public PaymentOrder()
		{
		}
	}
}