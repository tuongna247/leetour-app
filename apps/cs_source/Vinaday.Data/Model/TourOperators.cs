using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;

namespace Vinaday.Data.Models
{
    public class TourOperators : Entity
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string PNR { get; set; }
        public string Guest_Leads { get; set; }
        public string Email { get; set; }
        public string WhatsApp { get; set; }
        public int Adult { get; set; }
        public int Children { get; set; }
        public string Nationality { get; set; }
        public DateTime BookedDate { get; set; }
        public string NameOfTour { get; set; }
        public string SpecialRequest { get; set; }
        public string PickUpoint { get; set; }
        public string PickUpTime { get; set; }
        public string Guide { get; set; }
        public string ConfirmedBy { get; set; }
      
    }
}
