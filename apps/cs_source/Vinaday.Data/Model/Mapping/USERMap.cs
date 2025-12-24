using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class USERMap : EntityTypeConfiguration<USER>
	{
		public USERMap()
		{
			HasKey(t => t.USERID);
			Property(t => t.USERNAME).IsRequired().HasMaxLength(250);
			Property(t => t.PASSWORD).IsRequired().HasMaxLength(250);
			Property(t => t.DESCRIPTION).HasMaxLength(250);
			ToTable("USER");
			Property(t => t.USERID).HasColumnName("USERID");
			Property(t => t.USERNAME).HasColumnName("USERNAME");
			Property(t => t.PASSWORD).HasColumnName("PASSWORD");
			Property(t => t.DESCRIPTION).HasColumnName("DESCRIPTION");
			Property(t => t.ROLEID).HasColumnName("ROLEID");
			Property(t => t.HotelID).HasColumnName("HotelID");
			HasOptional(t => t.ROLE).WithMany(t => t.USERs).HasForeignKey(d => d.ROLEID);
		}
	}
}