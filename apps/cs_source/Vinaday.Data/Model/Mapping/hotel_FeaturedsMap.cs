using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class hotel_FeaturedsMap : EntityTypeConfiguration<HotelFeatureds>
	{
		public hotel_FeaturedsMap()
		{
			HasKey(t => t.Id);
			ToTable("hotel_Featureds");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.HotelId).HasColumnName("HotelId");
			Property(t => t.ImageUrl).HasColumnName("ImageUrl");
			Property(t => t.Priority).HasColumnName("Priority");
			Property(t => t.CreatedDate).HasColumnName("CreatedDate");
			Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
		}
	}
}