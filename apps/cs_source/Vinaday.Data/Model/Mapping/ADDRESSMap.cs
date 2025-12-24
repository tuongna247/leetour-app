using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class ADDRESSMap : EntityTypeConfiguration<ADDRESS>
	{
		public ADDRESSMap()
		{
			HasKey(t => t.ADDRESSID);
			Property(t => t.STREET).HasMaxLength(250);
			Property(t => t.WARD).HasMaxLength(30);
			Property(t => t.DISTRICT).HasMaxLength(250);
			Property(t => t.CITI).HasMaxLength(50);
			Property(t => t.PROVINCE).HasMaxLength(50);
			ToTable("ADDRESS");
			Property(t => t.ADDRESSID).HasColumnName("ADDRESSID");
			Property(t => t.STREET).HasColumnName("STREET");
			Property(t => t.WARD).HasColumnName("WARD");
			Property(t => t.DISTRICT).HasColumnName("DISTRICT");
			Property(t => t.CITI).HasColumnName("CITI");
			Property(t => t.PROVINCE).HasColumnName("PROVINCE");
		}
	}
}