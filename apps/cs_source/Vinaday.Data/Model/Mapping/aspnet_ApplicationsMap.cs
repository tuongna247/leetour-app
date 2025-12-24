using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class aspnet_ApplicationsMap : EntityTypeConfiguration<aspnet_Applications>
	{
		public aspnet_ApplicationsMap()
		{
			HasKey(t => t.ApplicationId);
			Property(t => t.ApplicationName).IsRequired().HasMaxLength(256);
			Property(t => t.LoweredApplicationName).IsRequired().HasMaxLength(256);
			Property(t => t.Description).HasMaxLength(256);
			ToTable("aspnet_Applications");
			Property(t => t.ApplicationName).HasColumnName("ApplicationName");
			Property(t => t.LoweredApplicationName).HasColumnName("LoweredApplicationName");
			Property(t => t.ApplicationId).HasColumnName("ApplicationId");
			Property(t => t.Description).HasColumnName("Description");
		}
	}
}