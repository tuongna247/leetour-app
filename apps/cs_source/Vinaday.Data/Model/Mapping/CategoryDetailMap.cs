using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class CategoryDetailMap : EntityTypeConfiguration<CategoryDetail>
	{
		public CategoryDetailMap()
		{
			HasKey(t => t.Id);
			Property(t => t.Name).IsRequired().HasMaxLength(250);
			Property(t => t.Description).IsRequired().HasMaxLength(250);
			Property(t => t.DescriptionVn).IsRequired().HasMaxLength(250);
			ToTable("CategoryDetails");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.CategoryId).HasColumnName("CategoryId");
			Property(t => t.Name).HasColumnName("Name");
			Property(t => t.Description).HasColumnName("Description");
			Property(t => t.DescriptionVn).HasColumnName("DescriptionVn");
			Property(t => t.Status).HasColumnName("Status");
			Property(t => t.CreatedDate).HasColumnName("CreatedDate");
			Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
			HasRequired(t => t.Category).WithMany(t => t.CategoryDetails).HasForeignKey(d => d.CategoryId);
		}
	}
}