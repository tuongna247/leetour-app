using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models.Extention
{
	public class DataTableData
	{
		public List<OrderModel> data
		{
			get;
			set;
		}

		public int draw
		{
			get;
			set;
		}

		public int recordsFiltered
		{
			get;
			set;
		}

		public int recordsTotal
		{
			get;
			set;
		}

		public DataTableData()
		{
		}
	}
}