using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class tour_DetailsMap : EntityTypeConfiguration<Detail>
	{
		public tour_DetailsMap()
		{
			HasKey(t => t.Id);
			Property(t => t.Itininary).HasMaxLength(250);
			Property(t => t.Meal).HasMaxLength(250);
            Property(t => t.OverNight).HasMaxLength(250);
            Property(t => t.Transport).HasMaxLength(250);
			Property(t => t.ImageId).HasMaxLength(300);
			ToTable("tour_Details");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.TourId).HasColumnName("TourId");
			Property(t => t.Itininary).HasColumnName("Itininary");
			Property(t => t.Content).HasColumnName("Content");
			Property(t => t.Meal).HasColumnName("Meal");
			Property(t => t.ImageDetail).HasColumnName("ImageDetail");
			Property(t => t.ImageDetailDescription).HasColumnName("ImageDetailDescription");
			Property(t => t.Transport).HasColumnName("Transport");
			Property(t => t.ImageId).HasColumnName("ImageId");
			Property(t => t.CreatedDate).HasColumnName("CreatedDate");
			Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
			Property(t => t.SortOrder).HasColumnName("SortOrder");
            Property(t => t.OverNight).HasColumnName("OverNight");
            Property(t => t.Status).HasColumnName("Status");
			HasOptional(t => t.Tour).WithMany(t => t.Details).HasForeignKey(d => d.TourId);
		}
	}
}