using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class HotelPackage_Surcharge : Entity
	{
        public int Id { get; set; }
        public int Package_Id { get; set; }
        public int Hotel_Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal Price { get; set; }
        public string SurchargeName { get; set; }
        public string DateOfWeek { get; set; }
	}
}