using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class DayTripRateMap : EntityTypeConfiguration<DayTripRate>
	{
		public DayTripRateMap()
		{
			HasKey(t => t.id);
			Property(t => t.Description).IsFixedLength().HasMaxLength(255);
			ToTable("DayTripRate");
			Property(t => t.id).HasColumnName("id");
			Property(t => t.persons).HasColumnName("persons");
			Property(t => t.RetailRate).HasColumnName("RetailRate");
			Property(t => t.NetRate).HasColumnName("NetRate");
			Property(t => t.AgeFrom).HasColumnName("AgeFrom");
			Property(t => t.AgeTo).HasColumnName("AgeTo");
			Property(t => t.DaytripId).HasColumnName("DaytripId");
			Property(t => t.Description).HasColumnName("Description");
			HasRequired(t => t.DAYTRIP).WithMany(t => t.DayTripRates).HasForeignKey(d => d.DaytripId);
		}
	}
}