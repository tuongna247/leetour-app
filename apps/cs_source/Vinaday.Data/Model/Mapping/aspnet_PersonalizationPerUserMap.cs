using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class aspnet_PersonalizationPerUserMap : EntityTypeConfiguration<aspnet_PersonalizationPerUser>
	{
		public aspnet_PersonalizationPerUserMap()
		{
			HasKey(t => t.Id);
			Property(t => t.PageSettings).IsRequired();
			ToTable("aspnet_PersonalizationPerUser");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.PathId).HasColumnName("PathId");
			Property(t => t.UserId).HasColumnName("UserId");
			Property(t => t.PageSettings).HasColumnName("PageSettings");
			Property(t => t.LastUpdatedDate).HasColumnName("LastUpdatedDate");
			HasOptional(t => t.aspnet_Paths).WithMany(t => t.aspnet_PersonalizationPerUser).HasForeignKey(d => d.PathId);
			HasOptional(t => t.aspnet_Users).WithMany(t => t.aspnet_PersonalizationPerUser).HasForeignKey(d => d.UserId);
		}
	}
}