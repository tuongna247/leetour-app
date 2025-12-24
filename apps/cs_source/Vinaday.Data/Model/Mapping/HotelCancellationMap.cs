using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class HotelCancellationMap : EntityTypeConfiguration<HotelCancellation>
	{
		public HotelCancellationMap()
		{
			HasKey(t => t.ID);
			ToTable("HotelCancellation");
			Property(t => t.ID).HasColumnName("ID");
			Property(t => t.HotelID).HasColumnName("HotelID");
			Property(t => t.CancellationID).HasColumnName("CancellationID");
			Property(t => t.CheckInFrom).HasColumnName("CheckInFrom");
			Property(t => t.CheckOutTo).HasColumnName("CheckOutTo");
			Property(t => t.HotelDetailId).HasColumnName("HotelDetailId");
			Property(t => t.Status).HasColumnName("Status");
			HasRequired(t => t.CancellationPolicy).WithMany(t => t.HotelCancellations).HasForeignKey(d => d.CancellationID);
			HasRequired(t => t.Hotel).WithMany(t => t.HotelCancellations).HasForeignKey(d => d.HotelID);
			HasOptional(t => t.HOTELDETAIL).WithMany(t => t.HotelCancellations).HasForeignKey(d => d.HotelDetailId);
		}
	}
}