using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class CustomerCallMap : EntityTypeConfiguration<CustomerCall>
	{
		public CustomerCallMap()
		{
			HasKey(t => t.Id);
			Property(t => t.Note).HasMaxLength(500);
			ToTable("CustomerCall");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.CallDate).HasColumnName("CallDate");
			Property(t => t.Note).HasColumnName("Note");
			Property(t => t.CustomerId).HasColumnName("CustomerId");
		}
	}
}