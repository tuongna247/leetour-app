using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class sysdiagramMap : EntityTypeConfiguration<sysdiagram>
	{
		public sysdiagramMap()
		{
			HasKey(t => t.diagram_id);
			Property(t => t.name).IsRequired().HasMaxLength(128);
			ToTable("sysdiagrams");
			Property(t => t.name).HasColumnName("name");
			Property(t => t.principal_id).HasColumnName("principal_id");
			Property(t => t.diagram_id).HasColumnName("diagram_id");
			Property(t => t.version).HasColumnName("version");
			Property(t => t.definition).HasColumnName("definition");
		}
	}
}