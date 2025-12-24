using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class CatDetail : Entity
	{
		public virtual Category1 Category
		{
			get;
			set;
		}

		public int CatId
		{
			get;
			set;
		}

		public string CheckedItem
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public string DescriptionDe
		{
			get;
			set;
		}

		public string DescriptionFr
		{
			get;
			set;
		}

		public string DescriptionGe
		{
			get;
			set;
		}

		public string DescriptionVn
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

		public virtual ICollection<ReviewDetail> ReviewDetails
		{
			get;
			set;
		}

		public bool Status
		{
			get;
			set;
		}

		public CatDetail()
		{
			this.ReviewDetails = new List<ReviewDetail>();
		}
	}
}