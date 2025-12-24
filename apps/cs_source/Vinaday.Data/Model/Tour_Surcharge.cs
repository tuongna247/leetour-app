using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class Tour_Surcharge : Entity
	{
		public string DateOfWeek { get; set; }

        public int Id { get; set; }

        public int Type { get; set; }
        
        public decimal? Price { get; set; }

        public DateTime StayDateFrom { get; set; }
        public DateTime StayDateTo { get; set; }

        public string SurchargeName { get; set; }

        public int TourId { get; set; }

        public Tour_Surcharge()
		{
		}
	}
}