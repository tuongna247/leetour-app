using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public sealed class Category : Entity
	{
		public ICollection<CategoryDetail> CategoryDetails
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

		public int Id
		{
			get;
			set;
		}

		public string KeyCode
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

		public Category()
		{
			this.CategoryDetails = new List<CategoryDetail>();
		}
	}
}