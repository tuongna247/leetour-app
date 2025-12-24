using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Extention
{
	public class CategoryModel
	{
		public List<CategoryDetail> CategoryDetails
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

		public CategoryModel()
		{
		}
	}
}