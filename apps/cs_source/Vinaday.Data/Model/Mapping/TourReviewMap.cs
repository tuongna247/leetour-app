using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class TourReviewMap : EntityTypeConfiguration<TourReview>
	{
		public TourReviewMap()
		{
			HasKey(t => t.Id);
            ToTable("TourReview");
			//Property(t => t.Id).HasColumnName("Id");
			Property(t => t.TourId).HasColumnName("TourId");
			Property(t => t.Title).HasColumnName("Title");
			Property(t => t.GuestName).HasColumnName("GuestName");
			Property(t => t.Start).HasColumnName("Start");
			Property(t => t.ReviewContent).HasColumnName("ReviewContent");
			Property(t => t.BookDate).HasColumnName("BookDate");
			Property(t => t.CreatedBy).HasColumnName("CreatedBy");
			Property(t => t.ApprovalBy).HasColumnName("ApprovalBy");
			Property(t => t.Status).HasColumnName("Status");
			Property(t => t.CreatedDate).HasColumnName("CreatedDate");
			Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
		}
	}
}