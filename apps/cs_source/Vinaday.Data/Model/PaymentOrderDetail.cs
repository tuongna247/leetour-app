using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class PaymentOrderDetail : Entity
	{
		public decimal Amount
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public int PaymentId
		{
			get;
			set;
		}

		public decimal Price
		{
			get;
			set;
		}

		public int Quantity
		{
			get;
			set;
		}

		public PaymentOrderDetail()
		{
		}
	}
}