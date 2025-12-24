using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class OrderDetail2 : Entity
	{
		public string ChangedName
		{
			get;
			set;
		}

		public string ChangedValue
		{
			get;
			set;
		}

		public DateTime CreatedDate
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public bool IsSend
		{
			get;
			set;
		}

		public string Note
		{
			get;
			set;
		}

		public virtual Order2 Order
		{
			get;
			set;
		}

		public int OrderId
		{
			get;
			set;
		}

		public string UserId
		{
			get;
			set;
		}

		public string Value
		{
			get;
			set;
		}

		public OrderDetail2()
		{
		}
	}
}