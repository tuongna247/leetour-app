using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class HOTELFACILITYMap : EntityTypeConfiguration<HOTELFACILITY>
	{
		public HOTELFACILITYMap()
		{
			HasKey(t => t.ID);
			ToTable("HOTELFACILITY");
			Property(t => t.ID).HasColumnName("ID");
			Property(t => t.TYPE).HasColumnName("TYPE");
			Property(t => t.NAME).HasColumnName("NAME");
			Property(t => t.DESCRIPTION).HasColumnName("DESCRIPTION");
			Property(t => t.STATUS).HasColumnName("STATUS");
		}
	}
}