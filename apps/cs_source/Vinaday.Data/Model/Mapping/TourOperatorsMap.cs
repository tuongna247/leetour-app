using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinaday.Data.Models.Mapping
{
    public class TourOperatorsMap : EntityTypeConfiguration<TourOperators>
    {
        public TourOperatorsMap()
        {
            HasKey(t => t.Id);
            Property(t => t.Adult);
            Property(t => t.BookedDate);
            Property(t => t.BookingId);
            Property(t => t.Children);
            Property(t => t.ConfirmedBy);
            Property(t => t.Email);
            Property(t => t.Guest_Leads);
            Property(t => t.Guide);
            Property(t => t.NameOfTour);
            Property(t => t.Nationality);
            Property(t => t.PNR);
            Property(t => t.PickUpTime);
            Property(t => t.PickUpoint);
            Property(t => t.SpecialRequest);
            Property(t => t.WhatsApp);
            ToTable("TourOperators");

        }
    }
}
