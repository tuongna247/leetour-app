using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class PasswordResetRequest
	{
		public int? AccountId
		{
			get;
			set;
		}

		public DateTime? Created
		{
			get;
			set;
		}

		public virtual Customer CUSTOMER
		{
			get;
			set;
		}

		public Guid Id
		{
			get;
			set;
		}

		public PasswordResetRequest()
		{
		}
	}
}