using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class BusinessCardMap : EntityTypeConfiguration<BusinessCard>
	{
		public BusinessCardMap()
		{
			HasKey(t => t.Id);
			ToTable("BusinessCard");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.FullName).HasColumnName("FullName");
			Property(t => t.Address).HasColumnName("Address");
			Property(t => t.Email).HasColumnName("Email");
			Property(t => t.Phone).HasColumnName("Phone");
			Property(t => t.Status).HasColumnName("Status");
			Property(t => t.UserAssignmentId).HasColumnName("UserAssignmentId");
			Property(t => t.IsCall).HasColumnName("IsCall");
			Property(t => t.WrongNumber).HasColumnName("WrongNumber");
			Property(t => t.Priority).HasColumnName("Priority");
			Property(t => t.Note).HasColumnName("Note");
			Property(t => t.Disconnected).HasColumnName("Disconnected");
			Property(t => t.EmailSecond).HasColumnName("EmailSecond");
			Property(t => t.PhoneSecond).HasColumnName("PhoneSecond");
		}
	}
}