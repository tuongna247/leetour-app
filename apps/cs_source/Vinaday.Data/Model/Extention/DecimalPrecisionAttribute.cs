using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models.Extention
{
	[AttributeUsage(AttributeTargets.Property, Inherited=false, AllowMultiple=false)]
	public sealed class DecimalPrecisionAttribute : Attribute
	{
		public byte Precision
		{
			get;
			set;
		}

		public byte Scale
		{
			get;
			set;
		}

		public DecimalPrecisionAttribute(byte precision, byte scale)
		{
			this.Precision = precision;
			this.Scale = scale;
		}
	}
}