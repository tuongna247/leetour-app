using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class NationalityMap : EntityTypeConfiguration<Nationality>
	{
		public NationalityMap()
		{
			HasKey(t => t.ID);
			Property(t => t.Name).IsRequired().HasMaxLength(50);
			Property(t => t.Description).HasMaxLength(50);
			ToTable("Nationality");
			Property(t => t.ID).HasColumnName("ID");
			Property(t => t.Name).HasColumnName("Name");
			Property(t => t.Description).HasColumnName("Description");
			Property(t => t.Priority).HasColumnName("Priority");
		}
	}
}