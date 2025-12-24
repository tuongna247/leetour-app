using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class CategoryMap : EntityTypeConfiguration<Category>
	{
		public CategoryMap()
		{
			HasKey(t => t.Id);
			Property(t => t.Name).IsRequired().HasMaxLength(250);
			Property(t => t.Description).IsRequired().HasMaxLength(250);
			Property(t => t.KeyCode).IsRequired().HasMaxLength(250);
			ToTable("Categories");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.Name).HasColumnName("Name");
			Property(t => t.Description).HasColumnName("Description");
			Property(t => t.KeyCode).HasColumnName("KeyCode");
			Property(t => t.Status).HasColumnName("Status");
			Property(t => t.CreatedDate).HasColumnName("CreatedDate");
			Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
		}
	}
}