using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class RateControlMap : EntityTypeConfiguration<RateControl>
	{
		public RateControlMap()
		{
			HasKey(t => t.Id);
			ToTable("RateControl");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.RoomId).HasColumnName("RoomId");
			Property(t => t.DateRate).HasColumnName("DateRate");
			Property(t => t.Profit).HasColumnName("Profit");
			Property(t => t.TARate).HasColumnName("TARate");
			Property(t => t.Surcharge1).HasColumnName("Surcharge1");
			Property(t => t.Surcharge2).HasColumnName("Surcharge2");
			Property(t => t.CompulsoryMeal).HasColumnName("CompulsoryMeal");
			Property(t => t.SellingRate).HasColumnName("SellingRate");
			Property(t => t.FinalPrice).HasColumnName("FinalPrice");
			HasRequired(t => t.HOTELDETAIL).WithMany(t => t.RateControls).HasForeignKey(d => d.RoomId);
		}
	}
}