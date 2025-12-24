using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class RATEOFEXCHANGEMap : EntityTypeConfiguration<RateExchange>
	{
		public RATEOFEXCHANGEMap()
		{
			HasKey(t => t.Id);
			ToTable("RATEOFEXCHANGE");
			Property(t => t.Id).HasColumnName("RATEOFEXCHANGEID");
			Property(t => t.Name).HasColumnName("NAME");
			Property(t => t.Money).HasColumnName("MONEY");
			Property(t => t.CurrentPrice).HasColumnName("CURRENTPRICE");
			Property(t => t.Description).HasColumnName("DESCRIPTION");
		}
	}
}