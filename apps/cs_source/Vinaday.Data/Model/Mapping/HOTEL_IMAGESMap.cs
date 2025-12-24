using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class HOTEL_IMAGESMap : EntityTypeConfiguration<HotelImages>
	{
		public HOTEL_IMAGESMap()
		{
			HasKey(t => t.Id);
			Property(t => t.ImageOrigin).IsRequired().HasMaxLength(250);
			Property(t => t.ImageThumbnail).HasMaxLength(250);
			Property(t => t.ImageName).HasMaxLength(250);
			Property(t => t.EnglishTitle).HasMaxLength(250);
			Property(t => t.PictureType).HasMaxLength(50);
			ToTable("HOTEL_IMAGES");
			Property(t => t.Id).HasColumnName("ID");
			Property(t => t.HotelId).HasColumnName("HOTEL_ID");
			Property(t => t.ImageOrigin).HasColumnName("IMAGE_ORIGIN");
			Property(t => t.ImageThumbnail).HasColumnName("IMAGE_THUMBNAIL");
			Property(t => t.ImageType).HasColumnName("IMAGE_TYPE");
			Property(t => t.ImageName).HasColumnName("IMAGE_NAME");
			Property(t => t.ImageWidth).HasColumnName("IMAGE_WIDTH");
			Property(t => t.ImageHeight).HasColumnName("IMAGE_HEIGHT");
			Property(t => t.ImageSize).HasColumnName("IMAGE_SIZE");
			Property(t => t.ImageQuanlity).HasColumnName("IMAGE_QUANLITY");
			Property(t => t.EnglishTitle).HasColumnName("English_Title");
			Property(t => t.PictureType).HasColumnName("PICTURE_TYPE");
			HasRequired(t => t.Hotel).WithMany(t => t.HotelImages).HasForeignKey(d => d.HotelId);
		}
	}
}