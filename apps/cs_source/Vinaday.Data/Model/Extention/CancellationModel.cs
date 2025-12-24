using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models.Extention
{
	public class CancellationModel
	{
		public int CancellationId
		{
			get;
			set;
		}

		public DateTime CheckInFrom
		{
			get;
			set;
		}

		public DateTime CheckOutTo
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public string DescriptionVn
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public CancellationModel()
		{
		}
	}
}