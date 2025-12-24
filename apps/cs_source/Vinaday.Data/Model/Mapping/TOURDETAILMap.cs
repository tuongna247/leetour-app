using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class TOURDETAILMap : EntityTypeConfiguration<TOURDETAIL>
	{
		public TOURDETAILMap()
		{
			HasKey(t => t.TOURDETAILID);
			Property(t => t.Itininary).HasMaxLength(250);
			Property(t => t.Content).HasMaxLength(4000);
			Property(t => t.Meal).HasMaxLength(250);
			Property(t => t.Transport).HasMaxLength(250);
            Property(t => t.OverNight).HasMaxLength(250);
            Property(t => t.ImageName).HasMaxLength(300);
			ToTable("TOURDETAIL");
			Property(t => t.TOURDETAILID).HasColumnName("TOURDETAILID");
			Property(t => t.TOURID).HasColumnName("TOURID");
			Property(t => t.Itininary).HasColumnName("Itininary");
			Property(t => t.Content).HasColumnName("Content");
			Property(t => t.Meal).HasColumnName("Meal");
			Property(t => t.Transport).HasColumnName("Transport");
            Property(t => t.OverNight).HasColumnName("OverNight");
            Property(t => t.ImageName).HasColumnName("ImageName");
			HasOptional(t => t.TOUR).WithMany(t => t.TOURDETAILs).HasForeignKey(d => d.TOURID);
		}
	}
}