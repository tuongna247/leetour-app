using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class LottoDetail : Entity
	{
		public string Code
		{
			get;
			set;
		}

		public DateTime Created
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

		public int Id
		{
			get;
			set;
		}

		public string LastIdNumber
		{
			get;
			set;
		}

		public string LastName
		{
			get;
			set;
		}

		public int LottoId
		{
			get;
			set;
		}

		public string Phone
		{
			get;
			set;
		}

		public bool Status
		{
			get;
			set;
		}

		public LottoDetail()
		{
		}
	}
}