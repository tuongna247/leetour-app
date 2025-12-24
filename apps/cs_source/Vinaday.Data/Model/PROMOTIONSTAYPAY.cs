using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class PROMOTIONSTAYPAY
	{
		public string DESCRIPTION
		{
			get;
			set;
		}

		public virtual ICollection<HOTELPROMOTION> HOTELPROMOTIONs
		{
			get;
			set;
		}

		public int ID
		{
			get;
			set;
		}

		public int PAY
		{
			get;
			set;
		}

		public bool? STATUS
		{
			get;
			set;
		}

		public int STAY
		{
			get;
			set;
		}

		public string TITLE
		{
			get;
			set;
		}

		public string TitleVietNam
		{
			get;
			set;
		}

		public PROMOTIONSTAYPAY()
		{
			this.HOTELPROMOTIONs = new List<HOTELPROMOTION>();
		}
	}
}