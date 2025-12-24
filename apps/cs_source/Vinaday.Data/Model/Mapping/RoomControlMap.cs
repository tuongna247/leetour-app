using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class RoomControlMap : EntityTypeConfiguration<RoomControl>
	{
		public RoomControlMap()
		{
			HasKey(t => t.Id);
			ToTable("RoomControl");
			Property(t => t.Id).HasColumnName("ID");
			Property(t => t.RoomId).HasColumnName("HotelDetailID");
			Property(t => t.RoomDate).HasColumnName("RoomDate");
			Property(t => t.Guaranteed).HasColumnName("Guaranteed");
			Property(t => t.UseGuaranteed).HasColumnName("UseGuaranteed");
			Property(t => t.Regular).HasColumnName("Regular");
			Property(t => t.UseRegular).HasColumnName("UseRegular");
			Property(t => t.CloseOutRegular).HasColumnName("CloseOutRegular");
			Property(t => t.AutoTopUp).HasColumnName("AutoTopUp");
			Property(t => t.TotalAvailable).HasColumnName("TotalAvailable");
			Property(t => t.Profit).HasColumnName("Profit");
			Property(t => t.TaRate).HasColumnName("TARate");
			Property(t => t.Surcharge1).HasColumnName("Surcharge1");
			Property(t => t.Surcharge2).HasColumnName("Surcharge2");
			Property(t => t.CompulsoryMeal).HasColumnName("CompulsoryMeal");
			Property(t => t.SellingRate).HasColumnName("SellingRate");
			Property(t => t.FinalPrice).HasColumnName("FinalPrice");
			Property(t => t.Breakfast).HasColumnName("Breakfast");
			HasRequired(t => t.Room).WithMany(t => t.RoomControls).HasForeignKey(d => d.RoomId);
		}
	}
}