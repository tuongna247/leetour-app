using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class aspnet_MembershipMap : EntityTypeConfiguration<aspnet_Membership>
	{
		public aspnet_MembershipMap()
		{
			HasKey(t => t.UserId);
			Property(t => t.Password).IsRequired().HasMaxLength(128);
			Property(t => t.PasswordSalt).IsRequired().HasMaxLength(128);
			Property(t => t.MobilePIN).HasMaxLength(16);
			Property(t => t.Email).HasMaxLength(256);
			Property(t => t.LoweredEmail).HasMaxLength(256);
			Property(t => t.PasswordQuestion).HasMaxLength(256);
			Property(t => t.PasswordAnswer).HasMaxLength(128);
			ToTable("aspnet_Membership");
			Property(t => t.ApplicationId).HasColumnName("ApplicationId");
			Property(t => t.UserId).HasColumnName("UserId");
			Property(t => t.Password).HasColumnName("Password");
			Property(t => t.PasswordFormat).HasColumnName("PasswordFormat");
			Property(t => t.PasswordSalt).HasColumnName("PasswordSalt");
			Property(t => t.MobilePIN).HasColumnName("MobilePIN");
			Property(t => t.Email).HasColumnName("Email");
			Property(t => t.LoweredEmail).HasColumnName("LoweredEmail");
			Property(t => t.PasswordQuestion).HasColumnName("PasswordQuestion");
			Property(t => t.PasswordAnswer).HasColumnName("PasswordAnswer");
			Property(t => t.IsApproved).HasColumnName("IsApproved");
			Property(t => t.IsLockedOut).HasColumnName("IsLockedOut");
			Property(t => t.CreateDate).HasColumnName("CreateDate");
			Property(t => t.LastLoginDate).HasColumnName("LastLoginDate");
			Property(t => t.LastPasswordChangedDate).HasColumnName("LastPasswordChangedDate");
			Property(t => t.LastLockoutDate).HasColumnName("LastLockoutDate");
			Property(t => t.FailedPasswordAttemptCount).HasColumnName("FailedPasswordAttemptCount");
			Property(t => t.FailedPasswordAttemptWindowStart).HasColumnName("FailedPasswordAttemptWindowStart");
			Property(t => t.FailedPasswordAnswerAttemptCount).HasColumnName("FailedPasswordAnswerAttemptCount");
			Property(t => t.FailedPasswordAnswerAttemptWindowStart).HasColumnName("FailedPasswordAnswerAttemptWindowStart");
			Property(t => t.Comment).HasColumnName("Comment");
			HasRequired(t => t.aspnet_Applications).WithMany(t => t.aspnet_Membership).HasForeignKey(d => d.ApplicationId);
			HasRequired(t => t.aspnet_Users).WithOptional(t => t.aspnet_Membership);
		}
	}
}