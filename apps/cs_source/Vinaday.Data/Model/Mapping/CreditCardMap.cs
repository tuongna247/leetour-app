using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class CreditCardMap : EntityTypeConfiguration<CreditCard>
	{
		public CreditCardMap()
		{
			HasKey(t => t.Id);
			ToTable("CreditCards");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.Name).HasColumnName("Name");
			Property(t => t.FirstName).HasColumnName("FirstName");
			Property(t => t.LastName).HasColumnName("LastName");
			Property(t => t.Cvv).HasColumnName("Cvv");
			Property(t => t.CardNumber).HasColumnName("CardNumber");
			Property(t => t.ExpMonth).HasColumnName("ExpMonth");
			Property(t => t.ExpYear).HasColumnName("ExpYear");
			Property(t => t.Address).HasColumnName("Address");
			Property(t => t.Status).HasColumnName("Status");
			Property(t => t.Type).HasColumnName("Type");
			Property(t => t.CreatedDate).HasColumnName("CreatedDate");
			Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
			Property(t => t.IsUse).HasColumnName("IsUse");
		}
	}
}