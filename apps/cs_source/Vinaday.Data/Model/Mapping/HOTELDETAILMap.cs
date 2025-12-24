using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class HOTELDETAILMap : EntityTypeConfiguration<Room>
	{
		public HOTELDETAILMap()
		{
			HasKey(t => t.Id);
			Property(t => t.Name).HasMaxLength(250);
			Property(t => t.EnglishName).HasMaxLength(250);
			Property(t => t.LocalName).HasMaxLength(250);
			Property(t => t.ImageUrl).HasMaxLength(500);
			Property(t => t.CancelPolicy).HasMaxLength(500);
			Property(t => t.PromotionType).HasMaxLength(500);
			Property(t => t.BreakfastSurcharge).HasMaxLength(100);
			Property(t => t.View).HasMaxLength(100);
			ToTable("HOTELDETAIL");
			Property(t => t.Id).HasColumnName("HOTELDETAILID");
			Property(t => t.HotelId).HasColumnName("HOTELID");
			Property(t => t.Name).HasColumnName("NAME");
			Property(t => t.Status).HasColumnName("STATUS");
			Property(t => t.EnglishName).HasColumnName("ENGLISHNAME");
			Property(t => t.LocalName).HasColumnName("LOCALNAME");
			Property(t => t.MaxOccupancy).HasColumnName("MAXOCCUPANCY");
			Property(t => t.MaxExtrabed).HasColumnName("MAXEXTRABED");
			Property(t => t.SellingRate).HasColumnName("SELL_RATE");
			Property(t => t.AvailableRoom).HasColumnName("AVAILABLEROOM");
			Property(t => t.BreakfastInclude).HasColumnName("BREAKFASTINCLUDE");
			Property(t => t.ImageUrl).HasColumnName("IMAGE_URL");
			Property(t => t.TaRate).HasColumnName("TA_RATE");
			Property(t => t.RackRate).HasColumnName("RACK_RATE");
			Property(t => t.SellingTa).HasColumnName("SELLING_TA");
			Property(t => t.HotelPromotionId).HasColumnName("HOTELPROMOTIONID");
			Property(t => t.CancelPolicy).HasColumnName("CANCELPOLICY");
			Property(t => t.PromotionType).HasColumnName("PROMOTIONTYPE");
			Property(t => t.IsExpire).HasColumnName("IsExpire");
			Property(t => t.RoomFacilities).HasColumnName("RoomFacilities");
			Property(t => t.ExtraBed).HasColumnName("ExtraBed");
			Property(t => t.ExtraBedPrice).HasColumnName("ExtraBedPrice");
			Property(t => t.RoomSize).HasColumnName("RoomSize");
			Property(t => t.AdultNumber).HasColumnName("AdultNumber");
			Property(t => t.ChildrenNumber).HasColumnName("ChildrenNumber");
			Property(t => t.ChildrenAge).HasColumnName("ChildrenAge");
			Property(t => t.BreakfastSurcharge).HasColumnName("BreakfastSurcharge");
            Property(t => t.Sort).HasColumnName("Sort");
            Property(t => t.View).HasColumnName("View");
			HasOptional(t => t.Hotel).WithMany(t => t.HotelDetails).HasForeignKey(d => d.HotelId);
		}
	}
}