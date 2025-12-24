using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class aspnet_ProfileMap : EntityTypeConfiguration<aspnet_Profile>
	{
		public aspnet_ProfileMap()
		{
			HasKey(t => t.UserId);
			Property(t => t.PropertyNames).IsRequired();
			Property(t => t.PropertyValuesString).IsRequired();
			Property(t => t.PropertyValuesBinary).IsRequired();
			ToTable("aspnet_Profile");
			Property(t => t.UserId).HasColumnName("UserId");
			Property(t => t.PropertyNames).HasColumnName("PropertyNames");
			Property(t => t.PropertyValuesString).HasColumnName("PropertyValuesString");
			Property(t => t.PropertyValuesBinary).HasColumnName("PropertyValuesBinary");
			Property(t => t.LastUpdatedDate).HasColumnName("LastUpdatedDate");
			HasRequired(t => t.aspnet_Users).WithOptional(t => t.aspnet_Profile);
		}
	}
}