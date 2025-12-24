using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class TourPromotionMap : EntityTypeConfiguration<Tour_Promotion>
	{
		public TourPromotionMap()
		{
			HasKey(t => t.Id);
			Property(t => t.Name).HasMaxLength(500);
			Property(t => t.Description).HasMaxLength(1000);
			Property(t => t.DateOfWeek).HasMaxLength(100);
			ToTable("Tour_Promotions");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.Name).HasColumnName("Name");
			Property(t => t.Description).HasColumnName("Description");
			Property(t => t.TourId).HasColumnName("TourId");
			Property(t => t.CheckIn).HasColumnName("CheckIn");
			Property(t => t.CheckOut).HasColumnName("CheckOut");
			Property(t => t.BookingDateFrom).HasColumnName("BookingDateFrom");
			Property(t => t.BookingDateTo).HasColumnName("BookingDateTo");
			Property(t => t.Status).HasColumnName("Status");
			Property(t => t.Cancelation).HasColumnName("Cancelation");
			Property(t => t.DateOfWeek).HasColumnName("DateOfWeek");
			Property(t => t.PromotionType).HasColumnName("PromotionType");
			Property(t => t.Language).HasColumnName("Language");
			Property(t => t.Get).HasColumnName("Get");
			Property(t => t.DiscountType).HasColumnName("DiscountType");
			Property(t => t.ApplyOn).HasColumnName("ApplyOn");
			Property(t => t.MinimumStay).HasColumnName("MinimumStay");
			Property(t => t.CreateDate).HasColumnName("CreateDate");
            Property(t => t.NumberPerson).HasColumnName("NumberPerson");
            Property(t => t.MinimumDayAdvance).HasColumnName("MinimumDayAdvance");
		}
	}
}