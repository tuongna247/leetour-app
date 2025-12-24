using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class IMAGEMap : EntityTypeConfiguration<IMAGE>
	{
		public IMAGEMap()
		{
			HasKey(t => t.ID);
			Property(t => t.Hotel_Base_Url).HasMaxLength(250);
			ToTable("IMAGES");
			Property(t => t.ID).HasColumnName("ID");
			Property(t => t.Hotel_Base_Url).HasColumnName("Hotel_Base_Url");
			Property(t => t.SuffixLocation).HasColumnName("SuffixLocation");
			Property(t => t.PrefixLocation).HasColumnName("PrefixLocation");
		}
	}
}