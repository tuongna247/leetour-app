using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class DealsMap : EntityTypeConfiguration<Deals>
	{
		public DealsMap()
		{
			HasKey(t => t.Id);
			ToTable("Deals");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.Name).HasColumnName("Name");
			Property(t => t.Description).HasColumnName("Description");
			Property(t => t.Priority).HasColumnName("Priority");
			Property(t => t.Countdown).HasColumnName("Countdown");
			Property(t => t.Created).HasColumnName("Created");
			Property(t => t.Type).HasColumnName("Type");
			Property(t => t.Modified).HasColumnName("Modified");
			Property(t => t.Status).HasColumnName("Status");
			Property(t => t.Url).HasColumnName("Url");
			Property(t => t.ImageUrl).HasColumnName("ImageUrl");
		}
	}
}