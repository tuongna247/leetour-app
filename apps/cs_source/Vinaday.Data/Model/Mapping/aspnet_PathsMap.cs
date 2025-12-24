using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class aspnet_PathsMap : EntityTypeConfiguration<aspnet_Paths>
	{
		public aspnet_PathsMap()
		{
			HasKey(t => t.PathId);
			Property(t => t.Path).IsRequired().HasMaxLength(256);
			Property(t => t.LoweredPath).IsRequired().HasMaxLength(256);
			ToTable("aspnet_Paths");
			Property(t => t.ApplicationId).HasColumnName("ApplicationId");
			Property(t => t.PathId).HasColumnName("PathId");
			Property(t => t.Path).HasColumnName("Path");
			Property(t => t.LoweredPath).HasColumnName("LoweredPath");
			HasRequired(t => t.aspnet_Applications).WithMany(t => t.aspnet_Paths).HasForeignKey(d => d.ApplicationId);
		}
	}
}