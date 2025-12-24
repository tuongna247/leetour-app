using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class COMMENTMap : EntityTypeConfiguration<COMMENT>
	{
		public COMMENTMap()
		{
			HasKey(t => t.COMMENTID);
			Property(t => t.COMMENTID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			Property(t => t.NAME).HasMaxLength(250);
			Property(t => t.DESCRIPTION).HasMaxLength(250);
			Property(t => t.COMMENT1).HasMaxLength(250);
			ToTable("COMMENT");
			Property(t => t.COMMENTID).HasColumnName("COMMENTID");
			Property(t => t.CUSTOMERID).HasColumnName("CUSTOMERID");
			Property(t => t.HOTELID).HasColumnName("HOTELID");
			Property(t => t.NAME).HasColumnName("NAME");
			Property(t => t.DESCRIPTION).HasColumnName("DESCRIPTION");
			Property(t => t.COMMENT1).HasColumnName("COMMENT");
			Property(t => t.CREATEDDATE).HasColumnName("CREATEDDATE");
			HasOptional(t => t.CUSTOMER).WithMany(t => t.COMMENTs).HasForeignKey(d => d.CUSTOMERID);
			HasOptional(t => t.Hotel).WithMany(t => t.Comments).HasForeignKey(d => d.HOTELID);
		}
	}
}