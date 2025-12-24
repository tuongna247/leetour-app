using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;
using Vinaday.Data.Models;
using Vinaday.Data.Models.Extention;
using Vinaday.Services;
using Vinaday.Web.Framework;
using Vinaday.Web.Framework.EmailHelpers;
using Utilities = Vinaday.Web.Framework.Utilities;
using Vinaday.Web.Home.Models.ViewModel;
using Vinaday.Web.Home.Models;

namespace Vinaday.Web.Home.Controllers
{
    public class HotelController : Controller
    {
        private readonly IHotelService _hotelService;
        private readonly ITourService _tourService;
        private readonly HotelPackageService _hotelPackageService;
        private readonly IRateExchangeService _exchangeService;
        private readonly ITipService _tipService;
        private readonly ICatDetailService _catDetailService;
        private readonly IRoomService _roomService;
        private readonly IRoomControlService _roomControlService;
        private readonly ICountryService _countryService;
        private readonly IImageService _imageService;
        private readonly IRoomReguestService _roomReguestService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly ICategoryDetailService _categoryDetailService;
        private readonly DealHotelToursService _dealHotelTourService;
        private readonly ITourRateService _tourRateService;
        private readonly IDealService _dealService;
		private readonly INationalityService _nationalityService;
        private readonly IMediaService _mediaService;
        //private readonly IImageService _imagesService;
        public HotelController(
            IHotelService hotelService,
            IRoomService roomService,
          ITourService tourService,
           ITourRateService tourRateService,
        IRoomControlService roomControlService,
               INationalityService nationalityService,
            IImageService imageService,
            IMediaService mediaService,
            ICatDetailService catDetailService,
            ICountryService countryService,
            ITipService tipService,
            IRateExchangeService exchangeService,
            IRoomReguestService roomReguestService,
            DealHotelToursService dealHotelTourService,

        IDealService dealService,
            IUnitOfWorkAsync unitOfWorkAsync, ICategoryDetailService categoryDetailService, HotelPackageService hotelPackageService)
        {
            _hotelService = hotelService;
            _roomService = roomService;
            _tourService = tourService;
            _tourRateService = tourRateService;
            _roomControlService = roomControlService;
            _mediaService = mediaService;
            _imageService = imageService;
            _catDetailService = catDetailService;
            _countryService = countryService;
            _tipService = tipService;
            _exchangeService = exchangeService;
            _nationalityService = nationalityService;
            _hotelPackageService = hotelPackageService;
            _dealService = dealService;
            _roomReguestService = roomReguestService;
            _unitOfWorkAsync = unitOfWorkAsync;
            _categoryDetailService = categoryDetailService;
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
            //var hotelSimilars =
            //    _hotelService.Queryable().Where(a => a.CollectionValue.HasValue && a.CollectionValue > 0).Take(10).ToList();
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
            var HotelNices = new List<Hotel>();
          
            var hotelImages = _imageService.GetHotelImages().ToList();
            var hotelSimilarBindList = new List<HotelModel>();
            var hotelSimilars = new List<Hotel>();
            //foreach (var hotel in hotelSimilars)
            //{
            //    var image = hotelImages.FirstOrDefault(a => a.HotelId == hotel.Id);
            //    var _hotel = new HotelModel
            //    {
            //        Id = hotel.Id,
            //        Name = hotel.Name,
            //        Star = hotel.StartRating ?? 1,
            //        Description = hotel.Description,
            //        CityId = hotel.CityId ?? -1,
            //        Phone = hotel.Phone,
            //        ImageUrl =
            //            image != null
            //                ? !string.IsNullOrEmpty(image.ImageThumbnail)
            //                    ? "https://admin.goreise.com" + $"{image.ImageThumbnail.Substring(1)}-original.jpg"
            //                    : "/Content/images/demo/general/no-image.jpg"
            //                : "/Content/images/demo/general/no-image.jpg",
            //        HotelUrl = $"/hotel/{Utilities.GenerateSlug(hotel.Name)}-p{hotel.Id}"
            //    };
            //    hotelSimilarBindList.Add(_hotel);
            //}

            var hotelsId = new List<int>() { 14707, 15445, 11570, 22373 };
            var hotels2Id = new List<int>() { 22367, 20169, 21237, 19153 };
            var hotelList =
                _hotelService.Queryable().ToList();
            foreach (var id in hotelsId)
            {
                var hotel = hotelList.FirstOrDefault(a => a.Id == id);
                if (hotel != null)
                {
                    hotelSimilars.Add(hotel);
                }
            }
            foreach (var id in hotels2Id)
            {
                var hotel = hotelList.FirstOrDefault(a => a.Id == id);
                if (hotel != null)
                {
                    HotelNices.Add(hotel);
                }
            }
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
            var hotelHotelNicesBindList = new List<HotelModel>();
            foreach (var hotel in HotelNices)
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
                hotelHotelNicesBindList.Add(_hotel);
            }
            ViewBag.HotelNices = hotelHotelNicesBindList;
            ViewBag.HotelSimilars = hotelSimilarBindList;
            ViewBag.TourSimilars = tourModels;
        }
        public string GetCookie()
        {
            return string.Empty;
            //var cookieName = FormsAuthentication.FormsCookieName;
            //var authCookie = Request.Cookies[cookieName];

            //// This could throw an exception if it fails the decryption process. Check MachineKeys for consistency.  
            //if (authCookie == null) return string.Empty;
            //var authenticationTicket = FormsAuthentication.Decrypt(authCookie.Value);
            //// Retrieve information from the ticket  
            //return authenticationTicket != null ? authenticationTicket.Name : string.Empty;
        }
        private List<CategoryDetail> GetTheCollection(Hotel hotel)
        {
            if (hotel == null)
            {
                return new List<CategoryDetail>();
            }
            var theCollectionModelList = new List<CategoryDetail>();
            if (hotel.HotelStyle == null) return theCollectionModelList;
            var theCollection = hotel.HotelStyle.Split(',');
            for (var j = 0; j < theCollection.Count(); j++)
            {
                var f = Framework.Utilities.ConvertToInt(theCollection[j]);

                var category = _categoryDetailService.ODataQueryable().FirstOrDefault(c => c.Id == f);
                if (category == null) continue;
                var categoryName = !string.IsNullOrEmpty(category.Description)
                    ? category.Description
                    : category.Name;
                var theCollectionModel = new CategoryDetail
                {
                    Name = category.Name,
                    Description = categoryName
                };
                theCollectionModelList.Add(theCollectionModel);
            }
            return theCollectionModelList;
        }
        [Route("hotel/{name}-p{id}")]
        public ActionResult Detail(int id)
        {
            ViewBag.User = GetCookie();
            var checkInSessinon = Session[Constant.SessionHotelCheckin];
            var checkOutSessinon = Session[Constant.SessionHotelCheckout];
            if (checkInSessinon != null && checkOutSessinon != null)
            {
                ViewBag.CheckIn = checkInSessinon;
                ViewBag.CheckOut = checkOutSessinon;
            }
            ViewBag.Nationalities = _nationalityService.GetNationalityList();
            ViewBag.Reviews = _hotelService.GetReviewsByHotelId(id, 2);
            ViewBag.Exchanges = _exchangeService.GetRateExchanges();
            var hotel = _hotelService.GetHotelSingle(id);
            if (hotel == null) return Redirect("/404");
            if(hotel!=null && hotel.CollectionValue.Value <=0)
            {
                return Redirect("/404");
            }
            var hotelPackage = _hotelPackageService.ODataQueryable().Where(a => a.HotelId == id).ToList<HotelPackage>();
            var packages = hotelPackage.GroupBy(a => a.Night).Select(a => a.Key).ToList();
            ViewBag.Nights = packages;
            ViewBag.HotelPackage = hotelPackage;
            var catDetails = _catDetailService.GetCategoriesDetail();
            var facilities = Framework.Utilities.GetCategoryDetailListItem(hotel.HotelFacilities, catDetails);
            var sportRecreations = Framework.Utilities.GetCategoryDetailListItem(hotel.HotelSportRecreation, catDetails);
            var tip = _tipService.GetTipByHotelId(hotel.Id);
            var images =
                _imageService.GetImageListByHotelId(hotel.Id)
                    .Where(i => i.PictureType == Constant.ImageRestaurant || i.PictureType == "Bar" || i.PictureType == "MainPhoto")
                    .ToList();

            var hotelModel = new HotelModel
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Country = hotel.Country != null ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(hotel.Country.Replace("-", "")) : "",
                City = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(hotel.CITY),
                Address = hotel.StreetAddressEnglish,
                TotalRoom = hotel.NumberOfRoom.ToString(),
                Star = hotel.StartRating ?? 0,
                Overview = hotel.Description,
                Facilities = facilities,
                SportRecreations = sportRecreations,
                Description = hotel.Description,
                IsWifi = facilities.Find(f => f.Id == 24) != null,
                ParkingAvailable = facilities.Find(f => f.Id == 27) != null,
                AirportFee = hotel.AirportTransferFee ?? 0.0,
                AirportTransferFee = hotel.AirportTransferFee,
                ShowAirportTransferAvailable = hotel.ShowAirportTransferAvailable ?? false,
                BreakfastCharge = hotel.BreakfastCharge,
                ShowBreakfastCharge = hotel.ShowBreakfastCharge,
                HotelKeyword = hotel.HotelKeyword,
                CheckOut = hotel.ShowCheckOut.HasValue && hotel.ShowCheckOut.Value ? hotel.CheckOut : "",
                EarliestCheckIn = hotel.ShowCheckIn.HasValue && hotel.ShowCheckIn.Value ? hotel.CheckIn : "",
                NumberOfRestaurants = hotel.NumberOfRestaurants ?? 0,
                RoomService = hotel.RoomService,
                HashTagEn = hotel.HashTagEn,
                RoomVoltage = hotel.RoomVoltage,
                TimeToAirport = hotel.TimeToAirport,
                ImportantNoteVn =  hotel.ImportantNoteVn,
                ImportantNoteEn = hotel.ImportantNoteEn,
                YearHotelBuilt = hotel.YearHotelBuilt,
                YearHotelLastRenovated = hotel.YearHotelLastRenovated,
                Tip = tip,
                SeoTitle = hotel.SeoTitle,
                Images = images,
                CityId = hotel.CityId ?? 0,
                Longtitude = hotel.Longtitude ?? 0,
                Latitude = hotel.Latitude ?? 0,
                TheCollections = GetTheCollection(hotel)
            };
            var rooms = _roomService.GetRoomList(hotel.Id).OrderBy(a => a.Sort).ToList();
            List<RoomModel> roomsModel = new List<RoomModel>();
            if (rooms.Count > 0)
            {
                foreach (var room in rooms)
                {
                    var ratesModel = new List<RoomModel>();
                    var roomModel = new RoomModel
                    {
                        Id = room.Id,
                        AdultNumber = room.AdultNumber ?? 0,
                        Name = room.Name,
                        ExtraBed = room.ExtraBed ?? 0,
                        ExtraBedPrice = room.ExtraBedPrice ?? 0,
                        ImageUrl = room.ImageUrl,
                        ChildrenAge = room.ChildrenAge ?? 0,
                        ChildrenNumber = room.ChildrenNumber ?? 0,
                        RoomSize = room.RoomSize ?? 0,
                        MaxOccupancy = room.MaxOccupancy ?? 0,
                        MaxExtrabed = room.MaxExtrabed ?? 0,
                        RackRate = room.RackRate ?? 0,
                        Cancelation = _hotelService.GetCancellationHotel(hotel.Id),
                        IsBreakfast = false,
                        SellingRate = 0,
                        TotalAvailable = 0,
                        CloseOutRegular = false
                    };
                    int viewId;
                    int.TryParse(room.View, out viewId);
                    var firstOrDefault = _catDetailService.GetCategoryDetail(viewId);
                    if (firstOrDefault != null)
                    {
                        roomModel.View = firstOrDefault.Name;
                    }

                    if (room.RoomFacilities != null)
                    {
                        roomModel.RoomFacilities = GetRoomCategories(room.RoomFacilities);

                    }
                    ratesModel.Add(roomModel);
                    roomModel.Rates = ratesModel;
                    roomsModel.Add(roomModel);
                }
            }
            ViewBag.CheckIn = DateTime.Now;
            ViewBag.roomsModel = roomsModel;
            ViewBag.Description = hotel.SeoDesc;
            ViewBag.Keywords = hotel.HotelKeyword;
            return View(hotelModel);
        }
        [Route("hotel")]
        public ActionResult Index()
        {
            var deal = _dealService.GetMainDeal();
            ViewBag.GetCountries = _countryService.GetCountryList();
            ViewBag.User = GetCookie();
            ViewBag.Deal = deal;
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
        [Route("detail-test")]
        public ActionResult DetailTest()
        {
            return View();
        }
        [Route("vinaday/api/get-cities")]
        [HttpGet]
        public JsonResult GetCities()
        {
            ViewBag.User = GetCookie();
            var cities = _hotelService.GetCitieByLuxyHotels();
            var resutls = from s in cities select new { name = s.Name, description = s.Description, hotelsCount = s.Count };
            var jsonResult = Json(resutls, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [Route("vinaday/api/get-cancellation")]
        [HttpGet]
        public ActionResult GetCancellation(ObjectModel objectModel)
        {
            var cancelltionPolicies = _hotelService.GetHotelCancellationPoliciesByHotelId(objectModel.Id);
            return PartialView("_Cancellation", cancelltionPolicies);
        }

        [Route("error")]
        public ActionResult Error()
        {
            return View();
        }
        //[Route("search/{search}")]
        //public ActionResult Search()
        //{
        //    return PartialView();
        //}
        //viet-nam/ho-chi-minh/key-none/star-none/facility-none/accommodation-none/area-none/sort-star-slow/page-1
        //[Route("{country}/{city}")]
        //public ActionResult City()
        //{
        //    ViewBag.User = GetCookie();
        //    ViewBag.Title = "Found 83 hotels in Ho Chi Minh";
        //    return PartialView();

        //}
        private string GetRoomCategories(string roomCategory)
        {
            var result = string.Empty;
            var roomCategories = Framework.Utilities.GetSelectedItem(roomCategory);
            return (from item in roomCategories select _catDetailService.GetCategoryDetail(item.Key) into firstOrDefault where firstOrDefault != null select firstOrDefault.Name).Aggregate(result, (current, catgoryName) => current + string.Format(" {0},", catgoryName));
        }
        public List<RoomModel> GetRooms(int hotelId, DateTime checkIn, DateTime checkOut)
        {
            Session[Constant.SessionHotelCheckin] = checkIn;
            Session[Constant.SessionHotelCheckout] = checkOut;
            ViewBag.CheckIn = checkIn;
            ViewBag.CheckOut = checkOut;
            //var checkInTemp = checkIn;
            //var checkOutTemp = checkOut;
            var roomsModel = new List<RoomModel>();

            var rooms = _roomService.GetRoomList(hotelId);
            if (rooms.Count > 0)
            {
                foreach (var room in rooms)
                {
                    var ratesModel = new List<RoomModel>();
                    var checkInTemp = checkIn;
                    var checkOutTemp = checkOut;
                    var roomModel = new RoomModel
                    {
                        Id = room.Id,
                        AdultNumber = room.AdultNumber ?? 0,
                        Name = room.Name,
                        ExtraBed = room.ExtraBed ?? 0,
                        ExtraBedPrice = room.ExtraBedPrice ?? 0,
                        ImageUrl = room.ImageUrl,
                        ChildrenAge = room.ChildrenAge ?? 0,
                        ChildrenNumber = room.ChildrenNumber ?? 0,
                        RoomSize = room.RoomSize ?? 0,
                        MaxOccupancy = room.MaxOccupancy ?? 0,
                        MaxExtrabed = room.MaxExtrabed ?? 0,
                        RackRate = room.RackRate ?? 0,
                        Cancelation = _hotelService.GetCancellationHotel(hotelId),
                        IsBreakfast = false,
                        SellingRate = 0,
                        TotalAvailable = 0,
                        CloseOutRegular = false

                    };
                    int viewId;
                    int.TryParse(room.View, out viewId);
                    var firstOrDefault = _catDetailService.GetCategoryDetail(viewId);
                    if (firstOrDefault != null)
                    {
                        roomModel.View = firstOrDefault.Name;
                    }

                    if (room.RoomFacilities != null)
                    {
                        roomModel.RoomFacilities = GetRoomCategories(room.RoomFacilities);

                    }
                    var isRoom = true;
                    while (checkInTemp < checkOutTemp)
                    {
                        var checkInTemp1 = DateTime.Parse(checkIn.ToShortDateString());
                        var roomTemp = _roomControlService.GetSingleRoomControlByDateRate(room.Id,
                            checkInTemp1);
                        if (roomTemp == null)
                        {
                            isRoom = false;
                        }
                        else
                        {
                            if (roomTemp.CloseOutRegular || roomTemp.TotalAvailable < 0 || roomTemp.SellingRate < 0)
                            {
                                isRoom = false;
                            }
                        }
                        checkInTemp = checkInTemp.AddDays(1);
                    }
                    if (isRoom)
                    {
                        var checkInTemp2 = DateTime.Parse(checkIn.ToShortDateString());
                        var checkOutTemp2 = DateTime.Parse(checkOut.ToShortDateString());
                        var roomControl = _roomControlService.GetSingleRoomCheckInOut(room.Id, checkInTemp2,
                            checkOutTemp2);
                        if (roomControl != null)
                        {
                            roomModel.IsBreakfast = roomControl.Breakfast ?? false;
                            roomModel.SellingRate = roomControl.SellingRate / 1.15m ?? 0;
                            roomModel.TotalAvailable = roomControl.TotalAvailable;
                            roomModel.CloseOutRegular = roomControl.CloseOutRegular;

                        }
                        var checkInDate = Framework.Utilities.ConvertToDateTime(checkIn.ToString(CultureInfo.InvariantCulture));
                        var checkOutDate = Framework.Utilities.ConvertToDateTime(checkOut.ToString(CultureInfo.InvariantCulture));
                        var promotions = _roomService.GetPromotions(roomModel, hotelId, room.Id, checkInDate, checkOutDate);

                        if (promotions != null && promotions.Count > 0)
                        {
                            ratesModel.AddRange(promotions);
                        }
                    }
                    ratesModel.Add(roomModel);
                    roomModel.Rates = ratesModel;
                    roomsModel.Add(roomModel);
                }
            }
            return roomsModel.OrderBy(r => r.SellingRate).ToList();
        }

        public List<RoomModel> GetRooms1(int hotelId, DateTime checkIn, DateTime checkOut)
        {
            Session[Constant.SessionHotelCheckin] = checkIn;
            Session[Constant.SessionHotelCheckout] = checkOut;
            ViewBag.CheckIn = checkIn;
            ViewBag.CheckOut = checkOut;
            //var checkInTemp = checkIn;
            //var checkOutTemp = checkOut;
            var roomsModel = new List<RoomModel>();
            var rooms = _roomService.GetRoomList(hotelId);
            if (rooms.Count > 0)
            {
                foreach (var room in rooms)
                {
                    var checkInTemp = checkIn;
                    var checkOutTemp = checkOut;
                    var roomModel = new RoomModel
                    {
                        Id = room.Id,
                        AdultNumber = room.AdultNumber ?? 0,
                        Name = room.Name,
                        ExtraBed = room.ExtraBed ?? 0,
                        ExtraBedPrice = room.ExtraBedPrice ?? 0,
                        ImageUrl = room.ImageUrl,
                        ChildrenAge = room.ChildrenAge ?? 0,
                        ChildrenNumber = room.ChildrenNumber ?? 0,
                        RoomSize = room.RoomSize ?? 0,
                        MaxOccupancy = room.MaxOccupancy ?? 0,
                        MaxExtrabed = room.MaxExtrabed ?? 0,
                        RackRate = room.RackRate ?? 0,
                        Cancelation = _hotelService.GetCancellationHotel(hotelId),
                        IsBreakfast = false,
                        SellingRate = 0,
                        TotalAvailable = 0,
                        CloseOutRegular = false

                    };
                    int viewId;
                    int.TryParse(room.View, out viewId);
                    var firstOrDefault = _catDetailService.GetCategoryDetail(viewId);
                    if (firstOrDefault != null)
                    {
                        roomModel.View = firstOrDefault.Name;
                    }

                    if (room.RoomFacilities != null)
                    {
                        roomModel.RoomFacilities = GetRoomCategories(room.RoomFacilities);

                    }
                    var isRoom = true;
                    while (checkInTemp < checkOutTemp)
                    {
                        var checkInTemp1 = DateTime.Parse(checkIn.ToShortDateString());
                        var roomTemp = _roomControlService.GetSingleRoomControlByDateRate(room.Id,
                            checkInTemp1);
                        if (roomTemp == null)
                        {
                            isRoom = false;
                        }
                        else
                        {
                            if (roomTemp.CloseOutRegular)
                            {
                                isRoom = false;
                            }
                            if (roomTemp.TotalAvailable < 0)
                            {
                                isRoom = false;
                            }
                            if (roomTemp.SellingRate < 0)
                            {
                                isRoom = false;
                            }
                        }
                        checkInTemp = checkInTemp.AddDays(1);
                    }
                    if (isRoom)
                    {
                        var checkInTemp2 = DateTime.Parse(checkIn.ToShortDateString());
                        var checkOutTemp2 = DateTime.Parse(checkOut.ToShortDateString());
                        var roomControl = _roomControlService.GetSingleRoomCheckInOut(room.Id, checkInTemp2,
                            checkOutTemp2);
                        if (roomControl != null)
                        {
                            roomModel.IsBreakfast = roomControl.Breakfast ?? false;
                            roomModel.SellingRate = roomControl.SellingRate / 1.15m ?? 0;
                            roomModel.TotalAvailable = roomControl.TotalAvailable;
                            roomModel.CloseOutRegular = roomControl.CloseOutRegular;

                        }
                        var checkInDate = Framework.Utilities.ConvertToDateTime(checkIn.ToString(CultureInfo.InvariantCulture));
                        var checkOutDate = Framework.Utilities.ConvertToDateTime(checkOut.ToString(CultureInfo.InvariantCulture));
                        var promotions = _roomService.GetPromotions(roomModel, hotelId, room.Id, checkInDate, checkOutDate);

                        if (promotions != null && promotions.Count > 0)
                        {
                            roomsModel.AddRange(promotions);
                        }
                    }
                    roomsModel.Add(roomModel);
                }
            }
            return roomsModel;
        }

        [Route("vinaday/api/check-rates")]
        [HttpPost]
        public ActionResult CheckRates(ObjectModel objectModel)
        {
            ViewBag.Exchange = _exchangeService.GetRateExchangeById(objectModel.IntParam1);
            var checkIn = Framework.Utilities.ConvertToDateTime(objectModel.StrParam1);
            var checkOut = Framework.Utilities.ConvertToDateTime(objectModel.StrParam2);
            Session[Constant.SessionHotelCheckin] = checkIn;
            Session[Constant.SessionHotelCheckout] = checkOut;
            var rooms = GetRooms(objectModel.Id, checkIn, checkOut);
            return PartialView("_Room", rooms);
        }

        [Route("vinaday/api/check-rates-package")]
        [HttpPost]
        public ActionResult CheckRatePackages(ObjectModel objectModel)
        {
            ViewBag.Exchange = _exchangeService.GetRateExchangeById(objectModel.IntParam1);
            var checkIn = Utilities.ConvertToDateTime(objectModel.StrParam1);
            var checkOut = Utilities.ConvertToDateTime(objectModel.StrParam2);

            ViewBag.CheckIn = checkIn;
            Session[Constant.SessionHotelCheckin] = checkIn;
            Session[Constant.SessionHotelCheckout] = checkOut;
            var package = new List<HotelPackage>();
            var hotelPackage = _hotelPackageService.ODataQueryable().Where(a => a.HotelId == objectModel.Id && a.FromDate <= checkIn && a.ToDate >= checkIn).ToList();            
            if (hotelPackage.Count > 0)
                package = hotelPackage;
            return PartialView("_RoomPackage", package);
        }

        [Route("vinaday/api/get-rooms")]
        [HttpGet]
        public ActionResult GetRooms(ObjectModel objectModel)
        {
            ViewBag.Exchange = _exchangeService.GetRateExchangeById(objectModel.IntParam1);

            var checkIn = Framework.Utilities.ConvertToDateTime(objectModel.StrParam1);
            var checkOut = Framework.Utilities.ConvertToDateTime(objectModel.StrParam2);


            var checkInSessinon = Session[Constant.SessionHotelCheckin];
            var checkOutSessinon = Session[Constant.SessionHotelCheckout];
            if (checkInSessinon != null && checkOutSessinon != null)
            {
                ViewBag.CheckIn = checkInSessinon;
                ViewBag.CheckOut = checkOutSessinon;
            }

            var rooms = GetRooms(objectModel.Id, checkIn, checkOut);
            return PartialView("_Room", rooms);
        }

        [Route("vinaday/api/get-gallery")]
        [HttpPost]
        public ActionResult GetGallery(ObjectModel objectModel)
        {
            var images =
             _imageService.GetImageListByHotelId(objectModel.Id)
                 .Where(i => i.PictureType == Constant.ImageRestaurant).ToList();

            return PartialView("_Gallery", images);
        }

        [Route("vinaday/api/get-similar-hotels")]
        [HttpGet]
        public ActionResult GetSimilarHotel(ObjectModel objectModel)
        {

            //var strCheckBooking = "check-in-date";
            //var checkIn = Session[Constant.SessionHotelCheckin];
            //var checkOut = Session[Constant.SessionHotelCheckout];
            //if (checkIn != null || checkOut != null)
            //{
            //    strCheckBooking = $"{Framework.Utilities.ConvertStringToDate(checkIn.ToString()).ToString("MM-dd-yyyy")}_{Framework.Utilities.ConvertStringToDate(checkOut.ToString()).ToString("MM-dd-yyyy")}";
            //}
            var hotelSimilars = _hotelService.GetSimilarLuxuryHotel(objectModel.IntParam1, objectModel.Id, (int)Framework.Utilities.Status.Active, 6);

            return PartialView("_SimilarHotel", hotelSimilars);


            //var hotelSimilars = _hotelService.GetSimilarHotel(objectModel.IntParam1, objectModel.Id, (int)Framework.Utilities.Status.Active, 6);

            //return PartialView("_SimilarHotel", hotelSimilars);
        }

        [Route("vinaday/api/get-hotels")]
        [HttpGet]
        public JsonResult GetHotels()
        {
            var hotels = _hotelService.GetHotels();
            var resutls = from s in hotels where s.Status == 2 && s.CollectionValue.HasValue && s.CollectionValue > 0 select new { name = s.HotelName, address = s.StreetAddressEnglish };
            var jsonResult = Json(resutls, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


        [HttpPost]
        [Route("send-inquiry-hotel")]
        public ActionResult SendInquiryHotel(HotelInquiryModel inquiryModel)
        {
            ObjectModel objectModel = new ObjectModel();
            try
            {
                objectModel.StrParam1 = String.Format("Check in date: {0} <br/>How many nights: {1}", inquiryModel.CheckInDate, inquiryModel.HowManyNight);
                objectModel.StrParam2 = String.Format("Select your budget:{0} <br/>Flights: {1} <br/>Hotel Name goreise.com: {2}", inquiryModel.Budget, inquiryModel.Flight, inquiryModel.HotelName); 
                objectModel.StrParam3 = String.Format("Adults:{0} - Children( 4-15): {1} - Infants(0-3):{2}", inquiryModel.Adult, inquiryModel.Childrent, inquiryModel.Infants);
                objectModel.StrParam4 = String.Format("FullName:{0}  {1} ", inquiryModel.Gender, inquiryModel.FullName);
                objectModel.StrParam5 = String.Format("Email:{0}<Br>Nationality:{1}<br>WhatsApp/ Phone Number :{2}", inquiryModel.Email, inquiryModel.Nationality, inquiryModel.Phone);
                objectModel.StrParam6 = String.Format("Special Request:{0} ", inquiryModel.SpecialRequest);
                objectModel.StrParam7 = "";
                string inquiry = Utilities.ParseTemplate("InquireHotel", objectModel);
                MailClient.Inquiry(inquiry, "Inquiry Booking Hotel " + inquiryModel.FullName + " " + inquiryModel.Nationality);
                objectModel.Status = (int)Utilities.Status.Active;
                objectModel.Message = "Your Inquiry has been sent";
            }
            catch (Exception ex)
            {
                objectModel.Status = (int)Utilities.Status.Inactive;
                objectModel.Message = ex.Message;
            }
            return Json(objectModel);
        }

        [Route("vinaday/api/get-promotion")]
        [HttpGet]
        public ActionResult GetPromotion(ObjectModel objectModel)
        {
            var promotions = _roomService.GetPromotionsList(objectModel.Id, 2);

            return PartialView("_Promotion", promotions);
        }
        [Route("vinaday/api/get-review")]
        [HttpGet]
        public ActionResult GetReview(ObjectModel objectModel)
        {
            var reviews = _hotelService.GetReviewsByHotelId(objectModel.Id, 2);

            return PartialView("_Review", reviews);
        }
        [Route("vinaday/api/send-request")]
        [HttpPost]
        public ActionResult SendRequest(RoomReguest roomReguest)
        {

            if (roomReguest == null) return Json(new { success = false, error = "Error when processing send request" }, JsonRequestBehavior.AllowGet);
            roomReguest.CreateDate = DateTime.Now;
            roomReguest.ObjectState = ObjectState.Added;
            _roomReguestService.Insert(roomReguest);
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Json(new { success = false, error = "Error when processing send request" }, JsonRequestBehavior.AllowGet);
            }
            SendEmailRequest(roomReguest);
            var resutls = new { success = true };
            var jsonResult = Json(resutls, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public bool SendEmailRequest(RoomReguest roomReguest)
        {
            //Send email request for vinaday team.
            try
            {
                dynamic myObject = new
                {
                    Datetime = DateTime.Now.ToString("D", CultureInfo.CreateSpecificCulture("en-US")),
                    Name = string.Format("{0} {1}", roomReguest.FirstName, roomReguest.LastName),
                    HotelName = roomReguest.HotelName,
                    CheckIn =
                        roomReguest.CheckIn != null
                            ? roomReguest.CheckIn.Value.ToShortDateString()
                            : DateTime.Now.ToShortDateString(),
                    CheckOut =
                        roomReguest.CheckOut != null
                            ? roomReguest.CheckOut.Value.ToShortDateString()
                            : DateTime.Now.AddDays(2.0).ToShortDateString(),
                    //RoomName = roomReguest.RoomName,
                    //RoomTotal = roomReguest.RoomTotal,
                    Email = roomReguest.Email,
                    Phone = roomReguest.Phone,
                    SpecialRequest = !string.IsNullOrEmpty(roomReguest.Note) ? roomReguest.Note : "No reguest",
                    RequestDate = DateTime.Now.ToString("G", CultureInfo.CreateSpecificCulture("en-US"))
                };
                string bookingEmailResult = Framework.Utilities.ParseTemplate("BookingHotelRequest", myObject);
                MailClient.RequestBookingHotel(bookingEmailResult, "VINADAY Request Booking");
            }
            catch (Exception ex)
            {
                //log error here
            }

            return true;
        }
        [Route("vinaday/api/get-hotel-featured")]
        [HttpGet]
        public ActionResult GetHotelFeatureds()
        {
            //var strCheckBooking = "check-in-date";
            //var checkIn = Session[Constant.SessionHotelCheckin];
            //var checkOut = Session[Constant.SessionHotelCheckout];
            //if (checkIn != null || checkOut != null)
            //{
            //    strCheckBooking = $"{Framework.Utilities.ConvertStringToDate(checkIn.ToString()).ToString("MM-dd-yyyy")}_{Framework.Utilities.ConvertStringToDate(checkOut.ToString()).ToString("MM-dd-yyyy")}";
            //}
            var hotelFeatureds = _hotelService.GetHotelFeatured();
            return PartialView("_HotelFeatured", hotelFeatureds);
        }
        [Route("vinaday/api/get-hotel-recently")]
        [HttpGet]
        public ActionResult GetHotelRecently()
        {
            //var strCheckBooking = "check-in-date";
            //var checkIn = Session[Constant.SessionHotelCheckin];
            //var checkOut = Session[Constant.SessionHotelCheckout];
            //if (checkIn != null || checkOut != null)
            //{
            //    strCheckBooking = $"{Framework.Utilities.ConvertStringToDate(checkIn.ToString()).ToString("MM-dd-yyyy")}_{Framework.Utilities.ConvertStringToDate(checkOut.ToString()).ToString("MM-dd-yyyy")}";
            //}
            var hotelRecently = _hotelService.HotelBookingRecently(8);
            return PartialView("_HotelRecently", hotelRecently);
        }
    }
}