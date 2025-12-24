using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class User1Map : EntityTypeConfiguration<User1>
	{
		public User1Map()
		{
			HasKey(t => t.Id);
			ToTable("Users");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.Username).HasColumnName("Username");
			Property(t => t.Password).HasColumnName("Password");
			Property(t => t.CreationDate).HasColumnName("CreationDate");
			Property(t => t.RoleId).HasColumnName("RoleId");
			HasRequired(t => t.Role).WithMany(t => t.Users).HasForeignKey(d => d.RoleId);
		}
	}
}