using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class Order : Entity
	{
        public string CouponCode { get; set; }
        public bool? AmenBooking
		{
			get;
			set;
		}

		public decimal? Amount
		{
			get;
			set;
		}

		public decimal? CancelFee
		{
			get;
			set;
		}

		public string CancellationPolicy
		{
			get;
			set;
		}

		public int? CardId
		{
			get;
			set;
		}

		public string CardNumber
		{
			get;
			set;
		}

		public int? Children
		{
			get;
			set;
		}

		public DateTime CreatedDate
		{
			get;
			set;
		}

		public int? CustomerId
		{
			get;
			set;
		}
        public string DepartureOption { get; set; }
        public string GroupType { get; set; }
        public decimal? Deposit
		{
			get;
			set;
		}

		public decimal? Discount
		{
			get;
			set;
		}

		public string DiscountName
		{
			get;
			set;
		}

		public DateTime EndDate
		{
			get;
			set;
		}

		public decimal? ExtraBed
		{
			get;
			set;
		}

		public string GuestCountry
		{
			get;
			set;
		}

		public string GuestFirstName
		{
			get;
			set;
		}

		public string GuestLastName
		{
			get;
			set;
		}

		public int Id { get; set; }

        public string IpLocation { get; set; }

        public bool? IsRead { get; set; }

        public bool? IsRefund { get; set; }
        public int? LocalType { get; set; }

        public string Management { get; set; }
        public string MemberId { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int? Night { get; set; }

        public string Note { get; set; }


        public ICollection<Vinaday.Data.Models.OrderDetail> OrderDetail { get; set; }

        public bool? OwnerStay { get; set; }

        public int PaymentMethod { get; set; }

        public string Pnr { get; set; }

        public decimal? Price { get; set; }

        public int? ProductId { get; set; }
        public string ProductName { get; set; }

        public int? Quantity { get; set; }

        public decimal? RateExchange { get; set; }

        public string ReceiptId { get; set; }

        public string SpecialRequest { get; set; }

        public DateTime StartDate { get; set; }

        public int Status { get; set; }

        public decimal? SurchargeFee { get; set; }

        public string SurchargeName { get; set; }

        public decimal? TaxFee { get; set; }

        public decimal? ThirdPersonFee { get; set; }

        public decimal? TotalRefund { get; set; }

        public int Type { get; set; }
        public bool IsTourOperator { get; set; }


        public Order()
		{
			this.OrderDetail = new List<Vinaday.Data.Models.OrderDetail>();
		}
	}
}