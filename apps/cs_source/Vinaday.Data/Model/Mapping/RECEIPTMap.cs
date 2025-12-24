using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class RECEIPTMap : EntityTypeConfiguration<RECEIPT>
	{
		public RECEIPTMap()
		{
			HasKey(t => t.RECEIPTID);
			ToTable("RECEIPT");
			Property(t => t.RECEIPTID).HasColumnName("RECEIPTID");
			Property(t => t.NAME).HasColumnName("NAME");
			Property(t => t.DESCRIPTION).HasColumnName("DESCRIPTION");
		}
	}
}