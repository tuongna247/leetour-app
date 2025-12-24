using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class HotelImages : Entity
	{
		public string EnglishTitle
		{
			get;
			set;
		}

		public virtual Vinaday.Data.Models.Hotel Hotel
		{
			get;
			set;
		}

		public int HotelId
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public int? ImageHeight
		{
			get;
			set;
		}

		public string ImageName
		{
			get;
			set;
		}

		public string ImageOrigin
		{
			get;
			set;
		}

		public int? ImageQuanlity
		{
			get;
			set;
		}

		public double? ImageSize
		{
			get;
			set;
		}

		public string ImageThumbnail
		{
			get;
			set;
		}

		public int? ImageType
		{
			get;
			set;
		}

		public int? ImageWidth
		{
			get;
			set;
		}

		public string PictureType
		{
			get;
			set;
		}

        public string Description
        {
            get;
            set;
        }

        public HotelImages()
		{
		}
	}
}