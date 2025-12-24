using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class ExpiredRoomMap : EntityTypeConfiguration<ExpiredRoom>
	{
		public ExpiredRoomMap()
		{
			HasKey(t => t.ID);
			Property(t => t.DESCRIPTION).IsRequired().HasMaxLength(250);
			ToTable("ExpiredRoom");
			Property(t => t.ID).HasColumnName("ID");
			Property(t => t.FROMDATE).HasColumnName("FROMDATE");
			Property(t => t.TODATE).HasColumnName("TODATE");
			Property(t => t.DESCRIPTION).HasColumnName("DESCRIPTION");
			Property(t => t.STATUS).HasColumnName("STATUS");
			Property(t => t.HOTELDETAILID).HasColumnName("HOTELDETAILID");
			HasOptional(t => t.HOTELDETAIL).WithMany(t => t.ExpiredRooms).HasForeignKey(d => d.HOTELDETAILID);
		}
	}
}