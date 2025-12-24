using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class TourRateMap : EntityTypeConfiguration<TourRate>
	{
		public TourRateMap()
		{
			HasKey(t => t.id);
			Property(t => t.Description).IsFixedLength().HasMaxLength(255);
			ToTable("TourRate");
			Property(t => t.id).HasColumnName("id");
			Property(t => t.persons).HasColumnName("persons");
			Property(t => t.RetailRate).HasColumnName("RetailRate");
			Property(t => t.NetRate).HasColumnName("NetRate");
			Property(t => t.AgeFrom).HasColumnName("AgeFrom");
			Property(t => t.AgeTo).HasColumnName("AgeTo");
			Property(t => t.TourId).HasColumnName("TourId");
			Property(t => t.Description).HasColumnName("Description");
			HasRequired(t => t.TOUR).WithMany(t => t.TourRates).HasForeignKey(d => d.TourId);
		}
	}
}