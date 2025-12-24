using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class RoomReguestMap : EntityTypeConfiguration<RoomReguest>
	{
		public RoomReguestMap()
		{
			HasKey(t => t.Id);
			Property(t => t.RoomName).HasMaxLength(500);
			Property(t => t.HotelName).HasMaxLength(500);
			Property(t => t.FirstName).HasMaxLength(100);
			Property(t => t.LastName).HasMaxLength(100);
			Property(t => t.Email).HasMaxLength(100);
			Property(t => t.Phone).HasMaxLength(50);
			Property(t => t.Note).HasMaxLength(1000);
			ToTable("RoomReguest");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.RoomName).HasColumnName("RoomName");
			Property(t => t.HotelName).HasColumnName("HotelName");
			Property(t => t.RoomTotal).HasColumnName("RoomTotal");
			Property(t => t.FirstName).HasColumnName("FirstName");
			Property(t => t.LastName).HasColumnName("LastName");
			Property(t => t.Email).HasColumnName("Email");
			Property(t => t.Phone).HasColumnName("Phone");
			Property(t => t.CheckIn).HasColumnName("CheckIn");
			Property(t => t.CheckOut).HasColumnName("CheckOut");
			Property(t => t.IsRead).HasColumnName("IsRead");
			Property(t => t.Note).HasColumnName("Note");
			Property(t => t.CreateDate).HasColumnName("CreateDate");
			Property(t => t.Type).HasColumnName("Type");
		}
	}
}