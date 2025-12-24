using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class Medium : Entity
	{
		public string AlternateText
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

		public int MediaType
		{
			get;
			set;
		}

		public DateTime ModifiedDate
		{
			get;
			set;
		}

		public string OriginalPath
		{
			get;
			set;
		}

		public int OwnerId
		{
			get;
			set;
		}

		public int Status
		{
			get;
			set;
		}

		public string ThumbnailPath
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public int Type
		{
			get;
			set;
		}

		public Medium()
		{
		}
	}
}