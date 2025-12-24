using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class SpecialRateMap : EntityTypeConfiguration<SpecialRate>
	{
		public SpecialRateMap()
		{
			HasKey(t => t.Id);
			ToTable("tour_SpecialRates");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.TourId).HasColumnName("TourId");
			Property(t => t.Name).HasColumnName("Name");
			Property(t => t.BookingFrom).HasColumnName("BookingFrom");
			Property(t => t.BookingTo).HasColumnName("BookingTo");
			Property(t => t.Price).HasColumnName("Price");
			Property(t => t.Status).HasColumnName("Status");
			Property(t => t.Type).HasColumnName("Type");
			Property(t => t.CreatedDate).HasColumnName("CreatedDate");
			Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
		}
	}
}