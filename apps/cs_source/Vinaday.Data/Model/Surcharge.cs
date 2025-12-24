using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class Surcharge : Entity
	{
		public string DateOfWeek
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

		public decimal? Price
		{
			get;
			set;
		}

		public int RoomId
		{
			get;
			set;
		}

		public DateTime StayDateFrom
		{
			get;
			set;
		}

		public DateTime StayDateTo
		{
			get;
			set;
		}

		public string SurchargeName
		{
			get;
			set;
		}

		public Surcharge()
		{
		}
	}
}