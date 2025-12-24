using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Extention
{
    public class OrderModel
    {
        public string CouponCode { get; set; }


        public string Address
        {
            get;
            set;
        }

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

        public string Avatar
        {
            get;
            set;
        }

        public decimal? Balance
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

        public string CreatedDate
        {
            get;
            set;
        }

        public Vinaday.Data.Models.Customer Customer
        {
            get;
            set;
        }

        public int? CustomerId
        {
            get;
            set;
        }

        public decimal? Deposit
        {
            get;
            set;
        }

        public string Discount
        {
            get;
            set;
        }

        public string DiscountName
        {
            get;
            set;
        }

        public string EndDate
        {
            get;
            set;
        }

        public decimal? ExtraBed
        {
            get;
            set;
        }

        public string FullName
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

        public int Id
        {
            get;
            set;
        }

        public string ImagePath
        {
            get;
            set;
        }

        public decimal? InCome
        {
            get;
            set;
        }

        public string IpLocation
        {
            get;
            set;
        }

        public bool? IsRead
        {
            get;
            set;
        }

        public bool? IsRefund
        {
            get;
            set;
        }

        public bool? IsReview
        {
            get;
            set;
        }

        public int? LocalType
        {
            get;
            set;
        }

        public string Management
        {
            get;
            set;
        }

        public string MemberId
        {
            get;
            set;
        }

        public int? Night
        {
            get;
            set;
        }

        public string Note
        {
            get;
            set;
        }

        public List<OrderDetail> OrderDetails
        {
            get;
            set;
        }

        public int OrderId
        {
            get;
            set;
        }

        public List<Vinaday.Data.Models.OrderInformation2s> OrderInformation2s
        {
            get;
            set;
        }

        public List<Vinaday.Data.Models.OrderInformations> OrderInformations
        {
            get;
            set;
        }

        public decimal? OutCome
        {
            get;
            set;
        }

        public bool? OwnerStay
        {
            get;
            set;
        }

        public int PaymentMethod
        {
            get;
            set;
        }

        public string Pnr
        {
            get;
            set;
        }

        public decimal? Price
        {
            get;
            set;
        }

        public int ProductId
        {
            get;
            set;
        }

        public string ProductLink
        {
            get;
            set;
        }

        public string ProductName
        {
            get;
            set;
        }

        public int? Quantity
        {
            get;
            set;
        }

        public decimal? RateExchange
        {
            get;
            set;
        }

        public string ReceiptId
        {
            get;
            set;
        }

        public Vinaday.Data.Models.Room Room
        {
            get;
            set;
        }

        public string SpecialRequest
        {
            get;
            set;
        }
        public string DepartureOption { get; set; }
        public string GroupType { get; set; }
        public string StartDate
        {
            get;
            set;
        }

        public DateTime StartDateCompare
        {
            get;
            set;
        }

        public int Status
        {
            get;
            set;
        }

        public string SurchargeFee
        {
            get;
            set;
        }

        public string SurchargeName
        {
            get;
            set;
        }

        public string TaxFee
        {
            get;
            set;
        }

        public decimal? ThirdPersonFee
        {
            get;
            set;
        }

        public decimal? Total
        {
            get;
            set;
        }

        public decimal? TotalRefund
        {
            get;
            set;
        }

        public Vinaday.Data.Models.Tour Tour
        {
            get;
            set;
        }

        public int TourOrderId
        {
            get;
            set;
        }

        public int Type
        {
            get;
            set;
        }

        public OrderModel()
        {
        }
    }
}