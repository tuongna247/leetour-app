using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class NotifyMap : EntityTypeConfiguration<Notify>
	{
		public NotifyMap()
		{
			HasKey(t => t.Id);
			Property(t => t.NotifyTitle).IsRequired().HasMaxLength(200);
			Property(t => t.Message).IsRequired().HasMaxLength(500);
			Property(t => t.UserId).IsRequired().HasMaxLength(200);
			ToTable("Notify");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.NotifyType).HasColumnName("NotifyType");
			Property(t => t.NotifyTitle).HasColumnName("NotifyTitle");
			Property(t => t.Message).HasColumnName("Message");
			Property(t => t.IsRead).HasColumnName("IsRead");
			Property(t => t.CreateDate).HasColumnName("CreateDate");
			Property(t => t.HotelId).HasColumnName("HotelId");
			Property(t => t.UserId).HasColumnName("UserId");
			Property(t => t.Status).HasColumnName("Status");
		}
	}
}