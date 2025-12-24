using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Extention
{
	public class PaymentOrderModel
	{
		public Vinaday.Data.Models.PaymentOrder PaymentOrder
		{
			get;
			set;
		}

		public List<PaymentOrderDetail> PaymentOrderDetails
		{
			get;
			set;
		}

		public PaymentOrderModel()
		{
		}
	}
}