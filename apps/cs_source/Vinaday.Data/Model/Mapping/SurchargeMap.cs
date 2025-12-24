using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class SurchargeMap : EntityTypeConfiguration<Surcharge>
	{
		public SurchargeMap()
		{
			HasKey(t => t.Id);
			Property(t => t.DateOfWeek).HasMaxLength(50);
			Property(t => t.SurchargeName).HasMaxLength(500);
			ToTable("Surcharge");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.StayDateFrom).HasColumnName("StayDateFrom");
			Property(t => t.StayDateTo).HasColumnName("StayDateTo");
			Property(t => t.DateOfWeek).HasColumnName("DateOfWeek");
			Property(t => t.SurchargeName).HasColumnName("SurchargeName");
			Property(t => t.HotelId).HasColumnName("HotelId");
			Property(t => t.RoomId).HasColumnName("RoomId");
			Property(t => t.Price).HasColumnName("Price");
		}
	}
}