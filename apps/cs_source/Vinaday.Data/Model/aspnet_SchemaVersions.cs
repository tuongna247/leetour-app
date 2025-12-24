using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class aspnet_SchemaVersions
	{
		public string CompatibleSchemaVersion
		{
			get;
			set;
		}

		public string Feature
		{
			get;
			set;
		}

		public bool IsCurrentVersion
		{
			get;
			set;
		}

		public aspnet_SchemaVersions()
		{
		}
	}
}