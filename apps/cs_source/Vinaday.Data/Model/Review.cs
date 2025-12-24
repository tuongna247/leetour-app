using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class Review : Entity
	{
		public string Content
		{
			get;
			set;
		}

		public DateTime CreateDate
		{
			get;
			set;
		}

		public string CustomerAddress
		{
			get;
			set;
		}

		public string CustomerId
		{
			get;
			set;
		}

		public string CustomerName
		{
			get;
			set;
		}

		public int? Helpful
		{
			get;
			set;
		}

		public int? HotelId
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public DateTime ModifiedDate
		{
			get;
			set;
		}

		public int RatingValue
		{
			get;
			set;
		}

		public virtual ICollection<ReviewDetail> ReviewDetails
		{
			get;
			set;
		}

		public bool? Sharing
		{
			get;
			set;
		}

		public bool Status
		{
			get;
			set;
		}

		public string Tips
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public int? Type
		{
			get;
			set;
		}

		public DateTime VisitDate
		{
			get;
			set;
		}

		public Review()
		{
			this.ReviewDetails = new List<ReviewDetail>();
		}
	}
}