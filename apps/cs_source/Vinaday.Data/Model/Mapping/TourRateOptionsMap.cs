using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class TourRateOptionsMap : EntityTypeConfiguration<TourRateOptions>
	{
		public TourRateOptionsMap()
		{
            ToTable("tour_RateOptions");
            HasKey(t => t.Id);
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.Name).HasColumnName("Name");
			Property(t => t.Rate).HasColumnName("Rate");
			Property(t => t.Tour_Id).HasColumnName("Tour_Id");
		}
	}
}