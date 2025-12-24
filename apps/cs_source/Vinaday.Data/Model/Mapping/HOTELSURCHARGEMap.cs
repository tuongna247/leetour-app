using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class HOTELSURCHARGEMap : EntityTypeConfiguration<HOTELSURCHARGE>
	{
		public HOTELSURCHARGEMap()
		{
			HasKey(t => t.ID);
			Property(t => t.DAYS).HasMaxLength(100);
			Property(t => t.DESCRIPTION).HasMaxLength(250);
			ToTable("HOTELSURCHARGE");
			Property(t => t.ID).HasColumnName("ID");
			Property(t => t.STARTDATE).HasColumnName("STARTDATE");
			Property(t => t.ENDDATE).HasColumnName("ENDDATE");
			Property(t => t.DAYS).HasColumnName("DAYS");
			Property(t => t.AMOUNT).HasColumnName("AMOUNT");
			Property(t => t.DESCRIPTION).HasColumnName("DESCRIPTION");
			Property(t => t.HOTELID).HasColumnName("HOTELID");
			Property(t => t.STATUS).HasColumnName("STATUS");
			Property(t => t.TAAMOUNT).HasColumnName("TAAMOUNT");
			Property(t => t.ISCompulsoryDinner).HasColumnName("ISCompulsoryDinner");
			HasOptional(t => t.Hotel).WithMany(t => t.HotelSurcharges).HasForeignKey(d => d.HOTELID);
		}
	}
}