using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class DAYTRIPMap : EntityTypeConfiguration<DAYTRIP>
	{
		public DAYTRIPMap()
		{
			HasKey(t => t.DAYTRIPID);
			Property(t => t.NAME).HasMaxLength(250);
			Property(t => t.DESCRIPTION).HasMaxLength(250);
			Property(t => t.Title).HasMaxLength(250);
			Property(t => t.Location).HasMaxLength(250);
			Property(t => t.Duration).HasMaxLength(250);
			Property(t => t.TravelStyle).HasMaxLength(250);
			Property(t => t.GroupSize).HasMaxLength(250);
			Property(t => t.Include).HasMaxLength(1000);
			Property(t => t.Exclude).HasMaxLength(1000);
			Property(t => t.Transport).HasMaxLength(1000);
			Property(t => t.Notes).HasMaxLength(1000);
			Property(t => t.StartingTime).HasMaxLength(250);
			Property(t => t.PickupPoint).HasMaxLength(250);
			Property(t => t.DropOffPoint).HasMaxLength(250);
			Property(t => t.HightLight).HasMaxLength(500);
			Property(t => t.ProgrameDetail).HasMaxLength(4000);
			Property(t => t.IMAGE).HasMaxLength(500);
			Property(t => t.URL).HasMaxLength(500);
			Property(t => t.Country).HasMaxLength(250);
			Property(t => t.City).HasMaxLength(250);
			Property(t => t.SEO_Keyword).HasMaxLength(500);
			Property(t => t.CANCELPOLICYVALUE1).HasMaxLength(500);
			Property(t => t.CANCELPOLICYVALUE2).HasMaxLength(500);
			Property(t => t.CANCELPOLICYVALUE1_VN).HasMaxLength(500);
			Property(t => t.CANCELPOLICYVALUE2_VN).HasMaxLength(500);
			Property(t => t.SEO_DESCRIPTION).HasMaxLength(500);
			ToTable("DAYTRIP");
			Property(t => t.DAYTRIPID).HasColumnName("DAYTRIPID");
			Property(t => t.NAME).HasColumnName("NAME");
			Property(t => t.DESCRIPTION).HasColumnName("DESCRIPTION");
			Property(t => t.Title).HasColumnName("Title");
			Property(t => t.Location).HasColumnName("Location");
			Property(t => t.Duration).HasColumnName("Duration");
			Property(t => t.TravelStyle).HasColumnName("TravelStyle");
			Property(t => t.GroupSize).HasColumnName("GroupSize");
			Property(t => t.Include).HasColumnName("Include");
			Property(t => t.Exclude).HasColumnName("Exclude");
			Property(t => t.Transport).HasColumnName("Transport");
			Property(t => t.Notes).HasColumnName("Notes");
			Property(t => t.OperatorId).HasColumnName("OperatorId");
			Property(t => t.StartingTime).HasColumnName("StartingTime");
			Property(t => t.PickupPoint).HasColumnName("PickupPoint");
			Property(t => t.DropOffPoint).HasColumnName("DropOffPoint");
			Property(t => t.OverView).HasColumnName("OverView");
			Property(t => t.LocationId).HasColumnName("LocationId");
			Property(t => t.HightLight).HasColumnName("HightLight");
			Property(t => t.ProgrameDetail).HasColumnName("ProgrameDetail");
			Property(t => t.CommissionRate).HasColumnName("CommissionRate");
			Property(t => t.IMAGE).HasColumnName("IMAGE");
			Property(t => t.Status).HasColumnName("Status");
			Property(t => t.URL).HasColumnName("URL");
			Property(t => t.PRICE_FROM).HasColumnName("PRICE_FROM");
			Property(t => t.Country).HasColumnName("Country");
			Property(t => t.City).HasColumnName("City");
			Property(t => t.CountryId).HasColumnName("CountryId");
			Property(t => t.SEO_Keyword).HasColumnName("SEO_Keyword");
			Property(t => t.START_RATING).HasColumnName("START_RATING");
			Property(t => t.CANCELPOLICYVALUE1).HasColumnName("CANCELPOLICYVALUE1");
			Property(t => t.CANCELPOLICYVALUE2).HasColumnName("CANCELPOLICYVALUE2");
			Property(t => t.CANCELPOLICYTYPE).HasColumnName("CANCELPOLICYTYPE");
			Property(t => t.CANCELPOLICY_FROMDAY).HasColumnName("CANCELPOLICY_FROMDAY");
			Property(t => t.CANCELPOLICY_TODAY).HasColumnName("CANCELPOLICY_TODAY");
			Property(t => t.CANCELPOLICYVALUE1_VN).HasColumnName("CANCELPOLICYVALUE1_VN");
			Property(t => t.CANCELPOLICYVALUE2_VN).HasColumnName("CANCELPOLICYVALUE2_VN");
			Property(t => t.STARTBOOKING).HasColumnName("STARTBOOKING");
			Property(t => t.ENDBOOKING).HasColumnName("ENDBOOKING");
			Property(t => t.SEO_DESCRIPTION).HasColumnName("SEO_DESCRIPTION");
			HasOptional(t => t.Region).WithMany(t => t.DAYTRIPs).HasForeignKey(d => d.LocationId);
			HasOptional(t => t.TourOperator).WithMany(t => t.DAYTRIPs).HasForeignKey(d => d.OperatorId);
		}
	}
}