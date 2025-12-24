using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class UserHotelMap : EntityTypeConfiguration<UserHotel>
	{
		public UserHotelMap()
		{
			HasKey(t => t.Id);
			Property(t => t.UserId).IsRequired().HasMaxLength(200);
			ToTable("UserHotel");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.UserId).HasColumnName("UserId");
			Property(t => t.HotelId).HasColumnName("HotelId");
		}
	}
}