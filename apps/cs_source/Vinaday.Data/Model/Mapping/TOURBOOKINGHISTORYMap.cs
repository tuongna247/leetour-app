using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class TOURBOOKINGHISTORYMap : EntityTypeConfiguration<TOURBOOKINGHISTORY>
	{
		public TOURBOOKINGHISTORYMap()
		{
			HasKey(t => t.id);
			Property(t => t.Description).HasMaxLength(50);
			Property(t => t.CancellationReason).HasMaxLength(500);
			ToTable("TOURBOOKINGHISTORY");
			Property(t => t.id).HasColumnName("id");
			Property(t => t.BookingId).HasColumnName("BookingId");
			Property(t => t.Createdate).HasColumnName("Createdate");
			Property(t => t.Description).HasColumnName("Description");
			Property(t => t.BookingArrive).HasColumnName("BookingArrive");
			Property(t => t.BookingDepart).HasColumnName("BookingDepart");
			Property(t => t.CancellationType).HasColumnName("CancellationType");
			Property(t => t.CancellationReason).HasColumnName("CancellationReason");
			Property(t => t.IsSendReceipt).HasColumnName("IsSendReceipt");
			Property(t => t.IsSendVoucher).HasColumnName("IsSendVoucher");
			Property(t => t.STATUS).HasColumnName("STATUS");
			HasOptional(t => t.TOURBOOKING).WithMany(t => t.TOURBOOKINGHISTORies).HasForeignKey(d => d.BookingId);
		}
	}
}