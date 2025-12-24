using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class COUNTRYMap : EntityTypeConfiguration<Country>
	{
		public COUNTRYMap()
		{
			HasKey(t => t.CountryId);
			Property(t => t.Name).IsRequired().HasMaxLength(250);
			Property(t => t.ShortName).HasMaxLength(250);
			Property(t => t.ImageUrl).HasMaxLength(250);
			Property(t => t.Capital).HasMaxLength(250);
			Property(t => t.Description).HasMaxLength(250);
			Property(t => t.Url).HasMaxLength(250);
			Property(t => t.SeoMeta).HasMaxLength(2000);
			Property(t => t.SeoKeyword).HasMaxLength(2000);
			Property(t => t.SeoDescription).HasMaxLength(2000);
			Property(t => t.SeoTitle).HasMaxLength(1000);
			Property(t => t.DaytripUrl).HasMaxLength(250);
			Property(t => t.TourUrl).HasMaxLength(250);
			Property(t => t.TourDescription).HasMaxLength(4000);
			Property(t => t.DaytripDescription).HasMaxLength(4000);
			Property(t => t.TourImage).HasMaxLength(4000);
			Property(t => t.DaytripImage).HasMaxLength(4000);
			Property(t => t.DaytripSeoTitle).HasMaxLength(250);
			Property(t => t.TourSeoTitle).HasMaxLength(250);
			Property(t => t.VietnamSeoTitle).HasMaxLength(250);
			Property(t => t.NameVn).HasMaxLength(250);
			ToTable("COUNTRY");
			Property(t => t.CountryId).HasColumnName("COUNTRYID");
			Property(t => t.Name).HasColumnName("NAME");
			Property(t => t.ShortName).HasColumnName("SHORTNAME");
			Property(t => t.ImageUrl).HasColumnName("IMAGEURL");
			Property(t => t.Status).HasColumnName("STATUS");
			Property(t => t.Capital).HasColumnName("CAPITAL");
			Property(t => t.Description).HasColumnName("DESCRIPTION");
			Property(t => t.Hotels).HasColumnName("Hotels");
			Property(t => t.Tours).HasColumnName("Tours");
			Property(t => t.DayTrips).HasColumnName("DayTrips");
			Property(t => t.Url).HasColumnName("URL");
			Property(t => t.SeoMeta).HasColumnName("SEO_Meta");
			Property(t => t.SeoKeyword).HasColumnName("SEO_Keyword");
			Property(t => t.SeoDescription).HasColumnName("SEO_Description");
			Property(t => t.SeoTitle).HasColumnName("Seo_Title");
			Property(t => t.DaytripUrl).HasColumnName("DAYTRIP_URL");
			Property(t => t.TourUrl).HasColumnName("TOUR_URL");
			Property(t => t.TourDescription).HasColumnName("Tour_Description");
			Property(t => t.DaytripDescription).HasColumnName("Daytrip_Description");
			Property(t => t.TourImage).HasColumnName("Tour_Image");
			Property(t => t.DaytripImage).HasColumnName("Daytrip_Image");
			Property(t => t.DaytripSeoTitle).HasColumnName("daytrip_seo_title");
			Property(t => t.TourSeoTitle).HasColumnName("tour_seo_title");
			Property(t => t.VietnamSeoTitle).HasColumnName("vietnam_seo_title");
			Property(t => t.NameVn).HasColumnName("NameVN");
		}
	}
}