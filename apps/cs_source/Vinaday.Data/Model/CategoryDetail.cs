using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class CategoryDetail : Entity
	{
		public virtual Vinaday.Data.Models.Category Category
		{
			get;
			set;
		}

		public int CategoryId
		{
			get;
			set;
		}

		public DateTime CreatedDate
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

		public DateTime ModifiedDate
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public bool Status
		{
			get;
			set;
		}

		public CategoryDetail()
		{
		}
	}
}