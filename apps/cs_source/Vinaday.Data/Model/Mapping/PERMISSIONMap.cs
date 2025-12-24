using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class PERMISSIONMap : EntityTypeConfiguration<PERMISSION>
	{
		public PERMISSIONMap()
		{
			HasKey(t => t.PERMISSIONID);
			ToTable("PERMISSION");
			Property(t => t.PERMISSIONID).HasColumnName("PERMISSIONID");
			Property(t => t.ROLEID).HasColumnName("ROLEID");
			Property(t => t.NAME).HasColumnName("NAME");
			Property(t => t.DESCRIPTION).HasColumnName("DESCRIPTION");
			HasOptional(t => t.ROLE).WithMany(t => t.PERMISSIONs).HasForeignKey(d => d.ROLEID);
		}
	}
}