using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class CancellationPolicyMap : EntityTypeConfiguration<CancellationPolicy>
	{
		public CancellationPolicyMap()
		{
			HasKey(t => t.Id);
			Property(t => t.Code).IsRequired().HasMaxLength(30);
			Property(t => t.Name).IsRequired().HasMaxLength(250);
			ToTable("CancellationPolicy");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.Code).HasColumnName("Code");
			Property(t => t.Name).HasColumnName("Name");
			Property(t => t.Description).HasColumnName("Description");
			Property(t => t.DescriptionVn).HasColumnName("DescriptionVN");
			Property(t => t.Status).HasColumnName("Status");
		}
	}
}