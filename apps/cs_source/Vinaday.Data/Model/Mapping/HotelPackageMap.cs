using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class HotelPackageMap : EntityTypeConfiguration<HotelPackage>
	{
		public HotelPackageMap()
		{
			HasKey(t => t.Id);
			ToTable("HotelPackage");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.CancellationId).HasColumnName("CancellationId");
			Property(t => t.CancellationName).HasColumnName("CancellationName");
			Property(t => t.CancellationNameVn).HasColumnName("CancellationNameVn");
			Property(t => t.Including).HasColumnName("Including");
			Property(t => t.IncludingValue).HasColumnName("IncludingValue");
			Property(t => t.IncludingValueVN).HasColumnName("IncludingValueVN");
			Property(t => t.FromDate).HasColumnName("FromDate");
			Property(t => t.ToDate).HasColumnName("ToDate");
			Property(t => t.RoomName).HasColumnName("RoomName");
			Property(t => t.RoomNameVN).HasColumnName("RoomNameVN");
			Property(t => t.DiscountValue).HasColumnName("DiscountValue");
			Property(t => t.IsPromotion).HasColumnName("IsPromotion");
			Property(t => t.Night).HasColumnName("Night");
			Property(t => t.ImageUrl).HasColumnName("ImageUrl");
			Property(t => t.Price).HasColumnName("Price");
			Property(t => t.PriceFake).HasColumnName("PriceFake");
			Property(t => t.HotelId).HasColumnName("HotelId");
		}
	}
}