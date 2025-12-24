using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class OrderInformation2sMap : EntityTypeConfiguration<OrderInformations>
	{
		public OrderInformation2sMap()
		{
			HasKey(t => t.Id);
			ToTable("OrderInformations2");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.OrderId).HasColumnName("OrderId");
			Property(t => t.Date).HasColumnName("Date");
			Property(t => t.Price).HasColumnName("Price").HasPrecision(18, 6);
			Property(t => t.Amount).HasColumnName("Amount").HasPrecision(18, 6);
			Property(t => t.Discount).HasColumnName("Discount").HasPrecision(18, 6);
			Property(t => t.Surcharge).HasColumnName("Surcharge").HasPrecision(18, 6);
			Property(t => t.DiscountName).HasColumnName("DiscountName");
			Property(t => t.SurchargeName).HasColumnName("SurchargeName");
			Property(t => t.Quantity).HasColumnName("Quantity");
			Property(t => t.Status).HasColumnName("Status");
			Property(t => t.CreatedDate).HasColumnName("CreatedDate");
			Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
		}
	}
}