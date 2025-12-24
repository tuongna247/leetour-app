using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class MediumMap : EntityTypeConfiguration<Medium>
	{
		public MediumMap()
		{
			HasKey(t => t.Id);
			Property(t => t.Type).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			Property(t => t.MediaType).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			Property(t => t.OwnerId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			Property(t => t.OriginalPath).IsRequired().HasMaxLength(1000);
			Property(t => t.ThumbnailPath).HasMaxLength(1000);
			Property(t => t.Title).HasMaxLength(300);
			Property(t => t.Description).HasMaxLength(400);
			Property(t => t.AlternateText).HasMaxLength(300);
			Property(t => t.Status).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			ToTable("Media");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.Type).HasColumnName("Type");
			Property(t => t.MediaType).HasColumnName("MediaType");
			Property(t => t.OwnerId).HasColumnName("OwnerId");
			Property(t => t.OriginalPath).HasColumnName("OriginalPath");
			Property(t => t.ThumbnailPath).HasColumnName("ThumbnailPath");
			Property(t => t.Title).HasColumnName("Title");
			Property(t => t.Description).HasColumnName("Description");
			Property(t => t.AlternateText).HasColumnName("AlternateText");
			Property(t => t.Status).HasColumnName("Status");
			Property(t => t.CreatedDate).HasColumnName("CreatedDate");
			Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
		}
	}
}