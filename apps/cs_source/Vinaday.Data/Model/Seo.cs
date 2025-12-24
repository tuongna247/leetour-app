using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class Seo : Entity
	{
		public string Description
		{
			get;
			set;
		}

		public string DescriptionVn
		{
			get;
			set;
		}

		public int EntityId
		{
			get;
			set;
		}

		public string EntityName
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public bool IsActive
		{
			get;
			set;
		}

		public string Keyword
		{
			get;
			set;
		}

		public string KeywordVn
		{
			get;
			set;
		}

		public int ProductType
		{
			get;
			set;
		}

		public string Slug
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public string TitleVn
		{
			get;
			set;
		}

		public Seo()
		{
		}
	}
}