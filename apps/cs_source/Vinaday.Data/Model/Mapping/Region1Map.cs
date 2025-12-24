using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class Region1Map : EntityTypeConfiguration<Region1>
	{
		public Region1Map()
		{
			HasKey(t => t.Id);
			Property(t => t.Name).HasMaxLength(500);
			Property(t => t.Slug).HasMaxLength(500);
			Property(t => t.ImageUrl).HasMaxLength(500);
			Property(t => t.NameVn).HasMaxLength(500);
			Property(t => t.SeoTitleVn).HasMaxLength(1000);
			Property(t => t.SeoDescriptionVn).HasMaxLength(1000);
			Property(t => t.SeoKeywordVn).HasMaxLength(1000);
			Property(t => t.SeoTitle).HasMaxLength(1000);
			Property(t => t.SeoDescription).HasMaxLength(1000);
			Property(t => t.SeoKeyword).HasMaxLength(1000);
			ToTable("Regions");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.Name).HasColumnName("Name");
			Property(t => t.Description).HasColumnName("Description");
			Property(t => t.Slug).HasColumnName("Slug");
			Property(t => t.ImageUrl).HasColumnName("ImageUrl");
			Property(t => t.Status).HasColumnName("Status");
			Property(t => t.Type).HasColumnName("Type");
			Property(t => t.CityId).HasColumnName("CityId");
			Property(t => t.NameVn).HasColumnName("NameVn");
			Property(t => t.DescriptionVn).HasColumnName("DescriptionVn");
			Property(t => t.SeoTitleVn).HasColumnName("SeoTitleVn");
			Property(t => t.SeoDescriptionVn).HasColumnName("SeoDescriptionVn");
			Property(t => t.SeoKeywordVn).HasColumnName("SeoKeywordVn");
			Property(t => t.SeoTitle).HasColumnName("SeoTitle");
			Property(t => t.SeoDescription).HasColumnName("SeoDescription");
			Property(t => t.SeoKeyword).HasColumnName("SeoKeyword");
		}
	}
}