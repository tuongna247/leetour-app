using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class BookingMap : EntityTypeConfiguration<Booking>
	{
		public BookingMap()
		{
			HasKey(t => t.Id);
			Property(t => t.Name).IsFixedLength().HasMaxLength(250);
			Property(t => t.IpLocation).HasMaxLength(50);
			Property(t => t.SpecialRequest).HasMaxLength(250);
			Property(t => t.GuestFirstName).HasMaxLength(250);
			Property(t => t.GuestLastName).HasMaxLength(250);
			Property(t => t.SurchargeName).HasMaxLength(100);
			Property(t => t.ReceiptId).HasMaxLength(20);
			Property(t => t.Pnr).IsFixedLength().HasMaxLength(20);
			ToTable("BOOKING");
			Property(t => t.Id).HasColumnName("BOOKINGID");
			Property(t => t.CustomerId).HasColumnName("CUSTOMERID");
			Property(t => t.Name).HasColumnName("NAME");
			Property(t => t.Description).HasColumnName("DESCRIPTION");
			Property(t => t.SendReceipt).HasColumnName("SENDRECEIPT");
			Property(t => t.RoomId).HasColumnName("HOTELDETAILID");
			Property(t => t.Night).HasColumnName("NIGHT");
			Property(t => t.CheckIn).HasColumnName("CHECK_IN");
			Property(t => t.CheckOut).HasColumnName("CHECK_OUT");
			Property(t => t.Rooms).HasColumnName("ROOMS");
			Property(t => t.RoomRate).HasColumnName("ROOM_RATE");
			Property(t => t.Surcharge).HasColumnName("SURCHARGE");
			Property(t => t.FeeTax).HasColumnName("FEE_TAX");
			Property(t => t.Total).HasColumnName("TOTAL");
			Property(t => t.PaymentType).HasColumnName("PaymentType");
			Property(t => t.PaymentStatus).HasColumnName("PaymentStatus");
			Property(t => t.IsRefund).HasColumnName("ISREFUND");
			Property(t => t.IpLocation).HasColumnName("IPLOCATION");
			Property(t => t.PromotionId).HasColumnName("PromotionId");
			Property(t => t.Date).HasColumnName("Date");
			Property(t => t.SendVoucher).HasColumnName("SENDVOUCHER");
			Property(t => t.AmenBooking).HasColumnName("AMENBOOKING");
			Property(t => t.EditBy).HasColumnName("EDITBY");
			Property(t => t.OwnerNotStayAtHotel).HasColumnName("OwnerNotStayAtHotel");
			Property(t => t.SpecialRequest).HasColumnName("SpecialRequest");
			Property(t => t.GuestFirstName).HasColumnName("GuestFirstName");
			Property(t => t.GuestLastName).HasColumnName("GuestLastName");
			Property(t => t.GuestNationality).HasColumnName("GuestNationality");
			Property(t => t.RefundFee).HasColumnName("RefundFee");
			Property(t => t.SurchargeName).HasColumnName("SURCHARGENAME");
			Property(t => t.ReceiptId).HasColumnName("RECEIPTID");
			Property(t => t.Pnr).HasColumnName("PNR");
			HasOptional(t => t.Customer).WithMany(t => t.BOOKINGs).HasForeignKey(d => d.CustomerId);
			HasOptional(t => t.Room).WithMany(t => t.Bookings).HasForeignKey(d => d.RoomId);
			HasOptional(t => t.HotelPromotion).WithMany(t => t.BOOKINGs).HasForeignKey(d => d.PromotionId);
			HasOptional(t => t.Nationality).WithMany(t => t.BOOKINGs).HasForeignKey(d => d.GuestNationality);
			HasOptional(t => t.User).WithMany(t => t.BOOKINGs).HasForeignKey(d => d.EditBy);
		}
	}
}