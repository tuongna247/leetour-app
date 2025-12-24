using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class ViewListTourMap : EntityTypeConfiguration<ViewListTour>
	{
		public ViewListTourMap()
		{
			HasKey(t => t.Id);
			ToTable("ViewListTour");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.Name).HasColumnName("Name");
			Property(t => t.Duration).HasColumnName("Duration");
			Property(t => t.Location).HasColumnName("Location");
			Property(t => t.PriceFrom).HasColumnName("PriceFrom");
			Property(t => t.ThumbmailPath).HasColumnName("ThumbmailPath");
		}
	}
}