using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class Rate2 : Entity
	{
		public DateTime CreatedDate { get; set; }
		public int? ExpandRateId { get; set; }

        public int Id { get; set; }

        public DateTime ModifiedDate { get; set; }

        public decimal? NetRate { get; set; }

        public int? PersonNo { get; set; }

        public decimal? RetailRate { get; set; }

        public int? Status { get; set; }

        public decimal? TotalRate { get; set; }

        public virtual Vinaday.Data.Models.Tour Tour { get; set; }

        public int TourId { get; set; }

        public Rate2()
		{
		}
	}
}