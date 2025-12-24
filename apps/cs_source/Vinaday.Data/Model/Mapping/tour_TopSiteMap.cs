using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class tour_TopSiteMap : EntityTypeConfiguration<TourTopSite>
	{
		public tour_TopSiteMap()
		{
			HasKey(t => t.Id);
			ToTable("tour_Topten");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.TourId).HasColumnName("TourId");
			Property(t => t.ImageUrl).HasColumnName("ImageUrl");
			Property(t => t.Priority).HasColumnName("Priority");
			Property(t => t.CreatedDate).HasColumnName("CreatedDate");
			Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
			HasRequired(t => t.Tour).WithMany(t => t.TourTopSites).HasForeignKey(d => d.TourId);
		}
	}
}