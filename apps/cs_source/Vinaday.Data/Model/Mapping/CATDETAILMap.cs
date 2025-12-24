using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class CATDETAILMap : EntityTypeConfiguration<CatDetail>
	{
		public CATDETAILMap()
		{
			HasKey(t => t.Id);
			Property(t => t.Name).IsRequired().HasMaxLength(250);
			Property(t => t.Description).HasMaxLength(250);
			Property(t => t.CheckedItem).HasMaxLength(250);
			Property(t => t.DescriptionVn).HasMaxLength(255);
			ToTable("CATDETAIL");
			Property(t => t.Id).HasColumnName("ID");
			Property(t => t.Name).HasColumnName("NAME");
			Property(t => t.CatId).HasColumnName("CATID");
			Property(t => t.Status).HasColumnName("STATUS");
			Property(t => t.Description).HasColumnName("DESCRIPTION");
			Property(t => t.CheckedItem).HasColumnName("CheckedItem");
			Property(t => t.DescriptionVn).HasColumnName("DESCRIPTION_VN");
			Property(t => t.DescriptionFr).HasColumnName("DescriptionFr");
			Property(t => t.DescriptionDe).HasColumnName("DescriptionDe");
			Property(t => t.DescriptionGe).HasColumnName("DescriptionGe");
			HasRequired(t => t.Category).WithMany(t => t.CatDetails).HasForeignKey(d => d.CatId);
		}
	}
}