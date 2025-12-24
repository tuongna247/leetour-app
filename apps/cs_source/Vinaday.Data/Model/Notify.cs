using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class Notify : Entity
	{
		public DateTime CreateDate
		{
			get;
			set;
		}

		public int HotelId
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public bool IsRead
		{
			get;
			set;
		}

		public string Message
		{
			get;
			set;
		}

		public string NotifyTitle
		{
			get;
			set;
		}

		public int NotifyType
		{
			get;
			set;
		}

		public int Status
		{
			get;
			set;
		}

		public string UserId
		{
			get;
			set;
		}

		public Notify()
		{
		}
	}
}