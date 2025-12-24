using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class TourReview : Entity
    {
		public int? Id { get; set; }

		public int? TourId { get; set; }

        public int Start { get; set; }
        public string Title { get; set; }
        public string GuestName { get; set; }
        public string ReviewContent { get; set; }
        public string CreatedBy { get; set; }
        public string ApprovalBy { get; set; }
        public string BookDate { get; set; }

        public int? Status { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Vinaday.Data.Models.TOUR TOUR { get; set; }


        public TourReview()
		{
		}
	}
}