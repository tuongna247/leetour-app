using System;
using System.Runtime.CompilerServices;
using Repository.Pattern.Ef6;

namespace Vinaday.Data.Models
{
	public class DealHotelToursVN : Entity
    {
        public DealHotelToursVN()
        {
        }
        public int Id { get; set; }
        public string DealName { get; set; }
        public string LocationCity { get; set; }
        public string LocationCountry { get; set; }
        public string Night { get; set; }
        public string Price { get; set; }
        public string RoomType { get; set; }
        public string LinkDetail { get; set; }
        public string Discount { get; set; }
        public DateTime ExpiredDate { get; set; }
        public string DealAvarta { get; set; }
        public string DealBanner { get; set; }
        public int DealType { get; set; }
    }
}