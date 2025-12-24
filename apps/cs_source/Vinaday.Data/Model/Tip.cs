using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class Tip : Entity
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

		public int Status
		{
			get;
			set;
		}

		public string Tip1
		{
			get;
			set;
		}

		public string Tip2
		{
			get;
			set;
		}

		public string Tip3
		{
			get;
			set;
		}

		public Tip()
		{
		}
	}
}