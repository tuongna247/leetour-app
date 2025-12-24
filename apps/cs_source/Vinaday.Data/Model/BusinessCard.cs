using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class BusinessCard : Entity
	{
		public string Address
		{
			get;
			set;
		}

		public bool? Disconnected
		{
			get;
			set;
		}

		public string Email
		{
			get;
			set;
		}

		public string EmailSecond
		{
			get;
			set;
		}

		public string FullName
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public bool IsCall
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

		public string PhoneSecond
		{
			get;
			set;
		}

		public int Priority
		{
			get;
			set;
		}

		public int Status
		{
			get;
			set;
		}

		public string UserAssignmentId
		{
			get;
			set;
		}

		public bool WrongNumber
		{
			get;
			set;
		}

		public BusinessCard()
		{
		}
	}
}