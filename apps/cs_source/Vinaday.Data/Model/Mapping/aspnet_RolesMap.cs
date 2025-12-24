using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class aspnet_RolesMap : EntityTypeConfiguration<aspnet_Roles>
	{
		public aspnet_RolesMap()
		{
			HasKey(t => t.RoleId);
			Property(t => t.RoleName).IsRequired().HasMaxLength(256);
			Property(t => t.LoweredRoleName).IsRequired().HasMaxLength(256);
			Property(t => t.Description).HasMaxLength(256);
			ToTable("aspnet_Roles");
			Property(t => t.ApplicationId).HasColumnName("ApplicationId");
			Property(t => t.RoleId).HasColumnName("RoleId");
			Property(t => t.RoleName).HasColumnName("RoleName");
			Property(t => t.LoweredRoleName).HasColumnName("LoweredRoleName");
			Property(t => t.Description).HasColumnName("Description");
			HasMany(t => t.aspnet_Users).WithMany(t => t.aspnet_Roles).Map(m => {
				m.ToTable("aspnet_UsersInRoles");
				m.MapLeftKey(new string[] { "RoleId" });
				m.MapRightKey(new string[] { "UserId" });
			});
			HasRequired(t => t.aspnet_Applications).WithMany(t => t.aspnet_Roles).HasForeignKey(d => d.ApplicationId);
		}
	}
}