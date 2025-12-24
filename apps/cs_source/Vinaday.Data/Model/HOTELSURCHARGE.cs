using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class HOTELSURCHARGE
	{
		public decimal? AMOUNT
		{
			get;
			set;
		}

		public string DAYS
		{
			get;
			set;
		}

		public string DESCRIPTION
		{
			get;
			set;
		}

		public DateTime? ENDDATE
		{
			get;
			set;
		}

		public virtual Vinaday.Data.Models.Hotel Hotel
		{
			get;
			set;
		}

		public int? HOTELID
		{
			get;
			set;
		}

		public int ID
		{
			get;
			set;
		}

		public bool? ISCompulsoryDinner
		{
			get;
			set;
		}

		public DateTime? STARTDATE
		{
			get;
			set;
		}

		public bool? STATUS
		{
			get;
			set;
		}

		public decimal? TAAMOUNT
		{
			get;
			set;
		}

		public HOTELSURCHARGE()
		{
		}
	}
}