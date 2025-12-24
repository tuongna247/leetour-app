using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class OrdersMap : EntityTypeConfiguration<Order>
	{
		public OrdersMap()
		{
			HasKey(t => t.Id);
			Property(t => t.SurchargeName).HasMaxLength(100);
			Property(t => t.IpLocation).HasMaxLength(50);
			Property(t => t.SpecialRequest).HasMaxLength(250);
			Property(t => t.ReceiptId).HasMaxLength(20);
            
            Property(t => t.Pnr).IsFixedLength().HasMaxLength(20);
			ToTable("tour_Orders");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.CardId).HasColumnName("CardId");
			Property(t => t.CustomerId).HasColumnName("CustomerId");
            Property(t => t.CouponCode).HasColumnName("CouponCode");
            Property(t => t.ProductId).HasColumnName("ProductId");
			Property(t => t.ProductName).HasColumnName("ProductName");
			Property(t => t.Night).HasColumnName("Night");
			Property(t => t.StartDate).HasColumnName("StartDate");
			Property(t => t.EndDate).HasColumnName("EndDate");
			Property(t => t.Quantity).HasColumnName("Quantity");
			Property(t => t.SurchargeFee).HasColumnName("SurchargeFee");
			Property(t => t.SurchargeName).HasColumnName("SurchargeName");
			Property(t => t.TaxFee).HasColumnName("TaxFee");
			Property(t => t.Price).HasColumnName("Price");
			Property(t => t.Amount).HasColumnName("Amount");
			Property(t => t.IsRefund).HasColumnName("IsRefund");
			Property(t => t.TotalRefund).HasColumnName("TotalRefund");
			Property(t => t.IpLocation).HasColumnName("IpLocation");
			Property(t => t.Discount).HasColumnName("Discount");
			Property(t => t.DiscountName).HasColumnName("DiscountName");
			Property(t => t.AmenBooking).HasColumnName("AmenBooking");
			Property(t => t.OwnerStay).HasColumnName("OwnerStay");
			Property(t => t.SpecialRequest).HasColumnName("SpecialRequest");
			Property(t => t.ReceiptId).HasColumnName("ReceiptId");
			Property(t => t.Note).HasColumnName("Note");
			Property(t => t.Pnr).HasColumnName("Pnr");
			Property(t => t.Type).HasColumnName("Type");
			Property(t => t.Status).HasColumnName("Status");
			Property(t => t.MemberId).HasColumnName("MemberId");
			Property(t => t.CreatedDate).HasColumnName("CreatedDate");
			Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
			Property(t => t.PaymentMethod).HasColumnName("PaymentMethod");
			Property(t => t.CardNumber).HasColumnName("CardNumber");
			Property(t => t.CancellationPolicy).HasColumnName("CancellationPolicy");
			Property(t => t.ExtraBed).HasColumnName("ExtraBed");
			Property(t => t.ThirdPersonFee).HasColumnName("ThirdPersonFee");
			Property(t => t.CancelFee).HasColumnName("CancelFee");
			Property(t => t.LocalType).HasColumnName("LocalType");
			Property(t => t.Management).HasColumnName("Management");
			Property(t => t.RateExchange).HasColumnName("RateExchange");
			Property(t => t.GuestCountry).HasColumnName("GuestCountry");
			Property(t => t.GuestFirstName).HasColumnName("GuestFirstName");
			Property(t => t.GuestLastName).HasColumnName("GuestLastName");
			Property(t => t.Deposit).HasColumnName("Deposit");
			Property(t => t.DepartureOption).HasColumnName("DepartureOption");
			Property(t => t.GroupType).HasColumnName("GroupType");
			Property(t => t.IsTourOperator).HasColumnName("IsTourOperator");
		}
	}
}