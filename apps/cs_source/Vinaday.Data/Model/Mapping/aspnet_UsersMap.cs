using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class aspnet_UsersMap : EntityTypeConfiguration<aspnet_Users>
	{
		public aspnet_UsersMap()
		{
			HasKey(t => t.UserId);
			Property(t => t.UserName).IsRequired().HasMaxLength(256);
			Property(t => t.LoweredUserName).IsRequired().HasMaxLength(256);
			Property(t => t.MobileAlias).HasMaxLength(16);
			ToTable("aspnet_Users");
			Property(t => t.ApplicationId).HasColumnName("ApplicationId");
			Property(t => t.UserId).HasColumnName("UserId");
			Property(t => t.UserName).HasColumnName("UserName");
			Property(t => t.LoweredUserName).HasColumnName("LoweredUserName");
			Property(t => t.MobileAlias).HasColumnName("MobileAlias");
			Property(t => t.IsAnonymous).HasColumnName("IsAnonymous");
			Property(t => t.LastActivityDate).HasColumnName("LastActivityDate");
			HasRequired(t => t.aspnet_Applications).WithMany(t => t.aspnet_Users).HasForeignKey(d => d.ApplicationId);
		}
	}
}