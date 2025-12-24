using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class Voucher : Entity
	{
		public int Adult { get; set; }

		public int BookingId { get; set; }

        public string Cancellation { get; set; }
        //public string Address { get; set; }
      //  public string RoomType { get; set; }

        public DateTime CheckIn { get; set; }

        public DateTime CheckOut { get; set; }
        public int Children { get; set; }

        public string Description { get; set; }

        public int Extra { get; set; }

        public string Guest { get; set; }

        public int Id { get; set; }

        public string Localtion { get; set; }

        public string Meal { get; set; }

        public string Name { get; set; }
        public string HotelPhone { get; set; }

        public string Nationality { get; set; }

        public string Promotion { get; set; }

        public int Quantity { get; set; }

        public int? Type { get; set; }

        public Voucher()
		{
		}
	}
}