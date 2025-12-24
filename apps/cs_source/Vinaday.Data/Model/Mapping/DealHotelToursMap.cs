using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class DealHotelToursMap : EntityTypeConfiguration<DealHotelTours>
	{
		public DealHotelToursMap()
		{
            ToTable("DealHotelTours");
            HasKey(t => t.Id);
			Property(t => t.DealName).HasColumnName("DealName");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.LocationCity).HasColumnName("LocationCity");
			Property(t => t.LocationCountry).HasColumnName("LocationCountry");
			Property(t => t.Night).HasColumnName("Night");
			Property(t => t.Price).HasColumnName("Price");
			Property(t => t.RoomType).HasColumnName("RoomType");
			Property(t => t.LinkDetail).HasColumnName("LinkDetail");
			Property(t => t.Discount).HasColumnName("Discount");
			Property(t => t.ExpiredDate).HasColumnName("ExpiredDate");
			Property(t => t.DealAvarta).HasColumnName("DealAvarta");
			Property(t => t.DealBanner).HasColumnName("DealBanner");
			Property(t => t.DealType).HasColumnName("DealType");
		}
	}
}