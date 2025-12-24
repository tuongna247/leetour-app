using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class vw_aspnet_WebPartState_UserMap : EntityTypeConfiguration<vw_aspnet_WebPartState_User>
	{
		public vw_aspnet_WebPartState_UserMap()
		{
			base.HasKey<DateTime>((vw_aspnet_WebPartState_User t) => t.LastUpdatedDate);
			base.ToTable("vw_aspnet_WebPartState_User");
			base.Property<Guid>((vw_aspnet_WebPartState_User t) => t.PathId).HasColumnName("PathId");
			base.Property<Guid>((vw_aspnet_WebPartState_User t) => t.UserId).HasColumnName("UserId");
			base.Property<int>((vw_aspnet_WebPartState_User t) => t.DataSize).HasColumnName("DataSize");
			base.Property((vw_aspnet_WebPartState_User t) => t.LastUpdatedDate).HasColumnName("LastUpdatedDate");
		}
	}
}