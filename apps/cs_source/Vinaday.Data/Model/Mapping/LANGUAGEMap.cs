using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class LANGUAGEMap : EntityTypeConfiguration<LANGUAGE>
	{
		public LANGUAGEMap()
		{
			HasKey(t => t.LANGUAGEID);
			ToTable("LANGUAGE");
			Property(t => t.LANGUAGEID).HasColumnName("LANGUAGEID");
			Property(t => t.NAME).HasColumnName("NAME");
			Property(t => t.DESCRIPTION).HasColumnName("DESCRIPTION");
		}
	}
}