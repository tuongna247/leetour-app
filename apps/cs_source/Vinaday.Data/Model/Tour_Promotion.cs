using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
    public class Tour_Promotion : Entity
    {
        public int? ApplyOn { get; set; }

        public DateTime? BookingDateFrom { get; set; }

        public DateTime? BookingDateTo { get; set; }

        public int? Cancelation { get; set; }

        public DateTime? CheckIn { get; set; }

        public DateTime? CheckOut { get; set; }

        public DateTime? CreateDate { get; set; }

        public string DateOfWeek { get; set; }

        public string Description { get; set; }

        public int? DiscountType { get; set; }

        public double? Get { get; set; }
        public int Id { get; set; }

        public int NumberPerson { get; set; }
        public int? Language { get; set; }

        public int? MinimumDayAdvance { get; set; }
        public int? MinimumStay { get; set; }

        public string Name { get; set; }

        public int? PromotionType { get; set; }

        public bool? Status { get; set; }

        public int TourId { get; set; }


        public Tour_Promotion()
        {
        }
    }
}