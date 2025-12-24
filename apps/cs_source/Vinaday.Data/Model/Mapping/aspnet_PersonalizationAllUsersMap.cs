using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class aspnet_PersonalizationAllUsersMap : EntityTypeConfiguration<aspnet_PersonalizationAllUsers>
	{
		public aspnet_PersonalizationAllUsersMap()
		{
			HasKey(t => t.PathId);
			Property(t => t.PageSettings).IsRequired();
			ToTable("aspnet_PersonalizationAllUsers");
			Property(t => t.PathId).HasColumnName("PathId");
			Property(t => t.PageSettings).HasColumnName("PageSettings");
			Property(t => t.LastUpdatedDate).HasColumnName("LastUpdatedDate");
			HasRequired(t => t.aspnet_Paths).WithOptional(t => t.aspnet_PersonalizationAllUsers);
		}
	}
}