using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class DAYTRIP_IMAGESMap : EntityTypeConfiguration<DAYTRIP_IMAGES>
	{
		public DAYTRIP_IMAGESMap()
		{
			HasKey(t => t.ID);
			Property(t => t.IMAGE_ORIGIN).IsRequired().HasMaxLength(250);
			Property(t => t.IMAGE_THUMBNAIL).HasMaxLength(250);
			Property(t => t.IMAGE_NAME).HasMaxLength(250);
			Property(t => t.English_Title).HasMaxLength(50);
			Property(t => t.PICTURE_TYPE).HasMaxLength(50);
			ToTable("DAYTRIP_IMAGES");
			Property(t => t.ID).HasColumnName("ID");
			Property(t => t.DAYTRIP_ID).HasColumnName("DAYTRIP_ID");
			Property(t => t.IMAGE_ORIGIN).HasColumnName("IMAGE_ORIGIN");
			Property(t => t.IMAGE_THUMBNAIL).HasColumnName("IMAGE_THUMBNAIL");
			Property(t => t.IMAGE_TYPE).HasColumnName("IMAGE_TYPE");
			Property(t => t.IMAGE_NAME).HasColumnName("IMAGE_NAME");
			Property(t => t.IMAGE_WIDTH).HasColumnName("IMAGE_WIDTH");
			Property(t => t.IMAGE_HEIGHT).HasColumnName("IMAGE_HEIGHT");
			Property(t => t.IMAGE_SIZE).HasColumnName("IMAGE_SIZE");
			Property(t => t.IMAGE_QUANLITY).HasColumnName("IMAGE_QUANLITY");
			Property(t => t.English_Title).HasColumnName("English_Title");
			Property(t => t.PICTURE_TYPE).HasColumnName("PICTURE_TYPE");
			HasRequired(t => t.DAYTRIP).WithMany(t => t.DAYTRIP_IMAGES).HasForeignKey(d => d.DAYTRIP_ID);
		}
	}
}