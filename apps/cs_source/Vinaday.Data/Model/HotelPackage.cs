using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class HotelPackage : Entity
	{
        public int Id { get; set; }
        public int HotelId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Night { get; set; }
        public Decimal Price { get; set; }
        public Decimal? PriceFake { get; set; }
        public int CancellationId { get; set; }
        public string CancellationName { get; set; }
        public string CancellationNameVn { get; set; }
        public bool? IsPromotion { get; set; }
        public int? DiscountValue { get; set; }
        public string Including { get; set; }
        public string ImageUrl { get; set; }
        public string RoomName { get; set; }
        public string RoomNameVN { get; set; }
        public string IncludingValue { get; set; }
        public string IncludingValueVN { get; set; }
    }
}