using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class PasswordResetRequestMap : EntityTypeConfiguration<PasswordResetRequest>
	{
		public PasswordResetRequestMap()
		{
			HasKey(t => t.Id);
			ToTable("PasswordResetRequest");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.AccountId).HasColumnName("AccountId");
			Property(t => t.Created).HasColumnName("Created");
			HasOptional(t => t.CUSTOMER).WithMany(t => t.PasswordResetRequests).HasForeignKey(d => d.AccountId);
		}
	}
}