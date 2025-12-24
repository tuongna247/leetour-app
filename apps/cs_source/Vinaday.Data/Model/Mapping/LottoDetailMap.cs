using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class LottoDetailMap : EntityTypeConfiguration<LottoDetail>
	{
		public LottoDetailMap()
		{
			HasKey(t => t.Id);
			Property(t => t.LottoId).IsRequired();
			Property(t => t.Status);
			Property(t => t.FirstName).HasMaxLength(250);
			Property(t => t.LastName).HasMaxLength(250);
			Property(t => t.Email).HasMaxLength(250);
			Property(t => t.Phone).HasMaxLength(250);
			Property(t => t.Code).HasMaxLength(250);
			Property(t => t.LastIdNumber).HasMaxLength(250);
			Property(t => t.Created);
			ToTable("LottoDetail");
			Property(t => t.Id).HasColumnName("Id");
			Property(t => t.LottoId).HasColumnName("LottoId");
			Property(t => t.Status).HasColumnName("Status");
			Property(t => t.FirstName).HasColumnName("FirstName");
			Property(t => t.LastName).HasColumnName("LastName");
			Property(t => t.Email).HasColumnName("Email");
			Property(t => t.Phone).HasColumnName("Phone");
			Property(t => t.Code).HasColumnName("Code");
			Property(t => t.LastIdNumber).HasColumnName("LastIdNumber");
			Property(t => t.Created).HasColumnName("Created");
		}
	}
}