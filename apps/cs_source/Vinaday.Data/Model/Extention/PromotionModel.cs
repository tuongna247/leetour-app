using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models.Extention
{
	public class PromotionModel
	{
		public string BookingDate
		{
			get;
			set;
		}

		public int? CancelationId
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

		public string Name
		{
			get;
			set;
		}

		public decimal Price
		{
			get;
			set;
		}

		public List<PromotionModel> Promotions
		{
			get;
			set;
		}

		public string Stay
		{
			get;
			set;
		}

		public PromotionModel()
		{
		}
	}
}