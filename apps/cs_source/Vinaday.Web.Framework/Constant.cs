using System;
using System.Collections.Generic;

namespace Vinaday.Web.Framework
{
	public class Constant
	{
		public static readonly string ImageRoomtype;
		public static readonly string ImageMaptype;

		public static readonly string ImageMainphoto;

		public static readonly string ImageRestaurant;

		public static readonly string SessionHotelCheckin;

		public static readonly string SessionDaytripCheckin;

		public static readonly string SessionDaytripStartime;

		public static readonly string SessionTourCheckin;

		public static readonly string SessionHotelCheckout;

		public static readonly string SessionHotelNight;

		public static readonly string SessionHotelKeyword;

		public static readonly string SessionTourKeyword;

		public static readonly string SessionDaytripKeyword;

		public static readonly string SessionHotelDetailRoom;

		public static readonly string SessionLanguage;

		public static readonly string SessionCustomer;

		public static readonly string SessionExchangeRate;

		public static readonly string SessionPayment;

		public static readonly string ProductSession;

		public static readonly string ProductSessionEn;

		public static readonly string ShoppingCartUrl;

		public static decimal TaxFee;

		public static readonly string LocalSecureSecret;

		public static readonly string LocalMerchant;

		public static readonly string InternationalSecureSecretUsd;

		public static readonly string InternationalAccessCodeUsd;

		public static readonly string InternationalMerchantIdUsd;

		public static readonly string InternationalSecureSecretVnd;

		public static readonly string InternationalAccesscodeVnd;

		public static readonly string InternationalMerchantIdVnd;

		public static readonly string PromotionId;

		public static Dictionary<int, string> CancellationReason
		{
			get
			{
				return new Dictionary<int, string>()
				{
					{ 1, "Will book with hotel directly" },
					{ 2, "Forced to cancel or postpone trip" },
					{ 3, "Decided on a different hotel not offered by your site" },
					{ 4, "Will book a different hotel through your site" },
					{ 5, "Found lower price on the Internet" },
					{ 6, "Found lower price through a local agent" },
					{ 7, "Did not like payment terms" },
					{ 8, "Concerns about reliability / trustworthinessDid not like cancellation terms" },
					{ 9, "Concerns about safety at the hotel's location" },
					{ 10, "Booking was not confirmed quickly enough" },
					{ 11, "Booking was not confirmed quickly enough" },
					{ 12, "Natural Disaster" },
					{ 13, "Other" }
				};
			}
		}

        public static string HotelCouponSession { get; private set; }

        static Constant()
		{
			Constant.ImageRoomtype = "RoomType";
			Constant.ImageMaptype = "Map";
			Constant.ImageMainphoto = "MainPhoto";
			Constant.ImageRestaurant = "Restaurant";
			Constant.SessionHotelCheckin = "HotelCheckin";
			Constant.SessionDaytripCheckin = "DaytripCheckin";
			Constant.SessionDaytripStartime = "DaytripStartTime";
			Constant.SessionTourCheckin = "TourCheckin";
			Constant.SessionHotelCheckout = "HotelCheckout";
			Constant.SessionHotelNight = "HotelNight";
			Constant.SessionHotelKeyword = "HotelName";
			Constant.SessionTourKeyword = "TourName";
			Constant.SessionDaytripKeyword = "DaytripName";
			Constant.SessionHotelDetailRoom = "HoteldetailRoom";
			Constant.SessionLanguage = "CurrentLanguage";
			Constant.SessionCustomer = "CustomerSession";
			Constant.SessionExchangeRate = "ExchangeRateSession";
			Constant.SessionPayment = "PaymentSession";
			Constant.ProductSession = "ProductSession";
            Constant.HotelCouponSession = "HotelCouponSession";
			Constant.ProductSessionEn = "ProductSessionEn";
			Constant.ShoppingCartUrl = "ShoppingCartUrl";
			Constant.TaxFee = new decimal(115, 0, 0, false, 2);
			Constant.LocalSecureSecret = "A3EFDFABA8653DF2342E8DAC29B51AF0";
			Constant.LocalMerchant = "D67342C2";
			Constant.InternationalSecureSecretUsd = "AF7492A64462CB06103EE42744B8EC85";
			Constant.InternationalAccessCodeUsd = "44B0BBEC";
			Constant.InternationalMerchantIdUsd = "VINADAY";
			Constant.InternationalSecureSecretVnd = "AAC6AE26C96CF38ED4B5BDB84275A168";
			Constant.InternationalAccesscodeVnd = "8B5B0792";
			Constant.InternationalMerchantIdVnd = "VINADAYVND";
			Constant.PromotionId = "PromotionId";
		}

		public Constant()
		{
		}
	}
}