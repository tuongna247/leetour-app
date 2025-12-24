using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class AccountantOrdersMap : EntityTypeConfiguration<AccountantOrders>
	{
		public AccountantOrdersMap()
		{
			HasKey(t => t.Id);
			ToTable("AccountantOrders");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.ParentId).HasColumnName("ParentId");
			Property(t => t.Pnr).HasColumnName("Pnr");
			Property(t => t.Name).HasColumnName("Name");
			Property(t => t.Description).HasColumnName("Description");
			Property(t => t.Cancellation).HasColumnName("Cancellation");
			Property(t => t.Type).HasColumnName("Type");
			Property(t => t.CheckIn).HasColumnName("CheckIn");
			Property(t => t.CheckOut).HasColumnName("CheckOut");
			Property(t => t.Method).HasColumnName("Method");
			Property(t => t.TaxFee).HasColumnName("TaxFee");
			Property(t => t.ExtraBedFee).HasColumnName("ExtraBedFee");
			Property(t => t.ThirdPersonFee).HasColumnName("ThirdPersonFee");
			Property(t => t.Discount).HasColumnName("Discount");
			Property(t => t.SurchargeFee).HasColumnName("SurchargeFee");
			Property(t => t.Deposit).HasColumnName("Deposit");
			Property(t => t.GuestCountry).HasColumnName("GuestCountry");
			Property(t => t.GuestFullName).HasColumnName("GuestFullName");
			Property(t => t.GuestEmail).HasColumnName("GuestEmail");
			Property(t => t.GuestPhone).HasColumnName("GuestPhone");
			Property(t => t.RateExchange).HasColumnName("RateExchange");
		}
	}
}