using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class LottoMap : EntityTypeConfiguration<Lotto>
	{
		public LottoMap()
		{
			HasKey(t => t.Id);
			Property(t => t.Name).HasMaxLength(250);
			Property(t => t.Type);
			Property(t => t.HashLink).HasMaxLength(250);
			Property(t => t.ImageLink).HasMaxLength(250);
			Property(t => t.Status);
			Property(t => t.Total);
			Property(t => t.Created);
			ToTable("Lotto");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.Name).HasColumnName("Name");
			Property(t => t.Type).HasColumnName("Type");
			Property(t => t.HashLink).HasColumnName("HashLink");
			Property(t => t.ImageLink).HasColumnName("ImageLink");
			Property(t => t.Status).HasColumnName("Status");
			Property(t => t.Total).HasColumnName("Total");
			Property(t => t.Created).HasColumnName("Created");
		}
	}
}