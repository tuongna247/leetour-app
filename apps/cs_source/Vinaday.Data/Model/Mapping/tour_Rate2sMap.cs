using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class tour_Rate2sMap : EntityTypeConfiguration<Rate2>
	{
		public tour_Rate2sMap()
		{
			HasKey(t => t.Id);
			ToTable("tour_Rates2");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.PersonNo).HasColumnName("PersonNo");
			Property(t => t.RetailRate).HasColumnName("RetailRate");
			Property(t => t.NetRate).HasColumnName("NetRate");
			Property(t => t.TotalRate).HasColumnName("TotalRate");
			Property(t => t.TourId).HasColumnName("TourId");
			Property(t => t.CreatedDate).HasColumnName("CreatedDate");
			Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
			Property(t => t.Status).HasColumnName("Status");
			Property(t => t.ExpandRateId).HasColumnName("ExpandRateId");
			HasRequired(t => t.Tour).WithMany(t => t.Rate2s).HasForeignKey(d => d.TourId);
		}
	}
}