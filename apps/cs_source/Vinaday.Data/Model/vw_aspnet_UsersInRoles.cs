using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class vw_aspnet_UsersInRoles
	{
		public Guid RoleId
		{
			get;
			set;
		}

		public Guid UserId
		{
			get;
			set;
		}

		public vw_aspnet_UsersInRoles()
		{
		}
	}
}