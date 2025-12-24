using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class CATEGORY1Map : EntityTypeConfiguration<Category1>
	{
		public CATEGORY1Map()
		{
			HasKey(t => t.ID);
			Property(t => t.NAME).HasMaxLength(250);
			Property(t => t.DESCRIPTION).HasMaxLength(250);
			ToTable("CATEGORY");
			Property(t => t.ID).HasColumnName("ID");
			Property(t => t.NAME).HasColumnName("NAME");
			Property(t => t.STATUS).HasColumnName("STATUS");
			Property(t => t.DESCRIPTION).HasColumnName("DESCRIPTION");
		}
	}
}