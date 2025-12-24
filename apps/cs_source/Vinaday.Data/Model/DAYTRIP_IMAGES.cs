using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class DAYTRIP_IMAGES
	{
		public virtual Vinaday.Data.Models.DAYTRIP DAYTRIP
		{
			get;
			set;
		}

		public int DAYTRIP_ID
		{
			get;
			set;
		}

		public string English_Title
		{
			get;
			set;
		}

		public int ID
		{
			get;
			set;
		}

		public int? IMAGE_HEIGHT
		{
			get;
			set;
		}

		public string IMAGE_NAME
		{
			get;
			set;
		}

		public string IMAGE_ORIGIN
		{
			get;
			set;
		}

		public int? IMAGE_QUANLITY
		{
			get;
			set;
		}

		public double? IMAGE_SIZE
		{
			get;
			set;
		}

		public string IMAGE_THUMBNAIL
		{
			get;
			set;
		}

		public int? IMAGE_TYPE
		{
			get;
			set;
		}

		public int? IMAGE_WIDTH
		{
			get;
			set;
		}

		public string PICTURE_TYPE
		{
			get;
			set;
		}

		public DAYTRIP_IMAGES()
		{
		}
	}
}