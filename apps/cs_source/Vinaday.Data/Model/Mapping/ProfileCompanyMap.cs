using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class ProfileCompanyMap : EntityTypeConfiguration<ProfileCompany>
	{
		public ProfileCompanyMap()
		{
			HasKey(t => t.Id);
			Property(t => t.Name).IsRequired().HasMaxLength(500);
			Property(t => t.Address).HasMaxLength(500);
			Property(t => t.Tel).IsRequired().HasMaxLength(50);
			Property(t => t.Email).IsRequired().HasMaxLength(255);
			Property(t => t.Contacts).HasMaxLength(50);
			Property(t => t.CardNo).HasMaxLength(50);
			Property(t => t.UserName).IsRequired().HasMaxLength(50);
			ToTable("ProfileCompanies");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.Name).HasColumnName("Name");
			Property(t => t.Address).HasColumnName("Address");
			Property(t => t.CountryId).HasColumnName("CountryId");
			Property(t => t.Tel).HasColumnName("Tel");
			Property(t => t.Email).HasColumnName("Email");
			Property(t => t.Contacts).HasColumnName("Contacts");
			Property(t => t.CardNo).HasColumnName("CardNo");
			Property(t => t.UserName).HasColumnName("UserName");
			Property(t => t.CreatedDate).HasColumnName("CreatedDate");
			Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
		}
	}
}