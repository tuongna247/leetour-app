using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class HotelCoupon : Entity
	{
        public int Id { get; set; }
        public int HotelId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Decimal Price { get; set; }
        public Decimal Discount { get; set; }
        public int TotalQty { get; set; }
        public int HotelStart { get; set; }
        public int SalesQty { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CodePromo { get; set; }
        public string ConditionUsing { get; set; }
    }
}