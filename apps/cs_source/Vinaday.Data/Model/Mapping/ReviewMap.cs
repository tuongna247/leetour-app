using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class ReviewMap : EntityTypeConfiguration<Review>
	{
		public ReviewMap()
		{
			HasKey(t => t.Id);
			Property(t => t.Title).IsRequired().HasMaxLength(100);
			Property(t => t.Content).IsRequired();
			Property(t => t.CustomerName).HasMaxLength(50);
			Property(t => t.CustomerAddress).HasMaxLength(100);
			Property(t => t.Tips).HasMaxLength(500);
			ToTable("Reviews");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.Title).HasColumnName("Title");
			Property(t => t.Content).HasColumnName("Content");
			Property(t => t.RatingValue).HasColumnName("RatingValue");
			Property(t => t.Helpful).HasColumnName("Helpful");
			Property(t => t.VisitDate).HasColumnName("VisitDate");
			Property(t => t.CreateDate).HasColumnName("CreateDate");
			Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
			Property(t => t.CustomerId).HasColumnName("CustomerId");
			Property(t => t.CustomerName).HasColumnName("CustomerName");
			Property(t => t.CustomerAddress).HasColumnName("CustomerAddress");
			Property(t => t.Status).HasColumnName("Status");
			Property(t => t.Tips).HasColumnName("Tips");
			Property(t => t.Sharing).HasColumnName("Sharing");
			Property(t => t.Type).HasColumnName("Type");
			Property(t => t.HotelId).HasColumnName("HotelId");
		}
	}
}