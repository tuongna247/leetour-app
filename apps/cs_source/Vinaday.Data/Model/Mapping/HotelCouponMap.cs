using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
    public class HotelCouponMap : EntityTypeConfiguration<HotelCoupon>
    {
        public HotelCouponMap()
        {
            HasKey(t => t.Id);
            ToTable("HotelCoupon");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.Description).HasColumnName("Description");
            Property(t => t.Discount).HasColumnName("Discount");
            Property(t => t.EndDate).HasColumnName("EndDate");
            Property(t => t.CodePromo).HasColumnName("CodePromo");
            Property(t => t.ConditionUsing).HasColumnName("ConditionUsing");
            Property(t => t.HotelId).HasColumnName("HotelId");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.Price).HasColumnName("Price");
            Property(t => t.SalesQty).HasColumnName("SalesQty");
            Property(t => t.StartDate).HasColumnName("StartDate");
            Property(t => t.TotalQty).HasColumnName("TotalQty");
            Property(t => t.HotelStart).HasColumnName("HotelStart");
            Property(t => t.HotelId).HasColumnName("HotelId");
        }
    }
}