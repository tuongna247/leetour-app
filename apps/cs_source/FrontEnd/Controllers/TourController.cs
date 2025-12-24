using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PagedList;
using Repository.Pattern.UnitOfWork;
using Vinaday.Data.Models;
using Vinaday.Data.Models.Extention;
using Vinaday.Services;
using Vinaday.Web.Framework;
using Vinaday.Web.Framework.EmailHelpers;
using Vinaday.Web.Framework.Extension;
using Vinaday.Web.Home.Models;
using Utilities = Vinaday.Web.Framework.Utilities;

namespace Vinaday.Web.Home.Controllers
{
    public class TourController : Controller
    {
        private readonly IHotelService _hotelService;
        private readonly ITourService _tourService;
        private readonly ITourDetailService _tourDetailService;
        private readonly ICategoryDetailService _categoryDetailService;
        private readonly ICategoryService _categoryService;
        private readonly IMediaService _mediaService;
        private readonly ITourRateService _tourRateService;
        private readonly ITourRateService2 _tourRateService2;
        private readonly ITourRateService3 _tourRateService3;
        private readonly ITourSurchargeService _tourSurchargeService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly ITourPromotionService _tourPromotionService;
        private readonly IOrderService _orderService;
        private readonly ISeoService _seoService;
        private readonly ICountryService _countryService;
        private readonly IFeaturedService _featuredService;
        private readonly ICityService _cityService;
        private readonly ISpecialRateService _specialRateService;
        private readonly DealHotelToursService _dealHotelTourService;
        private readonly INationalityService _nationalityService;
        private readonly TourRateOptionsService _tourrateOptionsService;
        private readonly IRateExchangeService _rateExchangeService;
        private readonly TourReviewService _tourReviewService;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public TourController(IHotelService hotelService, ITourService tourService, ITourDetailService tourDetailService,
            ICategoryService categoryService, ICategoryDetailService categoryDetailService,
            IUnitOfWorkAsync unitOfWorkAsync,
            IMediaService mediaService, ITourRateService tourRateService, ISeoService seoService,
                   INationalityService nationalityService,
            DealHotelToursService dealHotelTourService,
			 ITourRateService2 tourRateService2, 
            ITourRateService3 tourRateService3, 
            ICountryService countryService, IFeaturedService featuredService, ICityService cityService, ISpecialRateService specialRateService, ITourSurchargeService tourSurchargeService, ITourPromotionService tourPromotionService, IOrderService orderService, TourRateOptionsService tourrateOptionsService, TourReviewService tourReviewService, IRateExchangeService rateExchangeService)
        {
            _hotelService = hotelService;
            _tourService = tourService;
            _tourDetailService = tourDetailService;
            _orderService = orderService;
            _categoryService = categoryService;
            _categoryDetailService = categoryDetailService;
            _mediaService = mediaService;
            _tourRateService = tourRateService;
            _tourReviewService = tourReviewService;
            _unitOfWorkAsync = unitOfWorkAsync;
            _tourRateService2 = tourRateService2;
            _tourRateService3 = tourRateService3;
            _seoService = seoService;
            _countryService = countryService;
            _featuredService = featuredService;
            _cityService = cityService;
            _specialRateService = specialRateService;
            _tourPromotionService = tourPromotionService;
            _nationalityService = nationalityService;
            _dealHotelTourService = dealHotelTourService;
            _tourSurchargeService = tourSurchargeService;
            _tourrateOptionsService = tourrateOptionsService;
            _rateExchangeService = rateExchangeService; 
        }
        string deniedIp = "46.229.168;66.249.79;66.249.66;66.249.75;157.55.174";

