using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Extention
{
	public class PaymentOrderModel2
	{
		public PaymentOrder2 PaymentOrder
		{
			get;
			set;
		}

		public List<PaymentOrderDetail2> PaymentOrderDetails
		{
			get;
			set;
		}

		public PaymentOrderModel2()
		{
		}
	}
}