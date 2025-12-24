using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class HOTELPROMOTIONMap : EntityTypeConfiguration<HOTELPROMOTION>
	{
		public HOTELPROMOTIONMap()
		{
			HasKey(t => t.HOTELPROMOTIONID);
			Property(t => t.CANCELPOLICYVALUE1).HasMaxLength(500);
			Property(t => t.CANCELPOLICYVALUE2).HasMaxLength(500);
			Property(t => t.TITLE).HasMaxLength(50);
			Property(t => t.CANCELPOLICYVALUE1_VN).HasMaxLength(500);
			Property(t => t.CANCELPOLICYVALUE2_VN).HasMaxLength(500);
			ToTable("HOTELPROMOTION");
			Property(t => t.HOTELPROMOTIONID).HasColumnName("HOTELPROMOTIONID");
			Property(t => t.HOTELDETAILID).HasColumnName("HOTELDETAILID");
			Property(t => t.DESCRIPTION).HasColumnName("DESCRIPTION");
			Property(t => t.STARTDATE).HasColumnName("STARTDATE");
			Property(t => t.ENDDATE).HasColumnName("ENDDATE");
			Property(t => t.STATUS).HasColumnName("STATUS");
			Property(t => t.BOOKINGSTARTDATE).HasColumnName("BOOKINGSTARTDATE");
			Property(t => t.BOOKINGENDDATE).HasColumnName("BOOKINGENDDATE");
			Property(t => t.CANCELPOLICY).HasColumnName("CANCELPOLICY");
			Property(t => t.CANCELPOLICYVALUE1).HasColumnName("CANCELPOLICYVALUE1");
			Property(t => t.PROMOTIONSTAYPAYID).HasColumnName("PROMOTIONSTAYPAYID");
			Property(t => t.FROMDATE).HasColumnName("FROMDATE");
			Property(t => t.TODATE).HasColumnName("TODATE");
			Property(t => t.CANCELPOLICYVALUE2).HasColumnName("CANCELPOLICYVALUE2");
			Property(t => t.TITLE).HasColumnName("TITLE");
			Property(t => t.TA_RATE).HasColumnName("TA_RATE");
			Property(t => t.SELLING_TA).HasColumnName("SELLING_TA");
			Property(t => t.SELL_RATE).HasColumnName("SELL_RATE");
			Property(t => t.CANCELPOLICYVALUE1_VN).HasColumnName("CANCELPOLICYVALUE1_VN");
			Property(t => t.CANCELPOLICYVALUE2_VN).HasColumnName("CANCELPOLICYVALUE2_VN");
			Property(t => t.PROMOTIONTYPE).HasColumnName("PROMOTIONTYPE");
			Property(t => t.IsVietNamHotel).HasColumnName("IsVietNamHotel");
			HasOptional(t => t.HOTELDETAIL).WithMany(t => t.HotelPromotions).HasForeignKey(d => d.HOTELDETAILID);
			HasOptional(t => t.PROMOTIONSTAYPAY).WithMany(t => t.HOTELPROMOTIONs).HasForeignKey(d => d.PROMOTIONSTAYPAYID);
		}
	}
}