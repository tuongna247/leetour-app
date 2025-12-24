using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models.Extention
{
	public class RoomModel
	{
		public int AdultNumber
		{
			get;
			set;
		}

		public string Cancelation
		{
			get;
			set;
		}

		public string CancelationVn
		{
			get;
			set;
		}

		public int ChildrenAge
		{
			get;
			set;
		}

		public int ChildrenNumber
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

		public int ExtraBed
		{
			get;
			set;
		}

		public int ExtraBedPrice
		{
			get;
			set;
		}

		public decimal FinalPrice
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public string ImageUrl
		{
			get;
			set;
		}

		public bool IsBreakfast
		{
			get;
			set;
		}

		public int MaxExtrabed
		{
			get;
			set;
		}

		public int MaxOccupancy
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public int PromotionId
		{
			get;
			set;
		}

		public string PromotionText
		{
			get;
			set;
		}

		public decimal RackRate
		{
			get;
			set;
		}

		public List<RoomModel> Rates
		{
			get;
			set;
		}

		public string RoomFacilities
		{
			get;
			set;
		}

		public string RoomFacilitiesVn
		{
			get;
			set;
		}

		public int RoomSize
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

		public int TotalAvailable
		{
			get;
			set;
		}

		public string View
		{
			get;
			set;
		}

		public string ViewVn
		{
			get;
			set;
		}

		public RoomModel()
		{
		}
	}
}