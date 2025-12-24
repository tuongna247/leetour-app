using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class SeoMap : EntityTypeConfiguration<Seo>
	{
		public SeoMap()
		{
			HasKey(t => t.Id);
			Property(t => t.EntityName).IsRequired().HasMaxLength(400);
			Property(t => t.Slug).IsRequired().HasMaxLength(400);
			Property(t => t.Keyword).HasMaxLength(1000);
			Property(t => t.Title).HasMaxLength(200);
			Property(t => t.Description).HasMaxLength(1000);
			Property(t => t.KeywordVn).HasMaxLength(1000);
			Property(t => t.TitleVn).HasMaxLength(1000);
			Property(t => t.DescriptionVn).HasMaxLength(1000);
			ToTable("Seo");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.EntityId).HasColumnName("EntityId");
			Property(t => t.EntityName).HasColumnName("EntityName");
			Property(t => t.Slug).HasColumnName("Slug");
			Property(t => t.Keyword).HasColumnName("Keyword");
			Property(t => t.Title).HasColumnName("Title");
			Property(t => t.Description).HasColumnName("Description");
			Property(t => t.KeywordVn).HasColumnName("KeywordVN");
			Property(t => t.TitleVn).HasColumnName("TitleVn");
			Property(t => t.DescriptionVn).HasColumnName("DescriptionVn");
			Property(t => t.IsActive).HasColumnName("IsActive");
			Property(t => t.ProductType).HasColumnName("ProductType");
		}
	}
}