using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class RoomReguest : Entity
	{
		public DateTime? CheckIn
		{
			get;
			set;
		}

		public DateTime? CheckOut
		{
			get;
			set;
		}

		public DateTime? CreateDate
		{
			get;
			set;
		}

		public string Email
		{
			get;
			set;
		}

		public string FirstName
		{
			get;
			set;
		}

		public string HotelName
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

		public string LastName
		{
			get;
			set;
		}

		public string Note
		{
			get;
			set;
		}

		public string Phone
		{
			get;
			set;
		}

		public string RoomName
		{
			get;
			set;
		}

		public int? RoomTotal
		{
			get;
			set;
		}

		public int Type
		{
			get;
			set;
		}

		public RoomReguest()
		{
		}
	}
}