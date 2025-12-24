using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class IMAGE
	{
		public string Hotel_Base_Url
		{
			get;
			set;
		}

		public virtual ICollection<Hotel> Hotels
		{
			get;
			set;
		}

		public int ID
		{
			get;
			set;
		}

		public int? PrefixLocation
		{
			get;
			set;
		}

		public int? SuffixLocation
		{
			get;
			set;
		}

		public IMAGE()
		{
			this.Hotels = new List<Hotel>();
		}
	}
}