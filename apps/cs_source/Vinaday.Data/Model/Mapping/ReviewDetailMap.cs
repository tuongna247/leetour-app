using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class ReviewDetailMap : EntityTypeConfiguration<ReviewDetail>
	{
		public ReviewDetailMap()
		{
			HasKey(t => t.Id);
			ToTable("ReviewDetails");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.ReviewId).HasColumnName("ReviewId");
			Property(t => t.CategoryDetailId).HasColumnName("CategoryDetailId");
			Property(t => t.Value).HasColumnName("Value");
			Property(t => t.CreateDate).HasColumnName("CreateDate");
			Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
			Property(t => t.Status).HasColumnName("Status");
			HasRequired(t => t.CatDetail).WithMany(t => t.ReviewDetails).HasForeignKey(d => d.CategoryDetailId);
			HasRequired(t => t.Review).WithMany(t => t.ReviewDetails).HasForeignKey(d => d.ReviewId);
		}
	}
}