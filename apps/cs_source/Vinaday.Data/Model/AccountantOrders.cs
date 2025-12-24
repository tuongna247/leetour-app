using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class AccountantOrders : Entity
	{
		public string Cancellation
		{
			get;
			set;
		}

		public DateTime CheckIn
		{
			get;
			set;
		}

		public DateTime CheckOut
		{
			get;
			set;
		}

		public decimal? Deposit
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public decimal? Discount
		{
			get;
			set;
		}

		public decimal? ExtraBedFee
		{
			get;
			set;
		}

		public string GuestCountry
		{
			get;
			set;
		}

		public string GuestEmail
		{
			get;
			set;
		}

		public string GuestFullName
		{
			get;
			set;
		}

		public string GuestPhone
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public string Method
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public int ParentId
		{
			get;
			set;
		}

		public string Pnr
		{
			get;
			set;
		}

		public decimal RateExchange
		{
			get;
			set;
		}

		public decimal? SurchargeFee
		{
			get;
			set;
		}

		public decimal? TaxFee
		{
			get;
			set;
		}

		public decimal? ThirdPersonFee
		{
			get;
			set;
		}

		public int Type
		{
			get;
			set;
		}

		public AccountantOrders()
		{
		}
	}
}