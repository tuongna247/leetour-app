using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;
using Vinaday.Data.Models;
using Vinaday.Data.Models.Extention;
using Vinaday.Data.Models.Mapping;
using Vinaday.Services;
using Vinaday.Web.Framework;
using Vinaday.Web.Framework.EmailHelpers;
using Vinaday.Web.Home;
using Vinaday.Web.Home.App_Start;
using static Vinaday.Web.Framework.Utilities;
using Utilities = Vinaday.Web.Framework.Utilities;

namespace Vinaday.Web.Home.Controllers
{
    public class SecureController : Controller
    {
        #region Constructor

        private string CacheCustomer = "_cacheCustomer";
        private readonly IBookingService _bookingService;
        private readonly IOrderInformationService _orderInformationService;
        private readonly IOrderInformationService2 _orderInformationService2;
        private readonly INationalityService _nationalityService;
        private readonly ICustomerService _customerService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly IOrderService _orderService;
        private readonly IOrderService2 _orderService2;
        private readonly IHotelService _hotelService;
        private readonly ICreditCardService _creditCardService;
        private readonly ITourService _tourService;
        private readonly IMediaService _mediaService;
        private readonly ISpecialRateService _specialRateService;
        private readonly ITourRateService _tourRateService;
        private readonly ICancellationService _cancellationService;
        private readonly ITourSurchargeService _tourSurchargeService;
        private readonly HotelPackageService _hotelPackageService;
        private readonly ITourPromotionService _tourPromotionService;
        private readonly CouponCodeService _couponcodeService;
        private readonly ICountryService _countryService;
        private readonly IRateExchangeService _exchangeService;
        private readonly IRateExchangeService _rateExchangeService;
        private readonly HotelCouponService _hotelCouponService;
        private readonly TourRateOptionsService _tourrateOptionsService;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public SecureController(
            IBookingService bookingService,
            INationalityService nationalityService,
            ICustomerService customerService,
            IUnitOfWorkAsync unitOfWorkAsync,
            IOrderService orderService,
            IOrderService2 orderService2,
            IOrderInformationService orderInformationService,
            IOrderInformationService2 orderInformationService2,
            IHotelService hotelService,
            ICreditCardService creditCardService,
            ITourService tourService,
            IMediaService mediaService,
            ISpecialRateService specialRateService,
            ITourRateService tourRateService,
            HotelPackageService hotelPackageService,
            ICancellationService cancellationService,
            CouponCodeService couponcodeService,
            IRateExchangeService rateExchangeService,
            ICountryService countryService, ITourSurchargeService tourSurchargeService, ITourPromotionService tourPromotionService,
            IRateExchangeService exchangeService,
            HotelCouponService hotelCouponService,
            TourRateOptionsService tourrateOptionsService
            )
        {
            _bookingService = bookingService;
            _nationalityService = nationalityService;
            _customerService = customerService;
            _unitOfWorkAsync = unitOfWorkAsync;
            _orderService = orderService;
            _orderService2 = orderService2;
            _orderInformationService = orderInformationService;
            _orderInformationService2 = orderInformationService2;
            _hotelService = hotelService;
            _creditCardService = creditCardService;
            _tourService = tourService;
            _mediaService = mediaService;
            _specialRateService = specialRateService;
            _tourRateService = tourRateService;
            _cancellationService = cancellationService;
            _countryService = countryService;
            _rateExchangeService = rateExchangeService;
            _couponcodeService = couponcodeService;
            _tourPromotionService = tourPromotionService;
            _tourSurchargeService = tourSurchargeService;
            _hotelPackageService = hotelPackageService;
            _hotelCouponService = hotelCouponService;
            _exchangeService = exchangeService;
            _tourrateOptionsService = tourrateOptionsService;
        }

        #endregion

        #region For Hotel

        public ActionResult Index()
        {
            ViewBag.Title = "Vinaday Secure";

            return View();
        }

