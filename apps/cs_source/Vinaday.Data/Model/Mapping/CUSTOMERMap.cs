using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class CUSTOMERMap : EntityTypeConfiguration<Customer>
	{
		public CUSTOMERMap()
		{
			HasKey(t => t.CustomerId);
			Property(t => t.Lastname).HasMaxLength(250);
			Property(t => t.Firstname).HasMaxLength(250);
			Property(t => t.IdCard).HasMaxLength(50);
			Property(t => t.Nickname).HasMaxLength(100);
			Property(t => t.Street).HasMaxLength(500);
			Property(t => t.Ward).HasMaxLength(50);
			Property(t => t.District).HasMaxLength(50);
			Property(t => t.PhoneNumber).HasMaxLength(50);
			Property(t => t.Email).HasMaxLength(100);
			Property(t => t.UserName).HasMaxLength(50);
			Property(t => t.Password).HasMaxLength(50);
			Property(t => t.SECRETQUESTION1).HasMaxLength(250);
			Property(t => t.ANSWER1).HasMaxLength(250);
			Property(t => t.SECRETQUESTION2).HasMaxLength(250);
			Property(t => t.ANSWER2).HasMaxLength(250);
			Property(t => t.BANKNAME).HasMaxLength(250);
			Property(t => t.CARDHOLDER).HasMaxLength(250);
			Property(t => t.BANKNUMBER).IsFixedLength().HasMaxLength(10);
			Property(t => t.CVCODE).HasMaxLength(10);
			Property(t => t.UserId).HasMaxLength(100);
			Property(t => t.PhoneNumber2).HasMaxLength(50);
			ToTable("CUSTOMER");
			Property(t => t.CustomerId).HasColumnName("CUSTOMERID");
			Property(t => t.Lastname).HasColumnName("LASTNAME");
			Property(t => t.Firstname).HasColumnName("FIRSTNAME");
			Property(t => t.DateOfBirth).HasColumnName("DateOfBirth");
			Property(t => t.IdCard).HasColumnName("IDCard");
			Property(t => t.AddressId).HasColumnName("AddressID");
			Property(t => t.NationalId).HasColumnName("NATIONALID");
			Property(t => t.Nickname).HasColumnName("NICKNAME");
			Property(t => t.Street).HasColumnName("STREET");
			Property(t => t.Ward).HasColumnName("WARD");
			Property(t => t.District).HasColumnName("DISTRICT");
			Property(t => t.PhoneNumber).HasColumnName("PHONENUMBER");
			Property(t => t.Email).HasColumnName("EMAIL");
			Property(t => t.UserName).HasColumnName("USERNAME");
			Property(t => t.Password).HasColumnName("PASSWORD");
			Property(t => t.SECRETQUESTION1).HasColumnName("SECRETQUESTION1");
			Property(t => t.ANSWER1).HasColumnName("ANSWER1");
			Property(t => t.SECRETQUESTION2).HasColumnName("SECRETQUESTION2");
			Property(t => t.ANSWER2).HasColumnName("ANSWER2");
			Property(t => t.ISBANK).HasColumnName("ISBANK");
			Property(t => t.BANKNAME).HasColumnName("BANKNAME");
			Property(t => t.CARDHOLDER).HasColumnName("CARDHOLDER");
			Property(t => t.BANKNUMBER).HasColumnName("BANKNUMBER");
			Property(t => t.CVCODE).HasColumnName("CVCODE");
			Property(t => t.EXPIRYDATE).HasColumnName("EXPIRYDATE");
			Property(t => t.ISSENDMAIL).HasColumnName("ISSENDMAIL");
			Property(t => t.MEMBERTYPE).HasColumnName("MEMBERTYPE");
			Property(t => t.Status).HasColumnName("Status");
			Property(t => t.UserId).HasColumnName("UserId");
			Property(t => t.PhoneNumber2).HasColumnName("PhoneNumber2");
			Property(t => t.IsCall).HasColumnName("IsCall");
			Property(t => t.Priority).HasColumnName("Priority");
			Property(t => t.MemberId).HasColumnName("MemberId");
			Property(t => t.CardId).HasColumnName("CardId");
			HasOptional(t => t.Nationality).WithMany(t => t.Customers).HasForeignKey(d => d.NationalId);
		}
	}
}