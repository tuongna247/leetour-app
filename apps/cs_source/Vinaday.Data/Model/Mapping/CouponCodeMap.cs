using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class CouponCodeMap : EntityTypeConfiguration<CouponCode>
	{
		public CouponCodeMap()
		{
			HasKey(t => t.Id);
			ToTable("CouponCode");

            Property(t => t.Id).HasColumnName("Id");
			Property(t => t.Code).HasColumnName("Code");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.Description).HasColumnName("Description");
            Property(t => t.FromDate).HasColumnName("FromDate");
            Property(t => t.ToDate).HasColumnName("ToDate");
            Property(t => t.DiscountType).HasColumnName("DiscountType");
            Property(t => t.DiscountPercent).HasColumnName("DiscountPercent");
            Property(t => t.DiscountValue).HasColumnName("DiscountValue");

            Property(t => t.TourId).HasColumnName("TourId");
            Property(t => t.TourName).HasColumnName("TourName");
            Property(t => t.Person).HasColumnName("Person");
            Property(t => t.HotelId).HasColumnName("HotelId");
            Property(t => t.HotelName).HasColumnName("HotelName");
            Property(t => t.RoomId).HasColumnName("RoomId");
            Property(t => t.RoomName).HasColumnName("RoomName");
            Property(t => t.CityId).HasColumnName("CityId");
            Property(t => t.CityName).HasColumnName("CityName");

            Property(t => t.Email).HasColumnName("Email");
            Property(t => t.Phone).HasColumnName("Phone");

            Property(t => t.Type).HasColumnName("Type");
            Property(t => t.Quantity).HasColumnName("Quantity");
            Property(t => t.ApplyTour).HasColumnName("ApplyTour");
            Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
            Property(t => t.WID).HasColumnName("WID");
            Property(t => t.UID).HasColumnName("UID");
        }
	}
}