        [HttpPost]
        public JsonResult ConfirmHotelPackage(Customer customer)
        {
            var _cacheCustomer = customer;
            var product = (ProductModel)Session[Constant.ProductSession];
            if (product == null || customer == null)
            {
                return Json(new { success = false, error = "Error when processing booking" }, JsonRequestBehavior.AllowGet);
            }
            //Add customer
            var specialRequest = customer.Street; //Rendering as special request property
            var existCustomer = _customerService.GetCustomerByEmail(customer.Email);
            if (existCustomer == null)
            {
                customer.ObjectState = ObjectState.Added;
                _customerService.Insert(customer);
                try
                {
                    _unitOfWorkAsync.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Json(new { success = false, error = "Error when processing booking" },
                        JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                customer = existCustomer;
            }
            //Add Order
            var pnr = Framework.Utilities.GetRandomString(7).ToUpper(); //Random PNR order
            var rateExchange = _rateExchangeService.GetRateExchangeById(3);
            var order = new Order
            {
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Status = (int)Framework.Utilities.BookingStatus.Holding,
                Quantity = product.Quantity,
                CustomerId = customer.CustomerId,
                Pnr = pnr,
                RateExchange = rateExchange != null ? rateExchange.CurrentPrice : 0,
                TaxFee = product.TotalTaxeFee,
                Price = rateExchange != null && rateExchange.CurrentPrice > 0 ? product.TotalPrice / rateExchange.CurrentPrice : 1,
                ProductId = product.Id,
                Amount = rateExchange != null && rateExchange.CurrentPrice > 0 ? product.FinalPrice / rateExchange.CurrentPrice : 1,
                Type = (int)Framework.Utilities.Product.HotelPackage,
                StartDate = product.CheckIn,
                EndDate = product.CheckOut,
                Night = product.Night,
                SurchargeFee = rateExchange != null && rateExchange.CurrentPrice > 0 ? product.TotalSurcharge / rateExchange.CurrentPrice : 1,
                IpLocation = Framework.Utilities.GetUserIp(Request),
                SpecialRequest = specialRequest,
                Discount = rateExchange != null && rateExchange.CurrentPrice > 0 ? product.TotalSave / rateExchange.CurrentPrice : 1,
                CancellationPolicy = product.CancellationPolicy != null ? product.CancellationPolicy.Description : string.Empty,
                ProductName = product.Name + " " + product.DetailName,
                DiscountName = product.Promotion != null ? $"{product.Promotion.Name}<br/>{product.Promotion.Description}" : string.Empty,
                ObjectState = ObjectState.Added,
                LocalType = (int)Utilities.Language.English
            };

            //Insert hotel order
            _orderService.Add(order);

            var order2 = new Order2
            {
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Status = (int)Framework.Utilities.BookingStatus.Holding,
                Quantity = product.Quantity,
                CustomerId = customer.CustomerId,
                Pnr = pnr,
                TaxFee = product.TotalTaxeFee,
                Price = product.TotalPrice,
                ProductId = product.Id,
                Amount = rateExchange != null && rateExchange.CurrentPrice > 0 ? product.TotalPrice / rateExchange.CurrentPrice : 1,
                RateExchange = rateExchange != null ? rateExchange.CurrentPrice : 0,
                Type = (int)Framework.Utilities.Product.HotelPackage,
                StartDate = product.CheckIn,
                EndDate = product.CheckOut,
                Night = product.Night,
                SurchargeFee = rateExchange != null && rateExchange.CurrentPrice > 0 ? product.TotalSurcharge / rateExchange.CurrentPrice : 1,
                IpLocation = Framework.Utilities.GetUserIp(Request),
                SpecialRequest = specialRequest,
                Discount = rateExchange != null && rateExchange.CurrentPrice > 0 ? product.TotalSave / rateExchange.CurrentPrice : 1,
                CancellationPolicy = product.CancellationPolicy != null ? product.CancellationPolicy.Description : string.Empty,
                ProductName = product.Name + " " + product.DetailName,
                DiscountName = product.Promotion != null ? $"{product.Promotion.Name}<br/>{product.Promotion.Description}" : string.Empty,
                ObjectState = ObjectState.Added,
                LocalType = (int)Utilities.Language.English,
                TourOrderId = order.Id

            };
            //Insert hotel order
            _orderService2.Add(order2);
            var productDetails = product.Details;
            if (productDetails.Count > 0)
            {
                foreach (var informationOrder in productDetails.Select(detail => new OrderInformations()
                {
                    OrderId = order.Id,
                    Price = rateExchange != null && rateExchange.CurrentPrice > 0 ? detail.PriceRoom / rateExchange.CurrentPrice : 1,
                    Date = detail.Date.ToString("MM/dd/yyyy"),
                    Quantity = product.Quantity ?? 0,
                    Discount = rateExchange != null && rateExchange.CurrentPrice > 0 ? product.TotalSave / rateExchange.CurrentPrice : 1,
                    DiscountName =
                        product.Promotion != null
                            ? string.Format("{0}<br/>{1}", product.Promotion.Name, product.Promotion.Description)
                            : string.Empty,
                    Surcharge = detail.PriceSurcharge > 0 ? detail.PriceSurcharge : 0,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    Status = (int)Framework.Utilities.Status.Active,
                    ObjectState = ObjectState.Added
                }))
                {
                    //Insert information order
                    _orderInformationService.Insert(informationOrder);
                }


                // save for accountant bill
                foreach (var informationOrder in productDetails.Select(detail => new OrderInformation2s()
                {
                    OrderId = order.Id,
                    Price = rateExchange != null && rateExchange.CurrentPrice > 0 ? detail.PriceRoom / rateExchange.CurrentPrice : 1,
                    Date = detail.Date.ToString("MM/dd/yyyy"),
                    Quantity = product.Quantity ?? 0,
                    Discount = rateExchange != null && rateExchange.CurrentPrice > 0 ? product.TotalSave / rateExchange.CurrentPrice : 1,
                    DiscountName =
                        product.Promotion != null
                            ? string.Format("{0}<br/>{1}", product.Promotion.Name, product.Promotion.Description)
                            : string.Empty,
                    Surcharge = detail.PriceSurcharge > 0 ? rateExchange != null && rateExchange.CurrentPrice > 0 ? detail.PriceSurcharge / rateExchange.CurrentPrice : 1 : 0,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    Status = (int)Framework.Utilities.Status.Active,
                    ObjectState = ObjectState.Added
                }))
                {
                    //Insert information order
                    _orderInformationService2.Insert(informationOrder);
                }
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Json(new { success = false, error = "Error when processing booking" }, JsonRequestBehavior.AllowGet);
            }
            // Session.Clear();
            var nationality = _nationalityService.GetNationality(customer.NationalId ?? -1);
            //Send email request for vinaday team.
            try
            {
                dynamic myObject = new
                {
                    DateTime = DateTime.Now.ToString("D", CultureInfo.CreateSpecificCulture("en-US")),
                    CustomerFullName = $"{customer.Firstname} {customer.Lastname}".ToUpper(),
                    HotelName = product.Name,
                    CheckIn = product.CheckIn,
                    CheckOut = product.CheckOut,
                    Pnr = order.Pnr,
                    Stay = product.Stay,
                    Address = product.Location,
                    RoomName = product.DetailName,
                    RoomPrice = product.TotalPrice,
                    TaxesPrice = product.TotalTaxeFee,
                    Surcharge = product.TotalSurcharge > 0 ? product.TotalSurcharge : 0,
                    Include = product.Include,
                    Quantity = product.Quantity,
                    Gender = _cacheCustomer.Gender,
                    IsThirdPerson = _cacheCustomer.IsThirdPerson,
                    ThirdPersonGender = _cacheCustomer.ThirdPersonGender,
                    ThirdPersonFirstname = _cacheCustomer.ThirdPersonFirstname,
                    ThirdPersonLastname = _cacheCustomer.ThirdPersonLastname,
                    IsGiftVoucher = _cacheCustomer.IsGiftVoucher,
                    Gift_type = _cacheCustomer.Gift_type,
                    PersonalMessage = _cacheCustomer.PersonalMessage,
                    FinalPrice = product.FinalPrice,
                    //GuestLead = customer.l,
                    Nationality = nationality != null ? nationality.Name : "",
                    SpecialRequest = order.SpecialRequest,
                    Email = customer.Email,
                    Phone = customer.PhoneNumber,

                    Cancellation = product.CancellationPolicy
                };
                string template = "BookingRequestVN";//customer.VNSite? "BookingRequestVN" : "BookingRequest";
                string bookingEmailResult = Framework.Utilities.ParseTemplate(template, myObject);
                var subject = $"VINADAY goREISE - Booking #{order.Pnr}";
                MailClient.RequestBookingHotel(customer.Email, bookingEmailResult, subject);
            }
            catch (Exception ex)
            {
                var test = ex.ToString();

                //log error here
            }
            var resutls = new { success = true, customer = $"{customer.Firstname} {customer.Lastname}", pnr };
            var jsonResult = Json(resutls, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult Confirm(Customer customer)
        {
            Session[this.CacheCustomer] = customer;
            var product = (ProductModel)Session[Constant.ProductSession];

            if (product == null || customer == null)
            {
                return Json(new { success = false, error = "Error when processing booking" }, JsonRequestBehavior.AllowGet);
            }
            //Add customer
            var specialRequest = customer.Street; //Rendering as special request property
            var existCustomer = _customerService.GetCustomerByEmail(customer.Email);
            if (existCustomer == null)
            {
                customer.ObjectState = ObjectState.Added;
                _customerService.Insert(customer);
                try
                {
                    _unitOfWorkAsync.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Json(new { success = false, error = "Error when processing booking" },
                        JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                customer = existCustomer;
            }
            //Add Order
            var pnr = Framework.Utilities.GetRandomString(7).ToUpper(); //Random PNR order
            var order = new Order
            {
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Status = (int)Framework.Utilities.BookingStatus.Holding,
                Quantity = product.Quantity,
                CustomerId = customer.CustomerId,
                Pnr = pnr,
                TaxFee = product.TotalTaxeFee,
                Price = product.TotalPrice,
                ProductId = product.Id,
                Amount = product.FinalPrice,
                Type = (int)Framework.Utilities.Product.Hotel,
                StartDate = product.CheckIn,
                EndDate = product.CheckOut,
                Night = product.Night,
                SurchargeFee = product.TotalSurcharge,
                IpLocation = Framework.Utilities.GetUserIp(Request),
                SpecialRequest = specialRequest,
                Discount = product.TotalSave,
                CancellationPolicy = product.CancellationPolicy != null ? product.CancellationPolicy.Description : string.Empty,
                ProductName = product.Name,
                DiscountName = product.Promotion != null ? $"{product.Promotion.Name}<br/>{product.Promotion.Description}" : string.Empty,
                ObjectState = ObjectState.Added,
                LocalType = (int)Utilities.Language.English
            };

            //Insert hotel order
            _orderService.Add(order);

            var order2 = new Order2
            {
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Status = (int)Framework.Utilities.BookingStatus.Holding,
                Quantity = product.Quantity,
                CustomerId = customer.CustomerId,
                Pnr = pnr,
                TaxFee = product.TotalTaxeFee,
                Price = product.TotalPrice,
                ProductId = product.Id,
                Amount = product.FinalPrice,
                Type = (int)Framework.Utilities.Product.Hotel,
                StartDate = product.CheckIn,
                EndDate = product.CheckOut,
                Night = product.Night,
                SurchargeFee = product.TotalSurcharge,
                IpLocation = Framework.Utilities.GetUserIp(Request),
                SpecialRequest = specialRequest,
                Discount = product.TotalSave,
                CancellationPolicy = product.CancellationPolicy != null ? product.CancellationPolicy.Description : string.Empty,
                ProductName = product.Name,
                DiscountName = product.Promotion != null ? $"{product.Promotion.Name}<br/>{product.Promotion.Description}" : string.Empty,
                ObjectState = ObjectState.Added,
                LocalType = (int)Utilities.Language.English,
                TourOrderId = order.Id

            };
            //Insert hotel order
            _orderService2.Add(order2);
            var productDetails = product.Details;
            if (productDetails.Count > 0)
            {
                foreach (var informationOrder in productDetails.Select(detail => new OrderInformations()
                {
                    OrderId = order.Id,
                    Price = detail.PriceRoom,
                    Date = detail.Date.ToString("MM/dd/yyyy"),
                    Quantity = product.Quantity ?? 0,
                    Discount = product.TotalSave,
                    DiscountName =
                        product.Promotion != null
                            ? string.Format("{0}<br/>{1}", product.Promotion.Name, product.Promotion.Description)
                            : string.Empty,
                    Surcharge = detail.PriceSurcharge > 0 ? detail.PriceSurcharge : 0,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    Status = (int)Framework.Utilities.Status.Active,
                    ObjectState = ObjectState.Added
                }))
                {
                    //Insert information order
                    _orderInformationService.Insert(informationOrder);
                }


                // save for accountant bill
                foreach (var informationOrder in productDetails.Select(detail => new OrderInformation2s()
                {
                    OrderId = order.Id,
                    Price = detail.PriceRoom,
                    Date = detail.Date.ToString("MM/dd/yyyy"),
                    Quantity = product.Quantity ?? 0,
                    Discount = product.TotalSave,
                    DiscountName =
                        product.Promotion != null
                            ? string.Format("{0}<br/>{1}", product.Promotion.Name, product.Promotion.Description)
                            : string.Empty,
                    Surcharge = detail.PriceSurcharge > 0 ? detail.PriceSurcharge : 0,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    Status = (int)Framework.Utilities.Status.Active,
                    ObjectState = ObjectState.Added
                }))
                {
                    //Insert information order
                    _orderInformationService2.Insert(informationOrder);
                }
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Json(new { success = false, error = "Error when processing booking" }, JsonRequestBehavior.AllowGet);
            }
            var _cacheCustomer = (Customer)Session[this.CacheCustomer];
            var nationality = _nationalityService.GetNationality(customer.NationalId ?? -1);
            Session.Clear();
            //Send email request for vinaday team.
            try
            {
                dynamic myObject = new
                {
                    DateTime = DateTime.Now.ToString("D", CultureInfo.CreateSpecificCulture("en-US")),
                    CustomerFullName = $"{customer.Firstname} {customer.Lastname}".ToUpper(),
                    HotelName = product.Name,
                    CheckIn = product.CheckIn,
                    CheckOut = product.CheckOut,
                    Pnr = order.Pnr,
                    Stay = product.Stay,
                    Address = product.Location,
                    RoomName = product.DetailName,
                    RoomPrice = product.TotalPrice,
                    TaxesPrice = product.TotalTaxeFee,
                    Surcharge = product.TotalSurcharge > 0 ? product.TotalSurcharge : 0,
                    FinalPrice = product.FinalPrice,
                    Email = customer.Email,
                    Phone = customer.PhoneNumber,
                    Cancellation = product.CancellationPolicy,
                    Gender = _cacheCustomer.Gender,

                    //GuestLead = customer.l,
                    Nationality = nationality != null ? nationality.Name : "",
                };
                string bookingEmailResult = Framework.Utilities.ParseTemplate("BookingRequest", myObject);
                var subject = $"VINADAY goREISE - Booking #{order.Pnr}";
                //var subject = $"Suite Back Packers Inn - Booking #{order.Pnr}";
                MailClient.RequestBookingHotel(customer.Email, bookingEmailResult, subject);
            }
            catch (Exception ex)
            {
                var test = ex.ToString();

                //log error here
            }
            var resutls = new { success = true, customer = $"{customer.Firstname} {customer.Lastname}", pnr };
            var jsonResult = Json(resutls, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult ConfirmWithCreditCard(Customer customer, CreditCard creditCard)
        {
            var product = (ProductModel)Session[Constant.ProductSession];
            if (product == null || customer == null || creditCard == null)
            {
                return Json(new { success = false, error = "Error when processing booking" }, JsonRequestBehavior.AllowGet);
            }
            //Add CreditCard

            var card = InsertCreditCard(creditCard);
            if (ValidateRequest)
            {

            }
            //Add customer
            var specialRequest = customer.Street; //Rendering as special request property
            var existCustomer = _customerService.GetCustomerByEmail(customer.Email);
            if (existCustomer == null)
            {
                customer.ObjectState = ObjectState.Added;
                _customerService.Insert(customer);
                try
                {
                    _unitOfWorkAsync.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Json(new { success = false, error = "Error when processing booking" },
                        JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                customer = existCustomer;
            }
            //Add Order
            var pnr = Framework.Utilities.GetRandomString(7).ToUpper(); //Random PNR order
            var order = new Order
            {
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Status = (int)Framework.Utilities.BookingStatus.Holding,
                Quantity = product.Quantity,
                CustomerId = customer.CustomerId,
                Pnr = pnr,
                TaxFee = product.TotalTaxeFee,
                Price = product.TotalPrice,
                ProductId = product.Id,
                Amount = product.FinalPrice,
                Type = (int)Framework.Utilities.Product.Hotel,
                StartDate = product.CheckIn,
                EndDate = product.CheckOut,
                Night = product.Night,
                SurchargeFee = product.TotalSurcharge > 0 ? product.TotalSurcharge : 0,
                IpLocation = Framework.Utilities.GetUserIp(Request),
                SpecialRequest = specialRequest,
                Discount = product.TotalSave,
                CancellationPolicy = product.CancellationPolicy != null ? product.CancellationPolicy.Description : string.Empty,
                ProductName = product.Name,
                DiscountName = product.Promotion != null ? $"{product.Promotion.Name}<br/>{product.Promotion.Description}" : string.Empty,
                CardId = card.Id,
                PaymentMethod = (int)Utilities.PaymentMethod.Credit,
                ObjectState = ObjectState.Added,
                LocalType = (int)Utilities.Language.English,
                CardNumber = !string.IsNullOrEmpty(card.CardNumber) ?
                    $"xxxx-xxxx-xxxx-x{Utilities.GetLast(card.CardNumber, 3)}"
                    : string.Empty
            };

            //Insert hotel order
            _orderService.Add(order);

            var order2 = new Order2
            {
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Status = (int)Framework.Utilities.BookingStatus.Holding,
                Quantity = product.Quantity,
                CustomerId = customer.CustomerId,
                Pnr = pnr,
                TaxFee = product.TotalTaxeFee,
                Price = product.TotalPrice,
                ProductId = product.Id,
                Amount = product.FinalPrice,
                Type = (int)Framework.Utilities.Product.Hotel,
                StartDate = product.CheckIn,
                EndDate = product.CheckOut,
                Night = product.Night,
                SurchargeFee = product.TotalSurcharge > 0 ? product.TotalSurcharge : 0,
                IpLocation = Framework.Utilities.GetUserIp(Request),
                SpecialRequest = specialRequest,
                Discount = product.TotalSave,
                CancellationPolicy = product.CancellationPolicy != null ? product.CancellationPolicy.Description : string.Empty,
                ProductName = product.Name,
                DiscountName = product.Promotion != null ? $"{product.Promotion.Name}<br/>{product.Promotion.Description}" : string.Empty,
                CardId = card.Id,
                PaymentMethod = (int)Utilities.PaymentMethod.Credit,
                ObjectState = ObjectState.Added,
                LocalType = (int)Utilities.Language.English,
                CardNumber = !string.IsNullOrEmpty(card.CardNumber) ?
                    $"xxxx-xxxx-xxxx-x{Utilities.GetLast(card.CardNumber, 3)}"
                    : string.Empty,
                TourOrderId = order.Id

            };
            //Insert hotel order
            _orderService2.Add(order2);
            //Add order information
            var productDetails = product.Details;
            if (productDetails.Count > 0)
            {
                foreach (var informationOrder in productDetails.Select(detail => new OrderInformations()
                {
                    OrderId = order.Id,
                    Price = detail.PriceRoom,
                    Date = detail.Date.ToString("MM/dd/yyyy"),
                    Quantity = product.Quantity ?? 0,
                    Discount = product.TotalSave,
                    DiscountName = product.Promotion != null ? $"{product.Promotion.Name}<br/>{product.Promotion.Description}" : string.Empty,
                    Surcharge = detail.PriceSurcharge > 0 ? detail.PriceSurcharge : 0,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    Status = (int)Framework.Utilities.Status.Active,
                    ObjectState = ObjectState.Added
                }))
                {
                    //Insert information order
                    _orderInformationService.Insert(informationOrder);
                }

                // save for accountant bill
                foreach (var informationOrder in productDetails.Select(detail => new OrderInformation2s()
                {
                    OrderId = order.Id,
                    Price = detail.PriceRoom,
                    Date = detail.Date.ToString("MM/dd/yyyy"),
                    Quantity = product.Quantity ?? 0,
                    Discount = product.TotalSave,
                    DiscountName = product.Promotion != null ? $"{product.Promotion.Name}<br/>{product.Promotion.Description}" : string.Empty,
                    Surcharge = detail.PriceSurcharge > 0 ? detail.PriceSurcharge : 0,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    Status = (int)Framework.Utilities.Status.Active,
                    ObjectState = ObjectState.Added
                }))
                {
                    //Insert information order
                    _orderInformationService2.Insert(informationOrder);
                }
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Json(new { success = false, error = "Error when processing booking" }, JsonRequestBehavior.AllowGet);
            }
            Session.Clear();
            //Send email request for vinaday team.
            try
            {
                dynamic myObject = new
                {
                    DateTime = DateTime.Now.ToString("D", CultureInfo.CreateSpecificCulture("en-US")),
                    CustomerFullName = $"{customer.Firstname} {customer.Lastname}".ToUpper(),
                    HotelName = product.Name,
                    CheckIn = product.CheckIn,
                    CheckOut = product.CheckOut,
                    Pnr = order.Pnr,
                    Stay = product.Stay,
                    Address = product.Location,
                    RoomName = product.DetailName,
                    RoomPrice = product.TotalPrice,
                    TaxesPrice = product.TotalTaxeFee,
                    Surcharge = product.TotalSurcharge > 0 ? product.TotalSurcharge : 0,
                    FinalPrice = product.FinalPrice,
                    Email = customer.Email,
                    Phone = customer.PhoneNumber,
                    Cancellation = product.CancellationPolicy
                };
                string bookingEmailResult = Framework.Utilities.ParseTemplate("BookingRequest", myObject);
                var subject = $"VINADAY goREISE - Booking #{order.Pnr}";
                MailClient.RequestBookingHotel(customer.Email, bookingEmailResult, subject);
            }
            catch (Exception ex)
            {
                var test = ex.ToString();
                return Json(new { success = false, error = "Error when end email" }, JsonRequestBehavior.AllowGet);
                //log error here
            }
            var resutls = new { success = true, customer = $"{customer.Firstname} {customer.Lastname}", pnr };
            var jsonResult = Json(resutls, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        //[Route("shopping-cart/{type}/{id}/{checkIn}/{checkOut}/{total}/{promotion}/{roomOption}/{rateName}")]
        //public ActionResult ShoppingCart(int type, int id, String checkIn, String checkOut, int total, int promotion, string rateName,double roomOption)
        [Route("shopping-cart/{type}/{id}/{checkIn}/{checkOut}/{total}/{promotion}")]
        public ActionResult ShoppingCart(int type, int id, String checkIn, String checkOut, int total, int promotion)
        {
            if (Request.Url != null)
            {
                Session[Constant.ShoppingCartUrl] = Request.Url.AbsoluteUri;
            }

            Session[Constant.ProductSession] = null;
            var product = _bookingService.GetHotelProduct(type, id, checkIn, checkOut, total, promotion);
            ViewBag.Product =
                Session[Constant.ProductSession] = product;


            // return View(product);
            return Redirect("/payment");
        }
        [Route("hotelpackagevn/{id}/{checkIn}/{totalRoom}")]
        public ActionResult HotelPackageVN(int id, string checkIn, int totalRoom)
        {
            if (Request.Url != null)
            {
                Session[Constant.ShoppingCartUrl] = Request.Url.AbsoluteUri;
            }

            Session[Constant.ProductSession] = null;
            ViewBag.Product = Session[Constant.ProductSession] = _bookingService.GetHotelPackageVN(id, checkIn, totalRoom);
            return Redirect("/PaymentPackageVn");
        }
        [Route("hotelpackage/{id}/{checkIn}/{totalRoom}")]
        public ActionResult HotelPackage(int id, string checkIn, int totalRoom)
        {
            if (Request.Url != null)
            {
                Session[Constant.ShoppingCartUrl] = Request.Url.AbsoluteUri;
            }

            Session[Constant.ProductSession] = null;
            ViewBag.Product = Session[Constant.ProductSession] = _bookingService.GetHotelPackage(id, checkIn, totalRoom);
            return Redirect("/PaymentPackage");
        }
        [Route("PaymentPackageVn")]
        public ActionResult PaymentPackageVn()
        {
            ViewBag.Nationalities = _nationalityService.GetNationalityList();
            var product = (ProductModel)Session[Constant.ProductSession];
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.CancellationPolicies = product.TotalSave > 0
                ? new List<CancellationModel>()
                : _hotelService.GetHotelCancellationPoliciesByHotelId(product.ProductId);
            product.ShoppingCartUrl = Server.UrlEncode(Session[Constant.ShoppingCartUrl] as string);
            var customer = new Customer();
            var claim = ((ClaimsIdentity)User.Identity).FindFirst("Id");
            if (claim != null)
            {
                var id = claim.Value;
                customer = _customerService.GetCustomerByMemberId(id);
            }
            ViewBag.Customer = customer;
            return View(product);
        }

        [Route("PaymentPackage")]
        public ActionResult PaymentPackage()
        {
            ViewBag.Nationalities = _nationalityService.GetNationalityList();
            var product = (ProductModel)Session[Constant.ProductSession];
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.CancellationPolicies = product.TotalSave > 0
                ? new List<CancellationModel>()
                : _hotelService.GetHotelCancellationPoliciesByHotelId(product.ProductId);
            product.ShoppingCartUrl = Server.UrlEncode(Session[Constant.ShoppingCartUrl] as string);
            var customer = new Customer();
            var claim = ((ClaimsIdentity)User.Identity).FindFirst("Id");
            if (claim != null)
            {
                var id = claim.Value;
                customer = _customerService.GetCustomerByMemberId(id);
            }
            ViewBag.Customer = customer;
            return View(product);
        }
        [Route("payment")]
        public ActionResult Payment()
        {
            ViewBag.Nationalities = _nationalityService.GetNationalityList();
            var product = (ProductModel)Session[Constant.ProductSession];
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.CancellationPolicies = product.TotalSave > 0
                ? new List<CancellationModel>()
                : _hotelService.GetHotelCancellationPoliciesByHotelId(product.ProductId);
            product.ShoppingCartUrl = Server.UrlEncode(Session[Constant.ShoppingCartUrl] as string);
            var customer = new Customer();
            var claim = ((ClaimsIdentity)User.Identity).FindFirst("Id");
            if (claim != null)
            {
                var id = claim.Value;
                customer = _customerService.GetCustomerByMemberId(id);
            }
            ViewBag.Customer = customer;
            return View(product);
        }
        private CreditCard InsertCreditCard(CreditCard creditCard)
        {
            creditCard.CreatedDate = DateTime.Now;
            creditCard.ModifiedDate = DateTime.Now;
            creditCard.Status = (int)Utilities.Status.Active;
            creditCard.Type = (int)Utilities.PaymentMethod.Credit;
            creditCard.ObjectState = ObjectState.Added;
            _creditCardService.Add(creditCard);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                return creditCard;
            }
            catch (DbUpdateConcurrencyException)
            {
                return new CreditCard();
            }
        }
        [Route("credit-card")]
        public ActionResult CreditCard()
        {
            return View();
        }


        #region Hotel Package Payment

        [Route("checkout-result")]
        public ActionResult CheckOutResult()
        {
            string SECURE_SECRET = "AF7492A64462CB06103EE42744B8EC85";
            string hashvalidateResult = "";
            // Khoi tao lop thu vien
            VPCRequest conn = new VPCRequest("http://onepay.vn");
            conn.SetSecureSecret(SECURE_SECRET);
            // Xu ly tham so tra ve va kiem tra chuoi du lieu ma hoa
            hashvalidateResult = conn.Process3PartyResponse(Request.QueryString);
            // Lay gia tri tham so tra ve tu cong thanh toan
            String vpc_TxnResponseCode = conn.GetResultField("vpc_TxnResponseCode", "Unknown");
            string amount = conn.GetResultField("vpc_Amount", "Unknown");
            string localed = conn.GetResultField("vpc_Locale", "Unknown");
            string command = conn.GetResultField("vpc_Command", "Unknown");
            string version = conn.GetResultField("vpc_Version", "Unknown");
            string cardType = conn.GetResultField("vpc_Card", "Unknown");
            string orderInfo = conn.GetResultField("vpc_OrderInfo", "Unknown");
            string merchantID = conn.GetResultField("vpc_Merchant", "Unknown");
            string authorizeID = conn.GetResultField("vpc_AuthorizeId", "Unknown");
            string merchTxnRef = conn.GetResultField("vpc_MerchTxnRef", "Unknown");
            string transactionNo = conn.GetResultField("vpc_TransactionNo", "Unknown");
            string acqResponseCode = conn.GetResultField("vpc_AcqResponseCode", "Unknown");
            string txnResponseCode = vpc_TxnResponseCode;
            string message = conn.GetResultField("vpc_Message", "Unknown");
            ViewBag.vpc_Version = version;
            if (!string.IsNullOrEmpty(amount) && amount != "Unknown")
            {
                ViewBag.vpc_Amount = decimal.Parse(amount) / 100;
            }
            else
            {
                ViewBag.vpc_Amount = 0;
            }

            ViewBag.vpc_Command = command;
            ViewBag.vpc_MerchantID = merchantID;
            ViewBag.vpc_MerchantRef = merchTxnRef;
            ViewBag.vpc_OderInfor = orderInfo;
            ViewBag.vpc_ResponseCode = txnResponseCode;
            ViewBag.vpc_Command = command;
            ViewBag.vpc_TransactionNo = transactionNo;
            ViewBag.hashvalidate = hashvalidateResult;
            ViewBag.vpc_Message = message;
            string status = "";
            /// If success change status to completed
            /// else redirecto to booking page.
            //merchTxnRef = payment_id - 19561
            if (!string.IsNullOrEmpty(merchTxnRef) && merchTxnRef.Contains("-"))
            {
                string paymentId = merchTxnRef.Split('-')[1];
                var order = _orderService.GetOrder(Int32.Parse(paymentId.Trim()));
                ViewBag.FullName = order.FullName;
                ViewBag.Pnr = order.Pnr;
                if (hashvalidateResult == "CORRECTED" && txnResponseCode.Trim() == "0")
                {
                    var orderUpdate = _orderService.GetOrderById(Int32.Parse(paymentId.Trim()));
                    orderUpdate.Status = (int)Framework.Utilities.BookingStatus.Paid;
                    var rateExchange = _rateExchangeService.GetRateExchangeById(3);
                    orderUpdate.RateExchange = rateExchange != null ? rateExchange.CurrentPrice : 0;
                    try
                    {
                        _orderService.Update(orderUpdate);
                        _unitOfWorkAsync.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return Json(new { success = false, error = "Error when processing booking" }, JsonRequestBehavior.AllowGet);
                    }

                    ViewBag.vpc_Result = "Transaction was paid successful";
                    try
                    {
                        var product = (ProductModel)Session[Constant.ProductSession];
                        var customer = order.Customer;
                        var _cacheCustomer = (Customer)Session[this.CacheCustomer];
                        dynamic myObject = new
                        {
                            DateTime = DateTime.Now.ToString("D", CultureInfo.CreateSpecificCulture("en-US")),
                            CustomerFullName = $"{customer.Firstname} {customer.Lastname}".ToUpper(),
                            HotelName = product.Name,
                            CheckIn = product.CheckIn.ToString("D"),
                            CheckOut = product.CheckOut.ToString("D"),
                            Pnr = order.Pnr,
                            Stay = product.Stay,
                            Address = product.Location,
                            RoomName = product.DetailName,
                            RoomPrice = product.TotalPrice,
                            TaxesPrice = product.TotalTaxeFee,
                            Surcharge = product.TotalSurcharge > 0 ? product.TotalSurcharge : 0,
                            FinalPrice = product.FinalPrice,
                            Email = customer.Email,
                            Phone = customer.PhoneNumber,
                            IsThirdPerson = _cacheCustomer.IsThirdPerson,
                            ThirdPersonGender = _cacheCustomer.ThirdPersonGender,
                            ThirdPersonFirstname = _cacheCustomer.ThirdPersonFirstname,
                            ThirdPersonLastname = _cacheCustomer.ThirdPersonLastname,
                            IsGiftVoucher = _cacheCustomer.IsGiftVoucher,
                            Gift_type = _cacheCustomer.Gift_type,
                            Cancellation = product.CancellationPolicy
                        };
                        string bookingEmailResult = Framework.Utilities.ParseTemplate("BookingRequest", myObject);
                        var subject = $"VINADAY goREISE - Booking #{order.Pnr}";
                        MailClient.RequestBookingHotel(customer.Email, bookingEmailResult, subject);
                    }
                    catch (Exception ex)
                    {
                        var test = ex.ToString();

                        //log error here
                    }
                }
                else if (hashvalidateResult == "INVALIDATED" && txnResponseCode.Trim() == "0")
                {
                    ViewBag.vpc_Result = "Transaction is pending";
                    var hotelPackage = _hotelPackageService.ODataQueryable().FirstOrDefault(a => a.Id == order.ProductId);
                    if (hotelPackage != null)
                    {
                        var hotel = _hotelService.GetHotelSingle(hotelPackage.HotelId);
                        string HotelUrl = $"https://goreise.com/hotel/{Utilities.GenerateSlug(hotel.Name)}-p{hotel.Id}";
                        return Redirect(HotelUrl);
                    }
                }
                else
                {

                    var hotelPackage = _hotelPackageService.ODataQueryable().FirstOrDefault(a => a.Id == order.ProductId);
                    if (hotelPackage != null)
                    {
                        var hotel = _hotelService.GetHotelSingle(hotelPackage.HotelId);
                        string HotelUrl = $"https://goreise.com/hotel/{Utilities.GenerateSlug(hotel.Name)}-p{hotel.Id}";
                        return Redirect(HotelUrl);
                    }
                }

            }
            return View();
        }

        [Route("checkout-result-dr")]
        public ActionResult CheckOutResultDR()
        {
            string SECURE_SECRET = "AF7492A64462CB06103EE42744B8EC85";
            string hashvalidateResult = "";
            // Khoi tao lop thu vien
            VPCRequest conn = new VPCRequest("http://onepay.vn");
            conn.SetSecureSecret(SECURE_SECRET);
            // Xu ly tham so tra ve va kiem tra chuoi du lieu ma hoa
            hashvalidateResult = conn.Process3PartyResponse(Request.QueryString);
            // Lay gia tri tham so tra ve tu cong thanh toan
            String vpc_TxnResponseCode = conn.GetResultField("vpc_TxnResponseCode", "Unknown");
            string amount = conn.GetResultField("vpc_Amount", "Unknown");
            string localed = conn.GetResultField("vpc_Locale", "Unknown");
            string command = conn.GetResultField("vpc_Command", "Unknown");
            string version = conn.GetResultField("vpc_Version", "Unknown");
            string cardType = conn.GetResultField("vpc_Card", "Unknown");
            string orderInfo = conn.GetResultField("vpc_OrderInfo", "Unknown");
            string merchantID = conn.GetResultField("vpc_Merchant", "Unknown");
            string authorizeID = conn.GetResultField("vpc_AuthorizeId", "Unknown");
            string merchTxnRef = conn.GetResultField("vpc_MerchTxnRef", "Unknown");
            string transactionNo = conn.GetResultField("vpc_TransactionNo", "Unknown");
            string acqResponseCode = conn.GetResultField("vpc_AcqResponseCode", "Unknown");
            string txnResponseCode = vpc_TxnResponseCode;
            string message = conn.GetResultField("vpc_Message", "Unknown");
            ViewBag.vpc_Version = version;
            if (!string.IsNullOrEmpty(amount) && amount != "Unknown")
            {
                ViewBag.vpc_Amount = decimal.Parse(amount) / 100;
            }
            else
            {
                ViewBag.vpc_Amount = 0;
            }

            ViewBag.vpc_Command = command;
            ViewBag.vpc_MerchantID = merchantID;
            ViewBag.vpc_MerchantRef = merchTxnRef;
            ViewBag.vpc_OderInfor = orderInfo;
            ViewBag.vpc_ResponseCode = txnResponseCode;
            ViewBag.vpc_Command = command;
            ViewBag.vpc_TransactionNo = transactionNo;
            ViewBag.hashvalidate = hashvalidateResult;
            ViewBag.vpc_Message = message;
            string status = "";
            /// If success change status to completed
            /// else redirecto to booking page.
            //merchTxnRef = payment_id - 19561
            if (!string.IsNullOrEmpty(merchTxnRef) && merchTxnRef.Contains("-"))
            {
                string paymentId = merchTxnRef.Split('-')[1];
                var order = _orderService.GetOrder(Int32.Parse(paymentId.Trim()));
                ViewBag.FullName = order.FullName;
                ViewBag.Pnr = order.Pnr;
                if (hashvalidateResult == "CORRECTED" && txnResponseCode.Trim() == "0")
                {
                    var orderUpdate = _orderService.GetOrderById(Int32.Parse(paymentId.Trim()));
                    orderUpdate.Status = (int)Framework.Utilities.BookingStatus.Paid;
                    var rateExchange = _rateExchangeService.GetRateExchangeById(3);
                    orderUpdate.RateExchange = rateExchange != null ? rateExchange.CurrentPrice : 0;
                    try
                    {
                        _orderService.Update(orderUpdate);
                        _unitOfWorkAsync.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return Json(new { success = false, error = "Error when processing booking" }, JsonRequestBehavior.AllowGet);
                    }

                    ViewBag.vpc_Result = "Transaction was paid successful";
                    try
                    {
                        var product = (ProductModel)Session[Constant.ProductSession];
                        var customer = order.Customer;
                        var _cacheCustomer = (Customer)Session[this.CacheCustomer];
                        dynamic myObject = new
                        {
                            DateTime = DateTime.Now.ToString("D", CultureInfo.CreateSpecificCulture("en-US")),
                            CustomerFullName = $"{customer.Firstname} {customer.Lastname}".ToUpper(),
                            HotelName = product.Name,
                            CheckIn = product.CheckIn.ToString("D"),
                            CheckOut = product.CheckOut.ToString("D"),
                            Pnr = order.Pnr,
                            Stay = product.Stay,
                            Address = product.Location,
                            RoomName = product.DetailName,
                            RoomPrice = product.TotalPrice,
                            TaxesPrice = product.TotalTaxeFee,
                            Surcharge = product.TotalSurcharge > 0 ? product.TotalSurcharge : 0,
                            FinalPrice = product.FinalPrice,
                            Email = customer.Email,
                            Phone = customer.PhoneNumber,
                            IsThirdPerson = _cacheCustomer.IsThirdPerson,
                            ThirdPersonGender = _cacheCustomer.ThirdPersonGender,
                            ThirdPersonFirstname = _cacheCustomer.ThirdPersonFirstname,
                            ThirdPersonLastname = _cacheCustomer.ThirdPersonLastname,
                            IsGiftVoucher = _cacheCustomer.IsGiftVoucher,
                            Gift_type = _cacheCustomer.Gift_type,
                            Cancellation = product.CancellationPolicy
                        };
                        string bookingEmailResult = Framework.Utilities.ParseTemplate("BookingRequest", myObject);
                        var subject = $"VINADAY goREISE - Booking #{order.Pnr}";
                        MailClient.RequestBookingHotel(customer.Email, bookingEmailResult, subject);
                    }
                    catch (Exception ex)
                    {
                        var test = ex.ToString();

                        //log error here
                    }
                }
                else if (hashvalidateResult == "INVALIDATED" && txnResponseCode.Trim() == "0")
                {
                    ViewBag.vpc_Result = "Transaction is pending";
                    var hotelPackage = _hotelPackageService.ODataQueryable().FirstOrDefault(a => a.Id == order.ProductId);
                    if (hotelPackage != null)
                    {
                        var hotel = _hotelService.GetHotelSingle(hotelPackage.HotelId);
                        string HotelUrl = $"https://goreise.com/hotel/{Utilities.GenerateSlug(hotel.Name)}-p{hotel.Id}";
                        return Redirect(HotelUrl);
                    }
                }
                else
                {

                    var hotelPackage = _hotelPackageService.ODataQueryable().FirstOrDefault(a => a.Id == order.ProductId);
                    if (hotelPackage != null)
                    {
                        var hotel = _hotelService.GetHotelSingle(hotelPackage.HotelId);
                        string HotelUrl = $"https://goreise.com/hotel/{Utilities.GenerateSlug(hotel.Name)}-p{hotel.Id}";
                        return Redirect(HotelUrl);
                    }
                }

            }
            return View();
        }
        

        [HttpPost]
        public JsonResult OnePayPayment(Customer customer)
        {
            #region Save Booking
            var product = (ProductModel)Session[Constant.ProductSession];
            Session[this.CacheCustomer] = customer;
            if (product == null || customer == null)
            {
                return Json(new { success = false, error = "Error when processing booking" }, JsonRequestBehavior.AllowGet);
            }
            //Add customer
            var specialRequest = customer.Street; //Rendering as special request property
            var existCustomer = _customerService.GetCustomerByEmail(customer.Email);
            if (existCustomer == null)
            {
                customer.ObjectState = ObjectState.Added;
                _customerService.Insert(customer);
                try
                {
                    _unitOfWorkAsync.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Json(new { success = false, error = "Error when processing booking" },
                        JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                customer = existCustomer;
            }
            //Add Order
            var pnr = Framework.Utilities.GetRandomString(7).ToUpper(); //Random PNR order
            var order = new Order
            {
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Status = (int)Framework.Utilities.BookingStatus.Failure,
                Quantity = product.Quantity,
                CustomerId = customer.CustomerId,
                Pnr = pnr,
                TaxFee = product.TotalTaxeFee,
                Price = product.TotalPrice,
                ProductId = product.Id,
                Amount = product.FinalPrice,
                Type = (int)Framework.Utilities.Product.Tour,
                StartDate = product.CheckIn,
                EndDate = product.CheckOut,
                Night = product.Night,
                SurchargeFee = product.TotalSurcharge,
                IpLocation = Framework.Utilities.GetUserIp(Request),
                SpecialRequest = specialRequest,
                Discount = product.TotalSave,
                CancellationPolicy = product.CancellationPolicy != null ? product.CancellationPolicy.Description : string.Empty,
                ProductName = product.Name + " " + product.DetailName,
                DiscountName = product.Promotion != null ? $"{product.Promotion.Name}<br/>{product.Promotion.Description}" : string.Empty,
                ObjectState = ObjectState.Added,
                LocalType = (int)Utilities.Language.English
            };

            //Insert hotel order
            _orderService.Add(order);

            var order2 = new Order2
            {
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Status = (int)Framework.Utilities.BookingStatus.Failure,
                Quantity = product.Quantity,
                CustomerId = customer.CustomerId,
                Pnr = pnr,
                TaxFee = product.TotalTaxeFee,
                Price = product.TotalPrice,
                ProductId = product.Id,
                Amount = product.FinalPrice,
                Type = (int)Framework.Utilities.Product.Tour,
                StartDate = product.CheckIn,
                EndDate = product.CheckOut,
                Night = product.Night,
                SurchargeFee = product.TotalSurcharge,
                IpLocation = Framework.Utilities.GetUserIp(Request),
                SpecialRequest = specialRequest,
                Discount = product.TotalSave,
                CancellationPolicy = product.CancellationPolicy != null ? product.CancellationPolicy.Description : string.Empty,
                ProductName = product.Name + " " + product.DetailName,
                DiscountName = product.Promotion != null ? $"{product.Promotion.Name}<br/>{product.Promotion.Description}" : string.Empty,
                ObjectState = ObjectState.Added,
                LocalType = (int)Utilities.Language.English,
                TourOrderId = order.Id

            };
            //Insert hotel order
            _orderService2.Add(order2);
            var productDetails = product.Details;
            if (productDetails.Count > 0)
            {
                foreach (var informationOrder in productDetails.Select(detail => new OrderInformations()
                {
                    OrderId = order.Id,
                    Price = detail.PriceRoom,
                    Date = detail.Date.ToString("MM/dd/yyyy"),
                    Quantity = product.Quantity ?? 0,
                    Discount = product.TotalSave,
                    DiscountName =
                        product.Promotion != null
                            ? string.Format("{0}<br/>{1}", product.Promotion.Name, product.Promotion.Description)
                            : string.Empty,
                    Surcharge = detail.PriceSurcharge > 0 ? detail.PriceSurcharge : 0,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    Status = (int)Framework.Utilities.Status.Active,
                    ObjectState = ObjectState.Added
                }))
                {
                    //Insert information order
                    _orderInformationService.Insert(informationOrder);
                }


                // save for accountant bill
                foreach (var informationOrder in productDetails.Select(detail => new OrderInformation2s()
                {
                    OrderId = order.Id,
                    Price = detail.PriceRoom,
                    Date = detail.Date.ToString("MM/dd/yyyy"),
                    Quantity = product.Quantity ?? 0,
                    Discount = product.TotalSave,
                    DiscountName =
                        product.Promotion != null
                            ? string.Format("{0}<br/>{1}", product.Promotion.Name, product.Promotion.Description)
                            : string.Empty,
                    Surcharge = detail.PriceSurcharge > 0 ? detail.PriceSurcharge : 0,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    Status = (int)Framework.Utilities.Status.Active,
                    ObjectState = ObjectState.Added
                }))
                {
                    //Insert information order
                    _orderInformationService2.Insert(informationOrder);
                }
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Json(new { success = false, error = "Error when processing booking" }, JsonRequestBehavior.AllowGet);
            }
            //Session.Clear();
            //Send email request for vinaday team.

            #endregion

            string amount = "0";
            if (order.Amount.HasValue)
                amount = ((decimal)(order.Amount.Value * 100)).ToString("###");

            //string SECURE_SECRET = "AF7492A64462CB06103EE42744B8EC85";
            string SECURE_SECRET = Config.MERCHANT_PAYNOW_HASH_CODE; 
            // Khoi tao lop thu vien va gan gia tri cac tham so gui sang cong thanh toan
            VPCRequest conn = new VPCRequest("https://onepay.vn/vpcpay/vpcpay.op");
            conn.SetSecureSecret(SECURE_SECRET);
            // Add the Digital Order Fields for the functionality you wish to use
            // Core Transaction Fields
            //conn.AddDigitalOrderField("AgainLink", "http://onepay.vn");
            conn.AddDigitalOrderField("AgainLink", "https://onepay.vn/paygate/vpcpay.op");
            conn.AddDigitalOrderField("Title", "onepay paygate");
            conn.AddDigitalOrderField("vpc_Locale", "en");//Chon ngon ngu hien thi tren cong thanh toan (vn/en)
            conn.AddDigitalOrderField("vpc_Version", "2");
            conn.AddDigitalOrderField("vpc_Command", "pay");
            conn.AddDigitalOrderField("vpc_Merchant", "VINADAY");
            //            conn.AddDigitalOrderField("SECURE_SECRET", "18D7EC3F36DF842B42E1AA729E4AB010");
            //conn.AddDigitalOrderField("vpc_AccessCode", "44B0BBEC");
            conn.AddDigitalOrderField("vpc_AccessCode", Config.MERCHANT_PAYNOW_ACCESS_CODE);
            conn.AddDigitalOrderField("vpc_MerchTxnRef", "payment_id-" + order.Id);
            conn.AddDigitalOrderField("vpc_OrderInfo", pnr + " " + product.DetailName);
            conn.AddDigitalOrderField("vpc_Amount", amount);


            conn.AddDigitalOrderField("vpc_ReturnURL", "https://goreise.com/checkout-result");
            conn.AddDigitalOrderField("vpc_querydr", "https://goreise.com/checkout-result-dr");
            //conn.AddDigitalOrderField("vpc_ReturnURL", "http://localhost:63697/checkout-result");
            string ipaddress = System.Web.HttpContext.Current.Request.UserHostAddress;
            conn.AddDigitalOrderField("vpc_TicketNo", ipaddress);
            // Chuyen huong trinh duyet sang cong thanh toan
            String url = conn.Create3PartyQueryString();
            return Json(new
            {
                success = true,
                message = url
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region HotelCoupon OnePay
        [Route("checkout-hotel-coupon-result")]
        public ActionResult CheckOutHotelCouponResult()
        {
            string SECURE_SECRET = "A3EFDFABA8653DF2342E8DAC29B51AF0";
            string hashvalidateResult = "";
            // Khoi tao lop thu vien
            VPCRequest conn = new VPCRequest("http://mtf.onepay.vn");
            conn.SetSecureSecret(SECURE_SECRET);
            // Xu ly tham so tra ve va kiem tra chuoi du lieu ma hoa
            hashvalidateResult = conn.Process3PartyResponse(Request.QueryString);
            // Lay gia tri tham so tra ve tu cong thanh toan
            String vpc_TxnResponseCode = conn.GetResultField("vpc_TxnResponseCode", "Unknown");
            string amount = conn.GetResultField("vpc_Amount", "Unknown");
            string localed = conn.GetResultField("vpc_Locale", "Unknown");
            string command = conn.GetResultField("vpc_Command", "Unknown");
            string version = conn.GetResultField("vpc_Version", "Unknown");
            string cardType = conn.GetResultField("vpc_Card", "Unknown");
            string orderInfo = conn.GetResultField("vpc_OrderInfo", "Unknown");
            string merchantID = conn.GetResultField("vpc_Merchant", "Unknown");
            string authorizeID = conn.GetResultField("vpc_AuthorizeId", "Unknown");
            string merchTxnRef = conn.GetResultField("vpc_MerchTxnRef", "Unknown");
            string transactionNo = conn.GetResultField("vpc_TransactionNo", "Unknown");
            string acqResponseCode = conn.GetResultField("vpc_AcqResponseCode", "Unknown");
            string txnResponseCode = vpc_TxnResponseCode;
            string message = conn.GetResultField("vpc_Message", "Unknown");
            ViewBag.vpc_Version = version;
            if (!string.IsNullOrEmpty(amount) && amount != "Unknown")
            {
                ViewBag.vpc_Amount = decimal.Parse(amount);
            }
            else
            {
                ViewBag.vpc_Amount = 0;
            }

            ViewBag.vpc_Command = command;
            ViewBag.vpc_MerchantID = merchantID;
            ViewBag.vpc_MerchantRef = merchTxnRef;
            ViewBag.vpc_OderInfor = orderInfo;
            ViewBag.vpc_ResponseCode = txnResponseCode;
            ViewBag.vpc_Command = command;
            ViewBag.vpc_TransactionNo = transactionNo;
            ViewBag.hashvalidate = hashvalidateResult;
            ViewBag.vpc_Message = message;
            string status = "";
            /// If success change status to completed
            /// else redirecto to booking page.
            //merchTxnRef = payment_id - 19561
            if (!string.IsNullOrEmpty(merchTxnRef) && merchTxnRef.Contains("-"))
            {
                string paymentId = merchTxnRef.Split('-')[1];
                var order = _orderService.GetOrder(Int32.Parse(paymentId.Trim()));


                ViewBag.FullName = order.FullName;
                ViewBag.Pnr = order.Pnr;
                ObjectModel objectModel = new ObjectModel();
                var customer = order.Customer;
                //Session[Constant.HotelCouponSession]
                var product = Session[Constant.HotelCouponSession] as ProductModel;
                if (product == null)
                {
                    objectModel.Status = (int)Utilities.Status.Active;
                    objectModel.Url = "/";
                    return Json(objectModel);
                }

                if (hashvalidateResult == "CORRECTED" && txnResponseCode.Trim() == "0")
                {
                    var orderUpdate = _orderService.GetOrderById(Int32.Parse(paymentId.Trim()));
                    orderUpdate.Status = (int)Framework.Utilities.BookingStatus.Paid;
                    var rateExchange = _rateExchangeService.GetRateExchangeById(3);
                    orderUpdate.RateExchange = rateExchange != null ? rateExchange.CurrentPrice : 0;
                    try
                    {
                        _orderService.Update(orderUpdate);
                        _unitOfWorkAsync.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return Json(new { success = false, error = "Error when processing booking" }, JsonRequestBehavior.AllowGet);
                    }
                    ViewBag.vpc_Result = "Transaction was paid successful";
                    //Send email request for vinaday team.
                    //var cancellation = _cancellationService.GetCancellationList().FirstOrDefault(c => c.Id == bookingModel.);
                    try
                    {
                        var _cacheCustomer = (Customer)Session[this.CacheCustomer];
                        dynamic myObject = new
                        {
                            DateTime = DateTime.Now.ToString("D", CultureInfo.CreateSpecificCulture("en-US")),
                            CustomerFullName = $"{customer.Firstname} {customer.Lastname}".ToUpper(),
                            HotelName = product.Name,
                            CheckIn = product.CheckIn,
                            CheckOut = product.CheckOut,
                            TotalPrice = product.TotalPrice,
                            VoucherName = product.DetailName,
                            VoucherDescription = product.Include,
                            Pnr = order.Pnr,
                            Stay = product.Stay,
                            Address = product.Location,
                            RoomName = product.DetailName,
                            RoomPrice = product.TotalPrice,
                            TaxesPrice = product.TotalTaxeFee,
                            Surcharge = product.TotalSurcharge > 0 ? product.TotalSurcharge : 0,
                            Include = product.Include,
                            Quantity = product.Quantity,
                            FinalPrice = product.FinalPrice,
                            //GuestLead = customer.l,
                            Nationality = customer.Nationality.Name,
                            SpecialRequest = order.SpecialRequest,
                            Email = customer.Email,
                            Phone = customer.PhoneNumber,
                            Gender = _cacheCustomer.Gender,
                            IsThirdPerson = _cacheCustomer.IsThirdPerson,
                            ThirdPersonGender = _cacheCustomer.ThirdPersonGender,
                            ThirdPersonFirstname = _cacheCustomer.ThirdPersonFirstname,
                            ThirdPersonLastname = _cacheCustomer.ThirdPersonLastname,
                            IsGiftVoucher = _cacheCustomer.IsGiftVoucher,
                            Gift_type = _cacheCustomer.Gift_type,
                            PersonalMessage = _cacheCustomer.PersonalMessage,
                            Cancellation = product.CancellationPolicy
                        };
                        string bookingEmailResult = Framework.Utilities.ParseTemplate("BookingHotelCoupon", myObject);
                        var subject = $"VINADAY goREISE - Booking #{order.Pnr}";
                        MailClient.RequestBookingHotel(customer.Email, bookingEmailResult, subject);
                    }
                    catch (Exception ex)
                    {
                        var test = ex.ToString();

                        //log error here
                    }

                    ViewBag.vpc_Result = "Transaction was paid successful";
                }
                else if (hashvalidateResult == "INVALIDATED" && txnResponseCode.Trim() == "0")
                {

                    ViewBag.vpc_Result = "Transaction is pending";
                    return Redirect("http://vinaday.vn");
                }
                else
                {

                    return Redirect("http://vinaday.vn");
                }

            }



            return View();
        }

        [HttpPost]
        public JsonResult HotelCouponPaymentOnePay(Customer customer, ObjectModel model)
        {
            decimal increment = (decimal)1.004;
            Session[this.CacheCustomer] = customer;
            #region Save Booking
            var objectModel = new ObjectModel();
            var rateExchange = _rateExchangeService.GetRateExchangeById(3);

            var product = Session[Constant.HotelCouponSession] as ProductModel;
            if (product == null)
            {
                objectModel.Status = (int)Utilities.Status.Active;
                objectModel.Url = "/";
                return Json(objectModel);
            }
            if (product.FinalPrice > 0)
            {
                product.FinalPrice = product.FinalPrice * increment;
            }
            //var bookingModel = Session[Utilities.BookingSession] as BookingModel

            //Add customer
            var existCustomer = new Customer();
            var specialRequest = customer.Street; //Rendering as special request property
            if (customer != null)
            {
                var country = _countryService.GetCountryList().FirstOrDefault(c => c.CountryId == customer.NationalId);
                customer.District = country != null ? country.Name : string.Empty;
                existCustomer = _customerService.GetCustomerByEmail(customer.Email);
            }
            if (existCustomer == null)
            {
                //Insert customer
                customer.ObjectState = ObjectState.Added;
                _customerService.Insert(customer);
                try
                {
                    _unitOfWorkAsync.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    objectModel.Status = (int)Utilities.Status.Active;
                    objectModel.Url = "/";
                    return Json(objectModel);
                }
            }
            else
            {
                customer = existCustomer;
            }
            decimal price = 0;
            decimal surcharge = 0;
            decimal promotion = 0;
            //Add Order
            var pnr = Framework.Utilities.GetRandomString(7).ToUpper(); //Random PNR order
            var order = new Order
            {
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,

                Status = (int)Framework.Utilities.BookingStatus.Holding,
                Quantity = product.Quantity,
                CustomerId = customer.CustomerId,
                Pnr = pnr,
                TaxFee = product.TotalTaxeFee > 0 ? product.TotalTaxeFee / rateExchange.CurrentPrice : product.TotalTaxeFee,// product.TotalTaxeFee,
                Price = product.TotalPrice > 0 ? product.TotalPrice / rateExchange.CurrentPrice : product.TotalPrice,
                ProductId = product.Id,
                Amount = product.FinalPrice > 0 ? product.FinalPrice / rateExchange.CurrentPrice : product.FinalPrice,//product.FinalPrice,
                Type = (int)Framework.Utilities.Product.HotelPackage,
                IpLocation = Framework.Utilities.GetUserIp(Request),
                SpecialRequest = specialRequest,
                Discount = product.TotalSave > 0 ? product.TotalSave / rateExchange.CurrentPrice : product.TotalSave,// product.TotalSave,
                CancellationPolicy = product.CancellationPolicy != null ? product.CancellationPolicy.Description : string.Empty,
                ProductName = product.Name + " " + product.DetailName,
                DiscountName = product.Promotion != null ? $"{product.Promotion.Name}<br/>{product.Promotion.Description}" : string.Empty,
                ObjectState = ObjectState.Added,
                LocalType = (int)Utilities.Language.English
            };

            //Insert hotel order
            _orderService.Add(order);

            var order2 = new Order2
            {
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Status = (int)Framework.Utilities.BookingStatus.Holding,
                Quantity = product.Quantity,
                CustomerId = customer.CustomerId,
                Pnr = pnr,
                TaxFee = product.TotalTaxeFee > 0 ? product.TotalTaxeFee / rateExchange.CurrentPrice : product.TotalTaxeFee,// product.TotalTaxeFee,
                Price = product.TotalPrice > 0 ? product.TotalPrice / rateExchange.CurrentPrice : product.TotalPrice,// product.TotalPrice,
                ProductId = product.Id,
                Amount = product.FinalPrice > 0 ? product.FinalPrice / rateExchange.CurrentPrice : product.FinalPrice,// product.FinalPrice,
                Type = (int)Framework.Utilities.Product.HotelPackage,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                Night = product.Night,
                SurchargeFee = product.TotalSurcharge,
                IpLocation = Framework.Utilities.GetUserIp(Request),
                SpecialRequest = specialRequest,
                Discount = product.TotalSave > 0 ? product.TotalSave / rateExchange.CurrentPrice : product.TotalSave,//product.TotalSave,
                CancellationPolicy = product.CancellationPolicy != null ? product.CancellationPolicy.Description : string.Empty,
                ProductName = product.Name + " " + product.DetailName,
                DiscountName = product.Promotion != null ? $"{product.Promotion.Name}<br/>{product.Promotion.Description}" : string.Empty,
                ObjectState = ObjectState.Added,
                LocalType = (int)Utilities.Language.English,
                TourOrderId = order.Id

            };
            //Insert hotel order
            _orderService2.Add(order2);
            var productDetails = product.Details;
            if (productDetails.Count > 0)
            {
                foreach (var informationOrder in productDetails.Select(detail => new OrderInformations()
                {
                    OrderId = order.Id,
                    Price = detail.PriceRoom,
                    Date = detail.Date.ToString("MM/dd/yyyy"),
                    Quantity = product.Quantity ?? 0,
                    Discount = product.TotalSave,
                    DiscountName =
                        product.Promotion != null
                            ? string.Format("{0}<br/>{1}", product.Promotion.Name, product.Promotion.Description)
                            : string.Empty,
                    Surcharge = detail.PriceSurcharge > 0 ? detail.PriceSurcharge : 0,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    Status = (int)Framework.Utilities.Status.Active,
                    ObjectState = ObjectState.Added
                }))
                {
                    //Insert information order
                    _orderInformationService.Insert(informationOrder);
                }


                // save for accountant bill
                foreach (var informationOrder in productDetails.Select(detail => new OrderInformation2s()
                {
                    OrderId = order.Id,
                    Price = detail.PriceRoom,
                    Date = detail.Date.ToString("MM/dd/yyyy"),
                    Quantity = product.Quantity ?? 0,
                    Discount = product.TotalSave > 0 ? product.TotalSave / rateExchange.CurrentPrice : product.TotalSave,// product.TotalSave,
                    DiscountName =
                        product.Promotion != null
                            ? string.Format("{0}<br/>{1}", product.Promotion.Name, product.Promotion.Description)
                            : string.Empty,
                    Surcharge = detail.PriceSurcharge > 0 ? (detail.PriceSurcharge > 0 ? detail.PriceSurcharge / rateExchange.CurrentPrice : detail.PriceSurcharge) : 0,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    Status = (int)Framework.Utilities.Status.Pending,
                    ObjectState = ObjectState.Added
                }))
                {
                    //Insert information order
                    _orderInformationService2.Insert(informationOrder);
                }
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Utilities.Status.Pending;
                objectModel.Url = "/";
                return Json(objectModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }



            #endregion

            string amount = "0";
            if (product.FinalPrice > 0)
                amount = (product.FinalPrice * 100).ToString();
            // old value
            string SECURE_SECRET = "A3EFDFABA8653DF2342E8DAC29B51AF0";
            // Khoi tao lop thu vien va gan gia tri cac tham so gui sang cong thanh toan
            VPCRequest conn = new VPCRequest("https://mtf.onepay.vn/onecomm-pay/vpc.op");
            conn.SetSecureSecret(SECURE_SECRET);
            conn.AddDigitalOrderField("AgainLink", "http://onepay.vn");
            conn.AddDigitalOrderField("Title", "onepay paygate");
            conn.AddDigitalOrderField("vpc_Locale", "vn");//Chon ngon ngu hien thi tren cong thanh toan (vn/en)
            conn.AddDigitalOrderField("vpc_Version", "2");
            conn.AddDigitalOrderField("vpc_Command", "pay");
            conn.AddDigitalOrderField("vpc_Merchant", "ONEPAY");
            conn.AddDigitalOrderField("vpc_Currency", "VND");
            //            conn.AddDigitalOrderField("SECURE_SECRET", "18D7EC3F36DF842B42E1AA729E4AB010");
            conn.AddDigitalOrderField("vpc_AccessCode", "D67342C2");
            conn.AddDigitalOrderField("vpc_MerchTxnRef", "payment_id-" + order.Id);
            conn.AddDigitalOrderField("vpc_OrderInfo", pnr + " tuong0001 ");
            conn.AddDigitalOrderField("vpc_Amount", amount);


            conn.AddDigitalOrderField("vpc_ReturnURL", "https://secure.goreise.com/checkout-hotel-coupon-result");

            conn.AddDigitalOrderField("vpc_SHIP_Street01", "");
            conn.AddDigitalOrderField("vpc_SHIP_Provice", "");
            conn.AddDigitalOrderField("vpc_SHIP_City", "");
            conn.AddDigitalOrderField("vpc_SHIP_Country", "");
            // Dia chi IP cua khach hang
            string ipaddress = System.Web.HttpContext.Current.Request.UserHostAddress;
            conn.AddDigitalOrderField("vpc_TicketNo", ipaddress);
            // Chuyen huong trinh duyet sang cong thanh toan
            String url = conn.Create3PartyQueryString();
            return Json(new
            {
                success = true,
                message = url
            }, JsonRequestBehavior.AllowGet);
        }


        #endregion 

        #region Tour OnePay
        [Route("checkout-tour-result")]
        [HttpGet]
        public ActionResult CheckOutTourResult()
        {
            string SECURE_SECRET = Config.MERCHANT_PAYNOW_HASH_CODE;
            string hashvalidateResult = "";
            // Khoi tao lop thu vien
            VPCRequest conn = new VPCRequest("https://onepay.vn/paygate/vpcpay.op");
            conn.SetSecureSecret(SECURE_SECRET);
            // Xu ly tham so tra ve va kiem tra chuoi du lieu ma hoa
            hashvalidateResult = conn.Process3PartyResponse(Request.QueryString);
            // Lay gia tri tham so tra ve tu cong thanh toan
            String vpc_TxnResponseCode = conn.GetResultField("vpc_TxnResponseCode", "Unknown");
            string amount = conn.GetResultField("vpc_Amount", "Unknown");
            string localed = conn.GetResultField("vpc_Locale", "Unknown");
            string command = conn.GetResultField("vpc_Command", "Unknown");
            string version = conn.GetResultField("vpc_Version", "Unknown");
            string cardType = conn.GetResultField("vpc_Card", "Unknown");
            string orderInfo = conn.GetResultField("vpc_OrderInfo", "Unknown");
            string merchantID = conn.GetResultField("vpc_Merchant", "Unknown");
            string authorizeID = conn.GetResultField("vpc_AuthorizeId", "Unknown"); 
            string merchTxnRef = conn.GetResultField("vpc_MerchTxnRef", "Unknown");
            string transactionNo = conn.GetResultField("vpc_TransactionNo", "Unknown");
            string acqResponseCode = conn.GetResultField("vpc_AcqResponseCode", "Unknown");
            string txnResponseCode = vpc_TxnResponseCode;
            string message = conn.GetResultField("vpc_Message", "Unknown");
            ViewBag.vpc_Version = version;
            if (!string.IsNullOrEmpty(amount) && amount != "Unknown")
            {
                ViewBag.vpc_Amount = decimal.Parse(amount) / 100;
            }
            else
            {
                ViewBag.vpc_Amount = 0;
            }

            ViewBag.vpc_Command = command;
            ViewBag.vpc_MerchantID = merchantID;
            ViewBag.vpc_MerchantRef = merchTxnRef;
            ViewBag.vpc_OderInfor = orderInfo;
            ViewBag.vpc_ResponseCode = txnResponseCode;
            ViewBag.vpc_Command = command;
            ViewBag.vpc_TransactionNo = transactionNo;
            ViewBag.hashvalidate = hashvalidateResult;
            ViewBag.vpc_Message = message;
            string status = "";
            /// If success change status to completed
            /// else redirecto to booking page.
            //merchTxnRef = payment_id - 19561
            if (!string.IsNullOrEmpty(merchTxnRef) && merchTxnRef.Contains("-"))
            {
                string paymentId = merchTxnRef.Split('-')[1];
                var order = _orderService.GetOrder(Int32.Parse(paymentId.Trim()));
                var objectModel = new ObjectModel();
                var bookingModel = Session[Constant.ProductSession] as BookingModel;
                if (bookingModel == null)
                {
                    objectModel.Status = (int)Utilities.Status.Active;
                    objectModel.Url = "/";
                    return Json(objectModel);
                }
                string tourGroup = string.Empty;
                string tourInclude = string.Empty;
                switch (bookingModel.RateType)
                {

                    case 1:
                        tourGroup = bookingModel.Product.TourGroup1;
                        tourInclude = bookingModel.Product.TourGroup1Include;
                        break;
                    case 2:
                        tourGroup = bookingModel.Product.TourGroup2;
                        tourInclude = bookingModel.Product.TourGroup2Include;
                        break;
                    case 3:
                        tourGroup = bookingModel.Product.TourGroup3;
                        tourInclude = bookingModel.Product.TourGroup3Include;
                        break;
                }
                ViewBag.FullName = order.FullName;
                ViewBag.Pnr = order.Pnr;                
                var customer = order.Customer;                                
                if (hashvalidateResult == "CORRECTED" && txnResponseCode.Trim() == "0")
                {
                    var orderUpdate = _orderService.GetOrderById(Int32.Parse(paymentId.Trim()));
                    orderUpdate.Status = (int)Framework.Utilities.BookingStatus.Paid;
                    var orderUpdate2 = _orderService2.GetOrderById(Int32.Parse(paymentId.Trim()));
                    orderUpdate2.Status = (int)Framework.Utilities.BookingStatus.Paid;
                    var rateExchange = _rateExchangeService.GetRateExchangeById(3);
                    orderUpdate.RateExchange = rateExchange != null ? rateExchange.CurrentPrice : 0;
                    orderUpdate2.RateExchange = rateExchange != null ? rateExchange.CurrentPrice : 0;

                    try
                    {
                        _orderService.Update(orderUpdate);
                        _orderService2.Update(orderUpdate2);
                        _unitOfWorkAsync.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return Json(new { success = false, error = "Error when processing booking" }, JsonRequestBehavior.AllowGet);
                    }
                    ViewBag.vpc_Result = "Transaction was paid successful";
                    //Send email request for vinaday team.
                    var cancellation = _cancellationService.GetCancellationList().FirstOrDefault(c => c.Id == bookingModel.Product.CancelationPolicy);
                    try
                    {
                        var _cacheCustomer = (Customer)Session[this.CacheCustomer];
                        dynamic myObject = new
                        {
                            DateTime = DateTime.Now.ToString("D", CultureInfo.CreateSpecificCulture("en-US")),
                            CustomerFullName = $"{customer.Firstname} {customer.Lastname}".ToUpper(),
                            TourName = bookingModel.Product.Name,
                            Departure = bookingModel.Product.Start,
                            DepartureOption = bookingModel.DepartureOption,
                            Pnr = order.Pnr,
                            CheckIn = order.StartDate,
                            Participants = bookingModel.Children > 0 ?
                        $"{(bookingModel.Adult > 1 ? $"{bookingModel.Adult} persons {bookingModel.Children} children" : $"{bookingModel.Adult} person {bookingModel.Children} children")} " :
                        $"{(bookingModel.Adult > 1 ? $"{bookingModel.Adult} persons" : $"{bookingModel.Adult} person")} ",
                            Price = bookingModel.Rate.RetailRate,
                            Total = order.Amount,
                            VN = false,
                            TourGroup = tourGroup,
                            TourInclude = tourInclude,
                            Children = order.Children,
                            Email = bookingModel.Customer.Email,
                            Phone = bookingModel.Customer.PhoneNumber,
                            Gender = _cacheCustomer.Gender,
                            IsThirdPerson = _cacheCustomer.IsThirdPerson,
                            ThirdPersonGender = _cacheCustomer.ThirdPersonGender,
                            ThirdPersonFirstname = _cacheCustomer.ThirdPersonFirstname,
                            ThirdPersonLastname = _cacheCustomer.ThirdPersonLastname,
                            IsGiftVoucher = _cacheCustomer.IsGiftVoucher,
                            Gift_type = _cacheCustomer.Gift_type,
                            PersonalMessage = _cacheCustomer.PersonalMessage,
                            SpecialRequest = bookingModel.Order.SpecialRequest,
                            TourPriceOption = bookingModel.TourPriceOption,
                            RoomOptionName = bookingModel.RoomOptionName,
                            Cancellation = cancellation != null ? cancellation.Description : string.Empty
                        };
                        string bookingEmailResult = Framework.Utilities.ParseTemplate("TourRequestOption", myObject);
                        var subject = $"VINADAY goREISE - Booking #{order.Pnr}";
                        MailClient.RequestBookingHotel(bookingModel.Customer.Email, bookingEmailResult, subject);

                    }
                    catch (Exception ex)
                    {
                        //log error here
                    }
                    ViewBag.vpc_Result = "Transaction was paid successful";
                }
                else if (message == "Canceled" && txnResponseCode.Trim() == "99")
                {
                    ViewBag.vpc_Result = "Transaction is Canceled";
                }
                else if (hashvalidateResult == "INVALIDATED" && txnResponseCode.Trim() == "0")
                {
                    ViewBag.vpc_Result = "Transaction is Invadlidate";
                }
                else
                {
                    return Redirect("https://goreise.com");
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> PaymentqueryDR(FormCollection frm)
        {
            var merchId = frm["MerchTxnRef"];
            string merchantId = Config.MERCHANT_PAYNOW_ID;
            string merchantAccessCode = Config.MERCHANT_PAYNOW_ACCESS_CODE;
            string merchantHashCode = Config.MERCHANT_PAYNOW_HASH_CODE;
            string merchTxnRef = merchId;
            string result = QueryDR.QueryDRApi(merchantId, merchantAccessCode, merchantHashCode, merchTxnRef);
            ViewBag.Result = result;
            return View();
        }
        
        public static string doPost(string vpc_Host, string postData)
        {
            string page = "Response:";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(vpc_Host);
            request.Method = "POST";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.UserAgent = "HTTP Client";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            page = page + responseFromServer;
            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();
            return page;
        }

        [Route("tour-payment-onepay")]
        [HttpPost]
        public JsonResult TourPaymentOnePay(Customer customer, ObjectModel model)
        {
            #region Save Booking
            var objectModel = new ObjectModel();
            var bookingModel = Session[Constant.ProductSession] as BookingModel;
            if (bookingModel == null)
            {
                objectModel.Status = (int)Utilities.Status.Active;
                objectModel.Url = "/";
                return Json(objectModel);
            }
            //model.StrParam1 = $("#promotionCode").val();
            //model.StrParam2 =  $("#productname").val().replace("&", "");
            //model.DecParam1 = $("#discountCouponValue").val();
            Session[this.CacheCustomer] = customer;
            //Add customer
            var existCustomer = new Customer();

            if (customer != null)
            {
                var country = _countryService.GetCountryList().FirstOrDefault(c => c.CountryId == customer.NationalId);
                customer.District = country != null ? country.Name : string.Empty;
                bookingModel.Customer = customer;

                bookingModel.Order.SpecialRequest = customer.Street;
                existCustomer = _customerService.GetCustomerByEmail(customer.Email);
            }
            if (existCustomer == null)
            {
                //Insert customer
                customer.ObjectState = ObjectState.Added;
                _customerService.Insert(customer);
                try
                {
                    _unitOfWorkAsync.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    objectModel.Status = (int)Utilities.Status.Active;
                    objectModel.Url = "/";
                    return Json(objectModel);
                }
            }
            else
            {
                customer = existCustomer;
            }
            decimal price = 0;
            decimal surcharge = 0;
            decimal promotion = 0;
            //Add Order
            var cancelationPolicy = _cancellationService.GetCancellationList().FirstOrDefault(c => c.Id == bookingModel.Product.CancelationPolicy);
            var order = bookingModel.Order;
            order.CreatedDate = DateTime.Now;
            order.ModifiedDate = DateTime.Now;
            order.Status = (int)Framework.Utilities.BookingStatus.Failure;
            if (!string.IsNullOrEmpty(bookingModel.DepartureOption))
            {
                order.DepartureOption = bookingModel.DepartureOption;
            }
            order.Quantity = bookingModel.Rate.PersonNo;
            order.CustomerId = customer.CustomerId;
            order.Pnr = Utilities.GetRandomString(7).ToUpper();
            order.TaxFee = bookingModel.Product.CommissionRate;
            order.Price = bookingModel.Rate.RetailRate;
            order.ProductId = bookingModel.Product.Id;
            order.LocalType = (int)Utilities.Language.English;
            order.ProductName = bookingModel.Product.Name;
            order.CancellationPolicy = cancelationPolicy != null ? cancelationPolicy.Description : string.Empty;
            order.Type = (int)Utilities.Product.Tour;
            order.Children = bookingModel.Children;
            order.CouponCode = model.StrParam1;
            order.Discount = model.DecParam1;
            order.EndDate = DateTime.Now;

            order.Amount = bookingModel.FinalRate;
            if (!string.IsNullOrEmpty(order.CouponCode))
            {
                var promotionValue = order.Discount.Value;
                order.Amount = order.Amount - promotionValue;
            }
            //Insert tour
            order.ObjectState = ObjectState.Added;
            _orderService.Insert(order);

            var order2 = new Order2
            {
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Status = (int)Framework.Utilities.BookingStatus.Failure,
                Children = bookingModel.Children,
                Quantity = bookingModel.Rate.PersonNo,
                CustomerId = customer.CustomerId,
                Pnr = Utilities.GetRandomString(7).ToUpper(),
                TaxFee = bookingModel.Product.CommissionRate,
                Price = bookingModel.Rate.RetailRate,
                ProductId = bookingModel.Product.Id,
                LocalType = (int)Utilities.Language.English,
                StartDate = order.StartDate,
                EndDate = order.EndDate,
                CouponCode = model.StrParam1,
                ProductName = bookingModel.Product.Name,
                Discount = model.DecParam1,
                CancellationPolicy = cancelationPolicy != null ? cancelationPolicy.Description : string.Empty,
                Type = (int)Utilities.Product.Tour
            };
            if (bookingModel.Promotion != null)
            {
                order2.DiscountName = bookingModel.Promotion.Name;
                order2.Discount = promotion = bookingModel.Promotion.Price ?? 0;
            }
            if (bookingModel.Surcharge != null)
            {
                order2.SurchargeName = bookingModel.Surcharge.Name;
                order2.SurchargeFee = surcharge = bookingModel.Surcharge.Price ?? 0;
            }
            order2.Amount = order.Amount;
            order2.EndDate = DateTime.Now;

            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Utilities.Status.Active;
                objectModel.Url = "/";
                return Json(objectModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }


            //Insert tour
            order2.ObjectState = ObjectState.Added;
            order2.TourOrderId = order.Id;
            _orderService2.Insert(order2);
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Utilities.Status.Active;
                objectModel.Url = "/";
                return Json(objectModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            #endregion

            string pnr = order.Pnr;
            string amount = "0";
            if (order.Amount.HasValue)
                amount = ((decimal)(order.Amount.Value * 100)).ToString("###");
            // old value
            string SECURE_SECRET = Config.MERCHANT_PAYNOW_HASH_CODE;
            // Khoi tao lop thu vien va gan gia tri cac tham so gui sang cong thanh toan
            VPCRequest conn = new VPCRequest("https://onepay.vn/paygate/vpcpay.op");
            conn.SetSecureSecret(SECURE_SECRET);
            conn.AddDigitalOrderField("AgainLink", "http://onepay.vn");
            conn.AddDigitalOrderField("Title", "onepay paygate");
            conn.AddDigitalOrderField("vpc_Locale", "vn");//Chon ngon ngu hien thi tren cong thanh toan (vn/en)
            conn.AddDigitalOrderField("vpc_Version", "2");
            conn.AddDigitalOrderField("vpc_Command", "pay");
            conn.AddDigitalOrderField("vpc_Merchant", Config.MERCHANT_PAYNOW_ID );
            conn.AddDigitalOrderField("vpc_AccessCode", Config.MERCHANT_PAYNOW_ACCESS_CODE);
            conn.AddDigitalOrderField("vpc_MerchTxnRef", "payment_id-" + order.Id);
            conn.AddDigitalOrderField("vpc_OrderInfo", pnr + " " + "ABC");
            conn.AddDigitalOrderField("vpc_Amount", amount);



            conn.AddDigitalOrderField("vpc_ReturnURL", "https://goreise.com/checkout-tour-result");
            //conn.AddDigitalOrderField("vpc_ReturnURL", "http://localhost:53417/checkout-tour-result");
            conn.AddDigitalOrderField("vpc_SHIP_Street01", "");
            conn.AddDigitalOrderField("vpc_SHIP_Provice", "");
            conn.AddDigitalOrderField("vpc_SHIP_City", "");
            conn.AddDigitalOrderField("vpc_SHIP_Country", "");
            // Dia chi IP cua khach hang
            string ipaddress = System.Web.HttpContext.Current.Request.UserHostAddress;
            conn.AddDigitalOrderField("vpc_TicketNo", ipaddress);
            // Chuyen huong trinh duyet sang cong thanh toan
            String url = conn.Create3PartyQueryString();
            return Json(new
            {
                success = true,
                message = url
            }, JsonRequestBehavior.AllowGet);
        }


        #endregion 
        [HttpPost]
        public JsonResult CheckCreditCard(ObjectModel objectModel)
        {
            var cardNumber = objectModel.StrParam1.Trim();
            if (!CreditCardUtility.IsValidNumber(cardNumber))
                return Json(new
                {
                    success = false,
                    message = "Card failed Luhn test. Please enter a valid card number."
                }, JsonRequestBehavior.AllowGet);
            var cardType = CreditCardUtility.GetCardTypeFromNumber(cardNumber);
            var strCardType = cardType?.ToString() ?? "Unknown";
            return Json(new
            {
                success = true,
                message = $"You have entered a valid card number. The card type is {strCardType}."
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region For Hotel Vietnam
        [Route("viet-nam")]
        public ActionResult IndexVn()
        {
            ViewBag.Title = "Vinaday Secure";

            return View();
        }
        [Route("vn/payment")]
        public ActionResult PaymentVn()
        {
            ViewBag.Nationalities = _nationalityService.GetNationalityList();
            var product = (ProductModel)Session[Constant.ProductSession];
            if (product == null)
            {
                return Redirect("/viet-nam");
            }
            ViewBag.CancellationPolicies = product.TotalSave > 0
                ? new List<CancellationModel>()
                : _hotelService.GetHotelCancellationPoliciesByHotelId(product.ProductId);
            product.ShoppingCartUrl = Server.UrlEncode(Session[Constant.ShoppingCartUrl] as string);
            var customer = new Customer();
            var claim = ((ClaimsIdentity)User.Identity).FindFirst("Id");
            if (claim != null)
            {
                var id = claim.Value;
                customer = _customerService.GetCustomerByMemberId(id);
            }
            ViewBag.Customer = customer;
            return View(product);

        }
        [Route("vn/shopping-cart/{type}/{id}/{checkIn}/{checkOut}/{total}/{promotion}")]
        public ActionResult ShoppingCartVn(int type, int id, String checkIn, String checkOut, int total, int promotion)
        {
            if (Request.Url != null)
            {
                Session[Constant.ShoppingCartUrl] = Request.Url.AbsoluteUri;
            }

            Session[Constant.ProductSession] = null;
            Session[Constant.ProductSessionEn] = null;
            var product = _bookingService.GetHotelProductVn(type, id, checkIn, checkOut, total, promotion);
            var productEn = _bookingService.GetHotelProduct(type, id, checkIn, checkOut, total, promotion);
            Session[Constant.ProductSession] = product;
            Session[Constant.ProductSessionEn] = productEn;
            //return View(product);
            return Redirect("/vn/payment");
        }
        public JsonResult ConfirmWithCreditCardVn(Customer customer, CreditCard creditCard)
        {
            var product = (ProductModel)Session[Constant.ProductSessionEn];
            if (product == null || customer == null || creditCard == null)
            {
                return Json(new { success = false, error = "Lỗi xảy ra khi xử lý đặt phòng" }, JsonRequestBehavior.AllowGet);
            }
            //Add CreditCard

            var card = InsertCreditCard(creditCard);

            //Add customer
            var specialRequest = customer.Street; //Rendering as special request property
            var existCustomer = _customerService.GetCustomerByEmail(customer.Email);
            if (existCustomer == null)
            {
                customer.ObjectState = ObjectState.Added;
                _customerService.Insert(customer);
                try
                {
                    _unitOfWorkAsync.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Json(new { success = false, error = "Lỗi xảy ra khi xử lý đặt phòng" },
                        JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                customer = existCustomer;
            }
            //Add Order
            var pnr = Framework.Utilities.GetRandomString(7).ToUpper(); //Random PNR order
            var order = new Order
            {
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Status = (int)Framework.Utilities.BookingStatus.Holding,
                Quantity = product.Quantity,
                CustomerId = customer.CustomerId,
                Pnr = pnr,
                TaxFee = product.TotalTaxeFee,
                Price = product.TotalPrice,
                ProductId = product.Id,
                Amount = product.FinalPrice,
                Type = (int)Framework.Utilities.Product.Hotel,
                StartDate = product.CheckIn,
                EndDate = product.CheckOut,
                Night = product.Night,
                SurchargeFee = product.TotalSurcharge > 0 ? product.TotalSurcharge : 0,
                IpLocation = Framework.Utilities.GetUserIp(Request),
                SpecialRequest = specialRequest,
                Discount = product.TotalSave,
                CancellationPolicy = product.CancellationPolicy != null ? product.CancellationPolicy.Description : string.Empty,
                ProductName = product.Name,
                DiscountName = product.Promotion != null ? $"{product.Promotion.Name}<br/>{product.Promotion.Description}" : string.Empty,
                CardId = card.Id,
                PaymentMethod = (int)Utilities.PaymentMethod.Credit,
                ObjectState = ObjectState.Added,
                LocalType = (int)Utilities.Language.Vietnam,
                CardNumber = !string.IsNullOrEmpty(card.CardNumber) ?
                    $"xxxx-xxxx-xxxx-x{Utilities.GetLast(card.CardNumber, 3)}"
                    : string.Empty
            };

            //Insert hotel order
            _orderService.Add(order);

            var order2 = new Order2
            {
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Status = (int)Framework.Utilities.BookingStatus.Holding,
                Quantity = product.Quantity,
                CustomerId = customer.CustomerId,
                Pnr = pnr,
                TaxFee = product.TotalTaxeFee,
                Price = product.TotalPrice,
                ProductId = product.Id,
                Amount = product.FinalPrice,
                Type = (int)Framework.Utilities.Product.Hotel,
                StartDate = product.CheckIn,
                EndDate = product.CheckOut,
                Night = product.Night,
                SurchargeFee = product.TotalSurcharge > 0 ? product.TotalSurcharge : 0,
                IpLocation = Framework.Utilities.GetUserIp(Request),
                SpecialRequest = specialRequest,
                Discount = product.TotalSave,
                CancellationPolicy = product.CancellationPolicy != null ? product.CancellationPolicy.Description : string.Empty,
                ProductName = product.Name,
                DiscountName = product.Promotion != null ? $"{product.Promotion.Name}<br/>{product.Promotion.Description}" : string.Empty,
                CardId = card.Id,
                PaymentMethod = (int)Utilities.PaymentMethod.Credit,
                ObjectState = ObjectState.Added,
                LocalType = (int)Utilities.Language.Vietnam,
                CardNumber = !string.IsNullOrEmpty(card.CardNumber) ?
                    $"xxxx-xxxx-xxxx-x{Utilities.GetLast(card.CardNumber, 3)}"
                    : string.Empty,
                TourOrderId = order.Id

            };
            //Insert hotel order
            _orderService2.Add(order2);
            //Add order information
            var productDetails = product.Details;
            if (productDetails.Count > 0)
            {
                foreach (var informationOrder in productDetails.Select(detail => new OrderInformations()
                {
                    OrderId = order.Id,
                    Price = detail.PriceRoom,
                    Date = detail.Date.ToString("MM/dd/yyyy"),
                    Quantity = product.Quantity ?? 0,
                    Discount = product.TotalSave,
                    DiscountName = product.Promotion != null ? $"{product.Promotion.Name}<br/>{product.Promotion.Description}" : string.Empty,
                    Surcharge = detail.PriceSurcharge > 0 ? detail.PriceSurcharge : 0,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    Status = (int)Framework.Utilities.Status.Active,
                    ObjectState = ObjectState.Added
                }))
                {
                    //Insert information order
                    _orderInformationService.Insert(informationOrder);
                }

                // save for accountant bill
                foreach (var informationOrder in productDetails.Select(detail => new OrderInformation2s()
                {
                    OrderId = order.Id,
                    Price = detail.PriceRoom,
                    Date = detail.Date.ToString("MM/dd/yyyy"),
                    Quantity = product.Quantity ?? 0,
                    Discount = product.TotalSave,
                    DiscountName = product.Promotion != null ? $"{product.Promotion.Name}<br/>{product.Promotion.Description}" : string.Empty,
                    Surcharge = detail.PriceSurcharge > 0 ? detail.PriceSurcharge : 0,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    Status = (int)Framework.Utilities.Status.Active,
                    ObjectState = ObjectState.Added
                }))
                {
                    //Insert information order
                    _orderInformationService2.Insert(informationOrder);
                }
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Json(new { success = false, error = "Lỗi xảy ra khi xử lý đặt phòng" }, JsonRequestBehavior.AllowGet);
            }
            Session.Clear();
            //Send email request for vinaday team.
            try
            {
                dynamic myObject = new
                {
                    DateTime = DateTime.Now.ToString("D", CultureInfo.CreateSpecificCulture("en-US")),
                    CustomerFullName = $"{customer.Firstname} {customer.Lastname}".ToUpper(),
                    HotelName = product.Name,
                    CheckIn = product.CheckIn,
                    CheckOut = product.CheckOut,
                    Pnr = order.Pnr,
                    Stay = product.Stay,
                    Address = product.Location,
                    RoomName = product.DetailName,
                    RoomPrice = product.TotalPrice,
                    TaxesPrice = product.TotalTaxeFee,
                    Surcharge = product.TotalSurcharge > 0 ? product.TotalSurcharge : 0,
                    FinalPrice = product.FinalPrice,
                    Email = customer.Email,
                    Phone = customer.PhoneNumber,
                    Cancellation = product.CancellationPolicy
                };
                string bookingEmailResult = Framework.Utilities.ParseTemplate("BookingRequestVn", myObject);
                var subject = $"VINADAY goREISE - Booking #{order.Pnr}";
                MailClient.RequestBookingHotel(customer.Email, bookingEmailResult, subject);
            }
            catch (Exception ex)
            {
                var test = ex.ToString();

                //log error here
            }
            var resutls = new { success = true, customer = $"{customer.Firstname} {customer.Lastname}", pnr };
            var jsonResult = Json(resutls, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult ConfirmVn(Customer customer)
        {

            var product = (ProductModel)Session[Constant.ProductSessionEn];
            if (product == null || customer == null)
            {
                return Json(new { success = false, error = "Error when processing booking" }, JsonRequestBehavior.AllowGet);
            }
            //Add customer
            var specialRequest = customer.Street; //Rendering as special request property
            var existCustomer = _customerService.GetCustomerByEmail(customer.Email);
            if (existCustomer == null)
            {
                customer.ObjectState = ObjectState.Added;
                _customerService.Insert(customer);
                try
                {
                    _unitOfWorkAsync.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Json(new { success = false, error = "Error when processing booking" },
                        JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                customer = existCustomer;
            }
            //Add Order
            //var checkIn = DateTime.Parse(product.CheckIn).ToString(CultureInfo.GetCultureInfo("en-US").DateTimeFormat.ShortDatePattern);
            //var checkOut = DateTime.Parse(product.CheckOut).ToString(CultureInfo.GetCultureInfo("en-US").DateTimeFormat.ShortDatePattern);
            var pnr = Framework.Utilities.GetRandomString(7).ToUpper(); //Random PNR order
            var order = new Order
            {
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Status = (int)Framework.Utilities.BookingStatus.Holding,
                Quantity = product.Quantity,
                CustomerId = customer.CustomerId,
                Pnr = pnr,
                TaxFee = product.TotalTaxeFee,
                Price = product.TotalPrice,
                ProductId = product.Id,
                Amount = product.FinalPrice,
                Type = (int)Framework.Utilities.Product.Hotel,
                StartDate = product.CheckIn,
                EndDate = product.CheckOut,
                Night = product.Night,
                SurchargeFee = product.TotalSurcharge > 0 ? product.TotalSurcharge : 0,
                IpLocation = Framework.Utilities.GetUserIp(Request),
                SpecialRequest = specialRequest,
                Discount = product.TotalSave,
                CancellationPolicy =
                    product.CancellationPolicy != null ? product.CancellationPolicy.Description : string.Empty,
                ProductName = product.Name,
                DiscountName =
                    product.Promotion != null
                        ? string.Format("{0}<br/>{1}", product.Promotion.Name, product.Promotion.Description)
                        : string.Empty,
                ObjectState = ObjectState.Added,
                LocalType = (int)Utilities.Language.Vietnam
            };

            //Insert hotel order
            _orderService.Add(order);

            var order2 = new Order2
            {
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Status = (int)Framework.Utilities.BookingStatus.Holding,
                Quantity = product.Quantity,
                CustomerId = customer.CustomerId,
                Pnr = pnr,
                TaxFee = product.TotalTaxeFee,
                Price = product.TotalPrice,
                ProductId = product.Id,
                Amount = product.FinalPrice,
                Type = (int)Framework.Utilities.Product.Hotel,
                StartDate = product.CheckIn,
                EndDate = product.CheckOut,
                Night = product.Night,
                SurchargeFee = product.TotalSurcharge > 0 ? product.TotalSurcharge : 0,
                IpLocation = Framework.Utilities.GetUserIp(Request),
                SpecialRequest = specialRequest,
                Discount = product.TotalSave,
                CancellationPolicy =
                   product.CancellationPolicy != null ? product.CancellationPolicy.Description : string.Empty,
                ProductName = product.Name,
                DiscountName =
                   product.Promotion != null
                       ? string.Format("{0}<br/>{1}", product.Promotion.Name, product.Promotion.Description)
                       : string.Empty,
                ObjectState = ObjectState.Added,
                LocalType = (int)Utilities.Language.Vietnam,
                TourOrderId = order.Id
            };

            //Insert hotel order
            _orderService2.Add(order2);
            //try
            //{
            //    _unitOfWorkAsync.SaveChanges();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    return Json(new { success = false, error = "Error when processing booking" }, JsonRequestBehavior.AllowGet);
            //}
            //Add order information
            var productDetails = product.Details;
            if (productDetails.Count > 0)
            {
                foreach (var informationOrder in productDetails.Select(detail => new OrderInformations()
                {
                    OrderId = order.Id,
                    Price = detail.PriceRoom,
                    Date = detail.Date.ToString("MM/dd/yyyy"),
                    Quantity = product.Quantity ?? 0,
                    Discount = product.TotalSave,
                    DiscountName =
                        product.Promotion != null
                            ? string.Format("{0}<br/>{1}", product.Promotion.Name, product.Promotion.Description)
                            : string.Empty,
                    Surcharge = detail.PriceSurcharge > 0 ? detail.PriceSurcharge : 0,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    Status = (int)Framework.Utilities.Status.Active,
                    ObjectState = ObjectState.Added
                }))
                {
                    //Insert information order
                    _orderInformationService.Insert(informationOrder);
                }

                // save for accountant bill
                foreach (var informationOrder in productDetails.Select(detail => new OrderInformation2s()
                {
                    OrderId = order.Id,
                    Price = detail.PriceRoom,
                    Date = detail.Date.ToString("MM/dd/yyyy"),
                    Quantity = product.Quantity ?? 0,
                    Discount = product.TotalSave,
                    DiscountName =
                        product.Promotion != null
                            ? string.Format("{0}<br/>{1}", product.Promotion.Name, product.Promotion.Description)
                            : string.Empty,
                    Surcharge = detail.PriceSurcharge > 0 ? detail.PriceSurcharge : 0,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    Status = (int)Framework.Utilities.Status.Active,
                    ObjectState = ObjectState.Added
                }))
                {
                    //Insert information order
                    _orderInformationService2.Insert(informationOrder);
                }
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Json(new { success = false, error = "Error when processing booking" }, JsonRequestBehavior.AllowGet);
            }
            Session.Clear();
            //Send email request for vinaday team.
            try
            {
                dynamic myObject = new
                {
                    DateTime = DateTime.Now.ToString("D", CultureInfo.CreateSpecificCulture("en-US")),
                    CustomerFullName = $"{customer.Firstname} {customer.Lastname}".ToUpper(),
                    HotelName = product.Name,
                    CheckIn = product.CheckIn,
                    CheckOut = product.CheckOut,
                    Pnr = order.Pnr,
                    Stay = product.Stay,
                    Address = product.Location,
                    RoomName = product.DetailName,
                    RoomPrice = product.TotalPrice,
                    TaxesPrice = product.TotalTaxeFee,
                    Surcharge = product.TotalSurcharge > 0 ? product.TotalSurcharge : 0,
                    FinalPrice = product.FinalPrice,
                    Email = customer.Email,
                    Phone = customer.PhoneNumber,
                    Cancellation = product.CancellationPolicy
                };
                string bookingEmailResult = Framework.Utilities.ParseTemplate("BookingRequestVn", myObject);
                var subject = $"VINADAY goREISE - Booking #{order.Pnr}";
                MailClient.RequestBookingHotel(customer.Email, bookingEmailResult, subject);
            }
            catch (Exception ex)
            {
                var test = ex.ToString();

                //log error here
            }
            var resutls = new { success = true, customer = $"{customer.Firstname} {customer.Lastname}", pnr };
            var jsonResult = Json(resutls, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [Route("vn/book-hotel-vietnam-cam-on-{fullName}/pnr-{pnr}")]
        public ActionResult CompletedHotelVietnam(string fullName, string pnr)
        {
            ViewBag.FullName = fullName;
            ViewBag.Pnr = pnr;
            return View();
        }
        [Route("book-hotel-vietnam-thank-{fullName}/pnr-{pnr}")]
        public ActionResult CompletedHotel(string fullName, string pnr)
        {
            ViewBag.FullName = fullName;
            ViewBag.Pnr = pnr;
            return View();
        }
        #endregion

        #region For Tour
        [Route("shopping-cart-tour-option/{id}/{checkInDate}/{person}/{children}/{rateType}/{finalRate}/{startTime}/{retailRate}")]
        [HttpGet]
        [HttpPost]
        public ActionResult CartTourOption(int id, string checkInDate, int person, int children, int rateType, decimal finalRate, string startTime,decimal retailRate)
        {

            var tour = _tourService.Queryable().FirstOrDefault(t => t.Id == id);
            if (tour == null) return Redirect("/");
            var date = Utilities.ConvertStringToDateTime(checkInDate);
            var image = _mediaService.Queryable().FirstOrDefault(t => t.OwnerId == id && t.MediaType == (int)Utilities.MediaType.Banner);
            //Session[Utilities.BookingSession] = null;
            var order = new Order
            {
                StartDate = date
            };
            //var promotion = _specialRateService.GetSpecialRate(tour.Id, (int)Services.Utilities.SpecialRateType.Promotion, date);
            string optionName = string.Empty;
            string includeName = string.Empty;

            var bookingModel = new BookingModel
            {
                Product = tour,
                Rate = new Rate() { RetailRate = retailRate, PersonNo = person },
                Image = image,
                Order = order,
                FinalRate = finalRate,
                RateType = rateType,
                DepartureOption = startTime,
                RoomOptionName = optionName,
                Adult = person,
                
                Children = children

            };
            Session[Constant.ProductSession] = bookingModel;
            string url = $"/bookTour";
            return Redirect(url);
        }


        [Route("bookTour")]
        [HttpGet]
        [HttpPost]
        public ActionResult PaymentTourOption()
        {
            var objectModel = new ObjectModel();
            var bookingModel = Session[Constant.ProductSession] as BookingModel;
            if (bookingModel == null)
            {
                objectModel.Url = "/";
                return Json(objectModel);
            }
            ViewBag.Nationalities = _nationalityService.GetNationalityList();
            ViewBag.Cancellation = _cancellationService.GetCancellationList().FirstOrDefault(c => c.Id == bookingModel.Product.CancelationPolicy);
            return View(bookingModel);
        }
        [Route("confirm-tour-option")]
        public ActionResult ConfirmTourOption(Customer customer, ObjectModel model)
        {
            Customer _cacheCustomer = customer;
            var objectModel = new ObjectModel();
            var bookingModel = Session[Constant.ProductSession] as BookingModel;
            if (bookingModel == null)
            {
                objectModel.Status = (int)Utilities.Status.Active;
                objectModel.Url = "/";
                return Json(objectModel);
            }
            //Add customer
            var existCustomer = new Customer();
            if (customer != null)
            {
                var country = _countryService.GetCountryList().FirstOrDefault(c => c.CountryId == customer.NationalId);
                customer.District = country != null ? country.Name : string.Empty;
                bookingModel.Customer = customer;

                bookingModel.Order.SpecialRequest = customer.Street;
                existCustomer = _customerService.GetCustomerByEmail(customer.Email);
            }
            if (existCustomer == null)
            {
                //Insert customer
                customer.ObjectState = ObjectState.Added;
                _customerService.Insert(customer);
                try
                {
                    _unitOfWorkAsync.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    objectModel.Status = (int)Utilities.Status.Active;
                    objectModel.Url = "/";
                    return Json(objectModel);
                }
            }
            else
            {
                customer = existCustomer;
            }
            string tourGroup = string.Empty;
            string tourInclude = string.Empty;
            switch (bookingModel.RateType)
            {

                case 1:
                    tourGroup = bookingModel.Product.TourGroup1;
                    tourInclude = bookingModel.Product.TourGroup1Include;
                    break;
                case 2:
                    tourGroup = bookingModel.Product.TourGroup2;
                    tourInclude = bookingModel.Product.TourGroup2Include;
                    break;
                case 3:
                    tourGroup = bookingModel.Product.TourGroup3;
                    tourInclude = bookingModel.Product.TourGroup3Include;
                    break;
            }

            decimal price = 0;
            decimal surcharge = 0;
            decimal promotion = 0;
            //Add Order
            var cancelationPolicy = _cancellationService.GetCancellationList().FirstOrDefault(c => c.Id == bookingModel.Product.CancelationPolicy);

            var couponCode = _couponcodeService.Queryable().FirstOrDefault(a => a.Id == model.IntParam1);
            if (couponCode != null && couponCode.Quantity > 0)
            {
                couponCode.Quantity = couponCode.Quantity - 1;
                couponCode.ObjectState = ObjectState.Modified;
            }
            var order = bookingModel.Order;
            order.CreatedDate = DateTime.Now;
            order.ModifiedDate = DateTime.Now;
            order.Status = (int)Framework.Utilities.BookingStatus.Holding;
            order.Note = tourInclude;
            order.Quantity = bookingModel.Adult;
            order.CustomerId = customer.CustomerId;
            order.Pnr = Utilities.GetRandomString(7).ToUpper();
            order.TaxFee = bookingModel.Product.CommissionRate;
            order.Price = bookingModel.Rate.RetailRate;
            order.ProductId = bookingModel.Product.Id;
            order.LocalType = (int)Utilities.Language.English;
            order.CancellationPolicy = cancelationPolicy != null ? cancelationPolicy.Description : string.Empty;
            order.Type = (int)Utilities.Product.Tour;
            order.Children = bookingModel.Children;
            order.ProductName = bookingModel.Product.Name;
            if (!string.IsNullOrEmpty(tourGroup))
            {
                order.GroupType = tourGroup;
            }
            
            if (!string.IsNullOrEmpty(bookingModel.DepartureOption))
            {
                order.DepartureOption = bookingModel.DepartureOption;
            }
            if (bookingModel.Promotion != null)
            {
                order.DiscountName = bookingModel.Promotion.Name;
                order.Discount = promotion = bookingModel.Promotion.Price ?? 0;
            }
            if (bookingModel.Surcharge != null)
            {
                order.SurchargeName = bookingModel.Surcharge.Name;
                order.SurchargeFee = surcharge = bookingModel.Surcharge.Price ?? 0;
            }
            //price = (decimal)(bookingModel.Rate.RetailRate + (bookingModel.Rate.RetailRate * bookingModel.Product.CommissionRate / 100));
            //if (bookingModel.Children > 0)
            //{
            //    order.Amount = ((price + surcharge - promotion) * bookingModel.Rate.PersonNo) + ((price * 3 / 4) * bookingModel.Children);
            //}
            //else
            //{
            //    order.Amount = ((price + surcharge - promotion) * bookingModel.Rate.PersonNo);
            //}
            order.Amount = bookingModel.FinalRate;
            order.CouponCode = model.StrParam1;
            
            order.Discount = 0;
            //coupon code value 
            if (model.DecParam1 > 0)
            {
                order.Amount = order.Amount - model.DecParam1;
            }
            order.EndDate = DateTime.Now;

            //Insert tour
            order.ObjectState = ObjectState.Added;
            _orderService.Insert(order);

            var order2 = new Order2();
            order2.CreatedDate = DateTime.Now;
            order2.ModifiedDate = DateTime.Now;
            order2.Status = (int)Framework.Utilities.BookingStatus.Holding;
            order2.Children = bookingModel.Children;
            order2.Quantity = bookingModel.Rate.PersonNo;
            order2.CustomerId = customer.CustomerId;
            order2.Note = tourInclude;
            order2.Pnr = Utilities.GetRandomString(7).ToUpper();
            order2.TaxFee = bookingModel.Product.CommissionRate;
            order2.Price = bookingModel.Rate.RetailRate;
            order2.ProductId = bookingModel.Product.Id;
            order2.LocalType = (int)Utilities.Language.English;
            order2.StartDate = order.StartDate;
            order2.EndDate = order.EndDate;
            order2.CouponCode = model.StrParam1;
            order2.Discount = model.DecParam1;
            order2.Amount = order.Amount;
            order2.CancellationPolicy = cancelationPolicy != null ? cancelationPolicy.Description : string.Empty;
            order2.Type = (int)Utilities.Product.Tour;
            if (bookingModel.Promotion != null)
            {
                order2.DiscountName = bookingModel.Promotion.Name;
                order2.Discount = promotion = bookingModel.Promotion.Price ?? 0;
            }
            if (bookingModel.Surcharge != null)
            {
                order2.SurchargeName = bookingModel.Surcharge.Name;
                order2.SurchargeFee = surcharge = bookingModel.Surcharge.Price ?? 0;
            }
            order2.EndDate = DateTime.Now;
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Utilities.Status.Active;
                objectModel.Url = "/";
                return Json(objectModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }


            //Insert tour
            order2.ObjectState = ObjectState.Added;
            order2.TourOrderId = order.Id;
            _orderService2.Insert(order2);
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Utilities.Status.Active;
                objectModel.Url = "/";
                return Json(objectModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            bookingModel.RoomOptionName = "";
            bookingModel.TourPriceOption = 0;
            //Send email request for vinaday team.
            var cancellation = _cancellationService.GetCancellationList().FirstOrDefault(c => c.Id == bookingModel.Product.CancelationPolicy);
            try
            {
                dynamic myObject = new
                {
                    DateTime = DateTime.Now.ToString("D", CultureInfo.CreateSpecificCulture("en-US")),
                    CustomerFullName = $"{customer.Firstname} {customer.Lastname}".ToUpper(),
                    TourName = bookingModel.Product.Name,
                    Departure = bookingModel.Product.Start,
                    DepartureOption = bookingModel.DepartureOption,
                    Pnr = order.Pnr,
                    CheckIn = order.StartDate.ToShortDateString(),
                    Participants = bookingModel.Children > 0 ?
                        $"{(bookingModel.Adult > 1 ? $"{bookingModel.Adult} persons {bookingModel.Children} children" : $"{bookingModel.Adult} person {bookingModel.Children} children")} " :
                        $"{(bookingModel.Adult > 1 ? $"{bookingModel.Adult} persons" : $"{bookingModel.Adult} person")} ",
                    Price = bookingModel.Rate.RetailRate,
                    Total = order.Amount,
                    VN = false,
                    TourGroup = tourGroup,
                    TourInclude = tourInclude,
                    Children = order.Children,
                    Email = bookingModel.Customer.Email,
                    Phone = bookingModel.Customer.PhoneNumber,
                    Gender = _cacheCustomer.Gender,
                    IsThirdPerson = _cacheCustomer.IsThirdPerson,
                    ThirdPersonGender = _cacheCustomer.ThirdPersonGender,
                    ThirdPersonFirstname = _cacheCustomer.ThirdPersonFirstname,
                    ThirdPersonLastname = _cacheCustomer.ThirdPersonLastname,
                    IsGiftVoucher = _cacheCustomer.IsGiftVoucher,
                    Gift_type = _cacheCustomer.Gift_type,
                    PersonalMessage = _cacheCustomer.PersonalMessage,
                    SpecialRequest = bookingModel.Order.SpecialRequest,
                    TourPriceOption = bookingModel.TourPriceOption,
                    RoomOptionName = bookingModel.RoomOptionName,
                    Cancellation = cancellation != null ? cancellation.Description : string.Empty

                };
                string bookingEmailResult = Framework.Utilities.ParseTemplate("TourRequestOption", myObject);
                var subject = $"VINADAY goREISE - Booking #{order.Pnr}";
                MailClient.RequestBookingHotel(bookingModel.Customer.Email, bookingEmailResult, subject);
            }
            catch (Exception ex)
            {

                //log error here
            }
            objectModel.Status = (int)Utilities.Status.Active;
            return Json(objectModel);

        }

        [Route("book-tour|{id}/depart-date|{checkInDate}/person|{person}/children|{children}/option|{option}")]
        public ActionResult PaymentTour()
        {
            log.Info($"Payment tour");
            var objectModel = new ObjectModel();
            var bookingModel = Session[Utilities.BookingSession] as BookingModel;
            if (bookingModel == null)
            {
                objectModel.Url = "/";
                return Json(objectModel);
            }
            ViewBag.Nationalities = _nationalityService.GetNationalityList();
            ViewBag.Cancellation = _cancellationService.GetCancellationList().FirstOrDefault(c => c.Id == bookingModel.Product.CancelationPolicy);
            return View(bookingModel);
        }
        public ActionResult ConfirmTour(Customer customer, ObjectModel model)
        {
            Customer _cacheCustomer = customer;
            var objectModel = new ObjectModel();
            var bookingModel = Session[Utilities.BookingSession] as BookingModel;
            if (bookingModel == null)
            {
                objectModel.Status = (int)Utilities.Status.Active;
                objectModel.Url = "/";
                return Json(objectModel);
            }
            //Add customer
            var existCustomer = new Customer();
            if (customer != null)
            {
                var country = _countryService.GetCountryList().FirstOrDefault(c => c.CountryId == customer.NationalId);
                customer.District = country != null ? country.Name : string.Empty;
                bookingModel.Customer = customer;

                bookingModel.Order.SpecialRequest = customer.Street;
                existCustomer = _customerService.GetCustomerByEmail(customer.Email);
            }
            if (existCustomer == null)
            {
                //Insert customer
                customer.ObjectState = ObjectState.Added;
                _customerService.Insert(customer);
                try
                {
                    _unitOfWorkAsync.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    objectModel.Status = (int)Utilities.Status.Active;
                    objectModel.Url = "/";
                    return Json(objectModel);
                }
            }
            else
            {
                customer = existCustomer;
            }
            decimal price = 0;
            decimal surcharge = 0;
            decimal promotion = 0;
            //Add Order
            var cancelationPolicy = _cancellationService.GetCancellationList().FirstOrDefault(c => c.Id == bookingModel.Product.CancelationPolicy);

            var couponCode = _couponcodeService.Queryable().FirstOrDefault(a => a.Id == model.IntParam1);
            if (couponCode != null && couponCode.Quantity > 0)
            {
                couponCode.Quantity = couponCode.Quantity - 1;
                couponCode.ObjectState = ObjectState.Modified;
            }
            var order = bookingModel.Order;
            order.CreatedDate = DateTime.Now;
            order.ModifiedDate = DateTime.Now;
            order.Status = (int)Framework.Utilities.BookingStatus.Holding;

            order.Quantity = bookingModel.Adult;
            order.CustomerId = customer.CustomerId;
            order.Pnr = Utilities.GetRandomString(7).ToUpper();
            order.TaxFee = bookingModel.Product.CommissionRate;
            order.Price = bookingModel.Rate.RetailRate;
            order.ProductId = bookingModel.Product.Id;
            order.LocalType = (int)Utilities.Language.English;
            order.CancellationPolicy = cancelationPolicy != null ? cancelationPolicy.Description : string.Empty;
            order.Type = (int)Utilities.Product.Tour;
            order.Children = bookingModel.Children;
            order.ProductName = bookingModel.Product.Name;
            if (!string.IsNullOrEmpty(bookingModel.DepartureOption))
            {
                order.DepartureOption = bookingModel.DepartureOption;
            }
            string tourGroup = string.Empty;
            string tourInclude = string.Empty;
            switch (bookingModel.RateType)
            {

                case 1:
                    tourGroup = bookingModel.Product.TourGroup1;
                    tourInclude = bookingModel.Product.TourGroup1Include;
                    break;
                case 2:
                    tourGroup = bookingModel.Product.TourGroup2;
                    tourInclude = bookingModel.Product.TourGroup2Include;
                    break;
                case 3:
                    tourGroup = bookingModel.Product.TourGroup3;
                    tourInclude = bookingModel.Product.TourGroup3Include;
                    break;
            }

            if (!string.IsNullOrEmpty(tourGroup))
            {
                order.GroupType = tourGroup;
            }
            if (bookingModel.Promotion != null)
            {
                order.DiscountName = bookingModel.Promotion.Name;
                order.Discount = promotion = bookingModel.Promotion.Price ?? 0;
            }
            if (bookingModel.Surcharge != null)
            {
                order.SurchargeName = bookingModel.Surcharge.Name;
                order.SurchargeFee = surcharge = bookingModel.Surcharge.Price ?? 0;
            }
            price = (decimal)(bookingModel.Rate.RetailRate + (bookingModel.Rate.RetailRate * bookingModel.Product.CommissionRate / 100));
            if (bookingModel.Children > 0)
            {
                order.Amount = ((price + surcharge - promotion) * bookingModel.Rate.PersonNo) + ((price * 3 / 4) * bookingModel.Children);
            }
            else
            {
                order.Amount = ((price + surcharge - promotion) * bookingModel.Rate.PersonNo);
            }
            order.CouponCode = model.StrParam1;
            order.Discount = model.DecParam1;
            if (order.Discount > 0)
            {
                order.Amount = order.Amount - order.Discount;
            }
            order.EndDate = DateTime.Now;

            //Insert tour
            order.ObjectState = ObjectState.Added;
            _orderService.Insert(order);

            var order2 = new Order2();
            order2.CreatedDate = DateTime.Now;
            order2.ModifiedDate = DateTime.Now;
            order2.Status = (int)Framework.Utilities.BookingStatus.Holding;
            order2.Children = bookingModel.Children;
            order2.Quantity = bookingModel.Rate.PersonNo;
            order2.CustomerId = customer.CustomerId;
            order2.Pnr = Utilities.GetRandomString(7).ToUpper();
            order2.TaxFee = bookingModel.Product.CommissionRate;
            order2.Price = bookingModel.Rate.RetailRate;
            order2.ProductId = bookingModel.Product.Id;
            order2.LocalType = (int)Utilities.Language.English;
            order2.StartDate = order.StartDate;
            order2.EndDate = order.EndDate;
            order2.CouponCode = model.StrParam1;
            order2.Discount = model.DecParam1;
            if (order2.Discount > 0)
            {
                order2.Amount = order2.Amount - order2.Discount;
            }
            order2.CancellationPolicy = cancelationPolicy != null ? cancelationPolicy.Description : string.Empty;
            order2.Type = (int)Utilities.Product.Tour;
            if (bookingModel.Promotion != null)
            {
                order2.DiscountName = bookingModel.Promotion.Name;
                order2.Discount = promotion = bookingModel.Promotion.Price ?? 0;
            }
            if (bookingModel.Surcharge != null)
            {
                order2.SurchargeName = bookingModel.Surcharge.Name;
                order2.SurchargeFee = surcharge = bookingModel.Surcharge.Price ?? 0;
            }
            // var @decimal = Model.Rate.RetailRate + (Model.Rate.RetailRate * Model.Product.CommissionRate / 100);
            price = (decimal)(bookingModel.Rate.RetailRate + (bookingModel.Rate.RetailRate * bookingModel.Product.CommissionRate / 100));
            //  order2.Amount = ((price + surcharge - promotion) * bookingModel.Rate.PersonNo);
            if (bookingModel.Children > 0)
            {
                order2.Amount = ((price + surcharge - promotion) * bookingModel.Rate.PersonNo) + ((price * 3 / 2) * bookingModel.Children);
            }
            else
            {
                order2.Amount = ((price + surcharge - promotion) * bookingModel.Rate.PersonNo);
            }
            order2.EndDate = DateTime.Now;

            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Utilities.Status.Active;
                objectModel.Url = "/";
                return Json(objectModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }


            //Insert tour
            order2.ObjectState = ObjectState.Added;
            order2.TourOrderId = order.Id;
            _orderService2.Insert(order2);
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Utilities.Status.Active;
                objectModel.Url = "/";
                return Json(objectModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //Send email request for vinaday team.
            var cancellation = _cancellationService.GetCancellationList().FirstOrDefault(c => c.Id == bookingModel.Product.CancelationPolicy);
            try
            {
                dynamic myObject = new
                {
                    DateTime = DateTime.Now.ToString("D", CultureInfo.CreateSpecificCulture("en-US")),
                    CustomerFullName = $"{customer.Firstname} {customer.Lastname}".ToUpper(),
                    TourName = bookingModel.Product.Name,
                    Departure = bookingModel.Product.Start,
                    DepartureOption = bookingModel.DepartureOption,
                    Pnr = order.Pnr,
                    CheckIn = order.StartDate.ToShortDateString(),
                    Participants = bookingModel.Children > 0 ?
                        $"{(bookingModel.Rate.PersonNo > 1 ? $"{bookingModel.Rate.PersonNo} persons {bookingModel.Children} children" : $"{bookingModel.Rate.PersonNo} person {bookingModel.Children} children")} x US${order.Amount}" :
                        $"{(bookingModel.Rate.PersonNo > 1 ? $"{bookingModel.Rate.PersonNo} persons" : $"{bookingModel.Rate.PersonNo} person")} x US${order.Amount}",
                    Price = bookingModel.Rate.RetailRate,
                    Total = order.Amount,
                    VN = false,
                    Children = order.Children,
                    Email = bookingModel.Customer.Email,
                    Phone = bookingModel.Customer.PhoneNumber,
                    Gender = _cacheCustomer.Gender,
                    IsThirdPerson = _cacheCustomer.IsThirdPerson,
                    ThirdPersonGender = _cacheCustomer.ThirdPersonGender,
                    ThirdPersonFirstname = _cacheCustomer.ThirdPersonFirstname,
                    ThirdPersonLastname = _cacheCustomer.ThirdPersonLastname,
                    IsGiftVoucher = _cacheCustomer.IsGiftVoucher,
                    Gift_type = _cacheCustomer.Gift_type,
                    PersonalMessage = _cacheCustomer.PersonalMessage,
                    SpecialRequest = bookingModel.Order.SpecialRequest,
                    TourPriceOption = bookingModel.TourPriceOption,
                    RoomOptionName = bookingModel.RoomOptionName,
                    Cancellation = cancellation != null ? cancellation.Description : string.Empty

                };
                string bookingEmailResult = Framework.Utilities.ParseTemplate("TourRequest", myObject);
                var subject = $"VINADAY goREISE - Booking #{order.Pnr}";
                MailClient.RequestBookingHotel(bookingModel.Customer.Email, bookingEmailResult, subject);
            }
            catch (Exception ex)
            {

                //log error here
            }
            objectModel.Status = (int)Utilities.Status.Active;
            return Json(objectModel);

        }
        [HttpPost]
        public JsonResult ConfirmTourWithCreditCard(Customer customer, CreditCard creditCard, ObjectModel model)
        {
            //Add CreditCard
            var card = InsertCreditCard(creditCard);
            var objectModel = new ObjectModel();
            var bookingModel = Session[Utilities.BookingSession] as BookingModel;
            if (bookingModel == null)
            {
                objectModel.Status = (int)Utilities.Status.Active;
                objectModel.Url = "/";
                return Json(objectModel);
            }
            //Add customer
            var existCustomer = new Customer();
            if (customer != null)
            {
                var country = _countryService.GetCountryList().FirstOrDefault(c => c.CountryId == customer.NationalId);
                customer.District = country != null ? country.Name : string.Empty;
                bookingModel.Customer = customer;
                bookingModel.Order.SpecialRequest = customer.Street;
                existCustomer = _customerService.GetCustomerByEmail(customer.Email);
            }
            if (existCustomer == null)
            {
                //Insert customer
                customer.ObjectState = ObjectState.Added;
                _customerService.Insert(customer);
                try
                {
                    _unitOfWorkAsync.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    objectModel.Status = (int)Utilities.Status.Active;
                    objectModel.Url = "/";
                    return Json(objectModel);
                }
            }
            else
            {
                customer = existCustomer;
            }
            decimal price = 0;
            decimal surcharge = 0;
            decimal promotion = 0;
            //Add Order
            var order = bookingModel.Order;
            string tourGroup = string.Empty;
            string tourInclude = string.Empty;
            switch (bookingModel.RateType)
            {

                case 1:
                    tourGroup = bookingModel.Product.TourGroup1;
                    tourInclude = bookingModel.Product.TourGroup1Include;
                    break;
                case 2:
                    tourGroup = bookingModel.Product.TourGroup2;
                    tourInclude = bookingModel.Product.TourGroup2Include;
                    break;
                case 3:
                    tourGroup = bookingModel.Product.TourGroup3;
                    tourInclude = bookingModel.Product.TourGroup3Include;
                    break;
            }
            Customer _cacheCustomer = customer;
            order.CreatedDate = DateTime.Now;
            order.ModifiedDate = DateTime.Now;
            order.Status = (int)Framework.Utilities.BookingStatus.Holding;
            if (!string.IsNullOrEmpty(bookingModel.DepartureOption))
            {
                order.DepartureOption = bookingModel.DepartureOption;
            }
            if (!string.IsNullOrEmpty(tourInclude))
            {
                order.Note = tourInclude;
            }
            if (!string.IsNullOrEmpty(tourGroup))
            {
                order.GroupType = tourGroup;
            }
            order.Quantity = bookingModel.Rate.PersonNo;
            order.CustomerId = customer.CustomerId;
            order.Pnr = Utilities.GetRandomString(7).ToUpper();
            order.TaxFee = bookingModel.Product.CommissionRate;
            order.Price = bookingModel.Rate.RetailRate;
            order.ProductId = bookingModel.Product.Id;
            order.CardId = card.Id;
            order.LocalType = (int)Utilities.Language.English;
            order.Type = (int)Utilities.Product.Tour;
            order.PaymentMethod = (int)Utilities.PaymentMethod.Credit;
            order.CouponCode = model.StrParam1;
            order.Discount = model.DecParam1;
            order.CardNumber = !string.IsNullOrEmpty(card.CardNumber)
                ? $"xxxx-xxxx-xxxx-x{Utilities.GetLast(card.CardNumber, 3)}"
                : string.Empty;
            if (bookingModel.Promotion != null)
            {
                order.DiscountName = bookingModel.Promotion.Name;
                order.Discount = promotion = bookingModel.Promotion.Price ?? 0;
            }
            if (bookingModel.Surcharge != null)
            {
                order.SurchargeName = bookingModel.Surcharge.Name;
                order.SurchargeFee = surcharge = bookingModel.Surcharge.Price ?? 0;
            }
            price = (decimal)(bookingModel.Rate.RetailRate + (bookingModel.Rate.RetailRate * bookingModel.Product.CommissionRate / 100));
            order.Amount = ((price + surcharge - promotion) * bookingModel.Rate.PersonNo);
            order.EndDate = DateTime.Now;

            //Insert tour
            order.ObjectState = ObjectState.Added;
            _orderService.Insert(order);

            var order2 = new Order2();
            order2.CreatedDate = DateTime.Now;
            order2.ModifiedDate = DateTime.Now;
            order2.Status = (int)Framework.Utilities.BookingStatus.Holding;
            order2.StartDate = order.StartDate;
            order2.EndDate = order.EndDate;
            order2.Quantity = bookingModel.Rate.PersonNo;
            order2.CustomerId = customer.CustomerId;
            order2.Pnr = Utilities.GetRandomString(7).ToUpper();
            order2.TaxFee = bookingModel.Product.CommissionRate;
            order2.Price = bookingModel.Rate.RetailRate;
            order2.ProductId = bookingModel.Product.Id;
            order2.CardId = card.Id;
            order2.LocalType = (int)Utilities.Language.English;
            order2.Type = (int)Utilities.Product.Tour;
            order2.PaymentMethod = (int)Utilities.PaymentMethod.Credit;
            order2.CouponCode = model.StrParam1;
            order2.Discount = model.DecParam1;
            order2.CardNumber = !string.IsNullOrEmpty(card.CardNumber)
                ? $"xxxx-xxxx-xxxx-x{Utilities.GetLast(card.CardNumber, 3)}"
                : string.Empty;
            if (bookingModel.Promotion != null)
            {
                order2.DiscountName = bookingModel.Promotion.Name;
                order2.Discount = promotion = bookingModel.Promotion.Price ?? 0;
            }
            if (bookingModel.Surcharge != null)
            {
                order2.SurchargeName = bookingModel.Surcharge.Name;
                order2.SurchargeFee = surcharge = bookingModel.Surcharge.Price ?? 0;
            }
            price = (decimal)(bookingModel.Rate.RetailRate + (bookingModel.Rate.RetailRate * bookingModel.Product.CommissionRate / 100));
            order2.Amount = ((price + surcharge - promotion) * bookingModel.Rate.PersonNo);
            order2.EndDate = DateTime.Now;
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Utilities.Status.Active;
                objectModel.Url = "/";
                return Json(objectModel);
            }
            //Insert tour
            order2.ObjectState = ObjectState.Added;
            order2.TourOrderId = order.Id;
            _orderService2.Insert(order2);
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Utilities.Status.Active;
                objectModel.Url = "/";
                return Json(objectModel);
            }

            //Send email request for vinaday team.
            var cancellation = _cancellationService.GetCancellationList().FirstOrDefault(c => c.Id == bookingModel.Product.CancelationPolicy);
            try
            {
                dynamic myObject = new
                {
                    //DateTime = DateTime.Now.ToString("D", CultureInfo.CreateSpecificCulture("en-US")),
                    //CustomerFullName = $"{customer.Firstname} {customer.Lastname}",
                    //TourName = bookingModel.Product.Name,
                    //Departure = bookingModel.Product.Start,
                    //Pnr = order.Pnr,
                    //Participants =
                    //    $"{(bookingModel.Rate.PersonNo > 1 ? $"{bookingModel.Rate.PersonNo} persons" : $"{bookingModel.Rate.PersonNo} person")} x US${Math.Round((decimal)(bookingModel.Rate.RetailRate + (bookingModel.Rate.RetailRate * bookingModel.Product.CommissionRate / 100)), 2)}",
                    //Price = bookingModel.Rate.RetailRate,
                    //Total = ((bookingModel.Rate.RetailRate + (bookingModel.Rate.RetailRate * bookingModel.Product.CommissionRate / 100)) * bookingModel.Rate.PersonNo),
                    //Email = bookingModel.Customer.Email,
                    //Phone = bookingModel.Customer.PhoneNumber,
                    //SpecialRequest = bookingModel.Order.SpecialRequest,
                    //Cancellation = cancellation != null ? cancellation.Description : string.Empty
                    DateTime = DateTime.Now.ToString("D", CultureInfo.CreateSpecificCulture("en-US")),
                    CustomerFullName = $"{customer.Firstname} {customer.Lastname}".ToUpper(),
                    TourName = bookingModel.Product.Name,
                    Departure = bookingModel.Product.Start,
                    DepartureOption = bookingModel.DepartureOption,
                    Pnr = order.Pnr,
                    CheckIn = order.StartDate.ToShortDateString(),
                    Participants = bookingModel.Children > 0 ?
                        $"{(bookingModel.Rate.PersonNo > 1 ? $"{bookingModel.Rate.PersonNo} persons {bookingModel.Children} children" : $"{bookingModel.Rate.PersonNo} person {bookingModel.Children} children")} x US${order.Amount}" :
                        $"{(bookingModel.Rate.PersonNo > 1 ? $"{bookingModel.Rate.PersonNo} persons" : $"{bookingModel.Rate.PersonNo} person")} x US${order.Amount}",
                    Price = bookingModel.Rate.RetailRate,
                    Total = order.Amount,
                    VN = false,
                    Children = order.Children,
                    Email = bookingModel.Customer.Email,
                    Phone = bookingModel.Customer.PhoneNumber,
                    Gender = _cacheCustomer.Gender,
                    IsThirdPerson = _cacheCustomer.IsThirdPerson,
                    ThirdPersonGender = _cacheCustomer.ThirdPersonGender,
                    ThirdPersonFirstname = _cacheCustomer.ThirdPersonFirstname,
                    ThirdPersonLastname = _cacheCustomer.ThirdPersonLastname,
                    IsGiftVoucher = _cacheCustomer.IsGiftVoucher,
                    Gift_type = _cacheCustomer.Gift_type,
                    PersonalMessage = _cacheCustomer.PersonalMessage,
                    SpecialRequest = bookingModel.Order.SpecialRequest,
                    TourPriceOption = bookingModel.TourPriceOption,
                    RoomOptionName = bookingModel.RoomOptionName,
                    Cancellation = cancellation != null ? cancellation.Description : string.Empty
                };
                string bookingEmailResult = Framework.Utilities.ParseTemplate("TourRequest", myObject);
                var subject = $"BOOKING #{order.Pnr}";
                MailClient.RequestBookingHotel(bookingModel.Customer.Email, bookingEmailResult, subject);
            }
            catch (Exception)
            {
                //log error here
            }
            objectModel.Status = (int)Utilities.Status.Active;
            return Json(objectModel);
        }
        [Route("book-tour-completed")]
        public ActionResult CompletedTour()
        {

            var bookingModel = Session[Utilities.BookingSession] as BookingModel;
            if (bookingModel == null) return Redirect("/");
            ViewBag.Booking = bookingModel;
            Session[Utilities.BookingSession] = null;
            return View();
        }

        [HttpPost]
        public ActionResult GetPromotionCode(ObjectModel objectModel)
        {

            var couponCode = _couponcodeService.Queryable().FirstOrDefault(a => a.Code == objectModel.StrParam1);
            var jsonResult = Json(new
            {
                success = true,
                message = "Correct coupon code"
            }, JsonRequestBehavior.AllowGet);

            if (couponCode != null)
            {
                jsonResult.Data = couponCode;
            }
            else
            {
                jsonResult = Json(new
                {
                    success = false,
                    message = "Coupon not found"
                }, JsonRequestBehavior.AllowGet);
            }

            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

        #region For Tour Vietnam

        [Route("vn/shopping-cart/{id}/{checkInDate}/{person}/{children}")]
        public ActionResult CartVTour(int id, string checkInDate, int person, int children)
        {
            var tour = _tourService.Queryable().FirstOrDefault(t => t.Id == id);
            if (tour == null) return Redirect("/");
            var date = Utilities.ConvertStringToDateTime(checkInDate);
            var image = _mediaService.Queryable().FirstOrDefault(t => t.OwnerId == id && t.MediaType == (int)Utilities.MediaType.Banner);
            var rate = _tourRateService.Queryable().FirstOrDefault(t => t.TourId == id && t.PersonNo == person);
            var exchange = _exchangeService.GetRateExchangeById(3);
            var surcharges = _tourSurchargeService.GetSurchargesByDayTourId(date, id);
            var promotions = _tourPromotionService.GetPromotionByDayTourId(date, id);
            if (tour.ParentId.HasValue)
            {
                rate = _tourRateService.Queryable().FirstOrDefault(t => t.TourId == tour.ParentId && t.PersonNo == person);
                surcharges = _tourSurchargeService.GetSurchargesByDayTourId(date, tour.ParentId.Value);
                promotions = _tourPromotionService.GetPromotionByDayTourId(date, tour.ParentId.Value);
            }
            if (promotions != null)
            {
                foreach (var _promotion in promotions)
                {
                    if (_promotion.Language == 1)
                        continue;
                    if (_promotion.NumberPerson > 0 && _promotion.NumberPerson == person)
                    {
                        // Discount type = 1 then discoutn percent, else discount by price
                        if (_promotion.DiscountType == 1)
                        {
                            decimal percentage = _promotion.Get.HasValue ? (decimal)_promotion.Get.Value : 0;
                            rate.RetailRate = rate.RetailRate - ((percentage * rate.RetailRate) / 100);
                        }
                        else
                        {
                            decimal discountValue = _promotion.Get.HasValue ? (decimal)_promotion.Get.Value : 0;
                            rate.RetailRate = rate.RetailRate - discountValue > 0 ? rate.RetailRate - discountValue : rate.RetailRate;
                        }
                    }
                    else
                    {
                        // Discount type = 1 then discoutn percent, else discount by price
                        if (_promotion.DiscountType == 1)
                        {
                            decimal percentage = _promotion.Get.HasValue ? (decimal)_promotion.Get.Value : 0;
                            rate.RetailRate = rate.RetailRate - ((percentage * rate.RetailRate) / 100);
                        }
                        else
                        {
                            decimal discountValue = _promotion.Get.HasValue ? (decimal)_promotion.Get.Value : 0;
                            rate.RetailRate = rate.RetailRate - discountValue > 0 ? rate.RetailRate - discountValue : rate.RetailRate;
                        }
                    }
                }
            }

            rate.RetailRate = rate.RetailRate * exchange.CurrentPrice;
            if (image == null)
            {
                image = _mediaService.Queryable().FirstOrDefault(t => t.OwnerId == tour.ParentId && t.MediaType == (int)Utilities.MediaType.Banner);
            }
            Session[Utilities.BookingSession] = null;
            var order = new Order
            {
                StartDate = date
            };

            var bookingModel = new BookingModel
            {
                Product = tour,
                Rate = rate,
                Image = image,
                Order = order,
                //Promotion = promotion,
                //Surcharge = surcharge
            };
            Session[Utilities.BookingSession] = bookingModel;
            string url = $"/vn/book-tour|{id}/depart-date|{checkInDate}/person|{person}/children|{children}";
            return Redirect(url);
        }
        [Route("vn/book-tour|{id}/depart-date|{checkInDate}/person|{person}/children|{children}")]
        public ActionResult PaymentVTour()
        {
            var objectModel = new ObjectModel();
            var bookingModel = Session[Utilities.BookingSession] as BookingModel;
            if (bookingModel == null)
            {
                objectModel.Url = "/";
                return Json(objectModel);
            }
            ViewBag.Nationalities = _nationalityService.GetNationalityList();
            ViewBag.Cancellation = _cancellationService.GetCancellationList().FirstOrDefault(c => c.Id == bookingModel.Product.CancelationPolicy);
            return View(bookingModel);
        }
        public ActionResult ConfirmVTour(Customer customer)
        {
            var objectModel = new ObjectModel();
            Customer _cacheCustomer = customer;
            var bookingModel = Session[Utilities.BookingSession] as BookingModel;
            if (bookingModel == null)
            {
                objectModel.Status = (int)Utilities.Status.Active;
                objectModel.Url = "/";
                return Json(objectModel);
            }
            //Add customer
            var existCustomer = new Customer();
            if (customer != null)
            {
                var country = _countryService.GetCountryList().FirstOrDefault(c => c.CountryId == customer.NationalId);
                customer.District = country != null ? country.Name : string.Empty;
                bookingModel.Customer = customer;
                bookingModel.Order.SpecialRequest = customer.Street;
                existCustomer = _customerService.GetCustomerByEmail(customer.Email);
            }
            if (existCustomer == null)
            {
                //Insert customer
                customer.ObjectState = ObjectState.Added;
                _customerService.Insert(customer);
                try
                {
                    _unitOfWorkAsync.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    objectModel.Status = (int)Utilities.Status.Active;
                    objectModel.Url = "/";
                    return Json(objectModel);
                }
            }
            else
            {
                customer = existCustomer;
            }
            decimal price = 0;
            decimal surcharge = 0;
            decimal promotion = 0;
            var exchange = _exchangeService.GetRateExchangeById(3);
            //Add Order
            var cancelationPolicy = _cancellationService.GetCancellationList().FirstOrDefault(c => c.Id == bookingModel.Product.CancelationPolicy);
            var order = bookingModel.Order;
            order.CreatedDate = DateTime.Now;
            order.ModifiedDate = DateTime.Now;
            order.Status = (int)Framework.Utilities.BookingStatus.Holding;

            order.Quantity = bookingModel.Rate.PersonNo;
            order.CustomerId = customer.CustomerId;
            order.Pnr = Utilities.GetRandomString(7).ToUpper();
            order.TaxFee = bookingModel.Product.CommissionRate;
            order.Price = bookingModel.Rate.RetailRate / exchange.CurrentPrice;
            order.ProductId = bookingModel.Product.Id;
            order.LocalType = (int)Utilities.Language.English;
            order.CancellationPolicy = cancelationPolicy != null ? cancelationPolicy.Description : string.Empty;
            order.Type = (int)Utilities.Product.Tour;
            order.ProductName = bookingModel.Product.Name;
            if (bookingModel.Promotion != null)
            {
                order.DiscountName = bookingModel.Promotion.Name;
                order.Discount = promotion = bookingModel.Promotion.Price ?? 0;
            }
            if (bookingModel.Surcharge != null)
            {
                order.SurchargeName = bookingModel.Surcharge.Name;
                order.SurchargeFee = surcharge = bookingModel.Surcharge.Price ?? 0;
            }
            price = (decimal)(bookingModel.Rate.RetailRate + (bookingModel.Rate.RetailRate * bookingModel.Product.CommissionRate / 100));
            order.Amount = (((price + surcharge - promotion) * bookingModel.Rate.PersonNo) / exchange.CurrentPrice);
            order.EndDate = DateTime.Now;

            //Insert tour
            order.ObjectState = ObjectState.Added;
            _orderService.Insert(order);


            var order2 = new Order2();
            order2.CreatedDate = DateTime.Now;
            order2.ModifiedDate = DateTime.Now;
            order2.Status = (int)Framework.Utilities.BookingStatus.Holding;
            order2.StartDate = order.StartDate;
            order2.Quantity = bookingModel.Rate.PersonNo;
            order2.CustomerId = customer.CustomerId;
            order2.Pnr = Utilities.GetRandomString(7).ToUpper();
            order2.TaxFee = bookingModel.Product.CommissionRate;
            order2.Price = bookingModel.Rate.RetailRate / exchange.CurrentPrice;
            order2.ProductId = bookingModel.Product.Id;
            order2.LocalType = (int)Utilities.Language.English;
            order2.CancellationPolicy = cancelationPolicy != null ? cancelationPolicy.Description : string.Empty;
            order2.Type = (int)Utilities.Product.Tour;
            if (bookingModel.Promotion != null)
            {
                order2.DiscountName = bookingModel.Promotion.Name;
                order2.Discount = promotion = bookingModel.Promotion.Price ?? 0;
            }
            if (bookingModel.Surcharge != null)
            {
                order2.SurchargeName = bookingModel.Surcharge.Name;
                order2.SurchargeFee = surcharge = bookingModel.Surcharge.Price ?? 0;
            }
            price = (decimal)(bookingModel.Rate.RetailRate + (bookingModel.Rate.RetailRate * bookingModel.Product.CommissionRate / 100));
            order2.Amount = (((price + surcharge - promotion) * bookingModel.Rate.PersonNo) / exchange.CurrentPrice);
            order2.EndDate = DateTime.Now;

            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Utilities.Status.Active;
                objectModel.Url = "/";
                return Json(objectModel);
            }


            //Insert tour
            order2.ObjectState = ObjectState.Added;
            order2.TourOrderId = order.Id;
            _orderService2.Insert(order2);


            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Utilities.Status.Active;
                objectModel.Url = "/";
                return Json(objectModel);
            }

            //Send email request for vinaday team.
            var cancellation = _cancellationService.GetCancellationList().FirstOrDefault(c => c.Id == bookingModel.Product.CancelationPolicy);
            try
            {
                ///TODO: check function send booking request to customer and admin
                dynamic myObject = new
                {
                    //DateTime = DateTime.Now.ToString("D", CultureInfo.CreateSpecificCulture("en-US")),
                    //CustomerFullName = $"{customer.Firstname} {customer.Lastname}".ToUpper(),
                    //TourName = bookingModel.Product.Name,
                    //Departure = bookingModel.Product.Start,
                    //Pnr = order.Pnr,
                    //CheckIn = order.StartDate.ToString("MMM/dd/yyyy"),
                    //Participants =
                    //    $"{($"{bookingModel.Rate.PersonNo} người lớn")} x { (Math.Round((decimal)(bookingModel.Rate.RetailRate + (bookingModel.Rate.RetailRate * bookingModel.Product.CommissionRate / 100)), 2)).ToString("###,#")}",
                    //Gender = _cacheCustomer.Gender,
                    //IsThirdPerson = _cacheCustomer.IsThirdPerson,
                    //ThirdPersonGender = _cacheCustomer.ThirdPersonGender,
                    //ThirdPersonFirstname = _cacheCustomer.ThirdPersonFirstname,
                    //ThirdPersonLastname = _cacheCustomer.ThirdPersonLastname,
                    //IsGiftVoucher = _cacheCustomer.IsGiftVoucher,
                    //Gift_type = _cacheCustomer.Gift_type,
                    //PersonalMessage = _cacheCustomer.PersonalMessage,

                    //Price = bookingModel.Rate.RetailRate,
                    //Total = ((bookingModel.Rate.RetailRate + (bookingModel.Rate.RetailRate * bookingModel.Product.CommissionRate / 100)) * bookingModel.Rate.PersonNo),
                    //Email = bookingModel.Customer.Email,
                    //Phone = bookingModel.Customer.PhoneNumber,
                    //SpecialRequest = bookingModel.Order.SpecialRequest,
                    //VN = true,
                    //Cancellation = cancellation != null ? cancellation.Description : string.Empty
                    DateTime = DateTime.Now.ToString("D", CultureInfo.CreateSpecificCulture("en-US")),
                    CustomerFullName = $"{customer.Firstname} {customer.Lastname}".ToUpper(),
                    TourName = bookingModel.Product.Name,
                    Departure = bookingModel.Product.Start,
                    DepartureOption = bookingModel.DepartureOption,
                    Pnr = order.Pnr,
                    CheckIn = order.StartDate.ToShortDateString(),
                    Participants = bookingModel.Children > 0 ?
                        $"{(bookingModel.Rate.PersonNo > 1 ? $"{bookingModel.Rate.PersonNo} persons {bookingModel.Children} children" : $"{bookingModel.Rate.PersonNo} person {bookingModel.Children} children")} x US${order.Amount}" :
                        $"{(bookingModel.Rate.PersonNo > 1 ? $"{bookingModel.Rate.PersonNo} persons" : $"{bookingModel.Rate.PersonNo} person")} x US${order.Amount}",
                    Price = bookingModel.Rate.RetailRate,
                    Total = order.Amount,
                    VN = false,
                    Children = order.Children,
                    Email = bookingModel.Customer.Email,
                    Phone = bookingModel.Customer.PhoneNumber,
                    Gender = _cacheCustomer.Gender,
                    IsThirdPerson = _cacheCustomer.IsThirdPerson,
                    ThirdPersonGender = _cacheCustomer.ThirdPersonGender,
                    ThirdPersonFirstname = _cacheCustomer.ThirdPersonFirstname,
                    ThirdPersonLastname = _cacheCustomer.ThirdPersonLastname,
                    IsGiftVoucher = _cacheCustomer.IsGiftVoucher,
                    Gift_type = _cacheCustomer.Gift_type,
                    PersonalMessage = _cacheCustomer.PersonalMessage,
                    SpecialRequest = bookingModel.Order.SpecialRequest,
                    TourPriceOption = bookingModel.TourPriceOption,
                    RoomOptionName = bookingModel.RoomOptionName,
                    Cancellation = cancellation != null ? cancellation.Description : string.Empty
                };
                string bookingEmailResult = Framework.Utilities.ParseTemplate("TourRequest", myObject);
                var subject = $"VINADAY goREISE - Booking #{order.Pnr}";
                MailClient.RequestBookingHotel(bookingModel.Customer.Email, bookingEmailResult, subject);
            }
            catch (Exception)
            {
                //log error here
            }
            objectModel.Status = (int)Utilities.Status.Active;
            return Json(objectModel);

        }
        [HttpPost]
        public JsonResult ConfirmVTourWithCreditCard(Customer customer, CreditCard creditCard)
        {
            //Add CreditCard
            var card = InsertCreditCard(creditCard);
            var objectModel = new ObjectModel();
            var bookingModel = Session[Utilities.BookingSession] as BookingModel;
            if (bookingModel == null)
            {
                objectModel.Status = (int)Utilities.Status.Active;
                objectModel.Url = "/";
                return Json(objectModel);
            }
            //Add customer
            var existCustomer = new Customer();
            if (customer != null)
            {
                var country = _countryService.GetCountryList().FirstOrDefault(c => c.CountryId == customer.NationalId);
                customer.District = country != null ? country.Name : string.Empty;
                bookingModel.Customer = customer;
                bookingModel.Order.SpecialRequest = customer.Street;
                existCustomer = _customerService.GetCustomerByEmail(customer.Email);
            }
            if (existCustomer == null)
            {
                //Insert customer
                customer.ObjectState = ObjectState.Added;
                _customerService.Insert(customer);
                try
                {
                    _unitOfWorkAsync.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    objectModel.Status = (int)Utilities.Status.Active;
                    objectModel.Url = "/";
                    return Json(objectModel);
                }
            }
            else
            {
                customer = existCustomer;
            }
            decimal price = 0;
            decimal surcharge = 0;
            decimal promotion = 0;
            //Add Order
            var order = bookingModel.Order;
            order.CreatedDate = DateTime.Now;
            order.ModifiedDate = DateTime.Now;
            order.Status = (int)Framework.Utilities.BookingStatus.Holding;
            var exchange = _exchangeService.GetRateExchangeById(3);
            order.Quantity = bookingModel.Rate.PersonNo;
            order.CustomerId = customer.CustomerId;
            order.Pnr = Utilities.GetRandomString(7).ToUpper();
            order.TaxFee = bookingModel.Product.CommissionRate;
            order.Price = bookingModel.Rate.RetailRate / exchange.CurrentPrice;
            order.ProductId = bookingModel.Product.Id;
            order.CardId = card.Id;
            order.LocalType = (int)Utilities.Language.English;
            order.Type = (int)Utilities.Product.Tour;
            order.PaymentMethod = (int)Utilities.PaymentMethod.Credit;
            order.CardNumber = !string.IsNullOrEmpty(card.CardNumber)
                ? $"xxxx-xxxx-xxxx-x{Utilities.GetLast(card.CardNumber, 3)}"
                : string.Empty;
            if (bookingModel.Promotion != null)
            {
                order.DiscountName = bookingModel.Promotion.Name;
                order.Discount = promotion = bookingModel.Promotion.Price ?? 0;
            }
            if (bookingModel.Surcharge != null)
            {
                order.SurchargeName = bookingModel.Surcharge.Name;
                order.SurchargeFee = surcharge = bookingModel.Surcharge.Price ?? 0;
            }
            price = (decimal)(bookingModel.Rate.RetailRate + (bookingModel.Rate.RetailRate * bookingModel.Product.CommissionRate / 100));
            order.Amount = (((price + surcharge - promotion) * bookingModel.Rate.PersonNo) / exchange.CurrentPrice);
            order.EndDate = DateTime.Now;

            //Insert tour
            order.ObjectState = ObjectState.Added;
            _orderService.Insert(order);

            var order2 = new Order2();


            order2.CreatedDate = DateTime.Now;
            order2.ModifiedDate = DateTime.Now;
            order2.Status = (int)Framework.Utilities.BookingStatus.Holding;

            order2.Quantity = bookingModel.Rate.PersonNo;
            order2.CustomerId = customer.CustomerId;
            order2.Pnr = Utilities.GetRandomString(7).ToUpper();
            order2.TaxFee = bookingModel.Product.CommissionRate;
            order2.Price = bookingModel.Rate.RetailRate / exchange.CurrentPrice;
            order2.ProductId = bookingModel.Product.Id;
            order2.CardId = card.Id;
            order2.LocalType = (int)Utilities.Language.English;
            order2.Type = (int)Utilities.Product.Tour;
            order2.PaymentMethod = (int)Utilities.PaymentMethod.Credit;
            order2.CardNumber = !string.IsNullOrEmpty(card.CardNumber)
                ? $"xxxx-xxxx-xxxx-x{Utilities.GetLast(card.CardNumber, 3)}"
                : string.Empty;
            if (bookingModel.Promotion != null)
            {
                order2.DiscountName = bookingModel.Promotion.Name;
                order2.Discount = promotion = bookingModel.Promotion.Price ?? 0;
            }
            if (bookingModel.Surcharge != null)
            {
                order2.SurchargeName = bookingModel.Surcharge.Name;
                order2.SurchargeFee = surcharge = bookingModel.Surcharge.Price ?? 0;
            }
            price = (decimal)(bookingModel.Rate.RetailRate + (bookingModel.Rate.RetailRate * bookingModel.Product.CommissionRate / 100));
            order2.Amount = (((price + surcharge - promotion) * bookingModel.Rate.PersonNo) / exchange.CurrentPrice);
            order2.EndDate = DateTime.Now;

            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Utilities.Status.Active;
                objectModel.Url = "/";
                return Json(objectModel);
            }
            //Insert tour
            order2.TourOrderId = order.Id;
            order2.ObjectState = ObjectState.Added;
            _orderService2.Insert(order2);

            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Utilities.Status.Active;
                objectModel.Url = "/";
                return Json(objectModel);
            }

            //Send email request for vinaday team.
            var cancellation = _cancellationService.GetCancellationList().FirstOrDefault(c => c.Id == bookingModel.Product.CancelationPolicy);
            try
            {
                dynamic myObject = new
                {
                    DateTime = DateTime.Now.ToString("D", CultureInfo.CreateSpecificCulture("en-US")),
                    CustomerFullName = $"{customer.Firstname} {customer.Lastname}",
                    TourName = bookingModel.Product.Name,
                    Departure = bookingModel.Product.Start,
                    Pnr = order.Pnr,
                    Participants =
                        $"{(bookingModel.Rate.PersonNo > 1 ? $"{bookingModel.Rate.PersonNo} persons" : $"{bookingModel.Rate.PersonNo} person")} x US${Math.Round((decimal)(bookingModel.Rate.RetailRate + (bookingModel.Rate.RetailRate * bookingModel.Product.CommissionRate / 100)), 2)}",
                    Price = bookingModel.Rate.RetailRate,
                    Total = ((bookingModel.Rate.RetailRate + (bookingModel.Rate.RetailRate * bookingModel.Product.CommissionRate / 100)) * bookingModel.Rate.PersonNo),
                    Email = bookingModel.Customer.Email,
                    Phone = bookingModel.Customer.PhoneNumber,
                    SpecialRequest = bookingModel.Order.SpecialRequest,
                    Cancellation = cancellation != null ? cancellation.Description : string.Empty
                };
                string bookingEmailResult = Framework.Utilities.ParseTemplate("TourRequest", myObject);
                var subject = $"BOOKING #{order.Pnr}";
                MailClient.RequestBookingHotel(bookingModel.Customer.Email, bookingEmailResult, subject);
            }
            catch (Exception)
            {
                //log error here
            }
            objectModel.Status = (int)Utilities.Status.Active;
            return Json(objectModel);
        }
        [Route("vn/book-tour-completed")]
        public ActionResult CompletedVTour()
        {

            var bookingModel = Session[Utilities.BookingSession] as BookingModel;
            if (bookingModel == null) return Redirect("/");
            ViewBag.Booking = bookingModel;
            Session[Utilities.BookingSession] = null;
            return View();
        }
        #endregion

        #region for Coupon Hotel
        [Route("vn/HotelCoupon/{id}/{qty}")]
        public ActionResult HotelCoupon(int id, int qty)
        {
            if (Request.Url != null)
            {
                Session[Constant.ShoppingCartUrl] = Request.Url.AbsoluteUri;
            }
            Session[Constant.HotelCouponSession] = null;
            var product = _bookingService.GetHotelCoupon(id, qty);
            Session[Constant.HotelCouponSession] = product;
            //  Session[Utilities.BookingSession] = product;
            return Redirect("/vn/paymentHotelCoupon");
        }
        [Route("vn/paymentHotelCoupon")]
        public ActionResult PaymentHotelCoupon()
        {
            ViewBag.Nationalities = _nationalityService.GetNationalityList();
            var product = (ProductModel)Session[Constant.HotelCouponSession];
            if (product == null)
            {
                return Redirect("/viet-nam");
            }
            ViewBag.CancellationPolicies = product.TotalSave > 0
                ? new List<CancellationModel>()
                : _hotelService.GetHotelCancellationPoliciesByHotelId(product.ProductId);
            product.ShoppingCartUrl = Server.UrlEncode(Session[Constant.ShoppingCartUrl] as string);
            var customer = new Customer();
            var claim = ((ClaimsIdentity)User.Identity).FindFirst("Id");
            if (claim != null)
            {
                var id = claim.Value;
                customer = _customerService.GetCustomerByMemberId(id);
            }
            ViewBag.Customer = customer;
            return View(product);
        }

        [HttpPost]
        public JsonResult ConfirmHotelVoucher(Customer customer)
        {
            var _cacheCustomer = customer;
            var product = (ProductModel)Session[Constant.HotelCouponSession];
            if (product == null || customer == null)
            {
                return Json(new { success = false, error = "Error when processing booking" }, JsonRequestBehavior.AllowGet);
            }
            //Add customer
            var specialRequest = customer.Street; //Rendering as special request property
            var existCustomer = _customerService.GetCustomerByEmail(customer.Email);
            if (existCustomer == null)
            {
                customer.ObjectState = ObjectState.Added;
                _customerService.Insert(customer);
                try
                {
                    _unitOfWorkAsync.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Json(new { success = false, error = "Error when processing booking" },
                        JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                customer = existCustomer;
            }
            //Add Order
            var pnr = Framework.Utilities.GetRandomString(7).ToUpper(); //Random PNR order
            var order = new Order
            {
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,

                Status = (int)Framework.Utilities.BookingStatus.Holding,
                Quantity = product.Quantity,
                CustomerId = customer.CustomerId,
                Pnr = pnr,
                TaxFee = product.TotalTaxeFee,
                Price = product.TotalPrice,
                ProductId = product.Id,
                Amount = product.FinalPrice,
                Type = (int)Framework.Utilities.Product.HotelPackage,
                IpLocation = Framework.Utilities.GetUserIp(Request),
                SpecialRequest = specialRequest,
                Discount = product.TotalSave,
                CancellationPolicy = product.CancellationPolicy != null ? product.CancellationPolicy.Description : string.Empty,
                ProductName = product.Name + " " + product.DetailName,
                DiscountName = product.Promotion != null ? $"{product.Promotion.Name}<br/>{product.Promotion.Description}" : string.Empty,
                ObjectState = ObjectState.Added,
                LocalType = (int)Utilities.Language.English
            };

            //Insert hotel order
            _orderService.Add(order);

            var order2 = new Order2
            {
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Status = (int)Framework.Utilities.BookingStatus.Holding,
                Quantity = product.Quantity,
                CustomerId = customer.CustomerId,
                Pnr = pnr,
                TaxFee = product.TotalTaxeFee,
                Price = product.TotalPrice,
                ProductId = product.Id,
                Amount = product.FinalPrice,
                Type = (int)Framework.Utilities.Product.HotelPackage,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                Night = product.Night,
                SurchargeFee = product.TotalSurcharge,
                IpLocation = Framework.Utilities.GetUserIp(Request),
                SpecialRequest = specialRequest,
                Discount = product.TotalSave,
                CancellationPolicy = product.CancellationPolicy != null ? product.CancellationPolicy.Description : string.Empty,
                ProductName = product.Name + " " + product.DetailName,
                DiscountName = product.Promotion != null ? $"{product.Promotion.Name}<br/>{product.Promotion.Description}" : string.Empty,
                ObjectState = ObjectState.Added,
                LocalType = (int)Utilities.Language.English,
                TourOrderId = order.Id

            };
            //Insert hotel order
            _orderService2.Add(order2);
            var productDetails = product.Details;
            if (productDetails.Count > 0)
            {
                foreach (var informationOrder in productDetails.Select(detail => new OrderInformations()
                {
                    OrderId = order.Id,
                    Price = detail.PriceRoom,
                    Date = detail.Date.ToString("MM/dd/yyyy"),
                    Quantity = product.Quantity ?? 0,
                    Discount = product.TotalSave,
                    DiscountName =
                        product.Promotion != null
                            ? string.Format("{0}<br/>{1}", product.Promotion.Name, product.Promotion.Description)
                            : string.Empty,
                    Surcharge = detail.PriceSurcharge > 0 ? detail.PriceSurcharge : 0,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    Status = (int)Framework.Utilities.Status.Active,
                    ObjectState = ObjectState.Added
                }))
                {
                    //Insert information order
                    _orderInformationService.Insert(informationOrder);
                }


                // save for accountant bill
                foreach (var informationOrder in productDetails.Select(detail => new OrderInformation2s()
                {
                    OrderId = order.Id,
                    Price = detail.PriceRoom,
                    Date = detail.Date.ToString("MM/dd/yyyy"),
                    Quantity = product.Quantity ?? 0,
                    Discount = product.TotalSave,
                    DiscountName =
                        product.Promotion != null
                            ? string.Format("{0}<br/>{1}", product.Promotion.Name, product.Promotion.Description)
                            : string.Empty,
                    Surcharge = detail.PriceSurcharge > 0 ? detail.PriceSurcharge : 0,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    Status = (int)Framework.Utilities.Status.Pending,
                    ObjectState = ObjectState.Added
                }))
                {
                    //Insert information order
                    _orderInformationService2.Insert(informationOrder);
                }
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            //catch (Exception ex)
            //{

            //}
            catch (DbEntityValidationException ex)
            {
                List<string> errorMessages = new List<string>();
                foreach (DbEntityValidationResult validationResult in ex.EntityValidationErrors)
                {
                    string entityName = validationResult.Entry.Entity.GetType().Name;
                    foreach (DbValidationError error in validationResult.ValidationErrors)
                    {
                        errorMessages.Add(entityName + "." + error.PropertyName + ": " + error.ErrorMessage);
                    }
                }
                return Json(new { success = false, error = "some thing" }, JsonRequestBehavior.AllowGet);
            }
            Session.Clear();
            var nationality = _nationalityService.GetNationality(customer.NationalId ?? -1);
            //Send email request for vinaday team.
            try
            {
                dynamic myObject = new
                {
                    DateTime = DateTime.Now.ToString("D", CultureInfo.CreateSpecificCulture("en-US")),
                    CustomerFullName = $"{customer.Firstname} {customer.Lastname}".ToUpper(),
                    HotelName = product.Name,
                    CheckIn = product.CheckIn,
                    CheckOut = product.CheckOut,
                    TotalPrice = product.TotalPrice,
                    VoucherName = product.DetailName,
                    VoucherDescription = product.Include,
                    Pnr = order.Pnr,
                    Stay = product.Stay,
                    Address = product.Location,
                    RoomName = product.DetailName,
                    RoomPrice = product.TotalPrice,
                    TaxesPrice = product.TotalTaxeFee,
                    Surcharge = product.TotalSurcharge > 0 ? product.TotalSurcharge : 0,
                    Include = product.Include,
                    Quantity = product.Quantity,
                    FinalPrice = product.FinalPrice,
                    Gender = _cacheCustomer.Gender,
                    IsThirdPerson = _cacheCustomer.IsThirdPerson,
                    ThirdPersonGender = _cacheCustomer.ThirdPersonGender,
                    ThirdPersonFirstname = _cacheCustomer.ThirdPersonFirstname,
                    ThirdPersonLastname = _cacheCustomer.ThirdPersonLastname,
                    IsGiftVoucher = _cacheCustomer.IsGiftVoucher,
                    Gift_type = _cacheCustomer.Gift_type,
                    PersonalMessage = _cacheCustomer.PersonalMessage,
                    //GuestLead = customer.l,
                    Nationality = nationality != null ? nationality.Name : "",
                    SpecialRequest = order.SpecialRequest,
                    Email = customer.Email,
                    Phone = customer.PhoneNumber,
                    Cancellation = product.CancellationPolicy
                };
                string bookingEmailResult = Framework.Utilities.ParseTemplate("BookingHotelCoupon", myObject);
                var subject = $"VINADAY goREISE - Booking #{order.Pnr}";
                MailClient.RequestBookingHotel(customer.Email, bookingEmailResult, subject);
            }
            catch (Exception ex)
            {
                var test = ex.ToString();

                //log error here
            }
            var resutls = new { success = true, customer = $"{customer.Firstname} {customer.Lastname}", pnr };
            var jsonResult = Json(resutls, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region User
        [HttpPost]
        public JsonResult Register(Customer customer)
        {
            var objectModel = new ObjectModel();
            var validateCustomer = _customerService.ValidateCustomer(customer.Email, customer.UserName);
            if (validateCustomer)
            {
                customer.ObjectState = ObjectState.Added;
                customer.ISSENDMAIL = true;
                customer.Status = (int)Utilities.Status.Pending;
                _customerService.Insert(customer);
                try
                {
                    _unitOfWorkAsync.SaveChanges();
                    objectModel.Status = (int)Utilities.Status.Active;
                    objectModel.Message = "Register is successfully, please check your email to active account!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    objectModel.Status = (int)Utilities.Status.Inactive;
                    objectModel.Message = "Error when system processing!";
                    return Json(objectModel);
                }
            }
            else
            {
                objectModel.Status = (int)Utilities.Status.Inactive;
                objectModel.Message = "This email exist on system, please enter different email address and continue!";
                return Json(objectModel);
            }
            SendEmailRequestRegister(customer);
            var jsonResult = Json(objectModel, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult Activate(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return View();
            }
            var customer = _customerService.GetCustomerByEmail(email);
            if (customer == null)
            {
                return View();
            };

            return RedirectToAction("Create", "Account", customer);
        }

        public ActionResult ActivateCompleted()
        {
            return View();
        }

        //public ActionResult Activate()
        //{
        //    return View();
        //}
        public void SendEmailRequestRegister(Customer customer)
        {
            //Send email request for vinaday team.
            try
            {
                dynamic myObject = new
                {
                    DateTime = DateTime.Now.ToString("D", CultureInfo.CreateSpecificCulture("en-US")),
                    UserName = customer.UserName,
                    Email = customer.Email,
                    Password = customer.Password,
                    Url = $"https://secure.goreise.com/home/activate/?email={customer.Email}"
                };
                string registerEmailResult = Utilities.ParseTemplate("RegisterRequest", myObject);
                MailClient.RequestRegister(customer.Email, registerEmailResult);
            }
            catch (Exception)
            {
                //log error here
            }
        }

        #endregion
    }
}

