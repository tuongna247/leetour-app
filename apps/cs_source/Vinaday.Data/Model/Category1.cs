using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class Category1 : Entity
	{
		public virtual ICollection<CatDetail> CatDetails
		{
			get;
			set;
		}

		public string DESCRIPTION
		{
			get;
			set;
		}

		public int ID
		{
			get;
			set;
		}

		public string NAME
		{
			get;
			set;
		}

		public bool? STATUS
		{
			get;
			set;
		}

		public Category1()
		{
			this.CatDetails = new List<CatDetail>();
		}
	}
}