using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class AccessPermission
	{
		public DateTime CreatedDate { get; set; }

		public int EntityId { get; set; }

        public string EntityName { get; set; }

        public int Id { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int Type { get; set; }

        public AccessPermission()
		{
		}
	}
}