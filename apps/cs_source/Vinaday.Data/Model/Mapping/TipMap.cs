using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class TipMap : EntityTypeConfiguration<Tip>
	{
		public TipMap()
		{
			HasKey(t => t.Id);
			ToTable("Tips");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.HotelId).HasColumnName("HotelId");
			Property(t => t.Tip1).HasColumnName("Tip1");
			Property(t => t.Tip2).HasColumnName("Tip2");
			Property(t => t.Tip3).HasColumnName("Tip3");
			Property(t => t.Status).HasColumnName("Status");
			Property(t => t.CreateDate).HasColumnName("CreateDate");
		}
	}
}