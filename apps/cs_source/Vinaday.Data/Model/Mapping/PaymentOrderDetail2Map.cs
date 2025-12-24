using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class PaymentOrderDetail2Map : EntityTypeConfiguration<PaymentOrderDetail2>
	{
		public PaymentOrderDetail2Map()
		{
			HasKey(t => t.Id);
			ToTable("PaymentOrderDetail2");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.PaymentId).HasColumnName("PaymentId");
			Property(t => t.Description).HasColumnName("Description");
			Property(t => t.Price).HasColumnName("Price");
			Property(t => t.Quantity).HasColumnName("Quantity");
			Property(t => t.Amount).HasColumnName("Amount");
		}
	}
}