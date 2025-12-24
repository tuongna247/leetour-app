using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class ExpandRatesMap : EntityTypeConfiguration<ExpandRates>
	{
		public ExpandRatesMap()
		{
			HasKey(t => t.Id);
			ToTable("tour_ExpandRates");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.Name).HasColumnName("Name");
			Property(t => t.StartDate).HasColumnName("StartDate");
			Property(t => t.EndDate).HasColumnName("EndDate");
			Property(t => t.CreatedDate).HasColumnName("CreatedDate");
			Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
			Property(t => t.TourId).HasColumnName("TourId");
			Property(t => t.Status).HasColumnName("Status");
			Property(t => t.Price).HasColumnName("Price");
			Property(t => t.Type).HasColumnName("Type");
		}
	}
}