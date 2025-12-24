using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class TOURBOOKINGMap : EntityTypeConfiguration<TOURBOOKING>
	{
		public TOURBOOKINGMap()
		{
			HasKey(t => t.ID);
			Property(t => t.NAME).IsFixedLength().HasMaxLength(250);
			Property(t => t.IPLOCATION).HasMaxLength(50);
			Property(t => t.SpecialRequest).HasMaxLength(250);
			Property(t => t.GuestFirstName).HasMaxLength(250);
			Property(t => t.GuestLastName).HasMaxLength(250);
			Property(t => t.SURCHARGENAME).HasMaxLength(100);
			Property(t => t.RECEIPTID).HasMaxLength(20);
			ToTable("TOURBOOKING");
			Property(t => t.ID).HasColumnName("ID");
			Property(t => t.CUSTOMERID).HasColumnName("CUSTOMERID");
			Property(t => t.NAME).HasColumnName("NAME");
			Property(t => t.DESCRIPTION).HasColumnName("DESCRIPTION");
			Property(t => t.SENDRECEIPT).HasColumnName("SENDRECEIPT");
			Property(t => t.TourID).HasColumnName("TourID");
			Property(t => t.DAY).HasColumnName("DAY");
			Property(t => t.CHECK_IN).HasColumnName("CHECK_IN");
			Property(t => t.CHECK_OUT).HasColumnName("CHECK_OUT");
			Property(t => t.ROOMS).HasColumnName("ROOMS");
			Property(t => t.ROOM_RATE).HasColumnName("ROOM_RATE");
			Property(t => t.SURCHARGE).HasColumnName("SURCHARGE");
			Property(t => t.FEE_TAX).HasColumnName("FEE_TAX");
			Property(t => t.TOTAL).HasColumnName("TOTAL");
			Property(t => t.PaymentType).HasColumnName("PaymentType");
			Property(t => t.PaymentStatus).HasColumnName("PaymentStatus");
			Property(t => t.ISREFUND).HasColumnName("ISREFUND");
			Property(t => t.IPLOCATION).HasColumnName("IPLOCATION");
			Property(t => t.Person).HasColumnName("Person");
			Property(t => t.Date).HasColumnName("Date");
			Property(t => t.SENDVOUCHER).HasColumnName("SENDVOUCHER");
			Property(t => t.AMENBOOKING).HasColumnName("AMENBOOKING");
			Property(t => t.EDITBY).HasColumnName("EDITBY");
			Property(t => t.OwnerNotStayAtHotel).HasColumnName("OwnerNotStayAtHotel");
			Property(t => t.SpecialRequest).HasColumnName("SpecialRequest");
			Property(t => t.GuestFirstName).HasColumnName("GuestFirstName");
			Property(t => t.GuestLastName).HasColumnName("GuestLastName");
			Property(t => t.GuestNationality).HasColumnName("GuestNationality");
			Property(t => t.RefundFee).HasColumnName("RefundFee");
			Property(t => t.SURCHARGENAME).HasColumnName("SURCHARGENAME");
			Property(t => t.RECEIPTID).HasColumnName("RECEIPTID");
			HasOptional(t => t.Customer).WithMany(t => t.TOURBOOKINGs).HasForeignKey(d => d.CUSTOMERID);
			HasOptional(t => t.TOUR).WithMany(t => t.TOURBOOKINGs).HasForeignKey(d => d.TourID);
		}
	}
}