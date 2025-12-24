using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class AccountantOrderDetailsMap : EntityTypeConfiguration<AccountantOrderDetails>
	{
		public AccountantOrderDetailsMap()
		{
			HasKey(t => t.Id);
			ToTable("AccountantOrderDetails");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.OrderId).HasColumnName("OrderId");
			Property(t => t.Date).HasColumnName("Date");
			Property(t => t.Price).HasColumnName("Price");
			Property(t => t.Amount).HasColumnName("Amount");
			Property(t => t.Discount).HasColumnName("Discount");
			Property(t => t.Surcharge).HasColumnName("Surcharge");
			Property(t => t.DiscountName).HasColumnName("DiscountName");
			Property(t => t.SurchargeName).HasColumnName("SurchargeName");
			Property(t => t.Quantity).HasColumnName("Quantity");
			Property(t => t.Status).HasColumnName("Status");
			Property(t => t.CreatedDate).HasColumnName("CreatedDate");
			Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
		}
	}
}