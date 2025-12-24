using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class HotelPackage_SurchargeMap : EntityTypeConfiguration<HotelPackage_Surcharge>
	{
		public HotelPackage_SurchargeMap()
		{
			HasKey(t => t.Id);
			ToTable("HotelPackage_Surcharge");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.FromDate).HasColumnName("FromDate");
			Property(t => t.ToDate).HasColumnName("ToDate");
			Property(t => t.Package_Id).HasColumnName("Package_Id");
			Property(t => t.Price).HasColumnName("Price");
			Property(t => t.SurchargeName).HasColumnName("SurchargeName");
			Property(t => t.DateOfWeek).HasColumnName("DateOfWeek");
			Property(t => t.Hotel_Id).HasColumnName("Hotel_Id");
		}
	}
}