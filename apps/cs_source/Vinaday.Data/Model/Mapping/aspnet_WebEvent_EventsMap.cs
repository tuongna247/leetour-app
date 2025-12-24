using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class aspnet_WebEvent_EventsMap : EntityTypeConfiguration<aspnet_WebEvent_Events>
	{
		public aspnet_WebEvent_EventsMap()
		{
			HasKey(t => t.EventId);
			Property(t => t.EventId).IsRequired().IsFixedLength().HasMaxLength(32);
			Property(t => t.EventType).IsRequired().HasMaxLength(256);
			Property(t => t.Message).HasMaxLength(1024);
			Property(t => t.ApplicationPath).HasMaxLength(256);
			Property(t => t.ApplicationVirtualPath).HasMaxLength(256);
			Property(t => t.MachineName).IsRequired().HasMaxLength(256);
			Property(t => t.RequestUrl).HasMaxLength(1024);
			Property(t => t.ExceptionType).HasMaxLength(256);
			ToTable("aspnet_WebEvent_Events");
			Property(t => t.EventId).HasColumnName("EventId");
			Property(t => t.EventTimeUtc).HasColumnName("EventTimeUtc");
			Property(t => t.EventTime).HasColumnName("EventTime");
			Property(t => t.EventType).HasColumnName("EventType");
			Property(t => t.EventSequence).HasColumnName("EventSequence");
			Property(t => t.EventOccurrence).HasColumnName("EventOccurrence");
			Property(t => t.EventCode).HasColumnName("EventCode");
			Property(t => t.EventDetailCode).HasColumnName("EventDetailCode");
			Property(t => t.Message).HasColumnName("Message");
			Property(t => t.ApplicationPath).HasColumnName("ApplicationPath");
			Property(t => t.ApplicationVirtualPath).HasColumnName("ApplicationVirtualPath");
			Property(t => t.MachineName).HasColumnName("MachineName");
			Property(t => t.RequestUrl).HasColumnName("RequestUrl");
			Property(t => t.ExceptionType).HasColumnName("ExceptionType");
			Property(t => t.Details).HasColumnName("Details");
		}
	}
}