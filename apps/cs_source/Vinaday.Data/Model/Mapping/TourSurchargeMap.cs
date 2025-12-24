using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class TourSurchargeMap : EntityTypeConfiguration<Tour_Surcharge>
	{
		public TourSurchargeMap()
		{
			HasKey(t => t.Id);
			Property(t => t.DateOfWeek).HasMaxLength(50);
			Property(t => t.SurchargeName).HasMaxLength(500);
			ToTable("Tour_Surcharge");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.StayDateFrom).HasColumnName("StayDateFrom");
			Property(t => t.StayDateTo).HasColumnName("StayDateTo");
			Property(t => t.DateOfWeek).HasColumnName("DateOfWeek");
			Property(t => t.SurchargeName).HasColumnName("SurchargeName");
			Property(t => t.Type).HasColumnName("Type");
			Property(t => t.TourId).HasColumnName("TourId");
			Property(t => t.Price).HasColumnName("Price");
		}
	}
}