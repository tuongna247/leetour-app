using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class sysdiagram
	{
		public byte[] definition
		{
			get;
			set;
		}

		public int diagram_id
		{
			get;
			set;
		}

		public string name
		{
			get;
			set;
		}

		public int principal_id
		{
			get;
			set;
		}

		public int? version
		{
			get;
			set;
		}

		public sysdiagram()
		{
		}
	}
}