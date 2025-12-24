using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class OrderDetailMap2 : EntityTypeConfiguration<OrderDetail2>
	{
		public OrderDetailMap2()
		{
			HasKey(t => t.Id);
			Property(t => t.Note).IsRequired();
			ToTable("tour_OrderDetails2");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.UserId).HasColumnName("UserId");
			Property(t => t.OrderId).HasColumnName("OrderId");
			Property(t => t.Note).HasColumnName("Note");
			Property(t => t.ChangedName).HasColumnName("ChangedName");
			Property(t => t.ChangedValue).HasColumnName("ChangedValue");
			Property(t => t.CreatedDate).HasColumnName("CreatedDate");
			Property(t => t.Value).HasColumnName("Value");
			Property(t => t.IsSend).HasColumnName("IsSend");
			HasRequired(t => t.Order).WithMany(t => t.OrderDetail).HasForeignKey(d => d.OrderId);
		}
	}
}