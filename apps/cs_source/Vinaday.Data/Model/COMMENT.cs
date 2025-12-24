using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class COMMENT
	{
		public string COMMENT1
		{
			get;
			set;
		}

		public int COMMENTID
		{
			get;
			set;
		}

		public DateTime? CREATEDDATE
		{
			get;
			set;
		}

		public virtual Customer CUSTOMER
		{
			get;
			set;
		}

		public int? CUSTOMERID
		{
			get;
			set;
		}

		public string DESCRIPTION
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

		public string NAME
		{
			get;
			set;
		}

		public COMMENT()
		{
		}
	}
}