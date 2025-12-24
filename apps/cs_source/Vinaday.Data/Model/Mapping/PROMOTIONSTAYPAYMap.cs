using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class PROMOTIONSTAYPAYMap : EntityTypeConfiguration<PROMOTIONSTAYPAY>
	{
		public PROMOTIONSTAYPAYMap()
		{
			HasKey(t => t.ID);
			Property(t => t.TITLE).IsRequired().HasMaxLength(50);
			Property(t => t.DESCRIPTION).HasMaxLength(50);
			Property(t => t.TitleVietNam).HasMaxLength(150);
			ToTable("PROMOTIONSTAYPAY");
			Property(t => t.ID).HasColumnName("ID");
			Property(t => t.TITLE).HasColumnName("TITLE");
			Property(t => t.STAY).HasColumnName("STAY");
			Property(t => t.PAY).HasColumnName("PAY");
			Property(t => t.DESCRIPTION).HasColumnName("DESCRIPTION");
			Property(t => t.STATUS).HasColumnName("STATUS");
			Property(t => t.TitleVietNam).HasColumnName("TitleVietNam");
		}
	}
}