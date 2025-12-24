using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class RegionMap : EntityTypeConfiguration<Region>
	{
		public RegionMap()
		{
			HasKey(t => t.id);
			Property(t => t.Name).HasMaxLength(100);
			Property(t => t.Description).HasMaxLength(100);
			Property(t => t.SEO_Title).HasMaxLength(2000);
			Property(t => t.SEO_Description).HasMaxLength(2000);
			Property(t => t.SEO_Keyword).HasMaxLength(2000);
			Property(t => t.URL).HasMaxLength(500);
			Property(t => t.ImageUrl).HasMaxLength(500);
			Property(t => t.DayTrip_Url).HasMaxLength(500);
			ToTable("Region");
			Property(t => t.id).HasColumnName("id");
			Property(t => t.Name).HasColumnName("Name");
			Property(t => t.Description).HasColumnName("Description");
			Property(t => t.SEO_Title).HasColumnName("SEO_Title");
			Property(t => t.SEO_Description).HasColumnName("SEO_Description");
			Property(t => t.SEO_Keyword).HasColumnName("SEO_Keyword");
			Property(t => t.CountryId).HasColumnName("CountryId");
			Property(t => t.URL).HasColumnName("URL");
			Property(t => t.ImageUrl).HasColumnName("ImageUrl");
			Property(t => t.DayTrip_Url).HasColumnName("DayTrip_Url");
			HasOptional(t => t.COUNTRY).WithMany(t => t.Regions).HasForeignKey(d => d.CountryId);
		}
	}
}