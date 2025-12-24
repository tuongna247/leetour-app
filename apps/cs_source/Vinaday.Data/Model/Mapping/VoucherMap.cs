using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class VoucherMap : EntityTypeConfiguration<Voucher>
	{
		public VoucherMap()
		{
			HasKey(t => t.Id);
			ToTable("Voucher");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.Name).HasColumnName("Name");
			Property(t => t.Description).HasColumnName("Description");
			Property(t => t.Localtion).HasColumnName("Localtion");
			Property(t => t.Quantity).HasColumnName("Quantity");
			Property(t => t.Extra).HasColumnName("Extra");
			Property(t => t.Type).HasColumnName("Type");
			Property(t => t.Adult).HasColumnName("Adult");
			Property(t => t.Children).HasColumnName("Children");
			Property(t => t.Meal).HasColumnName("Meal");
			Property(t => t.CheckIn).HasColumnName("CheckIn");
			Property(t => t.CheckOut).HasColumnName("CheckOut");
			Property(t => t.Guest).HasColumnName("Guest");
            Property(t => t.HotelPhone).HasColumnName("HotelPhone");
            
            Property(t => t.Nationality).HasColumnName("Nationality");
			Property(t => t.Cancellation).HasColumnName("Cancellation");
			//Property(t => t.Address).HasColumnName("Address");
			//Property(t => t.RoomType).HasColumnName("RoomType");
			Property(t => t.Promotion).HasColumnName("Promotion");
			Property(t => t.BookingId).HasColumnName("BookingId");
		}
	}
}