using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class Deals : Entity
	{
		public DateTime Countdown
		{
			get;
			set;
		}

		public DateTime Created
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

		public string ImageUrl
		{
			get;
			set;
		}

		public DateTime Modified
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public int? Priority
		{
			get;
			set;
		}

		public int Status
		{
			get;
			set;
		}

		public int Type
		{
			get;
			set;
		}

		public string Url
		{
			get;
			set;
		}

		public Deals()
		{
		}
	}
}