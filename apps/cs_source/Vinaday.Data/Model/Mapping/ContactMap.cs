using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class ContactMap : EntityTypeConfiguration<Contact>
	{
		public ContactMap()
		{
			HasKey(t => t.CONTACTID);
			Property(t => t.FIRSTNAME).HasMaxLength(250);
			Property(t => t.LASTNAME).HasMaxLength(250);
			Property(t => t.EMAILADDRESS).HasMaxLength(250);
			Property(t => t.SALUTATION).HasMaxLength(20);
			Property(t => t.PHONE).HasMaxLength(20);
			Property(t => t.FAX).HasMaxLength(20);
			Property(t => t.COMPAREEMAILADDRESS).HasMaxLength(50);
			ToTable("Contact");
			Property(t => t.CONTACTID).HasColumnName("CONTACTID");
			Property(t => t.FIRSTNAME).HasColumnName("FIRSTNAME");
			Property(t => t.LASTNAME).HasColumnName("LASTNAME");
			Property(t => t.EMAILADDRESS).HasColumnName("EMAILADDRESS");
			Property(t => t.SALUTATION).HasColumnName("SALUTATION");
			Property(t => t.STATUS).HasColumnName("STATUS");
			Property(t => t.PHONE).HasColumnName("PHONE");
			Property(t => t.FAX).HasColumnName("FAX");
			Property(t => t.COMPAREEMAILADDRESS).HasColumnName("COMPAREEMAILADDRESS");
		}
	}
}