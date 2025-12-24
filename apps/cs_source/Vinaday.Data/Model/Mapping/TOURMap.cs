using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class TOURMap : EntityTypeConfiguration<TOUR>
	{
		public TOURMap()
		{
			HasKey(t => t.TOURID);
			ToTable("TOUR");
			Property(t => t.TOURID).HasColumnName("TOURID");
			Property(t => t.NAME).HasColumnName("NAME");
			
			Property(t => t.DESCRIPTION).HasColumnName("DESCRIPTION");
			Property(t => t.Overview).HasColumnName("Overview");
			Property(t => t.GroupSize).HasColumnName("GroupSize");
			Property(t => t.Ages).HasColumnName("Ages");
			Property(t => t.Accommondation).HasColumnName("Accommondation");
			Property(t => t.IncludeActivity).HasColumnName("IncludeActivity");
			Property(t => t.CarbonEmissionOffset).HasColumnName("CarbonEmissionOffset");
			Property(t => t.Notes).HasColumnName("Notes");
			Property(t => t.TravelStyle).HasColumnName("TravelStyle");
			Property(t => t.Duration).HasColumnName("Duration");
			Property(t => t.Location).HasColumnName("Location");
			Property(t => t.IMAGE).HasColumnName("IMAGE");
			Property(t => t.START).HasColumnName("START");
			Property(t => t.FINISH).HasColumnName("FINISH");
			Property(t => t.Transport).HasColumnName("Transport");
			Property(t => t.Meals).HasColumnName("Meals");
			Property(t => t.URL).HasColumnName("URL");
			Property(t => t.Price).HasColumnName("Price");
			Property(t => t.Country).HasColumnName("Country");
			Property(t => t.CountryId).HasColumnName("CountryId");
			Property(t => t.LocationId).HasColumnName("LocationId");
			Property(t => t.START_RATING).HasColumnName("START_RATING");
			Property(t => t.PRICE_FROM).HasColumnName("PRICE_FROM");
			Property(t => t.Total).HasColumnName("Total");
			Property(t => t.CostPerDay).HasColumnName("CostPerDay");
			Property(t => t.DepositRequired).HasColumnName("DepositRequired");
			Property(t => t.Status).HasColumnName("Status");
			Property(t => t.SEO_Keyword).HasColumnName("SEO_Keyword");
			Property(t => t.SEO_Description).HasColumnName("SEO_Description");
			Property(t => t.MainContactId).HasColumnName("MainContactId");
			Property(t => t.ReserContactId).HasColumnName("ReserContactId");
			Property(t => t.MarketingContactId).HasColumnName("MarketingContactId");
			Property(t => t.AccountContactId).HasColumnName("AccountContactId");
			Property(t => t.TourDetailCount).HasColumnName("TourDetailCount");
			Property(t => t.CommissionRate).HasColumnName("CommissionRate");
			Property(t => t.OperatorId).HasColumnName("OperatorId");
			Property(t => t.LinkRedirect).HasColumnName("LinkRedirect");
			Property(t => t.CANCELPOLICYVALUE1).HasColumnName("CANCELPOLICYVALUE1");
			Property(t => t.CANCELPOLICYVALUE2).HasColumnName("CANCELPOLICYVALUE2");
			Property(t => t.CANCELPOLICYTYPE).HasColumnName("CANCELPOLICYTYPE");
			Property(t => t.CANCELPOLICY_FROMDAY).HasColumnName("CANCELPOLICY_FROMDAY");
			Property(t => t.CANCELPOLICY_TODAY).HasColumnName("CANCELPOLICY_TODAY");
			Property(t => t.CANCELPOLICYVALUE1_VN).HasColumnName("CANCELPOLICYVALUE1_VN");
			Property(t => t.CANCELPOLICYVALUE2_VN).HasColumnName("CANCELPOLICYVALUE2_VN");
			HasOptional(t => t.Contact).WithMany(t => t.TOURs).HasForeignKey(d => d.AccountContactId);
			HasOptional(t => t.Contact1).WithMany(t => t.TOURs1).HasForeignKey(d => d.MainContactId);
			HasOptional(t => t.Contact2).WithMany(t => t.TOURs2).HasForeignKey(d => d.MarketingContactId);
			HasOptional(t => t.Contact3).WithMany(t => t.TOURs3).HasForeignKey(d => d.ReserContactId);
			HasOptional(t => t.Region).WithMany(t => t.TOURs).HasForeignKey(d => d.LocationId);
			HasOptional(t => t.TourOperator).WithMany(t => t.TOURs).HasForeignKey(d => d.OperatorId);
		}
	}
}