        public bool IsDenyIp(string ip)
        {

            if (!string.IsNullOrEmpty(ip))
            {
                ip = string.Join(".", ip.Split('.').Take(3).ToArray());
                if (deniedIp.Contains(ip))
                    return true;
            }
            return false;
        }
        [Route("tour")]
        public ActionResult Index()
        {
            var referer = Request.UrlReferrer?.AbsolutePath ?? "";
            var ip = Request.UserHostAddress;
            var currentUrl = Request.Url?.AbsolutePath ?? "";
            if (IsDenyIp(ip))
            {
                //log.Info($"deny from ip {ip} ");
                return null;
            }
            log.Info($"User request {currentUrl} from ip {ip} come from {referer}");
            var countries = _countryService.GetCountryList().Take(5).ToList();
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

            return View();
        }
        [Route("test-font")]
        public ActionResult TestFont()
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
                    Expr1 = tour.Expr1,
                    Expr2 = tour.Expr2,
                    PageUrl = Utilities.GenerateSlug(tour.Tour_Title, 200),
                    SHORTNAME = tour.SHORTNAME,
                }).ToList();
                ViewBag.Tours = list;

            }

            return View();
        }

        [Route("tour-test")]
        public ActionResult IndexTest()
        {
            return Redirect("https://goreise.com");
           // var countries = _countryService.GetCountryList();
           // ViewBag.Countries = countries;

           // using (vinadaydbEntities1 entities = new vinadaydbEntities1())
           // {
           //     var tours = entities.viewListTours.OrderBy(a => a.Expr1).ToList();
           //     var list = tours.Select(tour => new ViewListTourMapping
           //     {
           //         Id = tour.Id,
           //         Duration = tour.Duration,
           //         Name = tour.Name,
           //         PriceFrom = tour.PriceFrom,
           //         Location = tour.Location,
           //         ThumbnailPath = tour.ThumbnailPath,
           //         Expr1 = tour.Expr1,
           //         Expr2 = tour.Expr2,
           //         PageUrl = Utilities.GenerateSlug(tour.Tour_Title, 200),
           //         SHORTNAME = tour.SHORTNAME,
           //     }).ToList();
           //     ViewBag.Tours = list;

           // }
           // var hotelSimilars = _hotelService.GetVietnamHotelFeatured().Take(4).ToList();
           // var tourSimilars = _tourService.GetTours((int)Utilities.Language.English).Take(4).ToList();
           // var tourList = _tourService.Queryable().ToList();
           // var tourRates = _tourRateService.Queryable().ToList();
           // var medias = _mediaService.Queryable().Where(a => a.MediaType == (int)Utilities.MediaType.Banner).ToList();
           //var tourModels = new List<TourModel>();
           // foreach (var featured in tourSimilars)
           // {
           //     var tour = tourList.FirstOrDefault(a => a.Id == featured.Id);
           //     var country = countries.FirstOrDefault(c => c.CountryId == tour.CountryId);
           //     var seo = new Seo()
           //     {
           //         Title = string.Format("{0} | goreise.com", tour.Name),
           //         Description = tour.Name,
           //         Slug = Web.Framework.Utilities.GenerateSlug(tour.TourTitle, 200),
           //         Keyword = tour.Name,
           //     };

           //     var rates = tourRates.Where(a => a.TourId == featured.Id).Take(1).ToList();

           //     var seoModel = new SeoModel
           //     {
           //         Slug = seo.Slug,
           //         Description = seo.Description,
           //         Keyword = seo.Keyword,
           //         Title = seo.Title,
           //         ShortCountryName = country != null ? country.ShortName : string.Empty
           //     };
           //     var media = medias.FirstOrDefault(t => t.OwnerId == featured.Id);
           //     tourModels.Add(new TourModel
           //     {
           //         Rates = rates,
           //         Name = tour.Name,
           //         Description = tour.Description,
           //         Duration = tour.Duration,
           //         Id = tour.Id,
           //         ImageUrl = (media?.OriginalPath != null) ? $"{Utilities.NoneSslUrl}{Url.Content(media.OriginalPath)}" : string.Format("~/Content/Images/no-image.jpg"),
           //         Seo = seoModel,
           //         Tour = tour
           //     });
           // }
           // ViewBag.HotelSimilars = hotelSimilars;
           // ViewBag.TourSimilars = tourModels;
           // return View();
        }

        [Route("tour-test/{city}")]
        public ActionResult IndexTest_HoChiMinh(string city)
        {
            var countries = _countryService.GetCountryList();
            ViewBag.Countries = countries;
            ViewBag.City = city;

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
                    Expr1 = tour.Expr1,
                    Expr2 = tour.Expr2,
                    PageUrl = Utilities.GenerateSlug(tour.Tour_Title, 200),
                    SHORTNAME = tour.SHORTNAME,
                }).ToList();
                ViewBag.Tours = list;

            }

            return View("IndexTest");
        }

        [Route("about-vinaday")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            string referer = Request.UrlReferrer?.AbsolutePath ?? "";
            string ip = Request.UserHostAddress;
            string currentUrl = Request.Url?.AbsolutePath ?? "";
            log.Info($"User request {currentUrl} from ip {ip} come from {referer}");
            return View();
        }

        [Route("{country}/{style}/{location}/{name}/")]
        //  [Route("{country}/key-{key}/city-{city}/filter-{filter}/page-{page}/")]
        public ActionResult needRedirect(string country, string style, string location, string name)
        {
            return Redirect("http://goreise.com/tour/cu-chi-tunnels-half-day-small-group-tour-max-10-persons-p115?country=vn/");
        }


        //[Route("TourIndex")]
        [HttpGet]
        public ActionResult TourIndex()
        {
            return View();
        }

        [Route("faqs")]
        public ActionResult faqs()
        {
            return View();
        }

        ////terms-and-conditions
        //[Route("terms-and-conditions")]
        //public ActionResult TermsAndConditions()
        //{
        //    return View();
        //}
        //best-luxury-escapes-weekend-getaways 
        [Route("best-holiday-tour-package")]
        public ActionResult TourPackageDeal()
        {
            string referer = Request.UrlReferrer?.AbsolutePath ?? "";
            string ip = Request.UserHostAddress;
            string currentUrl = Request.Url?.AbsolutePath ?? "";
            log.Info($"User request {currentUrl} from ip {ip} come from {referer}");
            var items = _dealHotelTourService.ODataQueryable().Where(a => a.DealType == 2).ToList();
            foreach (var item in items)
            {
                item.LinkDetail = item.LinkDetail.Replace("vinadaytravel.com", "goreise.com");
                if (!string.IsNullOrEmpty(item.DealAvarta))
                {
                    item.DealAvarta = item.DealAvarta.Replace("\\", "//").Replace("~", "https://goreise.com/admin");
                }
                if (!string.IsNullOrEmpty(item.DealBanner))
                {
                    item.DealBanner = item.DealBanner.Replace("\\", "//").Replace("~", "https://goreise.com/admin");
                }

            }
            int half = items.Count > 1 ? items.Count / 2 : 0;
            var firstItemList = half > 0 ? items.Take(half) : items;
            ViewBag.Items1 = firstItemList.ToList();
            ViewBag.Items2 = half > 0 ? items.Skip(half).ToList() : new List<DealHotelTours>();
            return View();
        }

        [Route("small-group-tour-departuring")]
        [HttpGet]
        [HttpPost]
        public ActionResult GroupTourDeparture(FormCollection frm)
        {
            string referer = Request.UrlReferrer?.AbsolutePath ?? "";
            string ip = Request.UserHostAddress;
            string currentUrl = Request.Url?.AbsolutePath ?? "";
            log.Info($"User request {currentUrl} from ip {ip} come from {referer}");
            var countries = _countryService.GetCountryList().ToList();
            ViewBag.Countries = countries;
            var tourModels = new List<ViewListTourMapping>();
            using (vinadaydbEntities1 entities = new vinadaydbEntities1())
            {
                //var tours = entities.viewListTours.ToList();
                var tours = entities.viewListTours.OrderBy(a => a.Expr1).ToList();
                tourModels = tours.Select(tour => new ViewListTourMapping
                {
                    Id = tour.Id,
                    Duration = tour.Duration,
                    Name = tour.Name,
                    PriceFrom = tour.PriceFrom,
                    Location = tour.Location,
                    ThumbnailPath = tour.ThumbnailPath,
                    Expr1 = tour.Expr1,
                    Expr2 = tour.Expr2,
                    PageUrl = Utilities.GenerateSlug(tour.Tour_Title, 200),
                    SHORTNAME = tour.SHORTNAME
                }).ToList();
            }

            DateTime now = DateTime.Now.Date;
            DateTime fromdate = now;
            DateTime toDate = now;
            if (!string.IsNullOrEmpty(frm["fromdate"]))
            {
                fromdate = DateTime.Parse(frm["fromdate"]);
                toDate = DateTime.Parse(frm["todate"]);
            }
            var orders = _orderService.GetTourOrders().Where(a => a.StartDate >= fromdate && a.StartDate <= toDate).ToList();
            ViewBag.FromDate = fromdate.ToString("MM/dd/yyyy");
            ViewBag.ToDate = toDate.ToString("MM/dd/yyyy");
            var bindBookingInDayses = new List<TourBookingInDays>();

            while (fromdate <= toDate)
            {
                var item = new TourBookingInDays
                {
                    DepartDate = fromdate,
                    TourBookingInDayList = new List<TourBookingInDay> { }
                };
                foreach (var tour in tourModels)
                {
                    TourBookingInDay tourInDay = new TourBookingInDay
                    {
                        Durration = tour.Duration,
                        SerViceName = tour.Name,
                        TourId = tour.Id.Value,
                        Status = "Available",
                        Slug = tour.PageUrl,
                        Country = tour.SHORTNAME
                    };
                    var bookonDay = orders.Where(a => a.StartDate.Date == item.DepartDate && a.ProductId == tour.Id);
                    if (bookonDay.Any())
                    {
                        var bookOndayCount = bookonDay.Sum(a => a.Quantity ?? 0);
                        if (bookOndayCount > 0)
                        {
                            bookOndayCount = bookOndayCount % 12;
                            tourInDay.Status = 12 - bookOndayCount <= 10 ? 12 - bookOndayCount + " place left" : "Available";
                        }
                    }
                    if (tour.PriceFrom != null)
                    {
                        var tourRateFirstItem = (decimal)tour.PriceFrom.Value;
                        tourInDay.Total = tourRateFirstItem.ToString("C0") + " USD";
                    }
                    item.TourBookingInDayList.Add(tourInDay);
                }
                fromdate = fromdate.AddDays(1);
                bindBookingInDayses.Add(item);
            }
            ViewBag.Departures = bindBookingInDayses;
            return View();
        }

        [Route("contact-us")]
        public ActionResult Contact()
        {
            string referer = Request.UrlReferrer?.AbsolutePath ?? "";
            string ip = Request.UserHostAddress;
            string currentUrl = Request.Url?.AbsolutePath ?? "";
            log.Info($"User request {currentUrl} from ip {ip} come from {referer}");
            ViewBag.Message = "Your contact page.";

            return View();
        }
        private List<Detail> GetTourDetailsByTourId(int? id)
        {
            var categoryMeals = _categoryService.GetCategories().FirstOrDefault(c => c.KeyCode == Utilities.Meals);
            var categoryTransports = _categoryService.GetCategories().FirstOrDefault(c => c.KeyCode == Utilities.Transportations);
            List<Detail> tourDetails = _tourDetailService.GetDetailTours().Where(d => d.TourId == id).ToList();

            foreach (var tourDetail in tourDetails)
            {
                var mealStr = string.Empty;
                var transportStr = string.Empty;
                if (categoryMeals != null)
                {
                    var meals = Utilities.GetCategoryList(tourDetail.Meal, _categoryDetailService.GetCategoriesDetail(categoryMeals.Id));
                    mealStr = meals.Where(meal => meal != null && meal.Checked).Aggregate(mealStr, (current, meal) => current + string.Format("{0}, ", meal.Name));
                }
                if (categoryTransports != null)
                {
                    var transports = Utilities.GetCategoryList(tourDetail.Transport, _categoryDetailService.GetCategoriesDetail(categoryTransports.Id));
                    transportStr = transports.Where(transport => transport != null && transport.Checked).Aggregate(transportStr, (current, transport) => current + string.Format("{0}, ", transport.Name));
                }
                tourDetail.Meal = mealStr;
                tourDetail.Transport = transportStr;
            }
            return tourDetails;
        }

        [Route("{country}/key-{key}/city-{city}/filter-{filter}/page-{page}/")]
        public ActionResult Country(string country, string key, string filter, string city, int? page)
        {
            string referer = Request.UrlReferrer?.AbsolutePath ?? "";
            string ip = Request.UserHostAddress;
            string currentUrl = Request.Url?.AbsolutePath ?? "";
            if (IsDenyIp(ip))
            {
                //log.Info($"deny from ip {ip} ");
                return null;
            }
            log.Info($"User request {currentUrl} from ip {ip} come from {referer}");
            ViewBag.User = GetCookie();
            ViewBag.Country = country;
            string strCities = city.Replace("-", " ").Replace("|", ", ");
            ViewBag.City = strCities;
            ViewBag.Key = key;
            var currentCountry = _countryService.ODataQueryable().FirstOrDefault(a => a.Description.Equals(country, StringComparison.CurrentCultureIgnoreCase));
            if (currentCountry != null)
            {
                var tourList = _tourService.ODataQueryable().Where(a => a.CountryId == currentCountry.CountryId).ToList();
                IDictionary<string, int> cityDics = new Dictionary<string, int>();
                var citieList = _cityService.GetCities().ToList();
                foreach (var tour in tourList)
                {
                    var cityKeyList = tour.Cities.Split(',');
                    foreach (var cityKey in cityKeyList)
                    {
                        var cityItem = citieList.FirstOrDefault(a => a.CityId.ToString() == cityKey);
                        if (cityItem != null)
                        {
                            string cityName = cityItem.Description + "-" + cityItem.CityId;
                            if (!cityDics.Keys.Contains(cityName))
                            {
                                cityDics.Add(cityName, 1);
                            }
                            else
                            {
                                cityDics[cityName] = cityDics[cityName] + 1;
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(strCities) && strCities != "all")
                {
                    var cityRequests = city.Trim().Split('|');
                    var _tourListCopy = new List<Tour>();
                    foreach (var cityId in cityRequests)
                    {
                        if (string.IsNullOrEmpty(cityId))
                            continue;
                        var temp = tourList.Where(a => a.Cities.Contains(cityId));
                        _tourListCopy.AddRange(temp);
                    }
                    tourList = _tourListCopy.GroupBy(a => a.Id).Select(grp => grp.First()).ToList();
                }
                if (key != "none")
                {
                    tourList = tourList.Where(t => t.SearchKey.ToLower().Contains(key.ToLower().Replace("-", " "))).ToList();

                }
                ViewBag.Cities = cityDics;
                ViewBag.TotalItem = tourList.ToList();

                var tourModels = new List<TourModel>();
                #region city is not null
                var tourTemps = tourList;
                //Get and filter Travel Style
                var catList = _categoryService.GetCategories().ToList();
                var category = catList.FirstOrDefault(c => c.KeyCode == Utilities.TravelStyle);
                if (category == null) return View();
                var catdetails = _categoryDetailService.GetCategoriesDetail(category.Id);
                ViewBag.TravelStyles2 = Utilities.GetCategoryList(string.Empty, catdetails);
                var filterArray = filter.Split('|');
                var travelStyles = GetItemModel(tourTemps, (int)Utilities.SearchFilter.TravelStyle, key);
                var travelStylesItems = new List<ItemModel>();
                foreach (var travelStyle in travelStyles)
                {
                    int id;
                    int.TryParse(travelStyle.Name, out id);
                    var categoryDetail = _categoryDetailService.GetCategoryDetail(id);
                    if (categoryDetail != null)
                    {
                        var categoryName = categoryDetail.Name;
                        travelStyle.Name = categoryName;
                        travelStyle.Slug = Utilities.GenerateSlug(categoryDetail.Name);
                        travelStyle.Checked = Array.FindAll(filterArray, val => val.Equals(Utilities.GenerateSlug(categoryName))).Any();
                    }
                    travelStylesItems.Add(travelStyle);
                }


                if (filterArray.Any() && filter != "none")
                {
                    var tourListUpdate = new List<Tour>();
                    foreach (var f in filterArray)
                    {
                        var findOut = tourList.Where(a => a.Filter != null && a.Filter.Contains(f.ToLower()));
                        foreach (var tour in findOut.Where(tour => tourListUpdate.FirstOrDefault(a => a.Id == tour.Id) == null))
                        {
                            tourListUpdate.Add(tour);
                        }
                    }
                    tourList = tourListUpdate;
                }

                ViewBag.TravelStyles = travelStylesItems;
                var tourLists = _tourService.Queryable().ToList();
                var mediaList = _mediaService.Queryable().ToList();
                var tourRates = _tourRateService.Queryable().ToList();
                tourModels = new List<TourModel>();
                foreach (var tour in tourList.ToList())
                {
                    var tourDetail = tourLists.FirstOrDefault(a => a.Id == tour.Id);
                    var rates = tourRates.Where(a => a.TourId == tour.Id).ToList();
                    var seo = new Seo()
                    {
                        Title = string.Format("{0} | goreise.com", tour.Name),
                        Description = tour.Name,
                        Slug = Web.Framework.Utilities.GenerateSlug(tour.TourTitle, 200),
                        Keyword = tour.Name,
                    };
                    var seoModel = new SeoModel
                    {
                        Slug = seo.Slug,
                        Description = seo.Description,
                        Keyword = seo.Keyword,
                        Title = tourDetail.TourTitle,
                        ShortCountryName = string.Empty
                    };
                    var media = mediaList.FirstOrDefault(t => t.OwnerId == tour.Id && t.MediaType == (int)Utilities.MediaType.Banner);
                    tourModels.Add(new TourModel
                    {
                        Rates = rates,
                        Name = tour.Name,
                        Description = tour.Description,
                        Duration = tour.Duration,
                        Id = tour.Id,
                        Seo = seoModel,
                        ImageUrl = (media != null && media.ThumbnailPath != null) ? $"{Utilities.NoneSslUrl}{Url.Content(media.ThumbnailPath)}" : "~/Content/Images/no-image.jpg",
                        Tour = tourDetail
                    });
                }

                #endregion
                const int pageSize = 10;
                var pageNumber = (page ?? 1);
                return View(tourModels.ToPagedList(pageNumber, pageSize));
            }


            //ViewBag.Title = "Found 83 hotels in " + country;
            return PartialView();

        }
        [Route("blog/{category}/{name}/")]
        public ActionResult Test(string name, string category)
        {
            string url = Request.Url.AbsolutePath;
            return Redirect(url);
        }

        [HttpPost]
        [HttpGet]
        [Route("tour/GetTourPriceOptionDetail")]
        public ActionResult GetTourPriceOptionDetail(int id, DateTime date, int adult, int children)
        {
            var tourRates = _tourRateService.Queryable().Where(t => t.TourId == id && t.PersonNo == adult).ToList();
            var tourRates2 = _tourRateService2.Queryable().Where(t => t.TourId == id && t.PersonNo == adult).ToList();
            var tourRates3 = _tourRateService3.Queryable().Where(t => t.TourId == id && t.PersonNo == adult).ToList();
            //var exChange = _exchangeService.GetRateExchangeById(3);
            var tour = _tourService.GetTourById(id);
            var surcharges = _tourSurchargeService.GetSurchargesByDayTourId(date, id);
            var promotions = _tourPromotionService.GetPromotionByDayTourId(date, id);
            decimal rateVND = 1;
            var rateExchange = _rateExchangeService.GetRateExchanges().FirstOrDefault(a=>a.Name=="VND");
            if (rateExchange != null && rateExchange.CurrentPrice.HasValue)
            {
                rateVND = rateExchange.CurrentPrice.Value;
            }
            if (tour.ParentId > 0)
            {
                tourRates = _tourRateService.Queryable().Where(t => t.TourId == tour.ParentId && t.PersonNo == adult).ToList();
                tourRates2 = _tourRateService2.Queryable().Where(t => t.TourId == tour.ParentId && t.PersonNo == adult).ToList();
                tourRates3 = _tourRateService3.Queryable().Where(t => t.TourId == tour.ParentId && t.PersonNo == adult).ToList();
                surcharges = _tourSurchargeService.GetSurchargesByDayTourId(date, tour.ParentId.Value);
                promotions = _tourPromotionService.GetPromotionByDayTourId(date, tour.ParentId.Value);
            }

            foreach (var promotion in promotions)
            {
                //languages is english then continue to other promotion
                if (promotion.Language == 1)
                    continue;
                if (promotion != null)
                {
                    if (promotion.NumberPerson > 0)
                    {
                        foreach (var rate in tourRates)
                        {
                            if (rate.PersonNo == promotion.NumberPerson)
                            {
                                // Discount type = 1 then discount percent, else discount by price
                                if (promotion.DiscountType == 1)
                                {
                                    decimal percentage = promotion.Get.HasValue ? (decimal)promotion.Get.Value : 0;
                                    rate.RetailRate = (rate.RetailRate - ((percentage * rate.RetailRate) / 100));
                                }
                                else
                                {
                                    decimal discountValue = promotion.Get.HasValue ? (decimal)promotion.Get.Value : 0;
                                    rate.RetailRate = rate.RetailRate - discountValue > 0 ? rate.RetailRate - discountValue : rate.RetailRate;
                                }
                                rate.RetailRate *= rateVND;
                            }
                        }
                        foreach (var rate in tourRates3)
                        {
                            if (rate.PersonNo == promotion.NumberPerson)
                            {
                                // Discount type = 1 then discount percent, else discount by price
                                if (promotion.DiscountType == 1)
                                {
                                    decimal percentage = promotion.Get.HasValue ? (decimal)promotion.Get.Value : 0;
                                    rate.RetailRate = (rate.RetailRate - ((percentage * rate.RetailRate) / 100));
                                }
                                else
                                {
                                    decimal discountValue = promotion.Get.HasValue ? (decimal)promotion.Get.Value : 0;
                                    rate.RetailRate = rate.RetailRate - discountValue > 0 ? rate.RetailRate - discountValue : rate.RetailRate;
                                }
                                rate.RetailRate *= rateVND;
                            }
                        }
                        foreach (var rate in tourRates2)
                        {
                            if (rate.PersonNo == promotion.NumberPerson)
                            {
                                // Discount type = 1 then discount percent, else discount by price
                                if (promotion.DiscountType == 1)
                                {
                                    decimal percentage = promotion.Get.HasValue ? (decimal)promotion.Get.Value : 0;
                                    rate.RetailRate = (rate.RetailRate - ((percentage * rate.RetailRate) / 100));
                                }
                                else
                                {
                                    decimal discountValue = promotion.Get.HasValue ? (decimal)promotion.Get.Value : 0;
                                    rate.RetailRate = rate.RetailRate - discountValue > 0 ? rate.RetailRate - discountValue : rate.RetailRate;
                                }
                                rate.RetailRate *= rateVND;
                            }
                        }
                    }
                    else
                    {
                        foreach (var rate in tourRates)
                        {
                            // Discount type = 1 then discount percent, else discount by price
                            if (promotion.DiscountType == 1)
                            {
                                decimal percentage = promotion.Get.HasValue ? (decimal)promotion.Get.Value : 0;
                                rate.RetailRate = (rate.RetailRate - ((percentage * rate.RetailRate) / 100));
                            }
                            else
                            {
                                decimal discountValue = promotion.Get.HasValue ? (decimal)promotion.Get.Value : 0;
                                rate.RetailRate = rate.RetailRate - discountValue > 0 ? rate.RetailRate - discountValue : rate.RetailRate;
                            }
                            rate.RetailRate *= rateVND;
                        }
                        foreach (var rate in tourRates2)
                        {
                            // Discount type = 1 then discount percent, else discount by price
                            if (promotion.DiscountType == 1)
                            {
                                decimal percentage = promotion.Get.HasValue ? (decimal)promotion.Get.Value : 0;
                                rate.RetailRate = (rate.RetailRate - ((percentage * rate.RetailRate) / 100));
                            }
                            else
                            {
                                decimal discountValue = promotion.Get.HasValue ? (decimal)promotion.Get.Value : 0;
                                rate.RetailRate = rate.RetailRate - discountValue > 0 ? rate.RetailRate - discountValue : rate.RetailRate;
                            }
                            rate.RetailRate *= rateVND;
                        }
                        foreach (var rate in tourRates3)
                        {
                            // Discount type = 1 then discount percent, else discount by price
                            if (promotion.DiscountType == 1)
                            {
                                decimal percentage = promotion.Get.HasValue ? (decimal)promotion.Get.Value : 0;
                                rate.RetailRate = (rate.RetailRate - ((percentage * rate.RetailRate) / 100));
                            }
                            else
                            {
                                decimal discountValue = promotion.Get.HasValue ? (decimal)promotion.Get.Value : 0;
                                rate.RetailRate = rate.RetailRate - discountValue > 0 ? rate.RetailRate - discountValue : rate.RetailRate;
                            }
                            rate.RetailRate *= rateVND;
                        }
                    }
                }
            }

            if (surcharges != null)
            {
                foreach (Tour_Surcharge sc in surcharges)
                {
                    if (sc.Type == (int)Utilities.SurChargeType.Price)
                    {
                        foreach (var rate in tourRates)
                        {
                            rate.RetailRate = rate.RetailRate + (sc.Price);
                            rate.RetailRate *= rateVND;
                        }
                        foreach (var rate in tourRates2)
                        {
                            rate.RetailRate = rate.RetailRate + (sc.Price);
                            rate.RetailRate *= rateVND;
                        }
                        foreach (var rate in tourRates3)
                        {
                            rate.RetailRate = rate.RetailRate + (sc.Price);
                            rate.RetailRate *= rateVND;
                        }
                        
                    }
                    else
                    {
                        foreach (var rate in tourRates)
                        {
                            rate.RetailRate = rate.RetailRate + (rate.RetailRate * sc.Price / 100);
                            rate.RetailRate *= rateVND;
                        }
                        foreach (var rate in tourRates2)
                        {
                            rate.RetailRate = rate.RetailRate + (rate.RetailRate * sc.Price / 100);
                            rate.RetailRate *= rateVND;
                        }
                        foreach (var rate in tourRates3)
                        {
                            rate.RetailRate = rate.RetailRate + (rate.RetailRate * sc.Price / 100);
                            rate.RetailRate *= rateVND;
                        }
                    }
                   
                }
            }
            foreach (var rate in tourRates)
            {
                rate.RetailRate = Math.Round((decimal)rate.RetailRate * (decimal)1.15, 2) * rateVND;
            }
            foreach (var rate in tourRates2)
            {
                rate.RetailRate = Math.Round((decimal)rate.RetailRate * (decimal)1.15, 2) * rateVND;
            }
            foreach (var rate in tourRates3)
            {
                rate.RetailRate = Math.Round((decimal)rate.RetailRate * (decimal)1.15, 2) * rateVND;
            }

            var tourModel = new TourModel();
            tourModel.Tour = tour;
            if (tourRates.Count > 0)
            {
                tourModel.RetailRate1 = tourRates.First().RetailRate;
            }
            if (tourRates2.Count > 0)
            {
                tourModel.RetailRate2 = tourRates2.First().RetailRate;
            }
            if (tourRates3.Count > 0)
            {
                tourModel.RetailRate3 = tourRates3.First().RetailRate;
            }
            var total1 = (tourModel.RetailRate1 * adult);


            var total2 = (tourModel.RetailRate2 * adult);
            var total3 = (tourModel.RetailRate3 * adult);
            if (children > 0)
            {
                decimal childRatio = (decimal)0.75;
                total1 += (tourModel.RetailRate1 * children * childRatio);
                total2 += (tourModel.RetailRate2 * children * childRatio);
                total3 += (tourModel.RetailRate3 * children * childRatio);
            }
            tourModel.FindalRate1 = total1;
            tourModel.FindalRate2 = total2;
            tourModel.FindalRate3 = total3;

            tourModel.Children = children;
            tourModel.Adult = adult;
            tourModel.BookDate = date;
            return PartialView("_TourRateOptions", tourModel);


        }

        [HttpPost]
        [HttpGet]
        [Route("tour/GetTourPriceDetail")]
        public ActionResult GetTourPriceDetail(int id, DateTime date)
        {
            var tourRates = TourRates(id, date);
            return PartialView("_TourRates", tourRates);

            //return Json(tourRates, JsonRequestBehavior.AllowGet);
        }

        private List<Rate> TourRates(int id, DateTime date)
        {
            var tourRates = _tourRateService.Queryable().Where(t => t.TourId == id).ToList();
            var surcharges = _tourSurchargeService.GetSurchargesByDayTourId(date, id);
            var promotions = _tourPromotionService.GetPromotionByDayTourId(date, id);
            foreach (var promotion in promotions)
            {
                //languages is english then continue to other promotion
                if (promotion.Language == 1)
                    continue;
                if (promotion != null)
                {
                    if (promotion.NumberPerson > 0)
                    {
                        foreach (var rate in tourRates)
                        {
                            if (rate.PersonNo == promotion.NumberPerson)
                            {
                                // Discount type = 1 then discount percent, else discount by price
                                if (promotion.DiscountType == 1)
                                {
                                    decimal percentage = promotion.Get.HasValue ? (decimal)promotion.Get.Value : 0;
                                    rate.RetailRate = (rate.RetailRate - ((percentage * rate.RetailRate) / 100));
                                }
                                else
                                {
                                    decimal discountValue = promotion.Get.HasValue ? (decimal)promotion.Get.Value : 0;
                                    rate.RetailRate = rate.RetailRate - discountValue > 0
                                        ? rate.RetailRate - discountValue
                                        : rate.RetailRate;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var rate in tourRates)
                        {
                            // Discount type = 1 then discoutn percent, else discount by price
                            if (promotion.DiscountType == 1)
                            {
                                decimal percentage = promotion.Get.HasValue ? (decimal)promotion.Get.Value : 0;
                                rate.RetailRate = (rate.RetailRate - ((percentage * rate.RetailRate) / 100));
                            }
                            else
                            {
                                decimal discountValue = promotion.Get.HasValue ? (decimal)promotion.Get.Value : 0;
                                rate.RetailRate = rate.RetailRate - discountValue > 0
                                    ? rate.RetailRate - discountValue
                                    : rate.RetailRate;
                            }
                        }
                    }
                }
            }
            if (surcharges != null)
            {
                foreach (Tour_Surcharge sc in surcharges)
                {
                    foreach (var rate in tourRates)
                    {
                        if (sc.Type == (int)Utilities.SurChargeType.Price)
                        {
                            rate.RetailRate = rate.RetailRate + sc.Price;
                        }
                        else
                        {
                            rate.RetailRate = rate.RetailRate + (rate.RetailRate * sc.Price / 100);
                        }
                    }
                }
            }
            return tourRates;
        }

        public bool IsMobileBrowser()
        {
            HttpBrowserCapabilitiesBase myBrowserCaps = Request.Browser;
            return myBrowserCaps.IsMobileDevice;
        }

        [Route("tour/{title}-p{id}")]
        public ActionResult Detail(int id,string title)
        {
            string referer = Request.UrlReferrer?.AbsolutePath ?? "";
            string ip = Request.UserHostAddress;
            string currentUrl = Request.Url?.AbsolutePath ?? "";
            log.Info($"User request {currentUrl} from ip {ip} come from {referer}");
            var tourModel = new TourModel();
            var images = _mediaService.Queryable().Where(t => t.OwnerId == id && t.MediaType != 4 && t.MediaType != 1).ToList();
            var tour = _tourService.Queryable().FirstOrDefault(t => t.Id == id);
            if (tour == null) return View(tourModel);
            string _slug_title = Web.Framework.Utilities.GenerateSlug(tour.TourTitle, 200);
            if (!_slug_title.Equals(title))
            {
                 return Redirect("https://goreise.com");
            }
            var tourDetails = GetTourDetailsByTourId(tour.Id);
            var ratesList = _specialRateService.Queryable().Where(a => a.TourId == id).ToList();
            tourModel.Details = tourDetails;
            var tourRateOption = _tourrateOptionsService.Queryable().Where(a => a.Tour_Id == id).ToList();
            tourModel.Tour = tour;
            tourModel.Medium = images;
            ViewBag.IsMobile = IsMobileBrowser();
            tourModel.Rates = _tourRateService.Queryable().Where(t => t.TourId == id).ToList();
            tourModel.Promotions = ratesList.Where(a => a.Type == (int)Services.Utilities.SpecialRateType.Promotion).ToList();
            tourModel.Surcharges = ratesList.Where(a => a.Type == (int)Services.Utilities.SpecialRateType.Surcharge).ToList();
            //For Seo
            var similarTours = _tourService.GetSimilarToursByLocation(tour.Id, tour.Location, (int)Utilities.Status.Active,4);
          //  var similarKeyTours = _tourService.GetSimilarToursByKey(tour.Cities[0].ToString(), (int)Utilities.Status.Active, "cruising", 2);
            var similarTourModels = (from similarTour in similarTours
                                     let media = _mediaService.Queryable().FirstOrDefault(t => t.OwnerId == similarTour.Id && t.MediaType == (int)Utilities.MediaType.Banner)
                                     let tourFeatured = _tourService.GetTourById(similarTour.Id)
                                     let rates = _tourRateService.Queryable().Where(t => t.TourId == similarTour.Id).ToList()
                                     let seoTourFeatured = _seoService.GetSeoEntityId(similarTour.Id)
                                     let country = _countryService.GetCountryList().FirstOrDefault(c => c.CountryId == tourFeatured.CountryId)
                                     let seoModel = new SeoModel
                                     {
                                         Slug = seoTourFeatured.Slug,
                                         Description = "test",
                                         Keyword = seoTourFeatured.Keyword,
                                         Title = tourFeatured.TourTitle,
                                         ShortCountryName = country != null ? country.ShortName : string.Empty
                                     }
                                     select new TourModel
                                     {
                                         Seo = seoModel,
                                         Rates = rates,
                                         Tour = tourFeatured,
                                         Name = tourFeatured.Name,
                                         Description = tourFeatured.Description,
                                         Id = tourFeatured.Id,
                                         //OriginalPath 
                                         ImageUrl = (media != null ? $"{Utilities.NoneSslUrl}{Url.Content(media.ThumbnailPath)}" : "/content/images/vinaday-background.jpg")
                                     }).ToList();
            ViewBag.SimilarTours = similarTourModels;
            ViewBag.Keywords = tour.SEO_Meta;
            var tourReviews = _tourReviewService.GetTourReviewByTourId(id);
            ViewBag.TourReviews = tourReviews;
            ViewBag.Description = tour.SEO_Description;
            ViewBag.TourRates = TourRates(id, DateTime.Now);
            ViewBag.TourRateOptions = tourRateOption;
            ViewBag.Title = tour.SEO_Title;
            return View(tourModel);
        }

        [Route("search/{city}/key-{key}/filter-{filter}/price-{price}/page-{page}")]
        //search/key:ho-chi-minh/key-none/filter:sighseeing,cruises,walking-tours/price:1,3/page:1
        public ActionResult Search_Redirect(string city, string key, string filter, string price, int? page)
        {
            if (Request.Url != null)
            {
                string url = Request.Url.AbsolutePath;
                return Redirect(url.Replace("search", "searchs"));
            }
            else
            {
                return Redirect("https://goreise.com");
            }

        }

        [Route("searchs/{city}/key-{key}/filter-{filter}/price-{price}/page-{page}")]
        //search/key:ho-chi-minh/key-none/filter:sighseeing,cruises,walking-tours/price:1,3/page:1
        public ActionResult Search(string city, string key, string filter, string price, int? page)
        {
            string referer = Request.UrlReferrer?.AbsolutePath ?? "";
            string ip = Request.UserHostAddress;
            if (IsDenyIp(ip))
            {
                //log.Info($"deny from ip {ip} ");
                return null;
            }
            string currentUrl = Request.Url?.AbsolutePath ?? "";
            log.Info($"User request {currentUrl} from ip {ip} come from {referer}");


            //   ViewBag.User = GetCookie();
            List<Tour> searchResult;
            City cityModel = new City();
            var tourModels = new List<TourModel>();

            var filterArray = filter.Split('|');
            if (city == "all")
            {
                searchResult = _tourService.GetTours((int)Utilities.Language.English);

            }
            else
            {
                city = city.Replace("-", "");
                cityModel = _cityService.GetCity(city);
                city = cityModel.CityId.ToString(CultureInfo.InvariantCulture);
                searchResult = _tourService.GetToursByFilterKey(city, filterArray.Select(f => "[" + f + "]").ToArray(), (int)Utilities.Language.English);
            }

            if (key != "none")
            {
                searchResult = searchResult.Where(t => t.SearchKey.ToLower().Contains(key.ToLower().Replace("-", " "))).ToList();

            }
            if (filterArray.Length > 0 && filterArray[0] != "all")
            {
                var temp = new List<Tour>();
                foreach (var item in filterArray)
                {
                    var search1 = searchResult.Where(a => a.Filter != null && a.Filter.Contains(item)).ToList();
                    temp.AddRange(search1);
                }
                searchResult = temp;
            }



            #region city is not null
            if (!string.IsNullOrEmpty(city))
            {
                var tourTemps = searchResult;
                //Get and filter Travel Style
                var travelStyles = GetItemModel(tourTemps, (int)Utilities.SearchFilter.TravelStyle, filter);
                var catdetailList = _categoryDetailService.Queryable().ToList();
                var travelStylesItems = new List<ItemModel>();
                foreach (var travelStyle in travelStyles)
                {
                    int id;
                    int.TryParse(travelStyle.Name, out id);
                    var categoryDetail = catdetailList.FirstOrDefault(a => a.Id == id);
                    if (categoryDetail != null)
                    {
                        var categoryName = categoryDetail.Name;
                        travelStyle.Name = categoryName;
                        travelStyle.Slug = Utilities.GenerateSlug(categoryDetail.Name);
                        travelStyle.Checked = Array.FindAll(filterArray, val => val.Equals(Utilities.GenerateSlug(categoryName))).Any();
                    }
                    travelStylesItems.Add(travelStyle);
                }
                ViewBag.TravelStyles = travelStylesItems;
                var catList = _categoryService.GetCategories().ToList();
                var category = catList.FirstOrDefault(c => c.KeyCode == Utilities.TravelStyle);
                if (category == null) return View();
                var catdetails = _categoryDetailService.GetCategoriesDetail(category.Id);
                ViewBag.TravelStyles2 = Utilities.GetCategoryList(string.Empty, catdetails);
                List<ItemModel> servirces = GetItemModel(tourTemps, (int)Utilities.SearchFilter.Servirces, filter);
                var servirceItems = new List<ItemModel>();
                foreach (var servirce in servirces)
                {
                    int id;
                    int.TryParse(servirce.Name, out id);

                    var categoryDetail = catdetailList.FirstOrDefault(a => a.Id == id);
                    if (categoryDetail != null)
                    {
                        var categoryName = categoryDetail.Name;
                        servirce.Name = categoryName;
                        servirce.Slug = Utilities.GenerateSlug(categoryDetail.Name);
                        servirce.Checked = Array.FindAll(filterArray, val => val.Equals(Utilities.GenerateSlug(categoryName))).Any();
                    }
                    servirceItems.Add(servirce);
                }
                ViewBag.Servirces = servirceItems;
                //Get Duration
                var durations = GetItemModel(tourTemps, (int)Utilities.SearchFilter.Duration, filter);
                var durationItems = new List<ItemModel>();

                foreach (var duration in durations)
                {
                    int id;
                    int.TryParse(duration.Name, out id);

                    var categoryDetail = catdetailList.FirstOrDefault(a => a.Id == id);
                    if (categoryDetail != null)
                    {
                        var categoryName = categoryDetail.Name;
                        duration.Name = categoryName;
                        duration.Slug = Utilities.GenerateSlug(categoryDetail.Name);
                        duration.Checked = Array.FindAll(filterArray, val => val.Equals(Utilities.GenerateSlug(categoryName))).Any();
                    }
                    durationItems.Add(duration);
                }
                ViewBag.Durations = durationItems;
                //Check filters
                if (tourTemps.Count > 0)
                {

                    searchResult = searchResult.Distinct().ToList();
                    var tourList = _tourService.Queryable().Where(a=>a.Status==(int)Web.Framework.Utilities.Status.Active).ToList();
                    var countries = _countryService.GetCountryList();
                    var tourRates = _tourRateService.Queryable().ToList();
                    var medias = _mediaService.Queryable().Where(a => a.MediaType == (int)Utilities.MediaType.Banner).ToList();
                    tourModels = new List<TourModel>();
                    foreach (var featured in searchResult)
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
                            Title = tour.TourTitle,
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
                            ImageUrl = (media?.ThumbnailPath != null) ? $"{Utilities.NoneSslUrl}{Url.Content(media.ThumbnailPath)}" : string.Format("~/Content/Images/no-image.jpg"),
                            Seo = seoModel,
                            Tour = tour
                        });
                    }
                }
            }
            #endregion

            ViewBag.Filter = filter.Replace("-", " ").Replace("|", ", ");
            ViewBag.City = cityModel.Description;
            ViewBag.Key = key;
            ViewBag.TotalItem = tourModels.Count();
            const int pageSize = 10;
            var pageNumber = (page ?? 1);
            return View(tourModels.ToPagedList(pageNumber, pageSize));
        }
        [Route("vinaday-tours-daytrips/page-{page}")]
        public ViewResult TourList(int? page)
        {
            string referer = Request.UrlReferrer?.AbsolutePath ?? "";
            string ip = Request.UserHostAddress;
            string currentUrl = Request.Url?.AbsolutePath ?? "";
            log.Info($"User request {currentUrl} from ip {ip} come from {referer}");
            ViewBag.User = GetCookie();
            var tours = _tourService.Queryable().ToList();
            var seos = _seoService.Queryable().Where(a => a.EntityName != "Hotel").ToList();
            var medias = _mediaService.Queryable().Where(a => a.MediaType == (int)Utilities.MediaType.Banner).ToList();
            var countries = _countryService.GetCountryList();
            var tourRates = _tourRateService.Queryable().ToList();
            var tourModels = (from featured in tours
                              let tour = tours.FirstOrDefault(a => a.Id == featured.Id)
                              let seo = seos.FirstOrDefault(s => s.EntityId == featured.Id && s.IsActive)
                              let rates = tourRates.Where(a => a.TourId == featured.Id).Take(1).ToList()
                              let country = countries.FirstOrDefault(c => c.CountryId == tour.CountryId)
                              where seo != null
                              let seoModel = new SeoModel
                              {
                                  Slug = seo.Slug,
                                  Description = seo.Description,
                                  Keyword = seo.Keyword,
                                  Title = seo.Title,
                                  ShortCountryName = country != null ? country.ShortName : string.Empty
                              }
                              let media = medias.FirstOrDefault(t => t.OwnerId == featured.Id)
                              select new TourModel
                              {
                                  Rates = rates,
                                  Name = tour.Name,
                                  Description = tour.Description,
                                  Id = tour.Id,
                                  ImageUrl = (media != null && media.ThumbnailPath != null) ? string.Format("{0}{1}", Utilities.NoneSslUrl, Url.Content(media.ThumbnailPath)) : string.Format("~/Content/Images/no-image.jpg"),
                                  Seo = seoModel,
                                  Tour = tour
                              }).ToList();
            ViewBag.Tours = tourModels;

            const int pageSize = 12;
            var pageNumber = (page ?? 1);
            return View(tourModels.ToPagedList(pageNumber, pageSize));

            //return View();
        }
        [Route("links")]
        public ActionResult Links()
        {
            return View();
        }

        public ActionResult PrintPdf(int id, string url)
        {
            var tour = _tourService.Queryable().FirstOrDefault(t => t.Id == id);
            if (tour == null) return null;
            var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
            var fileName = Utilities.GenerateSlug(tour.Name) + "_" + id + "_" + DateTime.Now.ToString("yyMMddhhmm") + ".pdf";
            var fullPath = Server.MapPath($"~/PdfFiles/{fileName}");
            var server = Request.Url.GetLeftPart(UriPartial.Authority) + "/tour/PrintPdfToHtml/";
            htmlToPdf.GeneratePdfFromFile(server + id + "?url=" + url, null, fullPath);
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            return File(fullPath, "application/pdf");
        }

        public ActionResult PrintPdfToHtml(int id, string url)
        {
            var tourModel = new TourModel();
            var tour = _tourService.Queryable().FirstOrDefault(t => t.Id == id);
            if (tour == null) return null;
            var tourDetails = GetTourDetailsByTourId(tour.Id);
            tourModel.Details = tourDetails;
            tourModel.Tour = tour;
            ViewBag.Url = url;
            return PartialView(tourModel);
        }

        [HttpPost]
        [Route("get-tour-city")]
        public ActionResult GetCities(ObjectModel objectModel)
        {
            string referer = Request.UrlReferrer?.AbsolutePath ?? "";
            string ip = Request.UserHostAddress;
            string currentUrl = Request.Url?.AbsolutePath ?? "";
            log.Info($"User request {currentUrl} from ip {ip} come from {referer}");
            var cities = _cityService.GetCities().Where(c => c.CountryId == objectModel.Id && c.Status==1).ToList();

            //get tour by city if >0 then show it out, other wise hide it
            var tourListByCountry = _tourService.GetTours((int)Services.Utilities.Language.English).AsQueryable().Where(a => a.CountryId == objectModel.Id).ToList();
            var listCity = new List<City>();
            foreach (var city in cities)
            {
                var findOutCity = tourListByCountry.Where(a => a.Cities.Contains(city.CityId.ToString())).ToList();
                if (findOutCity.Count > 0)
                {
                    var newCity = new City()
                    {
                        CityId = city.CityId,
                        Description = city.Description,
                        Name = city.Description + string.Format(" ({0})", findOutCity.Count)
                    };
                    listCity.Add(newCity);
                }
            }
            listCity.Insert(0, new City() { Description = "All", Name = "All" });
            ViewBag.Cities = listCity;
            return PartialView("_CityTourPartial");
        }
        private static List<ItemModel> GetItemModel(IEnumerable<Tour> tours, int filterName, string keys)
        {
            var serviceStr = string.Empty;
            switch (filterName)
            {
                case (int)Utilities.SearchFilter.TravelStyle:
                    serviceStr = tours.Where(tour => tour != null && tour.TravelStyle != null).Aggregate(serviceStr, (current, tour) => current + string.Format("{0},", tour.TravelStyle));
                    break;
                case (int)Utilities.SearchFilter.Servirces:
                    serviceStr = tours.Where(tour => tour != null && tour.GroupSize != null).Aggregate(serviceStr, (current, tour) => current + string.Format("{0},", tour.GroupSize));
                    break;
                case (int)Utilities.SearchFilter.Duration:
                    serviceStr = tours.Where(tour => tour != null && tour.Duration != null).Aggregate(serviceStr, (current, tour) => current + string.Format("{0},", tour.Duration));
                    break;
            }
            var itemCheckedArray = keys.Split('|');
            var filterArray = serviceStr.Split(',');
            var filters = new List<ItemModel>();
            for (var i = 0; i < filterArray.Count(); i++)
            {
                var name = filterArray[i];
                int id;
                int.TryParse(name, out id);
                if (string.IsNullOrEmpty(name)) continue;

                var value = Array.FindAll(filterArray, val => val.Equals(filterArray[i])).Length;
                var itemExsist = filters.Any(k => k.Name == name);
                var itemChecked = itemCheckedArray.Any(k => k.Contains(Utilities.GenerateSlug(name)));
                if (itemExsist) continue;
                var filter = new ItemModel
                {
                    Id = id,
                    Name = name,
                    Count = value,
                    Slug = Utilities.GenerateSlug(name),
                    Checked = itemChecked
                };
                filters.Add(filter);
            }
            return filters.Distinct().ToList();
        }

        [Route("payment-information")]
        public ActionResult Payment()
        {
            return View();
            //return Redirect("https://goreise.com/payment-information");
        }
        [Route("affiliate-partners")]
        public ActionResult Affiliate()
        {

            return Redirect("https://goreise.com/affiliate-partners");
        }
        [Route("partners")]
        public ActionResult Partners()
        {
            return Redirect("https://goreise.com/partners");
        }
        [Route("websites-rules")]
        public ActionResult TermsOfUse()
        {
            return View();
            // return Redirect("https://goreise.com/websites-rules");
        }
        [Route("privacy-policy")]
        public ActionResult PrivacyPolicy()
        {
            return Redirect("https://goreise.com/privacy-policy");
        }
        [Route("help-faqs")]
        public ActionResult Help()
        {
            return Redirect("https://goreise.com/help-faqs");
        }
        [Route("rewards")]
        public ActionResult Rewards()
        {
            return Redirect("https://goreise.com/rewards");
        }
        [Route("careers")]
        public ActionResult Careers()

        {
            return Redirect("https://goreise.com/careers");
        }
        public string GetCookie()
        {
            var cookieName = "goreise.cookie";//FormsAuthentication.FormsCookieName;
            var authCookie = Request.Cookies[cookieName];

            // This could throw an exception if it fails the decryption process. Check MachineKeys for consistency.  
            if (authCookie == null) return string.Empty;
            var authenticationTicket = FormsAuthentication.Decrypt(authCookie.Value);
            // Retrieve information from the ticket  
            return authenticationTicket != null ? authenticationTicket.Name : string.Empty;
        }
        [Route("inquiry")]
        public ActionResult Inquiry()
        {
            ViewBag.Nationalities = _nationalityService.GetNationalityList();
            using (vinadaydbEntities1 entities = new vinadaydbEntities1())
            {
                //var tours = entities.viewListTours.ToList();
                var tours = entities.viewListTours.OrderBy(a => a.Expr1).ToList();
                var list = tours.Select(tour => new ViewListTourMapping
                {
                    Id = tour.Id,
                    Duration = tour.Duration,
                    Name = tour.Name,
                    PriceFrom = tour.PriceFrom,
                    Location = tour.Location,
                    ThumbnailPath = tour.ThumbnailPath,
                    Expr1 = tour.Expr1,
                    Expr2 = tour.Expr2,
                    PageUrl = Utilities.GenerateSlug(tour.Tour_Title, 200),
                    SHORTNAME = tour.SHORTNAME,
                }).ToList();
                ViewBag.Tours = list;

            }

            return View();
        }
        [Route("UserReviewEdit/{title}-p{id}")]
        public ActionResult UserReviewEdit(int id,string title)
        {
            string referer = Request.UrlReferrer?.AbsolutePath ?? "";
            var tourModel = new TourModel();
            var images = _mediaService.Queryable().Where(t => t.OwnerId == id && t.MediaType != 4 && t.MediaType != 1).ToList();
            var tour = _tourService.Queryable().FirstOrDefault(t => t.Id == id);
            if (tour == null) return View(tourModel);
            string _slug_title = Web.Framework.Utilities.GenerateSlug(tour.TourTitle, 200);
            if (!_slug_title.Equals(title))
            {
                return Redirect("https://goreise.com");
            }
            var tourRateOption = _tourrateOptionsService.Queryable().Where(a => a.Tour_Id == id).ToList();
            tourModel.Tour = tour;
            tourModel.Medium = images;
            ViewBag.Keywords = tour.SEO_Meta;
            ViewBag.Description = tour.SEO_Description;
            ViewBag.Title = tour.SEO_Title;
            return View(tourModel);
        }
        //[Route("UserReviewEdit-Save")]
        [Route("tour/UserReviewEditSave")]
        [HttpPost]
        public ActionResult UserReviewEditSave(ObjectModel objectModel)
        {
            try
            {
                
                var tourReview = new TourReview();
                tourReview.Id = 0;
                tourReview.TourId = objectModel.IntParam2;
                tourReview.Start = objectModel.IntParam1;
                tourReview.Title = objectModel.StrParam1;
                tourReview.ReviewContent = objectModel.StrParam2;
                tourReview.BookDate = objectModel.StrParam3;
                tourReview.GuestName = objectModel.StrParam4;
                _tourReviewService.Insert(tourReview);
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Utilities.Status.Active;
                objectModel.Message = "Your Review has been sent";
            }
            catch (Exception e)
            {
                objectModel.Status = (int)Utilities.Status.Inactive;
                objectModel.Message = e.Message;
            }
            return Json(objectModel);
        }
        [HttpPost]
        [Route("send-inquiry")]
        public ActionResult SendInquiry(ObjectModel objectModel)
        {
            try
            {
                dynamic myObject = new
                {
                    DateTime = DateTime.Now.ToString("D", CultureInfo.CreateSpecificCulture("en-US")),
                    StrParam1 = objectModel.StrParam1,
                    StrParam2 = objectModel.StrParam2,
                    StrParam3 = objectModel.StrParam3,
                    StrParam4 = objectModel.StrParam4,
                    StrParam5 = objectModel.StrParam5,
                    StrParam6 = objectModel.StrParam6,
                    StrParam7 = objectModel.StrParam7,
                    StrParam8 = objectModel.StrParam8,
                };
                string inquiry = Utilities.ParseTemplate("Inquiry", myObject);
                MailClient.Inquiry(inquiry, "Inquiry Booking Tour" + objectModel.StrParam8);
                objectModel.Status = (int)Utilities.Status.Active;
                objectModel.Message = "Your Inquiry has been sent";
            }
            catch (Exception)
            {
                objectModel.Status = (int)Utilities.Status.Inactive;
                objectModel.Message = "Delete this item is error!";
            }
            return Json(objectModel);
        }
    }
}