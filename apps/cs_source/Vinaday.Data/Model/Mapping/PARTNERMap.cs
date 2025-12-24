using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class PARTNERMap : EntityTypeConfiguration<PARTNER>
	{
		public PARTNERMap()
		{
			HasKey(t => t.PARNERID);
			Property(t => t.NAME).IsFixedLength().HasMaxLength(250);
			ToTable("PARTNER");
			Property(t => t.PARNERID).HasColumnName("PARNERID");
			Property(t => t.NAME).HasColumnName("NAME");
			Property(t => t.DESCRIPTION).HasColumnName("DESCRIPTION");
		}
	}
}