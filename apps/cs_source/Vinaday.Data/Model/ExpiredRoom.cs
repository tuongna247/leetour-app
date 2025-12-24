using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class ExpiredRoom
	{
		public string DESCRIPTION
		{
			get;
			set;
		}

		public DateTime FROMDATE
		{
			get;
			set;
		}

		public virtual Room HOTELDETAIL
		{
			get;
			set;
		}

		public int? HOTELDETAILID
		{
			get;
			set;
		}

		public int ID
		{
			get;
			set;
		}

		public bool? STATUS
		{
			get;
			set;
		}

		public DateTime TODATE
		{
			get;
			set;
		}

		public ExpiredRoom()
		{
		}
	}
}