using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class TourOperatorMap : EntityTypeConfiguration<TourOperator>
	{
		public TourOperatorMap()
		{
			HasKey(t => t.Id);
			Property(t => t.Name).IsRequired().HasMaxLength(255);
			Property(t => t.Address).HasMaxLength(500);
			Property(t => t.Tel).IsRequired().HasMaxLength(50);
			Property(t => t.Email).IsRequired().HasMaxLength(255);
			Property(t => t.ContactType).HasMaxLength(50);
			Property(t => t.IDCardNo).HasMaxLength(50);
			Property(t => t.UserName).IsRequired().HasMaxLength(50);
			Property(t => t.Password).IsRequired().HasMaxLength(50);
			ToTable("TourOperator");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.Name).HasColumnName("Name");
			Property(t => t.Address).HasColumnName("Address");
			Property(t => t.CountryId).HasColumnName("CountryId");
			Property(t => t.Tel).HasColumnName("Tel");
			Property(t => t.Email).HasColumnName("Email");
			Property(t => t.ContactType).HasColumnName("ContactType");
			Property(t => t.IDCardNo).HasColumnName("IDCardNo");
			Property(t => t.CreateDate).HasColumnName("CreateDate");
			Property(t => t.CreateBy).HasColumnName("CreateBy");
			Property(t => t.UpdateDate).HasColumnName("UpdateDate");
			Property(t => t.UserName).HasColumnName("UserName");
			Property(t => t.Password).HasColumnName("Password");
			HasOptional(t => t.Nationality).WithMany(t => t.TourOperators).HasForeignKey(d => d.CountryId);
			HasRequired(t => t.USER).WithMany(t => t.TourOperators).HasForeignKey(d => d.CreateBy);
		}
	}
}