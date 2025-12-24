using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Vinaday.Data.Models;
using Vinaday.Services;
using Vinaday.Web.Framework;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;
using Vinaday.Data.Models.Extention;
using Vinaday.Web.Framework.EmailHelpers;
using Vinaday.Web.Home.Models;
using Utilities = Vinaday.Web.Framework.Utilities;

namespace Vinaday.Web.Home.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ITourService _tourService;
        private readonly IOrderService2 _orderService2;
        private readonly IPaymentOrderService _paymentOrderService;
        private readonly ITourRateService _tourRateService;
        private readonly IMediaService _mediaService;
        private readonly IImageService _imagesService;
        private readonly IPaymentOrderService2 _paymentOrderService2;
        private readonly IPaymentOrderDetailService2 _paymentOrderDetailService2;
        private readonly IRateExchangeService _rateExchangeService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IHotelService _hotelService;
        private readonly ICountryService _countryService;
        private readonly IBookingService _bookingService;
        private readonly ICustomerService _customerService;
        private readonly ICityService _cityService;
        private readonly IDealService _dealService;
        private readonly DealHotelToursService _dealHotelTourService;

        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        public HomeController(
            IOrderService orderService, ITourService tourService, IImageService imagesService,
            IOrderService2 orderService2,
            ITourRateService tourRateService,
            IMediaService mediaService,
            IUnitOfWorkAsync unitOfWorkAsync,
            IOrderInformationService orderInformationService,
            IOrderInformationService2 orderInformationService2,
            IOrderDetailService orderDetailService,
            IHotelService hotelService,
            ICountryService countryService,
            ICustomerService customerService,
            IBookingService bookingService,
            IRateExchangeService rateExchangeService,
            IPaymentOrderService paymentOrderService, IPaymentOrderService2 paymentOrderService2,
            IPaymentOrderDetailService2 paymentOrderDetailService2,
            ICityService cityService,
            DealHotelToursService dealHotelTourService,
            IDealService dealService)
        {
            _orderService = orderService;
            _orderService2 = orderService2;
            _tourService = tourService;
            _imagesService = imagesService;
            _mediaService = mediaService;
            _hotelService = hotelService;
            _customerService = customerService;
            _tourRateService = tourRateService;
            _bookingService = bookingService;
            _unitOfWorkAsync = unitOfWorkAsync;
            _orderDetailService = orderDetailService;
            _countryService = countryService;
            _paymentOrderService2 = paymentOrderService2;
            _paymentOrderDetailService2 = paymentOrderDetailService2;
            _rateExchangeService = rateExchangeService;
            _cityService = cityService;
            _dealService = dealService;
            _dealHotelTourService = dealHotelTourService;
        }
        public ActionResult Index()
        {
            // return Redirect("https://goreise.com/tour");
            SetIndexValue();
            var list = new List<HotelModel>();
            list.Add(new HotelModel { Name = "Ho Chi Minh", ImageUrl = "~/Content/Images/ho-chi-minh.jpg", HotelUrl = "VietNam/50-ho-chi-minh" });
            list.Add(new HotelModel { Name = "Mui Ne", ImageUrl = "~/Content/Images/phan-thiet.jpg", HotelUrl = "VietNam/230-mui-ne" });
            list.Add(new HotelModel { Name = "Da Lat", ImageUrl = "~/Content/Images/da-lat.jpg", HotelUrl = "VietNam/183-Da-Lat" });
            list.Add(new HotelModel { Name = "Nha Trang", ImageUrl = "~/Content/Images/nha-trang.jpg", HotelUrl = "VietNam/186-nha-trang" });
            list.Add(new HotelModel { Name = "Ha Noi", ImageUrl = "~/Content/Images/ha-noi.jpg", HotelUrl = "VietNam/1-ha-noi" });
            list.Add(new HotelModel { Name = "Hoi An", ImageUrl = "~/Content/Images/hoi-an.jpg", HotelUrl = "VietNam/182-hoi-an" });
            list.Add(new HotelModel { Name = "Da Nang", ImageUrl = "~/Content/Images/da-nang.jpg", HotelUrl = "VietNam/32-da-nang" });
            list.Add(new HotelModel { Name = "Phu Quoc", ImageUrl = "~/Content/Images/phu-quoc.jpg", HotelUrl = "VietNam/185-phu-quoc" });
            list.Add(new HotelModel { Name = "Con Dao", ImageUrl = "~/Content/Images/con-dao.jpg", HotelUrl = "VietNam/188-con-dao" });
            list.Add(new HotelModel { Name = "BangKok", ImageUrl = "~/Content/Images/Bangkok.jpg", HotelUrl = "thailand/102-bangkok" });
            list.Add(new HotelModel { Name = "Phuket", ImageUrl = "~/Content/Images/thailand.jpg", HotelUrl = "thailand/133-phuket" });
            list.Add(new HotelModel { Name = "Singapore", ImageUrl = "~/Content/Images/singapore.jpg", HotelUrl = "singapore/214-singapore" });
            list.Add(new HotelModel { Name = "Bali", ImageUrl = "~/Content/Images/Bali.jpg", HotelUrl = "indonesia/209-bali" });
            list.Add(new HotelModel { Name = "SiemRiep", ImageUrl = "~/Content/Images/Siem-Reap.jpg", HotelUrl = "Campuchia/156-Siem-Riep" });
            list.Add(new HotelModel { Name = "Taipei", ImageUrl = "~/Content/Images/Taipei.jpg", HotelUrl = "Taipei/228-Taipei" });
            list.Add(new HotelModel { Name = "KualaLumpur", ImageUrl = "~/Content/Images/Kuala-lumpur.jpg", HotelUrl = "malaysia/212-kuala-lumpur" });

            var listTour = new List<HotelModel>();
            listTour.Add(new HotelModel { Name = "Ho Chi Minh", ImageUrl = "~/Content/Images/ho-chi-minh.jpg", HotelUrl = "searchs/all/key-ho%20chi%20minh/filter-all/price-all/page-1" });
            listTour.Add(new HotelModel { Name = "Mui Ne", ImageUrl = "~/Content/Images/phan-thiet.jpg", HotelUrl = "searchs/all/key-mui ne/filter-all/price-all/page-1" });
            listTour.Add(new HotelModel { Name = "Da Lat", ImageUrl = "~/Content/Images/da-lat.jpg", HotelUrl = "searchs/all/key-da lat/filter-all/price-all/page-1" });
            listTour.Add(new HotelModel { Name = "Nha Trang", ImageUrl = "~/Content/Images/nha-trang.jpg", HotelUrl = "searchs/all/key-nha trang/filter-all/price-all/page-1" });
            listTour.Add(new HotelModel { Name = "Ha Noi", ImageUrl = "~/Content/Images/ha-noi.jpg", HotelUrl = "searchs/all/key-ha noi/filter-all/price-all/page-1" });
            listTour.Add(new HotelModel { Name = "Hoi An", ImageUrl = "~/Content/Images/hoi-an.jpg", HotelUrl = "searchs/all/key-hoi an/filter-all/price-all/page-1" });
            listTour.Add(new HotelModel { Name = "Da Nang", ImageUrl = "~/Content/Images/da-nang.jpg", HotelUrl = "searchs/all/key-da nang/filter-all/price-all/page-1" });
            listTour.Add(new HotelModel { Name = "Phu Quoc", ImageUrl = "~/Content/Images/phu-quoc.jpg", HotelUrl = "searchs/all/key-phu quoc/filter-all/price-all/page-1" });
            listTour.Add(new HotelModel { Name = "Con Dao", ImageUrl = "~/Content/Images/con-dao.jpg", HotelUrl = "searchs/all/key-con dao/filter-all/price-all/page-1" });
            listTour.Add(new HotelModel { Name = "BangKok", ImageUrl = "~/Content/Images/Bangkok.jpg", HotelUrl = "searchs/all/key-bangkok/filter-all/price-all/page-1" });
            listTour.Add(new HotelModel { Name = "Phuket", ImageUrl = "~/Content/Images/thailand.jpg", HotelUrl = "searchs/all/key-thailand/filter-all/price-all/page-1" });
            listTour.Add(new HotelModel { Name = "Singapore", ImageUrl = "~/Content/Images/singapore.jpg", HotelUrl = "searchs/all/key-singapore/filter-all/price-all/page-1" });
            listTour.Add(new HotelModel { Name = "Bali", ImageUrl = "~/Content/Images/Bali.jpg", HotelUrl = "searchs/all/key-bali/filter-all/price-all/page-1" });
            listTour.Add(new HotelModel { Name = "SiemRiep", ImageUrl = "~/Content/Images/Siem-Reap.jpg", HotelUrl = "searchs/all/key-siem reap/filter-all/price-all/page-1" });
            listTour.Add(new HotelModel { Name = "Taipei", ImageUrl = "~/Content/Images/Taipei.jpg", HotelUrl = "searchs/all/key-taipei/filter-all/price-all/page-1" });
            listTour.Add(new HotelModel { Name = "KualaLumpur", ImageUrl = "~/Content/Images/Kuala-lumpur.jpg", HotelUrl = "searchs/all/key-kuala lumpur/filter-all/price-all/page-1" });


            ViewBag.TourCities = list;
            ViewBag.TourListCities = listTour;
            return View();
        }
        [Route("sidebar")]
        public ActionResult SideBarIndex()
        {
            return View();
        }

        
        private void SetIndexValue()
        {
            var countries = _countryService.GetCountryList();
            ViewBag.Countries = countries;

            using (vinadaydbEntities1 entities = new vinadaydbEntities1())
            {
                var tours = entities.viewListTours.OrderBy(a => a.Expr1).ToList();
                var list = tours.Select(tour => new ViewListTourMapping
                {
                    Id = tour.Id,
                    Duration = tour.Duration,
                    Name = tour.Name,
                    PriceFrom = tour.PriceFrom,
                    Location = tour.Location,
                    ThumbnailPath = tour.ThumbnailPath,
                    OriginalPath = tour.OriginalPath,
                    Expr1 = tour.Expr1,
                    Expr2 = tour.Expr2,
                    PageUrl = Utilities.GenerateSlug(tour.Tour_Title, 200),
                    SHORTNAME = tour.SHORTNAME,
                }).ToList();
                ViewBag.Tours = list;
            }
            var hotelSimilars =
                _hotelService.Queryable().Where(a => a.CollectionValue.HasValue && a.CollectionValue > 0).Take(10).ToList();
            var tourSimilars = _tourService.GetTours((int)Utilities.Language.English).Take(4).ToList();
            var tourList = _tourService.Queryable().ToList();
            var tourRates = _tourRateService.Queryable().ToList();
            var medias = _mediaService.Queryable().Where(a => a.MediaType == (int)Utilities.MediaType.Banner).ToList();
            var tourModels = new List<TourModel>();
            foreach (var featured in tourSimilars)
            {
                var tour = tourList.FirstOrDefault(a => a.Id == featured.Id);
                var country = countries.FirstOrDefault(c => c.CountryId == tour.CountryId);
                var seo = new Seo()
                {
                    Title = string.Format("{0} | goreise.com", tour.Name),
                    Description = tour.Name,
                    Slug = Web.Framework.Utilities.GenerateSlug(tour.TourTitle, 200),
                    Keyword = tour.Name,
                };

                var rates = tourRates.Where(a => a.TourId == featured.Id).Take(1).ToList();

                var seoModel = new SeoModel
                {
                    Slug = seo.Slug,
                    Description = seo.Description,
                    Keyword = seo.Keyword,
                    Title = seo.Title,
                    ShortCountryName = country != null ? country.ShortName : string.Empty
                };
                var media = medias.FirstOrDefault(t => t.OwnerId == featured.Id);

                tourModels.Add(new TourModel
                {
                    Rates = rates,
                    Name = tour.Name,
                    Description = tour.Description,
                    Duration = tour.Duration,
                    Id = tour.Id,
                    ImageUrl =
                        (media?.OriginalPath != null)
                            ? $"{Utilities.NoneSslUrl}{Url.Content(media.OriginalPath)}"
                            : string.Format("~/Content/Images/no-image.jpg"),
                    Seo = seoModel,
                    Tour = tour
                });
            }
            var hotelImages = _imagesService.GetHotelImages().ToList();
            var hotelSimilarBindList = new List<HotelModel>();
            foreach (var hotel in hotelSimilars)
            {
                var image = hotelImages.FirstOrDefault(a => a.HotelId == hotel.Id);
                var _hotel = new HotelModel
                {
                    Id = hotel.Id,
                    Name = hotel.Name,
                    Star = hotel.StartRating ?? 1,
                    Description = hotel.Description,
                    CityId = hotel.CityId ?? -1,
                    Phone = hotel.Phone,
                    ImageUrl =
                        image != null
                            ? !string.IsNullOrEmpty(image.ImageThumbnail)
                                ? "https://admin.goreise.com" + $"{image.ImageThumbnail.Substring(1)}-original.jpg"
                                : "/Content/images/demo/general/no-image.jpg"
                            : "/Content/images/demo/general/no-image.jpg",
                    HotelUrl = $"/hotel/{Utilities.GenerateSlug(hotel.Name)}-p{hotel.Id}"
                };
                hotelSimilarBindList.Add(_hotel);
            }
            ViewBag.HotelSimilars = hotelSimilarBindList;
            ViewBag.TourSimilars = tourModels;
        }

        [Route("index/{city}")]
        public ActionResult Index_HoChiMinh(string city)
        {
            var countries = _countryService.GetCountryList();
            ViewBag.Countries = countries;
            ViewBag.City = city;
            SetIndexValue();
            return View("Index");
        }

        public ActionResult UnSubscribe()
        {
            return PartialView();
        }
        public string GetCookie()
        {
            var cookieName = FormsAuthentication.FormsCookieName;
            var authCookie = Request.Cookies[cookieName];
            string result = string.Empty;
            // This could throw an exception if it fails the decryption process. Check MachineKeys for consistency.  
            if (authCookie == null) return string.Empty;
            try
            {
                var authenticationTicket = FormsAuthentication.Decrypt(authCookie.Value);
                // Retrieve information from the ticket  
                result = authenticationTicket != null ? authenticationTicket.Name : string.Empty;
            }
            catch (Exception ex)
            {

            }
            return result;


        }
        [Route("vinaday/api/get-hotels")]
        [HttpGet]
        public JsonResult GetHotels()
        {
            var hotels = _hotelService.GetHotels();
            var resutls = from s in hotels where s.Status == 2 && s.CollectionValue.HasValue && s.CollectionValue.Value > 0
                          select new { name = s.HotelName, address = s.StreetAddressEnglish };
            var jsonResult = Json(resutls, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [Route("vinaday/api/get-cities")]
        [HttpGet]
        public JsonResult GetCities()
        {
            var cities = _hotelService.GetCitiesEn();
            var resutls = from s in cities select new { name = s.Name, description = s.Description, hotelsCount = s.Count };
            var jsonResult = Json(resutls, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        //[Route("search/{search}")]
        //public ActionResult Search()
        //{
        //    return PartialView();
        //}

        [Route("thimut")]
        public ActionResult ThimUt()
        {
            return View();
        }
        [Route("GetInQuire")]
        public ActionResult GetInQuire()
        {
            return PartialView("_RequestHotel");
        }

        //[Route("{country}/{city}")]
        //public ActionResult City(string country,string city)
        //{
        //    ViewBag.User = GetCookie();
        //    string[] citys  = city.Split('-');
        //    int cityId = citys.Length > 0 ? int.Parse(citys[0]) : 0;
        //    ViewBag.Cities = _cityService.GetCities().FirstOrDefault(c => c.CityId == cityId);
        //    ViewBag.Title = "Found 83 hotels in Ho Chi Minh";
        //    return PartialView();

        //}

        //[Route("{country}")]
        //public ActionResult Country(string country,FormCollection frm)
        //{
        //    ViewBag.User = GetCookie();

        //    var item = frm["cities"];
        //    ViewBag.Country = country;
        //    var currentCountry = _countryService.ODataQueryable().FirstOrDefault(a => a.Description.Equals(country,StringComparison.CurrentCultureIgnoreCase) );
        //    if (currentCountry != null)
        //    {
        //        var cities = _hotelService.ODataQueryable().Where(a => a.CollectionValue.HasValue && a.CollectionValue.Value > 0 && a.CountryId == currentCountry.CountryId).GroupBy(a => a.CITY).ToList();
        //        var newList = cities.Select(city => new City {Name = city.Key, TourTrend = city.Count(), CityId = city.FirstOrDefault() != null ? city.FirstOrDefault().CityId.Value : 0}).ToList();
        //        ViewBag.Cities = newList;
        //    }
           
        //    ViewBag.Title = "Found 83 hotels in "+ country;
        //    return PartialView();

        //}
        //[Route("{country}")]
        //[HttpPost]
        //public ActionResult Country(FormCollection frm)
        //{
        //    ViewBag.User = GetCookie();
        //    string country = frm["Country"];
        //    var item = frm["cities"];
        //    var currentCountry = _countryService.ODataQueryable().FirstOrDefault(a => a.Description.Equals(country, StringComparison.CurrentCultureIgnoreCase));
        //    if (currentCountry != null)
        //    {
        //        var cities = _cityService.ODataQueryable().Where(a => a.CountryId == currentCountry.CountryId).ToList();
        //        var citiesCache = _cityService.GetCities();
        //        ViewBag.Cities = cities.Select(city => citiesCache.FirstOrDefault(c => c.CityId == city.CityId)).ToList();
        //    }

        //    ViewBag.Title = "Found 83 hotels in " + country;
        //    return PartialView();

        //}

        [Route("send-inquiry-successfully")]
        public ActionResult SendInquireSuccess()
        {
            return View();
        }
        //best-luxury-escapes-weekend-getaways 
        [Route("best-luxury-escapes-weekend-getaways")]
        public ActionResult HotelPackageDeal()
        {
            var items = _dealHotelTourService.ODataQueryable().Where(a => a.DealType == 1).ToList();
            foreach (var item in items)
            {
                if (!string.IsNullOrEmpty(item.DealAvarta))
                {
                    item.DealAvarta = item.DealAvarta.Replace("\\", "//").Replace("~", "https://admin.goreise.com");
                }
                if (!string.IsNullOrEmpty(item.DealBanner))
                {
                    item.DealBanner = item.DealBanner.Replace("\\", "//").Replace("~", "https://admin.goreise.com");
                }
            }
            int half = items.Count > 1 ? items.Count / 2 : items.Count;
            var firstItemList = half > 0 ? items.Take(half) : items;
            ViewBag.Items1 = firstItemList.ToList();
            ViewBag.Items2 = half > 0 ? items.Skip(half).ToList() : new List<DealHotelTours>();
            return View();
        }

        [Route("vinaday/api/search")]
        [HttpPost]
        public ActionResult SearchHotels(ObjectModel objectModel)
        {
            var search = objectModel.StrParam1;

            //var checkIn = Utilities.ConvertToDateTime(objectModel.StrParam2);
            //var checkOut = Utilities.ConvertToDateTime(objectModel.StrParam3);

            //Session[Constant.SessionHotelCheckin] = checkIn;
            //Session[Constant.SessionHotelCheckout] = checkOut;
            Session[Constant.SessionHotelKeyword] = search;

            if (string.IsNullOrEmpty(search)) return Json('/', JsonRequestBehavior.AllowGet);
            //For hotel.
            //            var hotel = _hotelService.GetHotelSingleByHotelName(search);
            var hotel = _hotelService.GetHotels().FirstOrDefault(a => a.HotelName != null && a.HotelName == search && a.Status == (int)Utilities.Status.Active);
            if (hotel != null)
            {
                var hotelUrl = $"/hotel/{Utilities.GenerateSlug(hotel.Name)}-p{hotel.Id}";
                return Json(hotelUrl, JsonRequestBehavior.AllowGet);
            }
            //For City
            var city = _hotelService.GetCityByName(search);
            if (city != null)
            {
                var country = _countryService.GetCountry(city.CountryId);

                var cityUrl = $"/{country.Description}/{city.CityId}-{Utilities.GenerateSlug(city.Description.ToLower())}";
                return Json(cityUrl, JsonRequestBehavior.AllowGet);
            }
            //For Key search
            var searchUrl = $"/search/{Utilities.GenerateSlug(search)}";
            return Json(searchUrl, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Route("getcity")]
        public ActionResult GetCities(ObjectModel objectModel)
        {
            var cities =  _cityService.GetCities().Where(c => c.CountryId == objectModel.Id).ToList();
            cities.Insert(0, new City() { Description = "All" });
            ViewBag.Cities = cities;
            return PartialView("_CityPartial");
        }
        [Route("coupon-send-inquiry")]
        [HttpPost]
        public ActionResult vinadaySendInquire(FormCollection form)
        {
            string amount = form["amount"];
            string productinfo = form["productinfo"];
            string firstname = form["firstname"];
            string email = form["email"];
            string surl = form["surl"];
            string phone = form["phone"];
            ViewBag.Amount = amount;
            ViewBag.productinfo = productinfo;
            ViewBag.phone = phone;
            ViewBag.surl = surl;

            string inquiry =
                $@" <table width='600' cellspacing='0' cellpadding='0' border='0' align='left' style='border-collapse:collapse;'>
	                <tbody>
		                <tr>
			                <td valign='top' style='padding-top:9px;padding-right:18px;padding-bottom:9px;padding-left:18px;color:#606060;font-family:Helvetica;font-size:15px;line-height:150%;text-align:left;'>
				                <h3 style='margin: 0px;padding:0;display:block;font-family:Helvetica;font-size:18px;font-style:normal;font-weight:bold;line-height:125%;letter-spacing:-.5px;text-align:left;color:#606060 !important;'>
					                Hi System Team!
				                </h3>
				                <p>Amount: {
                                    amount}</p>
				                <p>Product: {productinfo}</p>
				                <p>Customer {firstname}</p>
				                <p>Email: {
                                    email}</p>
				                <p>Phone: {phone}</p>
				                <p>Date: {DateTime.Now
                                    }</p>
			                </td>
		                </tr>
	                </tbody>
                </table>";
            //MailClient.Inquiry(inquiry, "Inquiry Coupon");
            return View();
        }

        public ActionResult TestSession()
        {
            Session["TUONGSESSION"] = "ABCDEF";
            return Redirect("/Home/SessionTuong");
        }
        public ActionResult SessionTuong()
        {
            return View();
        }
        public ActionResult TestMail()
        {
            MailClient.SendWelcome("Welcome","Welcome Tuong");
            return View();
        }

        [Route("coupon-send-inquiry-submit")]
        [HttpPost]
        public ActionResult vinadaySendInquireSubmit(ObjectModel objectModel)
        {
            //objectModel.StrParam3 = firstName;
            //objectModel.StrParam4 = lastName;
            //objectModel.StrParam5 = email;
            //objectModel.StrParam6 = phone;
            string name = objectModel.StrParam1;
            string phone = objectModel.StrParam6;
            string productinfo = objectModel.StrParam2;
            var firstName = objectModel.StrParam3;
            var amount = objectModel.DecParam1;
            var email = objectModel.StrParam5;//Pick email value
            var productId = 0;//Pick product id value (hotel or tour)
            var checkIn = objectModel.StrParam1;//Pick check in or depart date
            var checkOut = objectModel.StrParam2;//Pick check out date
            var total = objectModel.IntParam3;//Pick total product fr booking
            var status =3;//Pick status (paid or Hoding)
            var specialRequest = objectModel.Message;//rendering for special request
            const int promotionId = 0;//Create promotion code
            var pnr = Utilities.GetRandomString(7).ToUpper();//Random PNR order
                                                             //Get customer by email address.
            #region create update customer
            var customer = new Customer();
            var existCustomer = _customerService.GetCustomerByEmail(email);
            if (existCustomer == null)
            {
                customer.Firstname = objectModel.StrParam3;
                customer.Lastname = objectModel.StrParam4;
                customer.PhoneNumber = objectModel.StrParam6;
                customer.Email = objectModel.StrParam5;
                customer.NationalId = 4;
                customer.ObjectState = ObjectState.Added;
                _customerService.Insert(customer);
                try
                {
                    _unitOfWorkAsync.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Json(new { success = false, error = "Error when processing booking" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                customer = existCustomer;
            }

            #endregion


            var rateExchange = _rateExchangeService.GetRateExchangeById(3);
            var userId = "N/A";

            if (rateExchange != null && rateExchange.CurrentPrice.HasValue && rateExchange.CurrentPrice.Value> 0)
            {
                amount = amount / rateExchange.CurrentPrice.Value;
            }
            #region Create Order 
            var order = new Order
            {
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Status = status,
                Quantity = 1,
                CustomerId = customer.CustomerId,
                Pnr = pnr,
                TaxFee = 0,
                Price = amount,
                Management = userId,
                ProductId = 0,
                Amount = amount,
                Type = (int)Utilities.Product.OtherServices,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                Night = 1,
                SurchargeFee = 0,
                IpLocation = Utilities.GetUserIp(Request),
                SpecialRequest = "",
                Discount = 0,
                CancellationPolicy = "",//product.CancellationPolicy != null ? product.CancellationPolicy.Description : string.Empty,
                ProductName = productinfo,
                DiscountName = "",
                ObjectState = ObjectState.Added,
                Note = "Booking from vinaday.vn other type product",
                RateExchange = rateExchange != null ? rateExchange.CurrentPrice : 0
            };
            //Insert hotel order
            _orderService.Add(order);
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Json(new { success = false, error = "Error when processing booking" }, JsonRequestBehavior.AllowGet);
            }
            #endregion

            #region create order 2
            var order2 = new Order2
            {

                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Status = status,
                Quantity = 1,
                CustomerId = customer.CustomerId,
                Pnr = pnr,
                TaxFee = 0,
                Price = amount,
                Management = userId,
                ProductId = 0,
                Amount = amount,
                Type = (int)Utilities.Product.OtherServices,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                Night = 1,
                SurchargeFee = 0,
                IpLocation = Utilities.GetUserIp(Request),
                SpecialRequest = "",
                Discount = 0,
                CancellationPolicy = "",//product.CancellationPolicy != null ? product.CancellationPolicy.Description : string.Empty,
                ProductName = productinfo,
                DiscountName = "",
                ObjectState = ObjectState.Added,
                Note = "Booking from vinaday.vn other type product",
                TourOrderId = order.Id

            };
            //Insert hotel order
            _orderService2.Add(order2);
            #endregion

            #region Send Email
            string inquiry =
           $@" <table width='600' cellspacing='0' cellpadding='0' border='0' align='left' style='border-collapse:collapse;'>
	                <tbody>
		                <tr>
			                <td valign='top' style='padding-top:9px;padding-right:18px;padding-bottom:9px;padding-left:18px;color:#606060;font-family:Helvetica;font-size:15px;line-height:150%;text-align:left;'>
				                <h3 style='margin: 0px;padding:0;display:block;font-family:Helvetica;font-size:18px;font-style:normal;font-weight:bold;line-height:125%;letter-spacing:-.5px;text-align:left;color:#606060 !important;'>
					                Hi System Team!
				                </h3>
				                <p>Amount: {
                               amount}</p>
				                <p> {productinfo}</p>
				                <p>{name}</p>
				              
				                <p>Phone: {phone}</p>
				                <p>Date: {DateTime.Now
                               }</p>
			                </td>
		                </tr>
	                </tbody>
                </table>";
            MailClient.Inquiry(inquiry, "Inquiry Booking Tour");
            #endregion

            objectModel.Status = (int)Utilities.Status.Active;
            objectModel.Message = "Your Inquiry has been sent";

            return Json(objectModel);
        }

        //[Route("travel-services")]
        //public ActionResult TravelService()
        //{
        //    return View();
        //}

        //[Route("visa-services")]
        //public ActionResult VisaService()
        //{
        //    return View();
        //}

        //[Route("invest-in-vietnam")]
        //public ActionResult InvestInVietnam()
        //{
        //    return View();
        //}

        //[Route("working-in-vietnam")]
        //public ActionResult WorkingInVietnam()
        //{
        //    return View();
        //}

        [Route("careers")]
        public ActionResult Careers()
        {
            return View();
        }
        [Route("group-tour")]
        public ActionResult GroupTour()
        {
            return Redirect("https://goreise.com");
        }
        [Route("vietnam-vacation-travel-guide")]
        public ActionResult VietnamGuide()
        {
            return Redirect("http://www.goreise.com");
        }
        [Route("help-faqs")]
        public ActionResult Help()
        {
            return Redirect("https://goreise.com");
            //return View();
        }

        //[Route("payment-information")]
        //public ActionResult Payment()
        //{
        //    return View();
        //}
        [Route("affiliate-partners")]
        public ActionResult Affiliate()
        {
            return View();
        }
        [Route("partners")]
        public ActionResult Partners()
        {
            return View();
        }
        [Route("websites-rules")]
        public ActionResult TermsOfUse()
        {
            return View();
        }
        [Route("privacy-policy")]
        public ActionResult PrivacyPolicy()
        {
            return View();
        }
        [Route("rewards")]
        public ActionResult Rewards()
        {
            return View();
        }
        [Route("about-us")]
        public ActionResult AboutVersion2()//About()
        {
            return View();
        }
        [Route("test")]
        public ActionResult Test()
        {
            return View();
        }
        [Route("mekong-delta-tours")]
        public ActionResult MekongDelta()
        {
            return Redirect("http://www.goreise.com");
      //      return View();
        }
        [Route("hot-deals")]
        public ActionResult Deal()
        {
            return Redirect("http://www.goreise.com");
            //var deal = _dealService.GetDeals();
            //return View(deal);
        }
        [Route("gioi-thieu")]
        public ActionResult GioiThieu()
        {
            return View();
        }
        [Route("chinh-sach-chung")]
        public ActionResult ChinhSachQuyDinh()
        {
            return View();
        }

        [Route("hinh-thuc-thanh-toan")]
        public ActionResult HinhThucThanhToan()
        {
            return View();
        }

        [Route("chu-so-huu-website")]
        public ActionResult ChuSoHuuWebSite()
        {
            return View();
        }

        [Route("chinh-sach-van-chuyen")]
        public ActionResult ChinhSachVanChuyen()
        {
            return View();
        }

        [Route("chinh-sach-bao-hanh")]
        public ActionResult ChinhSachBaoHanh()
        {
            return View();
        }

        [Route("chinh-sach-doi-tra")]
        public ActionResult ChinhSachDoiTra()
        {
            return View();
        }

        [Route("chinh-sach-bao-mat")]
        public ActionResult ChinhSachBaoMat()
        {
            return View();
        }

        [Route("refund-policy")]
        public ActionResult RefundPolicy()
        {
            return View();
        }


    }
}
