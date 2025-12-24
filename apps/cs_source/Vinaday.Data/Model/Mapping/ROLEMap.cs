using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class ROLEMap : EntityTypeConfiguration<ROLE>
	{
		public ROLEMap()
		{
			HasKey(t => t.ROLEID);
			ToTable("ROLE");
			Property(t => t.ROLEID).HasColumnName("ROLEID");
			Property(t => t.NAME).HasColumnName("NAME");
			Property(t => t.DESCRIPTION).HasColumnName("DESCRIPTION");
		}
	}
}