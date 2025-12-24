using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class PaymentOrderMap : EntityTypeConfiguration<PaymentOrder>
	{
		public PaymentOrderMap()
		{
			HasKey(t => t.Id);
			ToTable("PaymentOrder");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.OrderId).HasColumnName("OrderId");
			Property(t => t.OrderDate).HasColumnName("OrderDate");
			Property(t => t.Income).HasColumnName("Income");
			Property(t => t.Outcome).HasColumnName("Outcome");
			Property(t => t.Profit).HasColumnName("Profit");
			Property(t => t.Ratio).HasColumnName("Ratio");
			Property(t => t.IssueBy).HasColumnName("IssueBy");
			Property(t => t.CreatedBy).HasColumnName("CreatedBy");
			Property(t => t.Discount).HasColumnName("Discount");
			Property(t => t.Status).HasColumnName("Status");
			Property(t => t.CreatedDate).HasColumnName("CreatedDate");
			Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
		}
	}
}