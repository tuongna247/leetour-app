using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class CityMap : EntityTypeConfiguration<City>
	{
		public CityMap()
		{
			HasKey(t => t.CityId);
			Property(t => t.Name).IsRequired().HasMaxLength(250);
			Property(t => t.Description).HasMaxLength(250);
			Property(t => t.ImageURL).HasMaxLength(250);
			Property(t => t.URL).HasMaxLength(250);
			Property(t => t.SEO_Meta).HasMaxLength(2000);
			Property(t => t.SEO_Keyword).HasMaxLength(2000);
			Property(t => t.SEO_Description).HasMaxLength(2000);
			Property(t => t.Seo_Title).HasMaxLength(1000);
			Property(t => t.vn_url).HasMaxLength(250);
			ToTable("CITY");
			Property(t => t.CityId).HasColumnName("CityID");
			Property(t => t.CountryId).HasColumnName("CountryID");
			Property(t => t.Name).HasColumnName("NAME");
			Property(t => t.Description).HasColumnName("DESCRIPTION");
			Property(t => t.Longtitude).HasColumnName("Longtitude");
			Property(t => t.Latitude).HasColumnName("Latitude");
			Property(t => t.Hotels).HasColumnName("Hotels");
			Property(t => t.Tours).HasColumnName("Tours");
			Property(t => t.DayTrips).HasColumnName("DayTrips");
			Property(t => t.HotelTrends).HasColumnName("HotelTrends");
			Property(t => t.TourTrend).HasColumnName("TourTrend");
			Property(t => t.DayTripTrend).HasColumnName("DayTripTrend");
			Property(t => t.ImageURL).HasColumnName("ImageURL");
			Property(t => t.URL).HasColumnName("URL");
			Property(t => t.SEO_Meta).HasColumnName("SEO_Meta");
			Property(t => t.SEO_Keyword).HasColumnName("SEO_Keyword");
			Property(t => t.SEO_Description).HasColumnName("SEO_Description");
			Property(t => t.Seo_Title).HasColumnName("Seo_Title");
			Property(t => t.vn_url).HasColumnName("vn_url");
			Property(t => t.Status).HasColumnName("Status");
			Property(t => t.SEO_Keyword_VN).HasColumnName("SEO_Keyword_VN");
			Property(t => t.SEO_Description_VN).HasColumnName("SEO_Description_VN");
			Property(t => t.SEO_Title_VN).HasColumnName("SEO_Title_VN");
			Property(t => t.Priority).HasColumnName("Priority");
		}
	}
}