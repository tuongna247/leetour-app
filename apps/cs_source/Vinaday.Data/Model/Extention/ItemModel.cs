using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models.Extention
{
	public class ItemModel
	{
		public bool Checked
		{
			get;
			set;
		}

		public int Count
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

		public string Name
		{
			get;
			set;
		}

		public string Slug
		{
			get;
			set;
		}

		public bool Status
		{
			get;
			set;
		}

		public ItemModel()
		{
		}
	}
}