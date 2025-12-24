using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
    public class tour_ToursMap : EntityTypeConfiguration<Tour>
    {
        public tour_ToursMap()
        {
            HasKey(t => t.Id);
            Property(t => t.TourCode).HasMaxLength(25);
            ToTable("tour_Tours");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.TourCode).HasColumnName("TourCode");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.TourTitle).HasColumnName("Tour_Title");
            Property(t => t.Description).HasColumnName("Description");
            Property(t => t.Overview).HasColumnName("Overview");
            Property(t => t.GroupSize).HasColumnName("GroupSize");
            Property(t => t.Cities).HasColumnName("Cities");
            Property(t => t.Ages).HasColumnName("Ages");
            Property(t => t.Accommondation).HasColumnName("Accommondation");
            Property(t => t.IncludeActivity).HasColumnName("IncludeActivity");
            Property(t => t.ExcludeActivity).HasColumnName("ExcludeActivity");
            Property(t => t.CarbonEmissionOffset).HasColumnName("CarbonEmissionOffset");
            Property(t => t.Notes).HasColumnName("Notes");
            Property(t => t.TravelStyle).HasColumnName("TravelStyle");
            Property(t => t.SEO_Description).HasColumnName("SEO_Description");
            Property(t => t.SEO_Meta).HasColumnName("SEO_Meta");
            Property(t => t.SEO_Title).HasColumnName("SEO_Title");
            Property(t => t.DepartureOption1).HasColumnName("DepartureOption1");
            Property(t => t.DepartureOption2).HasColumnName("DepartureOption2");
            Property(t => t.DepartureOption3).HasColumnName("DepartureOption3");
            Property(t => t.Duration).HasColumnName("Duration");
            Property(t => t.Location).HasColumnName("Location");
            Property(t => t.Start).HasColumnName("Start");
            Property(t => t.Finish).HasColumnName("Finish");
            Property(t => t.Transport).HasColumnName("Transport");
            Property(t => t.Meals).HasColumnName("Meals");
            Property(t => t.Price).HasColumnName("Price");
            Property(t => t.CountryId).HasColumnName("CountryId");
            Property(t => t.RegionId).HasColumnName("RegionId");
            Property(t => t.PriceFrom).HasColumnName("PriceFrom");
            Property(t => t.CostPerDay).HasColumnName("CostPerDay");
            Property(t => t.DepositRequired).HasColumnName("DepositRequired");
            Property(t => t.Status).HasColumnName("Status");
            Property(t => t.TourDetailCount).HasColumnName("TourDetailCount");
            Property(t => t.CommissionRate).HasColumnName("CommissionRate");
            Property(t => t.OperatorId).HasColumnName("OperatorId");
            Property(t => t.Type).HasColumnName("Type");
            Property(t => t.CancelationPolicy).HasColumnName("CancelationPolicy");
            Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            Property(t => t.Filter).HasColumnName("Filter");
            Property(t => t.SearchKey).HasColumnName("SearchKey");
            Property(t => t.Discount).HasColumnName("Discount");
            Property(t => t.YourSave).HasColumnName("YourSave");
            Property(t => t.TourGroup1).HasColumnName("TourGroup1");
            Property(t => t.TourGroup2).HasColumnName("TourGroup2");
            Property(t => t.TourGroup3).HasColumnName("TourGroup3");
            Property(t => t.TourGroup1Include).HasColumnName("TourGroup1Include");
            Property(t => t.TourGroup2Include).HasColumnName("TourGroup2Include");
            Property(t => t.TourGroup3Include).HasColumnName("TourGroup3Include");
            Property(t => t.InclusiveBenefit).HasColumnName("InclusiveBenefit");
            Property(t => t.VideoLink).HasColumnName("VideoLink");
            Property(t => t.Language).HasColumnName("Language");
            Property(t => t.ParentId).HasColumnName("ParentId");
            HasOptional(t => t.ProfileCompany).WithMany(t => t.Tours).HasForeignKey(d => d.OperatorId);
            HasOptional(t => t.Region).WithMany(t => t.Tours).HasForeignKey(d => d.RegionId);
        }
    }
}