using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models.Extention
{
	public class UserModel
	{
		public string Email
		{
			get;
			set;
		}

		public int HotelId
		{
			get;
			set;
		}

		public string Id
		{
			get;
			set;
		}

		public bool IsApproved
		{
			get;
			set;
		}

		public bool IsLockedOut
		{
			get;
			set;
		}

		public string NewUserName
		{
			get;
			set;
		}

		public string Password
		{
			get;
			set;
		}

		public string PasswordAnswer
		{
			get;
			set;
		}

		public string PasswordQuestion
		{
			get;
			set;
		}

		public string Roles
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public UserModel()
		{
		}
	}
}