using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;
using Vinaday.Data.Models;
using Vinaday.Data.Models.Extention;
using Vinaday.Services;
using Vinaday.Web.Framework;
using Utilities = Vinaday.Services.Utilities;
using AutoMapper;
using Vinaday.Admin.AppCode;

namespace Vinaday.Admin.Controllers
{
    //Admin
    // [Authorize(Roles = "Master Admin, Content Editor")]
    public class ProductController : Controller
    {
        private readonly IHotelService _hotelService;
        private readonly HotelPackageService _hotelPackageService;
        private readonly HotelPackageSurchargeService _hotelPackageSurchargeService;
        private readonly IImageService _imageService;
        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;
        private readonly ICatDetailService _catDetailService;
        private readonly ICategoryDetailService _categoryDetailService;
        private readonly ITourService _tourService;
        private readonly IRegionService _regionService;
        private readonly ICategoryService _categoryService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly ITourDetailService _tourDetailService;
        private readonly ISeoService _seoService;
        private readonly ICancellationService _cancellationService;
        private readonly IMediaService _mediaService;
        private readonly ITourRateService _tourRateService;
        private readonly ITourRateService2 _tourRateService2;
        private readonly ITourRateService3 _tourRateService3;
        private readonly IHotelCancellationService _hotelCancellationService;
        private readonly IRoomControlService _roomControlService;
        private readonly IRoomService _roomService;
        private readonly IReviewService _reviewService;
        private readonly ISurchargeService _surchargeService;
        private readonly INationalityService _nationalityService;
        private readonly IReviewDetailService _reviewDetailService;
        private readonly IPromotionService _promotionService;
        private readonly ITourPromotionService _tourPromotionService;
        private readonly ITourSurchargeService _tourSurchargeService;
        private readonly IFeaturedService _featuredService;
        private readonly ISpecialRateService _specialRateService;
        private readonly ITourExpandRateService _tourExpandRateService;
        private readonly CouponCodeService _couponCodeService;
        private readonly HotelCouponService _hotelCouponService;
        private readonly DealHotelToursService _dealHotelTourService;
        private readonly DealHotelToursVNService _dealHotelTourVnService;
        private readonly TourReviewService _tourReviewService;
        private readonly TourRateOptionsService _tourrateOptionsService;
        private readonly Hotel _hotel;
        public ProductController(IHotelService hotelService,
            ICountryService countryService,
            ICityService cityService,
            ICatDetailService catDetailService,
            ITourService tourService,
            TourReviewService tourReviewService,
            ICategoryDetailService categoryDetailService,
            IRegionService regionService,
            ICategoryService categoryService,
            IUnitOfWorkAsync unitOfWorkAsync,
            ITourDetailService tourDetailService,
            ISeoService seoService,
            ICancellationService cancellationService,
            IMediaService mediaService,
            ITourRateService tourRateService,
            ITourRateService2 tourRateService2,
            ITourRateService3 tourRateService3,
            IImageService imageService,
            IHotelCancellationService hotelCancellationService,
            IRoomControlService roomControlService,
            ISurchargeService surchargeService,
            ITourPromotionService tourPromotionService,
            ITourSurchargeService tourSurchargeService,
            IReviewService reviewService,
            INationalityService nationalityService,
            IReviewDetailService reviewDetailService,
            IRoomService roomService,
            IPromotionService promotionService,
            IFeaturedService featuredService,
            ISpecialRateService specialRateService,
            CouponCodeService couponCodeService,
            ITourExpandRateService tourExpandRateService,
            HotelPackageService hotelPackageService,
            HotelPackageSurchargeService hotelPackageSurchargeService,
            HotelCouponService hotelCouponService,
            DealHotelToursService dealHotelTourService,
            DealHotelToursVNService dealHotelTourVnService, TourRateOptionsService tourrateOptionsService

                )
        {
            _tourPromotionService = tourPromotionService;
            _tourSurchargeService = tourSurchargeService;
            _hotelService = hotelService;
            _countryService = countryService;
            _cityService = cityService;
            _catDetailService = catDetailService;
            _tourReviewService = tourReviewService;
            _tourService = tourService;
            _categoryDetailService = categoryDetailService;
            _regionService = regionService;
            _categoryService = categoryService;
            _unitOfWorkAsync = unitOfWorkAsync;
            _tourDetailService = tourDetailService;
            _seoService = seoService;
            _cancellationService = cancellationService;
            _mediaService = mediaService;
            _tourRateService = tourRateService;
            _tourRateService2 = tourRateService2;
            _tourRateService3 = tourRateService3;
            _imageService = imageService;
            _hotelCancellationService = hotelCancellationService;
            _roomControlService = roomControlService;
            _surchargeService = surchargeService;
            _reviewService = reviewService;
            _nationalityService = nationalityService;
            _reviewDetailService = reviewDetailService;
            _roomService = roomService;
            _promotionService = promotionService;
            _featuredService = featuredService;
            _specialRateService = specialRateService;
            _couponCodeService = couponCodeService;
            _tourExpandRateService = tourExpandRateService;
            _hotelPackageService = hotelPackageService;
            _hotelPackageSurchargeService = hotelPackageSurchargeService;
            _hotelCouponService = hotelCouponService;
            _dealHotelTourService = dealHotelTourService;
            _dealHotelTourVnService = dealHotelTourVnService;
            _tourrateOptionsService = tourrateOptionsService;
        }

        //protected override void HandleUnknownAction(string actionName)
        //{
        //    this.View(actionName).ExecuteResult(this.ControllerContext);
        //    //base.HandleUnknownAction(actionName);
        //}

        #region Deal Hotel Tour
        public ActionResult DealTourHotel()
        {
            var coupons = _dealHotelTourService.Queryable().ToList();
            ViewBag.HotelList = _hotelService.GetHotels().ToList();
            ViewBag.TourList = _tourService.GetTours().ToList();
            ViewBag.City = _cityService.GetCities().OrderBy(a => a.CityId).ToList();

            return View(coupons);
        }

        public ActionResult DealTourHotelAdd()
        {
            var coupons = _dealHotelTourService.Query().Select();

            return View(coupons);
        }

        [HttpPost]
        public ActionResult InsertDealTourHotel(DealHotelTours dealHotelTours)
        {
            var objectModel = new ObjectModel();
            var DealAvarta = "DealAvarta";
            var DealBanner = "DealBanner";
            var fileDealAvarta = Request.Files[DealAvarta];
            var fileDealBanner = Request.Files[DealBanner];
            if (fileDealAvarta != null && fileDealAvarta.FileName.Length > 0)
            {
                var systemLocation = "";
                string importPath = Path.Combine(ConstantFiles.DealHotelTours);
                var fileName = CommonFunction.GetNewFileName(importPath, Path.GetFileName(fileDealAvarta.FileName));
                if (fileName != null)
                {
                    systemLocation = Path.Combine(importPath, fileName);
                    importPath = Request.MapPath(importPath);
                    var physicalpath = Request.MapPath(systemLocation);
                    if (!Directory.Exists(importPath))
                    {
                        Directory.CreateDirectory(importPath);
                    }
                    fileDealAvarta.SaveAs(physicalpath);
                }
                if (!string.IsNullOrEmpty(systemLocation))
                {
                    dealHotelTours.DealAvarta = systemLocation;
                }
            }
            if (fileDealBanner != null && fileDealBanner.FileName.Length > 0)
            {
                var systemLocation = "";
                string importPath = Path.Combine(ConstantFiles.DealHotelTours);
                var fileName = CommonFunction.GetNewFileName(importPath, Path.GetFileName(fileDealBanner.FileName));
                if (fileName != null)
                {
                    systemLocation = Path.Combine(importPath, fileName);
                    importPath = Request.MapPath(importPath);
                    var physicalpath = Request.MapPath(systemLocation);
                    if (!Directory.Exists(importPath))
                    {
                        Directory.CreateDirectory(importPath);
                    }
                    fileDealBanner.SaveAs(physicalpath);
                }
                if (!string.IsNullOrEmpty(systemLocation))
                {
                    dealHotelTours.DealBanner = systemLocation;
                }
            }
            if (dealHotelTours == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Add promotion is error!";
                return Json(objectModel);
            }
            dealHotelTours.ObjectState = ObjectState.Added;
            _dealHotelTourService.Insert(dealHotelTours);

            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Deal Hotel Tours is added!");
            }
            catch (Exception)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Add Deal Hotel Tours is error!");
            }

            return Redirect("DealTourHotel");
        }

        [HttpPost]
        public ActionResult UpdateDealTourHotel(DealHotelTours dealHotelTours)
        {
            var objectModel = new ObjectModel();
            var DealAvarta = "DealAvarta";
            var DealBanner = "DealBanner";
            var fileDealAvarta = Request.Files[DealAvarta];
            var fileDealBanner = Request.Files[DealBanner];
            var dealDetail = _dealHotelTourService.Queryable().AsNoTracking().First(a => a.Id == dealHotelTours.Id);
            if (dealDetail != null)
            {
                dealHotelTours.DealAvarta = dealDetail.DealAvarta;
                dealHotelTours.DealBanner = dealDetail.DealBanner;
                if (fileDealAvarta != null && fileDealAvarta.FileName.Length > 0)
                {
                    var systemLocation = "";
                    string importPath = Path.Combine(ConstantFiles.DealHotelTours);
                    var fileName = CommonFunction.GetNewFileName(importPath, Path.GetFileName(fileDealAvarta.FileName));
                    if (fileName != null)
                    {
                        systemLocation = Path.Combine(importPath, fileName);
                        importPath = Request.MapPath(importPath);
                        var physicalpath = Request.MapPath(systemLocation);
                        if (!Directory.Exists(importPath))
                        {
                            Directory.CreateDirectory(importPath);
                        }
                        fileDealAvarta.SaveAs(physicalpath);
                    }
                    if (!string.IsNullOrEmpty(systemLocation))
                    {
                        dealHotelTours.DealAvarta = systemLocation;
                    }
                }
                if (fileDealBanner != null && fileDealBanner.FileName.Length > 0)
                {
                    var systemLocation = "";
                    string importPath = Path.Combine(ConstantFiles.DealHotelTours);
                    var fileName = CommonFunction.GetNewFileName(importPath, Path.GetFileName(fileDealBanner.FileName));
                    if (fileName != null)
                    {
                        systemLocation = Path.Combine(importPath, fileName);
                        importPath = Request.MapPath(importPath);
                        var physicalpath = Request.MapPath(systemLocation);
                        if (!Directory.Exists(importPath))
                        {
                            Directory.CreateDirectory(importPath);
                        }
                        fileDealBanner.SaveAs(physicalpath);
                    }
                    if (!string.IsNullOrEmpty(systemLocation))
                    {
                        dealHotelTours.DealBanner = systemLocation;
                    }
                }
                if (dealHotelTours == null)
                {
                    objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                    objectModel.Message = "Add promotion is error!";
                    return Json(objectModel);
                }
                dealHotelTours.ObjectState = ObjectState.Modified;
                _dealHotelTourService.Update(dealHotelTours);

                try
                {
                    _unitOfWorkAsync.SaveChanges();
                    objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                    objectModel.Message = string.Format("Deal Hotel Tours is updated!");
                }
                catch (Exception)
                {
                    objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                    objectModel.Message = string.Format("Add Deal Hotel Tours is error!");
                }
            }


            return Redirect("DealTourHotel");
        }


        [HttpPost]
        public ActionResult DeleteDealTourHotel(DealHotelTours couponCode)
        {
            var deleteItem = _dealHotelTourService.Queryable().FirstOrDefault(a => a.Id == couponCode.Id);
            var objectModel = new ObjectModel();
            if (deleteItem == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Delete Item is error!";
                return Json(objectModel);
            }
            _dealHotelTourService.Delete(deleteItem);

            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Coupon is Deleted!");
            }
            catch (Exception)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Add Coupon is error!");
            }

            return Json(objectModel);
        }

        [HttpPost]
        public ActionResult GetDealTourHotel(ObjectModel objectModel)
        {

            var rooms = _dealHotelTourService.Queryable().FirstOrDefault(a => a.Id == objectModel.Id);
            return PartialView("_DealTourHotel", rooms);
        }

        #endregion

        #region Deal Tour Services VN

        public ActionResult DealTourHotelVN()
        {
            var coupons = _dealHotelTourVnService.Queryable().ToList();
            ViewBag.HotelList = _hotelService.GetHotels().ToList();
            ViewBag.TourList = _tourService.GetTours().ToList();
            ViewBag.City = _cityService.GetCities().OrderBy(a => a.CityId).ToList();

            return View(coupons);
        }

        public ActionResult DealTourHotelVNAdd()
        {
            var coupons = _dealHotelTourVnService.Query().Select();

            return View(coupons);
        }
        [HttpPost]
        public ActionResult InsertDealTourHotelVN(DealHotelToursVN dealHotelTours)
        {
            var objectModel = new ObjectModel();
            var DealAvarta = "DealAvarta";
            var DealBanner = "DealBanner";
            var fileDealAvarta = Request.Files[DealAvarta];
            var fileDealBanner = Request.Files[DealBanner];
            if (fileDealAvarta != null && fileDealAvarta.FileName.Length > 0)
            {
                var systemLocation = "";
                string importPath = Path.Combine(ConstantFiles.DealHotelTours);
                var fileName = CommonFunction.GetNewFileName(importPath, Path.GetFileName(fileDealAvarta.FileName));
                if (fileName != null)
                {
                    systemLocation = Path.Combine(importPath, fileName);
                    importPath = Request.MapPath(importPath);
                    var physicalpath = Request.MapPath(systemLocation);
                    if (!Directory.Exists(importPath))
                    {
                        Directory.CreateDirectory(importPath);
                    }
                    fileDealAvarta.SaveAs(physicalpath);
                }
                if (!string.IsNullOrEmpty(systemLocation))
                {
                    dealHotelTours.DealAvarta = systemLocation;
                }
            }
            if (fileDealBanner != null && fileDealBanner.FileName.Length > 0)
            {
                var systemLocation = "";
                string importPath = Path.Combine(ConstantFiles.DealHotelTours);
                var fileName = CommonFunction.GetNewFileName(importPath, Path.GetFileName(fileDealBanner.FileName));
                if (fileName != null)
                {
                    systemLocation = Path.Combine(importPath, fileName);
                    importPath = Request.MapPath(importPath);
                    var physicalpath = Request.MapPath(systemLocation);
                    if (!Directory.Exists(importPath))
                    {
                        Directory.CreateDirectory(importPath);
                    }
                    fileDealBanner.SaveAs(physicalpath);
                }
                if (!string.IsNullOrEmpty(systemLocation))
                {
                    dealHotelTours.DealBanner = systemLocation;
                }
            }
            if (dealHotelTours == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Add promotion is error!";
                return Json(objectModel);
            }
            dealHotelTours.ObjectState = ObjectState.Added;
            _dealHotelTourVnService.Insert(dealHotelTours);

            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Deal Hotel Tours is added!");
            }
            catch (Exception)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Add Deal Hotel Tours is error!");
            }

            return Redirect("DealTourHotelVN");
        }

        [HttpPost]
        public ActionResult UpdateDealTourHotelVN(DealHotelToursVN dealHotelTours)
        {
            var objectModel = new ObjectModel();
            var DealAvarta = "DealAvarta";
            var DealBanner = "DealBanner";
            var fileDealAvarta = Request.Files[DealAvarta];
            var fileDealBanner = Request.Files[DealBanner];
            var dealDetail = _dealHotelTourVnService.Queryable().AsNoTracking().First(a => a.Id == dealHotelTours.Id);
            if (dealDetail != null)
            {
                dealHotelTours.DealAvarta = dealDetail.DealAvarta;
                dealHotelTours.DealBanner = dealDetail.DealBanner;
                if (fileDealAvarta != null && fileDealAvarta.FileName.Length > 0)
                {
                    var systemLocation = "";
                    string importPath = Path.Combine(ConstantFiles.DealHotelTours);
                    var fileName = CommonFunction.GetNewFileName(importPath, Path.GetFileName(fileDealAvarta.FileName));
                    if (fileName != null)
                    {
                        systemLocation = Path.Combine(importPath, fileName);
                        importPath = Request.MapPath(importPath);
                        var physicalpath = Request.MapPath(systemLocation);
                        if (!Directory.Exists(importPath))
                        {
                            Directory.CreateDirectory(importPath);
                        }
                        fileDealAvarta.SaveAs(physicalpath);
                    }
                    if (!string.IsNullOrEmpty(systemLocation))
                    {
                        dealHotelTours.DealAvarta = systemLocation;
                    }
                }
                if (fileDealBanner != null && fileDealBanner.FileName.Length > 0)
                {
                    var systemLocation = "";
                    string importPath = Path.Combine(ConstantFiles.DealHotelTours);
                    var fileName = CommonFunction.GetNewFileName(importPath, Path.GetFileName(fileDealBanner.FileName));
                    if (fileName != null)
                    {
                        systemLocation = Path.Combine(importPath, fileName);
                        importPath = Request.MapPath(importPath);
                        var physicalpath = Request.MapPath(systemLocation);
                        if (!Directory.Exists(importPath))
                        {
                            Directory.CreateDirectory(importPath);
                        }
                        fileDealBanner.SaveAs(physicalpath);
                    }
                    if (!string.IsNullOrEmpty(systemLocation))
                    {
                        dealHotelTours.DealBanner = systemLocation;
                    }
                }
                if (dealHotelTours == null)
                {
                    objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                    objectModel.Message = "Add promotion is error!";
                    return Json(objectModel);
                }
                dealHotelTours.ObjectState = ObjectState.Modified;
                _dealHotelTourVnService.Update(dealHotelTours);

                try
                {
                    _unitOfWorkAsync.SaveChanges();
                    objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                    objectModel.Message = string.Format("Deal Hotel Tours is updated!");
                }
                catch (Exception)
                {
                    objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                    objectModel.Message = string.Format("Add Deal Hotel Tours is error!");
                }
            }


            return Redirect("DealTourHotelVN");
        }


        [HttpPost]
        public ActionResult DeleteDealTourHotelVN(DealHotelToursVN couponCode)
        {
            var deleteItem = _dealHotelTourVnService.Queryable().FirstOrDefault(a => a.Id == couponCode.Id);
            var objectModel = new ObjectModel();
            if (deleteItem == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Delete Item is error!";
                return Json(objectModel);
            }
            _dealHotelTourVnService.Delete(deleteItem);

            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Coupon is Deleted!");
            }
            catch (Exception)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Add Coupon is error!");
            }

            return Json(objectModel);
        }

        [HttpPost]
        public ActionResult GetDealTourHotelVN(ObjectModel objectModel)
        {

            var rooms = _dealHotelTourVnService.Queryable().FirstOrDefault(a => a.Id == objectModel.Id);
            return PartialView("_DealTourHotelVN", rooms);
        }
        #endregion

        #region CouponCode

        public ActionResult CouponCode()
        {
            var coupons = _couponCodeService.Queryable().AsEnumerable<CouponCode>();
            //using to bind to coupon adding page
            ViewBag.HotelList = _hotelService.GetHotels().ToList();
            ViewBag.TourList = _tourService.GetTours().ToList();
            ViewBag.City = _cityService.GetCities().OrderBy(a => a.CityId).ToList();

            return View(coupons);
        }

        public ActionResult CouponCodeAdd()
        {
            var coupons = _couponCodeService.Query().Select();

            return View(coupons);
        }
        [HttpPost]
        public ActionResult InsertCouponCode(CouponCode couponCode)
        {
            var objectModel = new ObjectModel();
            if (couponCode == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Add promotion is error!";
                return Json(objectModel);
            }
            couponCode.CreatedDate = DateTime.Now;
            couponCode.ObjectState = ObjectState.Added;
            couponCode.Code = Web.Framework.Utilities.GetRandomString(7).ToUpper();
            _couponCodeService.Insert(couponCode);

            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Coupon is added!");
            }
            catch (Exception)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Add Coupon is error!");
            }

            return Json(objectModel);
        }

        [HttpPost]
        public ActionResult DeleteCoupon(CouponCode couponCode)
        {
            var deleteItem = _couponCodeService.Queryable().FirstOrDefault(a => a.Id == couponCode.Id);
            var objectModel = new ObjectModel();
            if (deleteItem == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Delete Item is error!";
                return Json(objectModel);
            }
            _couponCodeService.Delete(deleteItem);

            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Coupon is Deleted!");
            }
            catch (Exception)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Add Coupon is error!");
            }

            return Json(objectModel);
        }

        #endregion

        #region Hotel Coupon Code

        public ActionResult HotelCouponCode()
        {
            var list = _hotelCouponService.Queryable().ToList();
            ViewBag.HotelList = _hotelService.GetHotels().ToList();
            return View(list);
        }

        [HttpPost]
        public ActionResult InsertHotelCouponCode(HotelCoupon couponCode)
        {
            var objectModel = new ObjectModel();
            if (couponCode == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Add promotion is error!";
                return Json(objectModel);
            }
            couponCode.ObjectState = ObjectState.Added;
            var hotel = _hotelService.GetHotelSingle(couponCode.HotelId);
            if (hotel?.StartRating != null) couponCode.HotelStart = hotel.StartRating.Value;
            couponCode.CodePromo = Web.Framework.Utilities.GetRandomString(7).ToUpper();
            _hotelCouponService.Insert(couponCode);

            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Coupon is added!");
            }
            catch (Exception)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Add Coupon is error!");
            }

            return Json(objectModel);
        }


        [HttpPost]
        public ActionResult UpdateHotelCouponCode(HotelCoupon couponCode)
        {
            var objectModel = new ObjectModel();
            if (couponCode == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Add promotion is error!";
                return Json(objectModel);
            }
            try
            {
                var hotelCoupon = _hotelCouponService.Queryable().FirstOrDefault(a => a.Id == couponCode.Id);
                if (hotelCoupon != null)
                {
                    hotelCoupon.ObjectState = ObjectState.Modified;
                    hotelCoupon.Price = couponCode.Price;
                    hotelCoupon.ConditionUsing = couponCode.ConditionUsing;
                    hotelCoupon.Description = couponCode.Description;
                    hotelCoupon.Name = couponCode.Name;
                    hotelCoupon.Discount = couponCode.Discount;
                    hotelCoupon.StartDate = couponCode.StartDate;
                    hotelCoupon.EndDate = couponCode.EndDate;
                    hotelCoupon.SalesQty = couponCode.SalesQty;
                    hotelCoupon.TotalQty = couponCode.TotalQty;
                    hotelCoupon.HotelId = couponCode.HotelId;
                    var hotel = _hotelService.GetHotelSingle(couponCode.HotelId);
                    if (hotel?.StartRating != null) hotelCoupon.HotelStart = hotel.StartRating.Value;
                    _hotelCouponService.Update(hotelCoupon);
                }

                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Coupon is added!");
            }
            catch (Exception ex)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Add Coupon is error!");
            }

            return Json(objectModel);
        }


        [HttpPost]
        public ActionResult DeleteHotelCoupon(HotelCoupon couponCode)
        {
            var deleteItem = _hotelCouponService.Queryable().FirstOrDefault(a => a.Id == couponCode.Id);
            var objectModel = new ObjectModel();
            if (deleteItem == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Delete Item is error!";
                return Json(objectModel);
            }
            _hotelCouponService.Delete(deleteItem);


            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Delete Hotel Coupon successful!");
            }
            catch (Exception)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Delete Hotel Coupon is error!");
            }

            return Json(objectModel);
        }

        [HttpPost]
        public ActionResult GetHotelCoupon(ObjectModel objectModel)
        {

            var rooms = _hotelCouponService.Queryable().FirstOrDefault(a => a.Id == objectModel.Id);
            ViewBag.HotelList = _hotelService.GetHotels().ToList();
            return PartialView("_HotelCoupon", rooms);
        }

        #endregion

        #region Other Tag
        public ActionResult Hotel()
        {
            var hotels = _hotelService.GetHotels();
            return View(hotels);
        }

        public JsonResult GetHotel()
        {
            var hotels = _hotelService.GetHotels();
            var hotelModels = new List<HotelModel>();
            foreach (Hotel hotel in hotels)
            {
                var hotelModel = new HotelModel();
                Mapper.Map(hotel, hotelModel, typeof(Hotel), typeof(HotelModel));
                hotelModels.Add(hotelModel);
            }
            var jsonResult = Json(hotelModels, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        public ActionResult Tour()
        {
            var tours = _tourService.GetTours();
            return View(tours);
        }

        public ActionResult TourBasic(int? id)
        {
            ViewBag.Regions = _regionService.GetRegionList().ToList();
            ViewBag.Countries = _countryService.GetCountryList().ToList();
            var tour = _tourService.Queryable().FirstOrDefault(t => t.Id == id);

            var category = _categoryService.GetCategories().FirstOrDefault(c => c.KeyCode == Web.Framework.Utilities.TravelStyle);
            if (category == null) return View();
            var categoryStr = tour != null ? tour.TravelStyle : string.Empty;
            ViewBag.TravelStyles = Web.Framework.Utilities.GetCategoryList(categoryStr, _categoryDetailService.GetCategoriesDetail(category.Id));

            var cities = _cityService.GetCities();
            var citiStr = string.Empty;
            if (tour != null)
            {
                citiStr = tour.Cities;
            }
            ViewBag.Cities = Web.Framework.Utilities.GetCityList(citiStr, cities);
            return View(tour);

        }
        [HttpPost]
        public ActionResult GetTourBasic(ObjectModel objectModel)
        {
            //ViewBag.PartialRegions = _regionService.GetRegionList().ToList();
            ViewBag.PartialCountries = _countryService.GetCountryList().ToList();
            var tour = _tourService.Queryable().FirstOrDefault(t => t.ParentId == objectModel.Id && t.Language == objectModel.IntParam1);

            var category = _categoryService.GetCategories().FirstOrDefault(c => c.KeyCode == Web.Framework.Utilities.TravelStyle);
            if (category == null) return PartialView("_TourBasicForm", new Tour());
            var categoryStr = tour != null ? tour.TravelStyle : string.Empty;
            ViewBag.PartialTravelStyles = Web.Framework.Utilities.GetCategoryList(categoryStr, _categoryDetailService.GetCategoriesDetail(category.Id));

            var cities = _cityService.GetCities();
            var citiStr = string.Empty;
            if (tour != null)
            {
                citiStr = tour.Cities;
            }
            ViewBag.PartialCities = new List<ItemModel>();
            ViewBag.PartialCities = Web.Framework.Utilities.GetCityList(citiStr, cities);
            return PartialView("_TourBasicForm", tour ?? new Tour());
        }
        [HttpPost]
        public ActionResult GetTourDetail(ObjectModel objectModel)
        {
            var tour = _tourService.Queryable().FirstOrDefault(t => t.ParentId == objectModel.Id && t.Language == objectModel.IntParam1);
            ViewBag.Cancellations = _cancellationService.GetCancellationList();

            var category = _categoryService.GetCategories().FirstOrDefault(c => c.KeyCode == Web.Framework.Utilities.GroupSize);
            if (category != null)
            {
                ViewBag.GroupSizes = Web.Framework.Utilities.GetCategoryList(null,
                    _categoryDetailService.GetCategoriesDetail(category.Id));
            }
            var categoryDuration = _categoryService.GetCategories().FirstOrDefault(c => c.KeyCode == Web.Framework.Utilities.TourDuration);
            if (categoryDuration == null) return PartialView("_TourDetailForm", new Tour());
            ViewBag.Durations = Web.Framework.Utilities.GetCategoryList(null,
                _categoryDetailService.GetCategoriesDetail(categoryDuration.Id));
            return PartialView("_TourDetailForm", tour ?? new Tour());
        }
        [HttpPost]
        public ActionResult UpdateTourStep1(Tour tour)
        {
            var objectModel = new ObjectModel();
            if (tour == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Updated tour is error!";
                return Json(objectModel);
            }
            var tourUpdate = _tourService.Queryable().FirstOrDefault(t => t.Id == tour.Id);
            //Added comlunm
            if (tourUpdate != null)
            {
                if (string.IsNullOrEmpty(tour.TourTitle))
                {
                    tour.TourTitle = tour.Name;
                }
                tourUpdate.ModifiedDate = DateTime.Now;
                tourUpdate.Name = tour.Name;
                tourUpdate.Type = tour.Type;
                tourUpdate.CountryId = tour.CountryId;
                tourUpdate.Description = tour.Description;
                tourUpdate.Location = tour.Location;
                tourUpdate.TravelStyle = tour.TravelStyle;
                tourUpdate.Overview = tour.Overview;
                tourUpdate.VideoLink = tour.VideoLink;
                tourUpdate.SEO_Meta = tour.SEO_Meta;
                tourUpdate.SEO_Description = tour.SEO_Description;
                tourUpdate.SEO_Title = tour.SEO_Title;
                tourUpdate.TourTitle = tour.TourTitle;
                tourUpdate.Cities = tour.Cities;
                tourUpdate.SearchKey = tour.SearchKey;
                //Update tour
                tourUpdate.ObjectState = ObjectState.Modified;
                _tourService.Update(tourUpdate);
            }
            else
            {
                //Add tour
                tour.CreatedDate = DateTime.Now;
                tour.ModifiedDate = DateTime.Now;
                tour.ObjectState = ObjectState.Added;
                tour.CommissionRate = 15;
                _tourService.Insert(tour);
            }

            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Url = string.Format("~/Product/TourDetail/{0}", tour.Id);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(tour.Id)) throw;
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Update tour is error!");
                throw;
            }
            return Json(objectModel);
        }

        [HttpPost]
        public ActionResult UpdateTourStep2(Tour tour)
        {
            var objectModel = new ObjectModel();
            if (tour == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Updated tour is error!");
                return Json(objectModel);
            }
            var tourUpdate = _tourService.Queryable().FirstOrDefault(t => t.Id == tour.Id);
            //Added comlunm
            if (tourUpdate != null)
            {
                tourUpdate.ModifiedDate = DateTime.Now;
                tourUpdate.Start = tour.Start;
                tourUpdate.Finish = tour.Finish;
                tourUpdate.GroupSize = tour.GroupSize;
                tourUpdate.IncludeActivity = tour.IncludeActivity;
                tourUpdate.Meals = tour.Meals;
                tourUpdate.Accommondation = tour.Accommondation;
                tourUpdate.ExcludeActivity = tour.ExcludeActivity;
                tourUpdate.Transport = tour.Transport;
                tourUpdate.DepartureOption1 = tour.DepartureOption1;
                tourUpdate.DepartureOption2 = tour.DepartureOption2;
                tourUpdate.DepartureOption3 = tour.DepartureOption3;
                tourUpdate.TourGroup1 = tour.TourGroup1;
                tourUpdate.TourGroup2 = tour.TourGroup2;
                tourUpdate.TourGroup3 = tour.TourGroup3;
                tourUpdate.TourGroup1Include = tour.TourGroup1Include;
                tourUpdate.TourGroup2Include = tour.TourGroup2Include;
                tourUpdate.TourGroup3Include = tour.TourGroup3Include;
                tourUpdate.Duration = tour.Duration;
                tourUpdate.Notes = tour.Notes;
                if (!string.IsNullOrEmpty(tour.TourTitle))
                {
                    tourUpdate.TourTitle = tour.TourTitle;
                }

                tourUpdate.CancelationPolicy = tour.CancelationPolicy;
                //Update tour
                tourUpdate.ObjectState = ObjectState.Modified;
                _tourService.Update(tourUpdate);
            }
            else
            {
                //Add tour
                tour.CreatedDate = DateTime.Now;
                tour.ModifiedDate = DateTime.Now;
                tour.ObjectState = ObjectState.Added;
                _tourService.Insert(tour);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Url = string.Format("~/Product/TourPhoto/{0}", tour.Id);

            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(tour.Id)) throw;
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Update tour is error!");
                throw;
            }
            return Json(objectModel);
        }

        [HttpPost]
        public ActionResult InsertTourStep1(Tour tour)
        {
            var objectModel = new ObjectModel();
            //Added comlunm

            tour.CreatedDate = DateTime.Now;
            tour.ModifiedDate = DateTime.Now;
            tour.OperatorId = 1;
            tour.Status = (int)Web.Framework.Utilities.Status.Active;
            tour.CommissionRate = 15;
            tour.TourCode = Web.Framework.Utilities.GetRandomString(7);
            tour.SearchKey = tour.Name;
            //Inser tour
            tour.ObjectState = ObjectState.Added;
            _tourService.Add(tour);
            if (tour.Type == (int)Web.Framework.Utilities.TourType.Daytrip)
            {
                var detail = new Detail
                {
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    Status = (int)Web.Framework.Utilities.Status.Active,
                    TourId = tour.Id,
                    ObjectState = ObjectState.Added
                };
                _tourDetailService.Insert(detail);
            }
            //Add seo
            var country = _countryService.GetCountryList().FirstOrDefault(c => c.CountryId == tour.CountryId);
            var seo = new Seo
            {
                EntityId = tour.Id,
                EntityName = Enum.GetName(typeof(Web.Framework.Utilities.TourType), tour.Type),
                ProductType = tour.Type,
                Slug = Web.Framework.Utilities.GenerateSlug(tour.Name, 200),
                IsActive = true,
                Title = $"{tour.Name} ({(country != null ? country.Name : string.Empty)}) | vinaday.com",
                Description = tour.Description,
                Keyword = tour.Name,
                ObjectState = ObjectState.Added
            };
            _seoService.Insert(seo);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Url = $"/Product/TourDetail/{tour.Id}";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(tour.Id)) throw;
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Insert tour is error!");
                throw;
            }
            return Json(objectModel);
        }

        [HttpPost]
        public ActionResult DeleteTour(ObjectModel objectModel)
        {
            //delete all tour Itinerary
            var itineraries = _tourDetailService.GetDetailToursByTourId(objectModel.Id);

            if (itineraries.Count > 0)
            {
                foreach (var itinerary in itineraries.Where(itinerary => itinerary != null))
                {
                    itinerary.ObjectState = ObjectState.Deleted;
                    //   _tourDetailService.Delete(itinerary);
                }

            }
            //delete all rate
            var rates = _tourRateService.GetTourRatesByTourId(objectModel.Id);
            if (rates.Count > 0)
            {
                foreach (var rate in rates.Where(rate => rate != null))
                {
                    rate.ObjectState = ObjectState.Deleted;
                    //  _tourRateService.Delete(rate);
                }

            }
            //delete SEO object
            var tourSeo = _seoService.GetSeoEntityId(objectModel.Id);
            if (tourSeo != null)
            {
                //tourSeo.ObjectState = ObjectState.Deleted;
                //_seoService.Delete(tourSeo);

            }
            //delete tour featured
            var featured = _featuredService.GetFeatured(objectModel.Id);
            if (featured != null)
            {
                featured.ObjectState = ObjectState.Modified;
                var firstOrDefault = _tourService.GetTourOrtherId(objectModel.Id);
                if (firstOrDefault != null)
                    featured.TourId = firstOrDefault.Id;
                _featuredService.Update(featured);
            }
            //delete tour
            var tour = _tourService.GetTourById(objectModel.Id);
            if (tour != null)
            {
                tour.Status = (int)Web.Framework.Utilities.Status.Delete;
                _tourService.Delete(tour);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = "Delete this record is successfully!";

            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Delete is error!";
                throw;
            }
            return Json(objectModel);
        }

        private bool CustomerExists(int id)
        {
            return _tourService.Query(e => e.Id == id).Select().Any();
        }

        public ActionResult TourDetail(int id)
        {
            var tour = _tourService.Queryable().FirstOrDefault(t => t.Id == id);
            ViewBag.Cancellations = _cancellationService.GetCancellationList();

            var category = _categoryService.GetCategories().FirstOrDefault(c => c.KeyCode == Web.Framework.Utilities.GroupSize);
            if (category != null)
            {
                ViewBag.GroupSizes = Web.Framework.Utilities.GetCategoryList(null,
                    _categoryDetailService.GetCategoriesDetail(category.Id));
            }
            var categoryDuration = _categoryService.GetCategories().FirstOrDefault(c => c.KeyCode == Web.Framework.Utilities.TourDuration);
            if (categoryDuration == null) return View();

            ViewBag.Durations = Web.Framework.Utilities.GetCategoryList(null,
                _categoryDetailService.GetCategoriesDetail(categoryDuration.Id));
            return View(tour);
        }

        public ActionResult TourPhoto(int id)
        {
            ViewBag.TourId = id;
            var images = _mediaService.Queryable().Where(t => t.OwnerId == id).ToList();
            //var tour = _tourService.Queryable().FirstOrDefault(t => t.Id == id);
            return View(images);
        }

        [HttpPost]
        public ActionResult DeleteImages(int[] imgId)
        {
            var objectModel = new ObjectModel();

            foreach (var image in imgId.Select(id => _mediaService.GetMediaById(id)).Where(image => image != null))
            {
                //DeleteImage(image);//Delete current path
                image.ObjectState = ObjectState.Deleted;
                _mediaService.Delete(image);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = "Delete all record is successfully!";

            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Delete all record is not successfully!";
                throw;
            }
            return Json(objectModel);
        }
        [HttpPost]
        public ActionResult UpdateImages(List<ObjectModel> objectModels)
        {
            var objectResult = new ObjectModel();
            foreach (var objectModel in objectModels)
            {
                var media = _mediaService.GetMediaById(objectModel.Id);
                if (media == null) continue;
                media.MediaType = objectModel.IntParam1;

                media.ModifiedDate = DateTime.Now;
                media.Title = objectModel.StrParam1;
                media.ObjectState = ObjectState.Modified;
                _mediaService.Update(media);

            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectResult.Status = (int)Web.Framework.Utilities.Status.Active;
                objectResult.Message = "Tour images is updated!";
            }
            catch (Exception)
            {
                objectResult.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectResult.Message = "Update images is error!";
            }
            return Json(objectResult);
        }
        public ActionResult TourItinerary(int id)
        {
            var tourDetails = new List<Detail>();
            Tour tour = ViewBag.Tour = _tourService.Queryable().FirstOrDefault(t => t.Id == id);
            var categoryMeals = _categoryService.GetCategories().FirstOrDefault(c => c.KeyCode == Web.Framework.Utilities.Meals);
            var categoryTransports = _categoryService.GetCategories().FirstOrDefault(c => c.KeyCode == Web.Framework.Utilities.Transportations);

            var categoryMealStr = string.Empty;
            var categoryTransportStr = string.Empty;
            tourDetails = GetTourDetailsByTourId(tour.Id);


            if (categoryMeals == null) return View();
            ViewBag.Meals = Web.Framework.Utilities.GetCategoryList(categoryMealStr,
                _categoryDetailService.GetCategoriesDetail(categoryMeals.Id));


            if (categoryTransports == null) return View();
            ViewBag.Transports = Web.Framework.Utilities.GetCategoryList(categoryTransportStr,
                _categoryDetailService.GetCategoriesDetail(categoryTransports.Id));

            return View(tourDetails);
        }
        [HttpPost]
        public ActionResult GetTourItinerary(ObjectModel objectModel)
        {
            var tourDetails = new List<Detail>();
            var tour = ViewBag.Tour = _tourService.Queryable().FirstOrDefault(t => t.ParentId == objectModel.Id && t.Language == objectModel.IntParam1);
            var categoryMeals = _categoryService.GetCategories().FirstOrDefault(c => c.KeyCode == Web.Framework.Utilities.Meals);
            var categoryTransports = _categoryService.GetCategories().FirstOrDefault(c => c.KeyCode == Web.Framework.Utilities.Transportations);

            var categoryMealStr = string.Empty;
            var categoryTransportStr = string.Empty;
            if (tour != null) tourDetails = GetTourDetailsByTourId(tour.Id);

            if (categoryMeals == null) return PartialView("_TourItineraryForm", new List<Detail>());
            ViewBag.Meals = Web.Framework.Utilities.GetCategoryList(categoryMealStr,
                _categoryDetailService.GetCategoriesDetail(categoryMeals.Id));
            if (categoryTransports == null) return PartialView("_TourItineraryForm", new List<Detail>());
            ViewBag.Transports = Web.Framework.Utilities.GetCategoryList(categoryTransportStr,
                _categoryDetailService.GetCategoriesDetail(categoryTransports.Id));

            return PartialView("_TourItineraryForm", tourDetails.Count > 0 ? tourDetails : new List<Detail>());
        }
        public ActionResult TourSurcharge(int id)
        {
            var tour = _tourService.Queryable().FirstOrDefault(t => t.Id == id);
            ViewBag.Category = _categoryService.GetCategoryById(7);
            ViewBag.SpecialRates = _specialRateService.GetSpecialRates(id, (int)Utilities.SpecialRateType.Surcharge);
            ViewBag.TourSurcharges = _tourSurchargeService.GetSurchargesByTourId(id);
            ViewBag.Surcharges = _catDetailService.GetCategoriesDetail(16);
            return View(tour);
        }


        [HttpPost]
        public ActionResult DeleteTourSpecilRate(ObjectModel objectModel)
        {
            var surcharge = _specialRateService.GetSpecialRate(objectModel.Id);
            if (surcharge != null)
            {
                surcharge.ObjectState = ObjectState.Deleted;
                _specialRateService.Delete(surcharge);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = "Delete this record is successfully!";

            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Deleta this record is error!";
                throw;
            }
            return Json(objectModel);
        }

        public ActionResult TourPrice(int id)
        {
            ViewBag.Rates = _tourRateService.GetTourRates().Where(d => d.TourId == id && d.ExpandRateId == -1).OrderBy(d => d.PersonNo).ToList();
            ViewBag.Rates2 = _tourRateService2.GetTourRates().Where(d => d.TourId == id && d.ExpandRateId == -1).OrderBy(d => d.PersonNo).ToList();
            ViewBag.Rates3 = _tourRateService3.GetTourRates().Where(d => d.TourId == id && d.ExpandRateId == -1).OrderBy(d => d.PersonNo).ToList();
            var expandRates = _tourExpandRateService.GetTourRatesByTourId(id).ToList();
            var rateExpandModels = new List<ExpandRatesModel>();

            if (expandRates.Count > 0)
            {
                rateExpandModels.AddRange(from expandRate in expandRates
                                          where expandRate != null
                                          select new ExpandRatesModel
                                          {
                                              ExpandRates = expandRate,
                                              ExpandRatePrices = _tourRateService.GetTourRatesByTourId(expandRate.TourId).Where(e => e.ExpandRateId == expandRate.Id).ToList()
                                          });
            }




            ViewBag.RateExpands = rateExpandModels;
            var tour = _tourService.Queryable().FirstOrDefault(t => t.Id == id);
            if (!tour.CommissionRate.HasValue || tour.CommissionRate.Value == 0)
            {
                tour.CommissionRate = 15;
            }
            return View(tour);
        }
        [HttpPost]
        public ActionResult DeletePrice(Rate rate)
        {
            var objectModel = new ObjectModel();
            if (rate == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Delete is error!");
                return Json(objectModel);
            }
            var rateDelete = _tourRateService.Queryable().FirstOrDefault(t => t.Id == rate.Id);
            //Added comlunm
            if (rateDelete != null)
            {
                rateDelete.ObjectState = ObjectState.Deleted;
                _tourRateService.Delete(rateDelete);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Delete this record is successfully!");
                return Json(objectModel);

            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Delete is error!");
                throw;
            }
        }
        [HttpPost]
        public ActionResult DeletePrice2(Rate2 rate)
        {
            var objectModel = new ObjectModel();
            if (rate == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Delete is error!");
                return Json(objectModel);
            }
            var rateDelete = _tourRateService2.Queryable().FirstOrDefault(t => t.Id == rate.Id);
            //Added comlunm
            if (rateDelete != null)
            {
                rateDelete.ObjectState = ObjectState.Deleted;
                _tourRateService2.Delete(rateDelete);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Delete this record is successfully!");
                return Json(objectModel);

            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Delete is error!");
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeletePrice3(Rate3 rate)
        {
            var objectModel = new ObjectModel();
            if (rate == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Delete is error!");
                return Json(objectModel);
            }
            var rateDelete = _tourRateService3.Queryable().FirstOrDefault(t => t.Id == rate.Id);
            //Added comlunm
            if (rateDelete != null)
            {
                rateDelete.ObjectState = ObjectState.Deleted;
                _tourRateService3.Delete(rateDelete);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Delete this record is successfully!");
                return Json(objectModel);

            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Delete is error!");
                throw;
            }
        }

        [HttpPost]
        public ActionResult InsertPrices(List<Rate> rates)
        {
            var objectModel = new ObjectModel();
            //Added comlunm
            if (rates == null) return Json(objectModel);
            foreach (var rate in rates)
            {
                var rateQuery = _tourRateService.Queryable().FirstOrDefault(t => t.Id == rate.Id);
                if (rateQuery != null)
                {
                    rateQuery.RetailRate = rate.RetailRate;
                    rateQuery.PersonNo = rate.PersonNo;
                    rateQuery.ModifiedDate = DateTime.Now;
                    rate.ObjectState = ObjectState.Modified;
                    _tourRateService.Update(rateQuery);
                }
                else
                {
                    rate.CreatedDate = DateTime.Now;
                    rate.ModifiedDate = DateTime.Now;
                    rate.Status = (int)Web.Framework.Utilities.Status.Active;
                    rate.ExpandRateId = -1;
                    //Inser rate
                    rate.ObjectState = ObjectState.Added;
                    _tourRateService.Insert(rate);
                }


            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Record is added successfully!");
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Insert rate is error!");
                throw;
            }
            objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
            return Json(objectModel);
        }

        [HttpPost]
        public ActionResult InsertPrices2(List<Rate2> rates)
        {
            var objectModel = new ObjectModel();
            //Added comlunm
            if (rates == null) return Json(objectModel);
            foreach (var rate in rates)
            {
                var rateQuery = _tourRateService2.Queryable().FirstOrDefault(t => t.Id == rate.Id);
                if (rateQuery != null)
                {
                    rateQuery.RetailRate = rate.RetailRate;
                    rateQuery.PersonNo = rate.PersonNo;
                    rateQuery.ModifiedDate = DateTime.Now;
                    rate.ObjectState = ObjectState.Modified;
                    _tourRateService2.Update(rateQuery);
                }
                else
                {
                    rate.CreatedDate = DateTime.Now;
                    rate.ModifiedDate = DateTime.Now;
                    rate.Status = (int)Web.Framework.Utilities.Status.Active;
                    rate.ExpandRateId = -1;
                    //Inser rate
                    rate.ObjectState = ObjectState.Added;
                    _tourRateService2.Insert(rate);
                }


            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Record is added successfully!");
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Insert rate is error!");
                throw;
            }
            objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
            return Json(objectModel);
        }
        [HttpPost]
        public ActionResult InsertPrices3(List<Rate3> rates)
        {
            var objectModel = new ObjectModel();
            //Added comlunm
            if (rates == null) return Json(objectModel);
            foreach (var rate in rates)
            {
                var rateQuery = _tourRateService3.Queryable().FirstOrDefault(t => t.Id == rate.Id);
                if (rateQuery != null)
                {
                    rateQuery.RetailRate = rate.RetailRate;
                    rateQuery.PersonNo = rate.PersonNo;
                    rateQuery.ModifiedDate = DateTime.Now;
                    rate.ObjectState = ObjectState.Modified;
                    _tourRateService3.Update(rateQuery);
                }
                else
                {
                    rate.CreatedDate = DateTime.Now;
                    rate.ModifiedDate = DateTime.Now;
                    rate.Status = (int)Web.Framework.Utilities.Status.Active;
                    rate.ExpandRateId = -1;
                    //Inser rate
                    rate.ObjectState = ObjectState.Added;
                    _tourRateService3.Insert(rate);
                }


            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Record is added successfully!");
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Insert rate is error!");
                throw;
            }
            objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
            return Json(objectModel);
        }
        [HttpPost]
        public ActionResult InsertRatesExpandPrice(List<Rate> rates)
        {
            var objectModel = new ObjectModel();
            //Added comlunm
            if (rates == null) return Json(objectModel);
            foreach (var rate in rates)
            {
                var rateQuery = _tourRateService.Queryable().FirstOrDefault(t => t.Id == rate.Id);
                if (rateQuery != null)
                {
                    rateQuery.RetailRate = rate.RetailRate;
                    rateQuery.ExpandRateId = rate.ExpandRateId;
                    rateQuery.PersonNo = rate.PersonNo;
                    rateQuery.ModifiedDate = DateTime.Now;
                    rate.ObjectState = ObjectState.Modified;
                    _tourRateService.Update(rateQuery);
                }
                else
                {
                    rate.CreatedDate = DateTime.Now;
                    rate.ModifiedDate = DateTime.Now;
                    rate.Status = (int)Web.Framework.Utilities.Status.Active;
                    //Inser rate
                    rate.ObjectState = ObjectState.Added;
                    _tourRateService.Insert(rate);
                }


            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Record is added successfully!");
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Insert rate is error!");
                throw;
            }
            objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
            return Json(objectModel);
        }
        [HttpPost]
        public ActionResult InsertExpandPrice(ExpandRates expandRates)
        {
            var objectModel = new ObjectModel();
            //Added comlunm
            if (expandRates == null) return Json(objectModel);
            expandRates.CreatedDate = DateTime.Now;
            expandRates.ModifiedDate = DateTime.Now;
            expandRates.Type = (int)Utilities.ExpandRateType.ExpandRate;
            expandRates.Status = false;
            expandRates.ObjectState = ObjectState.Added;
            _tourExpandRateService.Insert(expandRates);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = "Record is added successfully!";
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Insert rate is error!";
                throw;
            }
            objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
            return Json(objectModel);
        }
        [HttpPost]
        public ActionResult DeleteItinerary(Detail detail)
        {
            var objectModel = new ObjectModel();
            if (detail == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Updated tour detail is error!");
                return Json(objectModel);
            }
            var detailDelete = _tourDetailService.Queryable().FirstOrDefault(t => t.Id == detail.Id);
            //Added comlunm
            if (detailDelete != null)
            {
                detailDelete.ObjectState = ObjectState.Deleted;
                _tourDetailService.Delete(detailDelete);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Delete this record is successfully!");

            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Update tour detail is error!");
                throw;
            }
            return Json(objectModel);
        }

        [HttpPost]
        public ActionResult UpdateItinerary(Detail detail)
        {
            var objectModel = new ObjectModel();
            if (detail == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Updated Itinerary is error!");
                return Json(objectModel);
            }
            var tourDetailUpdate = _tourDetailService.Queryable().FirstOrDefault(t => t.Id == detail.Id);
            //Added comlunm
            if (tourDetailUpdate != null)
            {
                tourDetailUpdate.ModifiedDate = DateTime.Now;
                tourDetailUpdate.Itininary = detail.Itininary;
                tourDetailUpdate.Content = detail.Content;
                tourDetailUpdate.Meal = detail.Meal;
                tourDetailUpdate.Transport = detail.Transport;
                tourDetailUpdate.OverNight = detail.OverNight;
                tourDetailUpdate.SortOrder = detail.SortOrder;

                tourDetailUpdate.ImageDetail = detail.ImageDetail;
                tourDetailUpdate.ImageDetailDescription = detail.ImageDetailDescription;

                //Update tour detail
                tourDetailUpdate.ObjectState = ObjectState.Modified;
                _tourDetailService.Update(tourDetailUpdate);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Itinerary is updated!");

            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Update Itinerary is error!");
                throw;
            }
            return Json(objectModel);
        }

        [HttpPost]
        public ActionResult GetItinerary(Detail detail)
        {
            var id = -1;
            int.TryParse(detail.Id.ToString(CultureInfo.InvariantCulture), out id);
            Detail tourDetail = ViewBag.Detail = _tourDetailService.GetDetail(id);
            //Get Transport
            var category = _categoryService.GetCategories().FirstOrDefault(c => c.KeyCode == Web.Framework.Utilities.Transportations);
            //if (category == null) return View();
            var categoryStr = tourDetail != null ? tourDetail.Transport : string.Empty;
            ViewBag.Transports = Web.Framework.Utilities.GetCategoryList(categoryStr, _categoryDetailService.GetCategoriesDetail(category.Id));
            //Get Meal
            var categoryMeal = _categoryService.GetCategories().FirstOrDefault(c => c.KeyCode == Web.Framework.Utilities.Meals);
            //if (categoryMeal == null) return View();
            var categoryStrMeal = tourDetail != null ? tourDetail.Meal : string.Empty;
            ViewBag.Meals = Web.Framework.Utilities.GetCategoryList(categoryStrMeal, _categoryDetailService.GetCategoriesDetail(categoryMeal.Id));


            //var objectModel = new ObjectModel();
            //if (detail == null)
            //{
            //    objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
            //    objectModel.Message = string.Format("Updated tour is error!");
            //    return Json(objectModel);
            //}
            //var tourDetailUpdate = _tourDetailService.Queryable().FirstOrDefault(t => t.Id == detail.Id);
            ////Added comlunm
            //if (tourDetailUpdate != null)
            //{
            //    tourDetailUpdate.ModifiedDate = DateTime.Now;
            //    tourDetailUpdate.Itininary = detail.Itininary;
            //    tourDetailUpdate.Content = detail.Content;
            //    tourDetailUpdate.Meal = detail.Meal;
            //    tourDetailUpdate.Transport = detail.Transport;
            //    //Update tour detail
            //    tourDetailUpdate.ObjectState = ObjectState.Modified;
            //    _tourDetailService.Update(tourDetailUpdate);
            //}
            //try
            //{
            //    _unitOfWorkAsync.SaveChanges();
            //    objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
            //    objectModel.Url = string.Format("~/Tour/Price/{0}", tourDetailUpdate != null ? tourDetailUpdate.TourId.ToString() : "");

            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
            //    objectModel.Message = string.Format("Update tour detail is error!");
            //    throw;
            //}
            return PartialView("_Itinerary");
        }

        [HttpPost]
        public ActionResult SaveItinerary(Detail detail)
        {
            var objectModel = new ObjectModel();
            //Added comlunm

            detail.CreatedDate = DateTime.Now;
            detail.ModifiedDate = DateTime.Now;
            detail.Status = (int)Web.Framework.Utilities.Status.Active;
            //Inser tour
            detail.ObjectState = ObjectState.Added;
            _tourDetailService.Insert(detail);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Insert itinerary is error!");
                throw;
            }
            return Json(objectModel);
        }
        [HttpPost]
        public ActionResult SavePromotion(ObjectModel objectModel)
        {
            var tour = _tourService.GetTourById(objectModel.Id);
            tour.Discount = objectModel.IntParam1;
            tour.YourSave = objectModel.IntParam2;
            tour.InclusiveBenefit = objectModel.StrParam1;
            tour.ModifiedDate = DateTime.Now;
            tour.ObjectState = ObjectState.Modified;
            _tourService.Update(tour);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Updated Tour is error!";
                throw;
            }
            return Json(objectModel);
        }

        [HttpPost]
        public ActionResult SaveTourItieraryUploadedFile(FormCollection forms)
        {

            var id = Web.Framework.Utilities.ConvertToInt(forms["id"]);
            var baseUrl = string.Format("~/Uploads/TourImages/");
            if (id <= 0) return Redirect(string.Format("~/Tour/BasicInformation/{0}", id));
            var tour = _tourDetailService.Queryable().FirstOrDefault(t => t.Id == id);
            const string fName = "";
            if (tour == null) return Redirect(string.Format("~/Product/TourItinerary/{0}", id));

            try
            {
                // Get the data
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    if (file == null) continue;
                    var virtualPath = Server.MapPath(baseUrl);

                    if (!Directory.Exists(virtualPath))
                    {
                        Directory.CreateDirectory(virtualPath);
                    }
                    var imageCode = Web.Framework.Utilities.GetRandomString(5);
                    var original = Image.FromStream(file.InputStream);
                    var imageName = string.Format("{0}-{1}", imageCode, Web.Framework.Utilities.GenerateSlug(tour.ImageDetailDescription));
                    var extension = Path.GetExtension(file.FileName);
                    //Save on folder
                    var imgOriginal = string.Format("{0}{1}-original{2}", virtualPath, imageName, extension);
                    var imgThumbnail = string.Format("{0}{1}-thumb{2}", virtualPath, imageName, extension);
                    //Save on data
                    var absOriginalUrl = string.Format("{0}{1}-original{2}", baseUrl, imageName, extension);
                    var absThumbnailUrl = string.Format("{0}{1}-thumb{2}", baseUrl, imageName, extension);
                    //Scale for thumbnal
                    var thumbnail = Web.Framework.Utilities.ScaleBySize(original, 600);

                    original.Save(imgOriginal);
                    thumbnail.Save(imgThumbnail);
                    tour.ImageDetail = absOriginalUrl;

                    var media = new Medium
                    {
                        Type = 2,
                        Title = string.Format("{0} - {1}", imageCode, tour.ImageDetailDescription),
                        Description = tour.ImageDetailDescription,
                        AlternateText = tour.ImageDetailDescription,
                        OriginalPath = absOriginalUrl,
                        ThumbnailPath = absThumbnailUrl,
                        OwnerId = id,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        Status = (int)Web.Framework.Utilities.Status.Active,
                        MediaType = (int)Web.Framework.Utilities.MediaType.Gallery,
                        ObjectState = ObjectState.Added
                    };
                    //Inser media
                    _mediaService.Insert(media);
                    _tourDetailService.Update(tour);
                    _unitOfWorkAsync.SaveChanges();
                    //Clear up
                    original.Dispose();
                    thumbnail.Dispose();
                }
                return Redirect(string.Format("~/Product/TourItinerary/{0}", tour.TourId));
            }
            catch
            {
                Response.StatusCode = 500;
                Response.Write("An error occured");
                Response.End();
            }
            return Redirect(string.Format("~/Product/TourItinerary/{0}", tour.TourId));
        }


        public ActionResult SaveUploadedFile(FormCollection forms)
        {

            var id = Web.Framework.Utilities.ConvertToInt(forms["id"]);
            var baseUrl = string.Format("~/Uploads/TourImages/");
            if (id <= 0) return Redirect(string.Format("~/Tour/BasicInformation/{0}", id));
            var tour = _tourService.Queryable().FirstOrDefault(t => t.Id == id);
            const string fName = "";
            if (tour == null) return Redirect(string.Format("~/Tour/BasicInformation/{0}", id));

            try
            {
                // Get the data
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    if (file == null) continue;
                    var virtualPath = Server.MapPath(baseUrl);

                    if (!Directory.Exists(virtualPath))
                    {
                        Directory.CreateDirectory(virtualPath);
                    }
                    var imageCode = Web.Framework.Utilities.GetRandomString(5);
                    var original = Image.FromStream(file.InputStream);
                    var imageName = string.Format("{0}-{1}", imageCode, Web.Framework.Utilities.GenerateSlug(tour.Name));
                    var extension = Path.GetExtension(file.FileName);
                    //Save on folder
                    var imgOriginal = string.Format("{0}{1}-original{2}", virtualPath, imageName, extension);
                    var imgThumbnail = string.Format("{0}{1}-thumb{2}", virtualPath, imageName, extension);
                    //Save on data
                    var absOriginalUrl = string.Format("{0}{1}-original{2}", baseUrl, imageName, extension);
                    var absThumbnailUrl = string.Format("{0}{1}-thumb{2}", baseUrl, imageName, extension);
                    //Scale for thumbnal
                    var thumbnail = Web.Framework.Utilities.ScaleBySize(original, 600);

                    original.Save(imgOriginal);
                    thumbnail.Save(imgThumbnail);
                    var media = new Medium
                    {
                        Type = 2,
                        Title = string.Format("{0} - {1}", imageCode, tour.Name),
                        Description = tour.Name,
                        AlternateText = tour.Name,
                        OriginalPath = absOriginalUrl,
                        ThumbnailPath = absThumbnailUrl,
                        OwnerId = id,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        Status = (int)Web.Framework.Utilities.Status.Active,
                        MediaType = (int)Web.Framework.Utilities.MediaType.Gallery,
                        ObjectState = ObjectState.Added
                    };
                    //Inser media
                    _mediaService.Insert(media);
                    _unitOfWorkAsync.SaveChanges();
                    //Clear up
                    original.Dispose();
                    thumbnail.Dispose();
                }
                return Json(new { Message = fName });
            }
            catch
            {
                Response.StatusCode = 500;
                Response.Write("An error occured");
                Response.End();
            }
            return Json(new { Message = "Error in saving file" });
        }

        [HttpPost]
        public ActionResult UpdateMedia(Medium media)
        {
            var objectModel = new ObjectModel();
            if (media == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Updated image is error!");
                return Json(objectModel);
            }
            var mediaUpdate = _mediaService.Queryable().FirstOrDefault(t => t.Id == media.Id);
            //Added comlunm
            if (mediaUpdate != null)
            {
                mediaUpdate.ModifiedDate = DateTime.Now;
                mediaUpdate.Title = media.Title;
                //Update tour
                mediaUpdate.ObjectState = ObjectState.Modified;
                _mediaService.Update(mediaUpdate);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;

            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Update media is error!");
                throw;
            }
            return Json(objectModel);
        }

        [HttpPost]
        public ActionResult DeleteMedia(Medium media)
        {
            var objectModel = new ObjectModel();
            if (media == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Updated image is error!");
                return Json(objectModel);
            }
            var mediaDelete = _mediaService.Queryable().FirstOrDefault(t => t.Id == media.Id);
            //Added comlunm
            if (mediaDelete != null)
            {
                DeleteMedium(mediaDelete);//Delete image path
                mediaDelete.ObjectState = ObjectState.Deleted;
                _mediaService.Delete(mediaDelete);

            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;

            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Update media is error!");
                throw;
            }
            return Json(objectModel);
        }

        public void DeleteMedium(Medium media)
        {
            //Chec media emplty
            if (media == null) { return; }
            //Delete Original imgage
            if (!string.IsNullOrEmpty(media.OriginalPath))
            {
                var fullPath = Request.MapPath(media.OriginalPath);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
            //Delete Thumbnail imgage
            if (!string.IsNullOrEmpty(media.ThumbnailPath))
            {
                var fullPath = Request.MapPath(media.ThumbnailPath);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
        }
        public void DeleteImage(HotelImages image)
        {
            //Chec media emplty
            if (image == null) { return; }
            //Delete Original imgage
            if (!string.IsNullOrEmpty(image.ImageOrigin))
            {
                var fullPath = Request.MapPath(image.ImageOrigin);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }


            }
            //Delete Thumbnail imgage
            if (!string.IsNullOrEmpty(image.ImageOrigin) && image.ImageOrigin.Contains("original.png"))
            {
                for (int i = 1; i <= 10; i++)
                {
                    string deletePath = string.Format("{0}", image.ImageOrigin.Replace("original.png", i + "0.jpeg"));
                    if (!string.IsNullOrEmpty(deletePath))
                    {
                        var fullPath = Request.MapPath(deletePath);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(image.ImageThumbnail))
            {
                var fullPath = Request.MapPath(image.ImageThumbnail);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
        }
        private List<Detail> GetTourDetailsByTourId(int? id)
        {
            var categoryMeals = _categoryService.GetCategories().FirstOrDefault(c => c.KeyCode == Web.Framework.Utilities.Meals);
            var categoryTransports = _categoryService.GetCategories().FirstOrDefault(c => c.KeyCode == Web.Framework.Utilities.Transportations);
            List<Detail> tourDetails = _tourDetailService.GetDetailTours().Where(d => d.TourId == id).ToList();
            foreach (var tourDetail in tourDetails)
            {
                var mealStr = string.Empty;
                var transportStr = string.Empty;
                if (categoryMeals != null)
                {
                    var meals = Web.Framework.Utilities.GetCategoryList(tourDetail.Meal, _categoryDetailService.GetCategoriesDetail(categoryMeals.Id));
                    mealStr = meals.Where(meal => meal != null && meal.Checked).Aggregate(mealStr, (current, meal) => current + string.Format("{0}, ", meal.Name));
                }
                if (categoryTransports != null)
                {
                    var transports = Web.Framework.Utilities.GetCategoryList(tourDetail.Transport, _categoryDetailService.GetCategoriesDetail(categoryTransports.Id));
                    transportStr = transports.Where(transport => transport != null && transport.Checked).Aggregate(transportStr, (current, transport) => current + string.Format("{0}, ", transport.Name));
                }
                tourDetail.Meal = mealStr;
                tourDetail.Transport = transportStr;
            }
            return tourDetails.OrderBy(d => d.SortOrder).ToList();
        }

        //Temple
        public ActionResult UpdatedTourSearchKey()
        {
            var tours = _tourService.GetTours();
            foreach (var tour in tours.Where(tour => tour != null))
            {
                tour.SearchKey = tour.Name;
                tour.ObjectState = ObjectState.Modified;
                _tourService.Update(tour);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();

            }
            catch (Exception)
            {
                // ignored
            }
            return Content("");
        }
        #endregion

        #region ----Hotel----

        //Hotel Step 1
        public ActionResult HotelBasic(int? id)
        {
            var hotel = _hotelService.GetHotelSingle(id ?? 0);
            return View(hotel ?? new Hotel());
        }
        [HttpPost]
        public ActionResult GetTabBasic(ObjectModel objectModel)
        {
            ViewBag.Cities = _cityService.GetCities();
            ViewBag.Countries = _countryService.GetCountryList();
            var hotel = _hotelService.GetHotelSingle(objectModel.Id);
            if (hotel != null)
            {
                ViewBag.ContactMain = _hotelService.GetContact(hotel.MaincontractId);
                ViewBag.Hotel = hotel;
                return PartialView("_TabBasic");
            }
            ViewBag.Hotel = new Hotel();
            ViewBag.ContactMain = new Contact();
            return PartialView("_TabBasic");
        }
        [HttpPost]
        public ActionResult DeleteHotel(ObjectModel objectModel)
        {
            var objectResult = new ObjectModel();
            var hotel = _hotelService.GetHotelSingle(objectModel.Id);
            //Update hotel
            if (hotel != null)
            {
                hotel.Status = (int)Utilities.Status.Delete;
                _hotelService.Update(hotel);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = "Hotel is deleted!";
                objectModel.Url = Url.Content("/Product/Hotel");
            }
            catch (Exception)
            {
                objectResult.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectResult.Message = "Delete Hotel is error!";
            }
            return Json(objectModel);
        }
        [HttpPost]
        public ActionResult UpdateHotelBasic(ObjectModel objectModel)
        {
            var objectResult = new ObjectModel();
            var hotel = _hotelService.GetHotelSingle(objectModel.Id);
            var contact = _hotelService.GetContact(objectModel.IntParam1);
            //Update contact
            if (contact != null)
            {
                contact.FIRSTNAME = objectModel.StrParam1;
                contact.LASTNAME = objectModel.StrParam2;
                contact.SALUTATION = objectModel.StrParam3;
                contact.EMAILADDRESS = objectModel.StrParam4;
                contact.PHONE = objectModel.StrParam5;
                contact.ObjectState = ObjectState.Modified;
                _hotelService.UpdateContact(contact);
            }
            //Update hotel
            if (hotel != null)
            {
                hotel.Name = objectModel.StrParam6;
                hotel.CountryId = objectModel.IntParam2;
                hotel.Country = objectModel.StrParam7;
                var city = _cityService.GetCityById(objectModel.IntParam3);
                if (city != null)
                {
                    hotel.CITY = city.Description;
                    hotel.CityId = objectModel.IntParam3;
                }
                hotel.ObjectState = ObjectState.Modified;
                _hotelService.Update(hotel);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Basic information is updated!");
            }
            catch (Exception)
            {
                objectResult.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectResult.Message = string.Format("Update basic information hotel is error!");
            }
            return Json(objectModel);
        }
        [HttpPost]
        public ActionResult InsertHotelBasic(ObjectModel objectModel)
        {
            var objectResult = new ObjectModel();
            //Add contact
            var contact = new Contact
            {
                FIRSTNAME = objectModel.StrParam1,
                LASTNAME = objectModel.StrParam2,
                SALUTATION = objectModel.StrParam3,
                EMAILADDRESS = objectModel.StrParam4,
                PHONE = objectModel.StrParam5,
                ObjectState = ObjectState.Added
            };
            _hotelService.InsertContact(contact);
            var city = _cityService.GetCityById(objectModel.IntParam3);
            var country = _countryService.GetCountry(objectModel.IntParam2);
            //Add hotel
            var hotel = new Hotel
            {
                Name = objectModel.StrParam6,
                CountryId = objectModel.IntParam2,
                CityId = objectModel.IntParam3,
                HotelName = objectModel.StrParam6,
                MaincontractId = contact.CONTACTID,
                Phone = objectModel.StrParam5,
                HotelStyleName = "Hotel",
                Status = 2,
                CITY = city != null ? city.Description : string.Empty,
                Country = country != null ? country.Description : string.Empty,
                ObjectState = ObjectState.Added

            };
            _hotelService.Add(hotel);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Hotel is added to data!");
                objectModel.Url = string.Format("~/Product/HotelBasic/{0}", hotel.Id);
            }
            catch (Exception)
            {
                objectResult.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectResult.Message = string.Format("Add hotel information is error!");
            }
            return Json(objectModel);
        }

        //Hotel Step 2
        [HttpPost]
        public ActionResult GetTabContact(ObjectModel objectModel)
        {
            var hotel = _hotelService.GetHotelSingle(objectModel.Id);
            ViewBag.ContactMain = _hotelService.GetContact(hotel.MaincontractId);
            ViewBag.ContactReservation = _hotelService.GetContact(hotel.ReservationcontractId ?? 0);
            ViewBag.ContactMarketing = _hotelService.GetContact(hotel.MarketingcontractId ?? 0);
            ViewBag.ContactAccount = _hotelService.GetContact(hotel.AccountcontractId ?? 0);

            return PartialView("_TabContact");
        }
        [HttpPost]
        public ActionResult GetContact(ObjectModel objectModel)
        {
            var contact = _hotelService.GetContact(objectModel.Id);
            return PartialView("_Contact", contact);
        }
        [HttpPost]
        public ActionResult UpdateContact(ObjectModel objectModel)
        {
            var objectResult = new ObjectModel();
            var hotel = _hotelService.GetHotelSingle(objectModel.Id);
            if (hotel == null)
            {
                return Json(objectModel);
            }
            var contact = _hotelService.GetContact(objectModel.IntParam1);
            //Update contact
            if (contact != null)
            {
                contact.FIRSTNAME = objectModel.StrParam1;
                contact.LASTNAME = objectModel.StrParam2;
                contact.SALUTATION = objectModel.StrParam3;
                contact.EMAILADDRESS = objectModel.StrParam4;
                contact.PHONE = objectModel.StrParam5;
                contact.FAX = objectModel.StrParam6;
                contact.ObjectState = ObjectState.Modified;
                _hotelService.UpdateContact(contact);
                try
                {
                    _unitOfWorkAsync.SaveChanges();
                    objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                    objectModel.Message = string.Format("Contact information is updated!");
                }
                catch (Exception)
                {
                    objectResult.Status = (int)Web.Framework.Utilities.Status.Inactive;
                    objectResult.Message = string.Format("Update contact information is error!");
                }
                return Json(objectModel);
            }
            contact = new Contact
            {
                FIRSTNAME = objectModel.StrParam1,
                LASTNAME = objectModel.StrParam2,
                SALUTATION = objectModel.StrParam3,
                EMAILADDRESS = objectModel.StrParam4,
                PHONE = objectModel.StrParam5,
                FAX = objectModel.StrParam6,
                ObjectState = ObjectState.Added
            };
            _hotelService.InsertContact(contact);
            //Update hotel
            switch (objectModel.IntParam2)
            {
                case (int)Web.Framework.Utilities.ContactType.Accountant:
                    hotel.AccountcontractId = contact.CONTACTID;
                    break;
                case (int)Web.Framework.Utilities.ContactType.Marketing:
                    hotel.MarketingcontractId = contact.CONTACTID;
                    break;
                case (int)Web.Framework.Utilities.ContactType.Reservation:
                    hotel.ReservationcontractId = contact.CONTACTID;
                    break;
            }
            hotel.ObjectState = ObjectState.Modified;
            _hotelService.Update(hotel);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Contact information is added!");
            }
            catch (Exception)
            {
                objectResult.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectResult.Message = string.Format("Add contact information is error!");
            }
            return Json(objectModel);
        }

        //Hotel Step 3
        [HttpPost]
        public ActionResult GetTabProperty(ObjectModel objectModel)
        {
            var hotel = _hotelService.GetHotelSingle(objectModel.Id);


            ViewBag.Regions = _regionService.GetRegionList().ToList();
            ViewBag.Cities = _cityService.GetCities();
            ViewBag.Countries = _countryService.GetCountryList();
            ViewBag.Categories = _catDetailService.GetCategoriesDetail(9).ToList();
            return PartialView("_TabProperty", hotel);
        }
        [HttpPost]
        public ActionResult UpdateTabProperty(ObjectModel objectModel)
        {
            var hotel = _hotelService.GetHotelSingle(objectModel.Id);
            var objectResult = new ObjectModel();

            //Update hotel
            if (hotel != null)
            {
                hotel.Name = objectModel.StrParam1;
                hotel.HotelNameLocal = objectModel.StrParam2;
                hotel.StreetAddressEnglish = objectModel.StrParam3;
                hotel.StreetAddressLocal = objectModel.StrParam4;

                hotel.StartRating = objectModel.IntParam1;
                hotel.NumberOfRoom = objectModel.IntParam2;
                hotel.HotelStyleName = objectModel.StrParam5;
                double lat = 0;
                double.TryParse(objectModel.StrParam6, out lat);
                hotel.Latitude = lat;
                double longtitude = 0;
                double.TryParse(objectModel.StrParam7, out longtitude);
                hotel.Longtitude = longtitude;
                hotel.RegionId = objectModel.IntParam3;
                var searchKey = _hotelService.GenerateSlugHotelKey(hotel);
                hotel.KeyValue = searchKey;
                hotel.ObjectState = ObjectState.Modified;
                _hotelService.Update(hotel);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Hotel property is updated!");
            }
            catch (Exception)
            {
                objectResult.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectResult.Message = string.Format("Update hotel property is error!");
            }
            return Json(objectModel);
        }

        //Hotel Step 4
        [HttpPost]
        public ActionResult GetTabStyle(ObjectModel objectModel)
        {
            var hotel = _hotelService.GetHotelSingle(objectModel.Id) ?? new Hotel();

            //var styles = _catDetailService.CategoriesList(hotel.HotelStyle, 4);

            var category = _categoryService.GetCategories().FirstOrDefault(c => c.KeyCode == Web.Framework.Utilities.TheCollection);

            if (category != null)
            {
                ViewBag.CollectionValue = hotel.CollectionValue;
                var styles = Web.Framework.Utilities.GetCategoryList(hotel.HotelStyle, _categoryDetailService.GetCategoriesDetail(category.Id));
                return PartialView("_TabStyle", styles);
            }
            ViewBag.CollectionValue = hotel.CollectionValue;
            return PartialView("_TabStyle", new List<ItemModel>());
        }
        [HttpPost]
        public ActionResult UpdateHotelStyle(ObjectModel objectModel)
        {
            var hotel = _hotelService.GetHotelSingle(objectModel.Id);
            var objectResult = new ObjectModel();

            //Update hotel
            if (hotel != null)
            {

                hotel.HotelStyle = objectModel.StrParam1;
                hotel.CollectionValue = objectModel.IntParam1;
                hotel.ObjectState = ObjectState.Modified;
                var searchKey = _hotelService.GenerateSlugHotelKey(hotel);
                hotel.KeyValue = searchKey;
                _hotelService.Update(hotel);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Hotel styles is updated!");
            }
            catch (Exception)
            {
                objectResult.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectResult.Message = string.Format("Update hotel styles is error!");
            }
            return Json(objectModel);
        }
        //Hotel Step 5
        [HttpPost]
        public ActionResult GetTabHotelFacility(ObjectModel objectModel)
        {
            var hotel = _hotelService.GetHotelSingle(objectModel.Id) ?? new Hotel();

            var facilities = _catDetailService.CategoriesList(hotel.HotelFacilities, 2);


            return PartialView("_TabHotelFacility", facilities);
        }
        [HttpPost]
        public ActionResult UpdateHotelFacility(ObjectModel objectModel)
        {
            var hotel = _hotelService.GetHotelSingle(objectModel.Id);
            var objectResult = new ObjectModel();

            //Update hotel
            if (hotel != null)
            {
                hotel.HotelFacilities = objectModel.StrParam1;
                hotel.ObjectState = ObjectState.Modified;
                _hotelService.Update(hotel);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Hotel facilities is updated!");
            }
            catch (Exception)
            {
                objectResult.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectResult.Message = string.Format("Update hotel facilities is error!");
            }
            return Json(objectModel);
        }
        //Hotel Step 6
        [HttpPost]
        public ActionResult GetTabRoomFacility(ObjectModel objectModel)
        {
            var hotel = _hotelService.GetHotelSingle(objectModel.Id) ?? new Hotel();

            var facilities = _catDetailService.CategoriesList(hotel.HotelRoomFacilities, 3);


            return PartialView("_TabRoomFacility", facilities);
        }
        [HttpPost]
        public ActionResult UpdateRoomFacility(ObjectModel objectModel)
        {
            var hotel = _hotelService.GetHotelSingle(objectModel.Id);
            var objectResult = new ObjectModel();

            //Update hotel
            if (hotel != null)
            {
                hotel.HotelRoomFacilities = objectModel.StrParam1;
                hotel.ObjectState = ObjectState.Modified;
                _hotelService.Update(hotel);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Hotel room facilities is updated!");
            }
            catch (Exception)
            {
                objectResult.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectResult.Message = string.Format("Update room facilities is error!");
            }
            return Json(objectModel);
        }
        //Hotel Step 7
        [HttpPost]
        public ActionResult GetTabSportRecreation(ObjectModel objectModel)
        {
            var hotel = _hotelService.GetHotelSingle(objectModel.Id) ?? new Hotel();

            var facilities = _catDetailService.CategoriesList(hotel.HotelSportRecreation, 5);


            return PartialView("_TabSportRecreation", facilities);
        }
        [HttpPost]
        public ActionResult UpdateSportRecreation(ObjectModel objectModel)
        {
            var hotel = _hotelService.GetHotelSingle(objectModel.Id);
            var objectResult = new ObjectModel();

            //Update hotel
            if (hotel != null)
            {
                hotel.HotelSportRecreation = objectModel.StrParam1;
                hotel.ObjectState = ObjectState.Modified;
                _hotelService.Update(hotel);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Hotel sports and recreations is updated!");
            }
            catch (Exception)
            {
                objectResult.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectResult.Message = string.Format("Update sports and recreations is error!");
            }
            return Json(objectModel);
        }
        //Hotel Step 8
        [HttpPost]
        public ActionResult GetTabPicture(ObjectModel objectModel)
        {
            var images = _hotelService.GetImagesesByHotelId(objectModel.Id);
            ViewBag.ImageTypes = _catDetailService.GetCategoriesDetail(11);
            ViewBag.Hotel = _hotelService.GetHotelSingle(objectModel.Id);
            return PartialView("_TabPicture", images);
        }

        [HttpPost]
        public ActionResult UpdatePicture(List<ObjectModel> objectModels)
        {
            //var hotel = _hotelService.GetHotelSingle(objectModel.Id);
            var objectResult = new ObjectModel();
            foreach (var objectModel in objectModels)
            {
                var hotelImage = _hotelService.GetImage(objectModel.Id);
                if (hotelImage == null) continue;
                hotelImage.ImageType = objectModel.IntParam1;
                hotelImage.PictureType = objectModel.StrParam2;
                hotelImage.Description = objectModel.StrParam3;
                hotelImage.ImageName = objectModel.StrParam1;
                hotelImage.ObjectState = ObjectState.Modified;
                _imageService.Update(hotelImage);

            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectResult.Status = (int)Web.Framework.Utilities.Status.Active;
                objectResult.Message = string.Format("Hotel images is updated!");
            }
            catch (Exception)
            {
                objectResult.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectResult.Message = string.Format("Update images is error!");
            }
            return Json(objectResult);
        }
        [HttpPost]
        public ActionResult DeletePictures(int hotelId, int[] imgId)
        {
            var objectModel = new ObjectModel();

            foreach (var image in imgId.Select(id => _hotelService.GetImage(id)).Where(image => image != null))
            {
                DeleteImage(image);//Delete current path
                image.ObjectState = ObjectState.Deleted;
                _imageService.Delete(image);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Delete all record is successfully!");

            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Delete all record is not successfully!");
                throw;
            }
            return Json(objectModel);
        }
        public ActionResult SaveUploadedPictures(FormCollection forms)
        {
            var objectModel = new ObjectModel();
            //Get Id
            var id = Web.Framework.Utilities.ConvertToInt(forms["id"]);
            //Create founder path
            var baseUrl = $"~/uploads/hotelimages/{id}";
            if (id <= 0) return Redirect($"/Product/HotelBasic/{id}/");
            //Get hotel by hotel Id
            var hotel = _hotelService.GetHotelSingle(id);

            if (hotel == null) return Redirect($"/Product/HotelBasic/{id}");
            // Get the data
            foreach (string fileName in Request.Files)
            {
                var file = Request.Files[fileName];
                if (file == null) continue;
                var virtualPath = Server.MapPath(baseUrl);

                if (!Directory.Exists(virtualPath))
                {
                    Directory.CreateDirectory(virtualPath);
                }
                var imageCode = Web.Framework.Utilities.GetRandomString(5);
                var imageName = $"{imageCode}-{Web.Framework.Utilities.GenerateSlug(hotel.Name)}";
                if (fileName.Contains(".mp4"))
                {
                    var fileOut1 = Path.Combine(virtualPath, imageName);
                    file.SaveAs(fileOut1);
                    var media = new HotelImages
                    {
                        HotelId = id,
                        ImageOrigin = fileOut1,
                        ImageThumbnail = fileOut1,
                        ImageName = $"{imageCode} - {hotel.Name}",
                        EnglishTitle = $"{imageCode} - {hotel.Name}",
                        ImageQuanlity = 70,
                        ImageHeight = 405,
                        ImageWidth = 720,
                        PictureType = "Restaurant",
                        ImageType = 1,
                        ImageSize = -1,
                        ObjectState = ObjectState.Added
                    };
                    //Inser media
                    _imageService.Insert(media);
                }
                else
                {
                    var original = Image.FromStream(file.InputStream);

                    var absOriginalUrl = $"{baseUrl}/{imageName}-original.jpg";
                    var absThumbnailUrl = $"{baseUrl}/{imageName}";
                    VariousQuality(original, virtualPath, imageName);
                    var media = new HotelImages
                    {
                        HotelId = id,
                        ImageOrigin = absOriginalUrl,
                        ImageThumbnail = absThumbnailUrl,
                        ImageName = $"{imageCode} - {hotel.Name}",
                        EnglishTitle = $"{imageCode} - {hotel.Name}",
                        ImageQuanlity = 70,
                        ImageHeight = 405,
                        ImageWidth = 720,
                        PictureType = "Restaurant",
                        ImageType = 1,
                        ImageSize = -1,
                        ObjectState = ObjectState.Added
                    };
                    //Inser media
                    _imageService.Insert(media);
                    //Clear up
                    original.Dispose();
                }



                //thumbnail.Dispose();
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = "Add all images are successfully!";
            }
            catch (Exception ex)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Add all images are not successfully!";
            }
            return Json(objectModel);
        }
        static void VariousQuality(Image original, string imagePath, string imageName)
        {
            var codecs = ImageCodecInfo.GetImageEncoders();
            var jpgEncoder = codecs.FirstOrDefault(codec => codec.FormatID == ImageFormat.Jpeg.Guid);
            if (jpgEncoder == null) return;
            var encoder = Encoder.Quality;
            var encoderParameters = new EncoderParameters(1);

            var encoderParameter = new EncoderParameter(encoder, 100L);
            //save original
            encoderParameters.Param[0] = encoderParameter;
            var fileOut1 = Path.Combine(imagePath, imageName + "-" + "original.jpg");
            var ms1 = new FileStream(fileOut1, FileMode.Create, FileAccess.Write);
            original.Save(ms1, jpgEncoder, encoderParameters);
            ms1.Flush();
            ms1.Close();

            for (long quality = 80; quality <= 90; quality += 10)
            {
                encoderParameter = new EncoderParameter(encoder, quality);
                encoderParameters.Param[0] = encoderParameter;

                var fileOut = Path.Combine(imagePath, imageName + "-" + quality + ".jpeg");
                var ms = new FileStream(fileOut, FileMode.Create, FileAccess.Write);
                original.Save(ms, jpgEncoder, encoderParameters);
                ms.Flush();
                ms.Close();
            }
        }
        static void DefaultCompressionPng(Image original, string imagePath, string imageName)
        {
            var ms = new MemoryStream();
            original.Save(ms, ImageFormat.Png);
            var compressed = new Bitmap(ms);
            ms.Close();

            var fileOutPng = Path.Combine(imagePath, $"{imageName}-original.png");
            compressed.Save(fileOutPng, ImageFormat.Png);
        }
        //Hotel Step 9
        [HttpPost]
        public ActionResult GetTabUseful(ObjectModel objectModel)
        {
            var hotel = _hotelService.GetHotelSingle(objectModel.Id);
            return PartialView("_TabUseful", hotel);
        }
        [HttpPost]
        public ActionResult UpdatebUseful(Hotel hotelModel)
        {
            var objectResult = new ObjectModel();
            var hotel = _hotelService.GetHotelSingle(hotelModel.Id);

            //Update hotel
            if (hotel != null)
            {
                hotel.ShowAirportTransferAvailable = hotelModel.ShowAirportTransferAvailable;
                hotel.AirportTransferAvailable = hotelModel.AirportTransferAvailable;

                hotel.ShowAirportTransferFee = hotelModel.ShowAirportTransferFee;
                hotel.AirportTransferFee = hotelModel.AirportTransferFee;

                hotel.ShowBreakfastCharge = hotelModel.ShowBreakfastCharge;
                hotel.BreakfastCharge = hotelModel.BreakfastCharge;

                hotel.ShowCheckOut = hotelModel.ShowCheckOut;
                hotel.CheckOut = hotelModel.CheckOut;

                hotel.ShowDistanceFromManCity = hotelModel.ShowDistanceFromManCity;
                hotel.DistanceFromMainCity = hotelModel.DistanceFromMainCity;

                hotel.ShowEarliestCheckIn = hotelModel.ShowEarliestCheckIn;
                hotel.EarliestCheckIn = hotelModel.EarliestCheckIn;

                hotel.ShowCheckIn = hotelModel.ShowCheckIn;
                hotel.CheckIn = hotelModel.CheckIn;

                hotel.ShowNumberOfRestaurants = hotelModel.ShowNumberOfRestaurants;
                hotel.NumberOfRestaurants = hotelModel.NumberOfRestaurants;

                hotel.ShowParkingAvailable = hotelModel.ShowParkingAvailable;
                hotel.ParkingAvailable = hotelModel.ParkingAvailable;

                hotel.ShowRoomService = hotelModel.ShowRoomService;
                hotel.RoomService = hotelModel.RoomService;

                hotel.ShowRoomVoltage = hotelModel.ShowRoomVoltage;
                hotel.RoomVoltage = hotelModel.RoomVoltage;

                hotel.ShowTimeToAirport = hotelModel.ShowTimeToAirport;
                hotel.TimeToAirport = hotelModel.TimeToAirport;

                hotel.ShowYearHotelBuilt = hotelModel.ShowYearHotelBuilt;
                hotel.YearHotelBuilt = hotelModel.YearHotelBuilt;

                hotel.ShowYearHotelLastRenovated = hotelModel.ShowYearHotelLastRenovated;
                hotel.YearHotelLastRenovated = hotelModel.YearHotelLastRenovated;

                hotel.ObjectState = ObjectState.Modified;
                _hotelService.Update(hotel);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectResult.Status = (int)Web.Framework.Utilities.Status.Active;
                objectResult.Message = string.Format("Userful hotel is updated!");
            }
            catch (Exception)
            {
                objectResult.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectResult.Message = string.Format("Update userful hotel is error!");
            }
            return Json(objectResult);
        }

        //Hotel Step 10
        [HttpPost]
        public ActionResult GetTabCancellation(ObjectModel objectModel)
        {
            //var hotel = _hotelService.GetHotelSingle(objectModel.Id);
            var cancelltionPolicies = _hotelService.GetHotelCancellationPoliciesByHotelId(objectModel.Id);
            ViewBag.CancellationPolicies = _cancellationService.GetCancellationList();

            return PartialView("_TabCancellation", cancelltionPolicies);
        }
        [HttpPost]
        public ActionResult InsertCancellation(ObjectModel objectModel)
        {
            var cancellation = new HotelCancellation
            {
                HotelID = objectModel.Id,

                CheckInFrom = DateTime.ParseExact(objectModel.StrParam1, "MM/dd/yyyy", null),
                CheckOutTo = DateTime.ParseExact(objectModel.StrParam2, "MM/dd/yyyy", null),
                CancellationID = objectModel.IntParam1,
                Status = (int)Web.Framework.Utilities.Status.Active,
                ObjectState = ObjectState.Added
            };
            _hotelCancellationService.Insert(cancellation);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Add cancellation is successfully!");
            }
            catch
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Add cancellation is not successfully!");
            }
            return Json(objectModel);
        }
        [HttpPost]
        public ActionResult DeleteCancellation(int cancellationId)
        {
            var objectResult = new ObjectModel();
            var cancellation = _hotelCancellationService.GetCancellationHotelById(cancellationId);
            if (cancellation != null)
            {
                cancellation.ObjectState = ObjectState.Deleted;
                _hotelCancellationService.Delete(cancellation);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectResult.Status = (int)Web.Framework.Utilities.Status.Active;
                objectResult.Message = string.Format("Delete this record is successfully!");

            }
            catch (DbUpdateConcurrencyException)
            {
                objectResult.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectResult.Message = string.Format("Deleta this record is error!");
                throw;
            }
            return Json(objectResult);
        }
        #endregion


        #region Room Control

        public ActionResult RoomManagement(int id)
        {
            var hotel = _hotelService.GetHotelSingle(id);
            ViewBag.Rooms = _hotelService.GetRoomByHotelId(id);
            return View(hotel ?? new Hotel());
        }

        public ActionResult HotelPackageSurcharge(int id)
        {
            var hotel = _hotelService.GetHotelSingle(id);
            ViewBag.RoomTypes = _hotelPackageService.ODataQueryable().Where(a => a.HotelId == id).ToList();
            ViewBag.Surcharges = _catDetailService.GetCategoriesDetail(16);
            return View(hotel ?? new Hotel());
        }
        [HttpPost]
        //        [HttpGet]
        public ActionResult GetHotelSurcharge(ObjectModel objectModel)
        {
            List<HotelPackage_Surcharge> surcharges = _hotelPackageSurchargeService.ODataQueryable().Where(a => a.Hotel_Id == objectModel.Id).ToList();
            if (objectModel.IntParam1 > 0)
            {
                surcharges = surcharges.Where(a => a.Package_Id == objectModel.IntParam1).ToList();
            }
            //
            return PartialView("_GetHotelSurcharge", surcharges);
        }

        [HttpPost]
        public ActionResult DeleteHotelSurcharge(ObjectModel objectModel)
        {
            var packageDelete = _hotelPackageSurchargeService.ODataQueryable().FirstOrDefault(a => a.Id == objectModel.IntParam1);
            if (packageDelete != null)
            {
                _hotelPackageSurchargeService.Delete(packageDelete);
                try
                {
                    _unitOfWorkAsync.SaveChanges();
                    objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                    objectModel.Message = string.Format("Package is Deleted!");
                }
                catch (Exception)
                {
                    objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                    objectModel.Message = string.Format("Delete Package is error!");
                }
            }
            return Json(objectModel);
        }

        [HttpPost]

        public ActionResult InsertHotelSurcharge(ObjectModel objectModel)
        {
            var checkIn = DateTime.ParseExact(objectModel.StrParam1, "MM/dd/yyyy", null);
            var checkOut = DateTime.ParseExact(objectModel.StrParam2, "MM/dd/yyyy", null);
            if (objectModel.IntParam1 == -1)
            {
                var packages = _hotelPackageService.ODataQueryable().Where(a => a.HotelId == objectModel.Id).ToList();
                if (packages.Count > 0)
                {
                    foreach (var package in packages)
                    {
                        var surch = new HotelPackage_Surcharge()
                        {
                            Package_Id = package.Id,
                            Hotel_Id = objectModel.Id,
                            DateOfWeek = objectModel.StrParam3,
                            Price = objectModel.DecParam1,
                            FromDate = checkIn,
                            ToDate = checkOut,
                            SurchargeName = objectModel.StrParam4
                        };
                        _hotelPackageSurchargeService.Insert(surch);
                        try
                        {
                            _unitOfWorkAsync.SaveChanges();
                            objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                            objectModel.Message = string.Format("Add surcharge is successfully!");
                        }
                        catch (Exception e)
                        {
                            objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                            objectModel.Message = string.Format("Add surcharge is not successfully!" + e.InnerException);
                        }
                        //UpdateRates(checkIn, checkOut, package.Id);

                    }
                    return Json(objectModel);
                }
            }
            else
            {

                var surcharge = new HotelPackage_Surcharge
                {
                    Package_Id = objectModel.IntParam1,
                    Hotel_Id = objectModel.Id,
                    DateOfWeek = objectModel.StrParam3,
                    Price = objectModel.DecParam1,
                    FromDate = checkIn,
                    ToDate = checkOut,
                    SurchargeName = objectModel.StrParam4
                };
                _hotelPackageSurchargeService.Insert(surcharge);

                try
                {
                    _unitOfWorkAsync.SaveChanges();
                    objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                    objectModel.Message = string.Format("Add surcharge is successfully!");
                }
                catch
                {
                    objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                    objectModel.Message = string.Format("Add surcharge is not successfully!");
                }
            }

            UpdateRates(checkIn, checkOut, objectModel.IntParam1);
            return Json(objectModel);
        }

        public ActionResult HotelPackage(int id)
        {
            var hotel = _hotelService.GetHotelSingle(id);
            var hotelPackage = _hotelPackageService.ODataQueryable().Where(a => a.HotelId == id).ToList();
            ViewBag.hotelPackage = hotelPackage;
            ViewBag.PromotionTypes = _catDetailService.GetCategoriesDetail(17);
            ViewBag.Cancellations = _cancellationService.GetCancellationList();
            ViewBag.HotelCancellation = _hotelCancellationService.GetCancellationHotelById(id);
            ViewBag.Photos = _hotelService.GetRoomImagesByHotelId(id);
            return View(hotel ?? new Hotel());
        }
        public ActionResult RoomControl(int id)
        {
            var hotel = _hotelService.GetHotelSingle(id);
            ViewBag.RoomTypes = _hotelService.GetRoomByHotelId(id);
            return View(hotel ?? new Hotel());
        }
        [HttpPost]
        public ActionResult GetRoomControl(ObjectModel objectModel)
        {
            var checkIn = DateTime.ParseExact(objectModel.StrParam1, "MM/dd/yyyy", null);
            var checkOut = DateTime.ParseExact(objectModel.StrParam2, "MM/dd/yyyy", null);
            var rooms = _roomControlService.GetRoomControlByRoomId(objectModel.IntParam1, checkIn, checkOut,
                objectModel.Id);
            return PartialView("_RoomControl", rooms);
        }
        [HttpPost]
        public ActionResult UpdateRoomControl(List<RoomControlModel> objectModels)
        {
            var result = InsertOrUpdateRooms(objectModels);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region  Rate Control
        public ActionResult RateControl(int id)
        {
            var hotel = _hotelService.GetHotelSingle(id);
            ViewBag.RoomTypes = _hotelService.GetRoomByHotelId(id);
            return View(hotel ?? new Hotel());
        }
        [HttpPost]
        public ActionResult GetRateControl(ObjectModel objectModel)
        {
            var checkIn = DateTime.ParseExact(objectModel.StrParam1, "MM/dd/yyyy", null);
            var checkOut = DateTime.ParseExact(objectModel.StrParam2, "MM/dd/yyyy", null);
            var rooms = _roomControlService.GetRoomControlByRoomId(objectModel.IntParam1, checkIn, checkOut,
                objectModel.Id);
            return PartialView("_RateControl", rooms);
        }
        public ActionResult UpdateRateControl(List<RoomControlModel> objectModels)
        {
            var result = InsertOrUpdateRates(objectModels);
            //return Json(result, JsonRequestBehavior.AllowGet);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

        #region //Surcharge
        public ActionResult Surcharge(int id)
        {
            var hotel = _hotelService.GetHotelSingle(id);
            ViewBag.RoomTypes = _hotelService.GetRoomByHotelId(id);
            ViewBag.Surcharges = _catDetailService.GetCategoriesDetail(16);
            return View(hotel ?? new Hotel());
        }
        [HttpPost]
        public ActionResult GetSurcharge(ObjectModel objectModel)
        {
            var surcharges = _surchargeService.GetSurchargesByRoomId(objectModel.IntParam1, objectModel.Id);
            return PartialView("_Surcharge", surcharges);
        }

        [HttpPost]
        public ActionResult InsertSurcharge(ObjectModel objectModel)
        {
            var checkIn = DateTime.ParseExact(objectModel.StrParam1, "MM/dd/yyyy", null);
            var checkOut = DateTime.ParseExact(objectModel.StrParam2, "MM/dd/yyyy", null);
            if (objectModel.IntParam1 == -1)
            {
                var rooms = _roomService.GetRoomList(objectModel.Id).ToList();
                if (rooms.Count > 0)
                {
                    foreach (var room in rooms)
                    {
                        var surch = new Surcharge
                        {
                            RoomId = room.Id,
                            HotelId = objectModel.Id,
                            DateOfWeek = objectModel.StrParam3,
                            Price = objectModel.DecParam1,
                            StayDateFrom = checkIn,
                            StayDateTo = checkOut,
                            SurchargeName = objectModel.StrParam4
                        };
                        _surchargeService.Insert(surch);
                        try
                        {
                            _unitOfWorkAsync.SaveChanges();
                            objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                            objectModel.Message = string.Format("Add surcharge is successfully!");
                        }
                        catch
                        {
                            objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                            objectModel.Message = string.Format("Add surcharge is not successfully!");
                        }
                        UpdateRates(checkIn, checkOut, room.Id);

                    }
                    return Json(objectModel);
                }
            }
            var surcharge = new Surcharge
            {
                RoomId = objectModel.IntParam1,
                HotelId = objectModel.Id,
                DateOfWeek = objectModel.StrParam3,
                Price = objectModel.DecParam1,
                StayDateFrom = checkIn,
                StayDateTo = checkOut,
                SurchargeName = objectModel.StrParam4
            };
            _surchargeService.Insert(surcharge);

            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Add surcharge is successfully!");
            }
            catch
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Add surcharge is not successfully!");
            }
            UpdateRates(checkIn, checkOut, objectModel.IntParam1);
            return Json(objectModel);
        }
        public void UpdateRates(DateTime fromDate, DateTime toDate, int roomId)
        {
            var rooms = _roomControlService.GetRoomListCheckInOut(roomId, fromDate, toDate).ToList();// _db.RoomControls.Where(a => a.RoomDate >= fromDate && a.RoomDate <= toDate && a.HOTELDETAIL.HOTELID == hotelId && a.HotelDetailID == roomId).ToList();
            //if (roomId == -1)
            //{
            //    rooms = _roomControlRepository.GetRoomListCheckInOut(roomId, fromDate, toDate).ToList();
            //}

            while (fromDate < toDate)
            {
                var room = rooms.FirstOrDefault(a => a.RoomDate.Equals(fromDate));
                if (room != null && fromDate.Date >= DateTime.Now.Date)
                {
                    decimal sur1 = 0;
                    decimal sur2 = 0;
                    decimal sur3 = 0;
                    var surchangs = GetSurchargesByDate(fromDate, roomId);
                    if (surchangs != null && surchangs.Count > 0)
                    {
                        sur1 = surchangs[0].Price ?? 0;
                    }
                    if (surchangs != null && surchangs.Count > 1)
                    {
                        sur2 = surchangs[1].Price ?? 0;
                    }
                    if (surchangs != null && surchangs.Count > 2)
                    {
                        sur3 = surchangs[2].Price ?? 0;
                    }
                    room.FinalPrice = (room.SellingRate + sur1 + sur2 + sur3) ?? 0;
                    room.Surcharge1 = sur1;
                    room.Surcharge2 = sur2;
                    room.CompulsoryMeal = sur3;
                    room.ObjectState = ObjectState.Modified;
                    _roomControlService.Update(room);
                    try
                    {
                        _unitOfWorkAsync.SaveChanges();
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
                fromDate = fromDate.AddDays(1);
            }
        }
        private void RomoveRates(DateTime fromDate, DateTime toDate, int roomId, int surchageId)
        {
            var rooms = _roomControlService.GetRoomListCheckInOut(roomId, fromDate, toDate).ToList();
            var surchang = _surchargeService.GetSurcharge(surchageId);
            while (fromDate < toDate)
            {
                var room = rooms.FirstOrDefault(a => a.RoomDate.Equals(fromDate));
                if (room != null && fromDate.Date >= DateTime.Now.Date)
                {
                    if (surchang != null && room.Surcharge1 == surchang.Price)
                    {
                        room.Surcharge1 = 0;
                    }
                    else if (surchang != null && room.Surcharge2 == surchang.Price)
                    {
                        room.Surcharge2 = 0;
                    }
                    else if (surchang != null && room.CompulsoryMeal == surchang.Price)
                    {
                        room.CompulsoryMeal = 0;
                    }
                    room.FinalPrice = (
                        room.SellingRate +
                        room.Surcharge1 +
                        room.Surcharge2 +
                        room.CompulsoryMeal) ?? 0;

                    room.ObjectState = ObjectState.Modified;
                    _roomControlService.Update(room);
                    try
                    {
                        _unitOfWorkAsync.SaveChanges();
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
                fromDate = fromDate.AddDays(1);
            }

        }
        private List<Surcharge> GetSurchargesByDate(DateTime date, int roomId)
        {
            var surcharges = _surchargeService.GetSurchargesByRoomId(date, roomId).ToList();
            if (surcharges.Count <= 0) return new List<Surcharge>();
            var week = date.ToString("ddd");
            return (from surcharge in surcharges where surcharge != null let array = surcharge.DateOfWeek.Split(',') where array.Contains(week) select surcharge).ToList();
        }
        [HttpPost]
        public ActionResult DeleteSurcharge(ObjectModel objectModel)
        {
            var surcharge = _surchargeService.GetSurcharge(objectModel.IntParam1);
            if (surcharge != null)
            {
                surcharge.ObjectState = ObjectState.Deleted;
                _surchargeService.Delete(surcharge);
                RomoveRates(surcharge.StayDateFrom, surcharge.StayDateTo, surcharge.RoomId, surcharge.Id);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Delete this record is successfully!");

            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Deleta this record is error!");
                throw;
            }
            return Json(objectModel);
        }
        #endregion

        #region //SEO
        public ActionResult HotelSeo(int id)
        {
            var hotel = _hotelService.GetHotelSingle(id);
            return View(hotel ?? new Hotel());
        }
        [HttpPost]
        public ActionResult UpdateSeoHotel(Hotel hotelModel)
        {
            var objectResult = new ObjectModel();
            var hotel = _hotelService.GetHotelSingle(hotelModel.Id);

            //Update hotel
            if (hotel != null)
            {
                hotel.SeoTitle = hotelModel.SeoTitle;
                hotel.SeoTitleVn = hotelModel.SeoTitleVn;
                hotel.SeoTitleVn2 = hotelModel.SeoTitleVn2;
                hotel.SeoDesc = hotelModel.SeoDesc;
                hotel.SeoDescVn = hotelModel.SeoDescVn;
                hotel.SeoDescVn2 = hotelModel.SeoDescVn2;
                hotel.HotelKeyword = hotelModel.HotelKeyword;
                hotel.KeywordVn = hotelModel.KeywordVn;
                hotel.KeywordVn2 = hotelModel.KeywordVn2;
                hotel.Description = hotelModel.Description;
                hotel.ImportantNoteVn = hotelModel.ImportantNoteVn;
                hotel.ImportantNoteEn = hotelModel.ImportantNoteEn;
                hotel.HotelNameLocalDesc = hotelModel.HotelNameLocalDesc;
                hotel.HashTagEn = hotelModel.HashTagEn;
                hotel.HashTagVn = hotelModel.HashTagVn;
                hotel.HotelNameLocalDesc2 = hotelModel.HotelNameLocalDesc2;
                hotel.IsSeo = true;

                hotel.ObjectState = ObjectState.Modified;
                _hotelService.Update(hotel);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectResult.Status = (int)Web.Framework.Utilities.Status.Active;
                objectResult.Message = string.Format("SEO of hotel is updated!");
            }
            catch (Exception)
            {
                objectResult.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectResult.Message = string.Format("Update SEO of hotel is error!");
            }
            return Json(objectResult);
        }
        //Promotion
        public ActionResult Promotion(int id)
        {
            var hotel = _hotelService.GetHotelSingle(id);
            //ViewBag.Promotions = _promotionService.GetPromotionByHotelId(id);
            var promotions = _promotionService.GetPromotionByHotelId(id);
            if (promotions.Count > 0)
            {
                foreach (var promotion in promotions)
                {
                    if (promotion == null)
                    {
                        continue;
                    }
                    var room = _roomService.GetRoom(promotion.RoomId);
                    promotion.Description = room != null ? room.Name : "Apply for all room";

                }
            }
            ViewBag.Promotions = promotions;
            ViewBag.PromotionTypes = _catDetailService.GetCategoriesDetail(17);
            ViewBag.Cancellations = _cancellationService.GetCancellationList();
            ViewBag.HotelCancellation = _hotelCancellationService.GetCancellationHotelById(id);
            ViewBag.RoomTypes = _hotelService.GetRoomByHotelId(id);
            return View(hotel ?? new Hotel());
        }
        //Promotion
        public ActionResult Review(int id)
        {
            var hotel = _hotelService.GetHotelSingle(id);
            ViewBag.Reviews = _reviewService.GetReviews(id);
            ViewBag.Categories = _catDetailService.GetCategoriesDetail(14);
            ViewBag.Nationalities = _nationalityService.GetNationalityList();
            return View(hotel ?? new Hotel());
        }
        [HttpPost]
        public ActionResult DeleteReview(ObjectModel objectModel)
        {
            var review = _reviewService.GetReview(objectModel.IntParam1);
            var reviewDetails = _reviewDetailService.GetReviews(objectModel.IntParam1);
            if (reviewDetails.Count > 0)
            {
                foreach (var reviewDetail in reviewDetails)
                {
                    reviewDetail.ObjectState = ObjectState.Deleted;
                    _reviewDetailService.Delete(reviewDetail);
                }
            }

            if (review != null)
            {
                review.ObjectState = ObjectState.Deleted;
                _reviewService.Delete(review);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Delete this record is successfully!");

            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Deleta this record is error!");
                throw;
            }
            return Json(objectModel);
        }

        [HttpPost]
        public JsonResult AddNew(Review review)
        {
            var objectModel = new ObjectModel();
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            var listReview = review.ReviewDetails;
            review.ReviewDetails = null;
            review.CreateDate = DateTime.Now;
            review.ModifiedDate = DateTime.Now;
            review.Status = true;
            review.ObjectState = ObjectState.Added;
            _reviewService.Add(review);

            foreach (var item in listReview)
            {
                item.ReviewId = review.Id;
                item.CreateDate = DateTime.Now;
                item.Status = true;
                item.ObjectState = ObjectState.Added;
                _reviewDetailService.Insert(item);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Add review is successfully!");
            }
            catch
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Add review is not successfully!");
            }
            return Json(objectModel);
        }

        #endregion

        #region ---Private Function---
        public ObjectModel InsertOrUpdateRooms(List<RoomControlModel> roomsModels)
        {
            var objectResult = new ObjectModel();
            foreach (var roomModel in roomsModels.Where(roomsModel => roomsModel.RoomId != -1 && roomsModel.Date != DateTime.MinValue))
            {
                var room = _roomControlService.GetRoomControlByRoomIdRoomDate(roomModel.RoomId, roomModel.Date);
                if (room != null)
                {
                    room.RoomId = roomModel.RoomId;
                    room.AutoTopUp = roomModel.AutoTopUp;
                    room.CloseOutRegular = roomModel.CloseOutRegular;
                    room.Guaranteed = roomModel.Guaranteed;
                    room.Regular = roomModel.Regular;
                    room.UseRegular = roomModel.UseRegular;
                    room.TotalAvailable = (roomModel.Guaranteed + roomModel.Regular + roomModel.AutoTopUp) -
                                          (roomModel.UseGuaranteed + roomModel.UseRegular);
                    room.UseGuaranteed = roomModel.UseGuaranteed;
                    room.RoomDate = roomModel.Date;
                    room.ObjectState = ObjectState.Modified;
                    _roomControlService.Update(room);
                }
                else
                {
                    room = new RoomControl
                    {
                        RoomId = roomModel.RoomId,
                        AutoTopUp = roomModel.AutoTopUp,
                        CloseOutRegular = roomModel.CloseOutRegular,
                        Guaranteed = roomModel.Guaranteed,
                        Regular = roomModel.Regular,
                        UseRegular = roomModel.UseRegular,
                        TotalAvailable = (roomModel.Guaranteed + roomModel.Regular + roomModel.AutoTopUp),
                        UseGuaranteed = roomModel.UseGuaranteed,
                        RoomDate = roomModel.Date,
                        ObjectState = ObjectState.Added
                    };
                    _roomControlService.Insert(room);
                }
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectResult.Status = (int)Web.Framework.Utilities.Status.Active;
                objectResult.Message = string.Format("All rooms control is updated!");
            }
            catch (Exception)
            {
                objectResult.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectResult.Message = string.Format("Update rooms control is error!");
            }
            return objectResult;
        }
        public ObjectModel InsertOrUpdateRates(List<RoomControlModel> roomsModels)
        {
            var objectResult = new ObjectModel();
            foreach (var roomModel in roomsModels.Where(roomsModel => roomsModel.RoomId != -1 && roomsModel.Date != DateTime.MinValue))
            {
                var room = _roomControlService.GetRoomControlByRoomIdRoomDate(roomModel.RoomId, roomModel.Date);// (//_roomControlService.GetRoomControlByRoomId(roomModel.Id);
                if (room != null)
                {
                    room.TaRate = roomModel.TaRate;
                    room.Profit = roomModel.Profit;
                    room.CompulsoryMeal = roomModel.CompulsoryMeal;
                    room.Surcharge1 = roomModel.Surcharge1;
                    room.Surcharge2 = roomModel.Surcharge2;
                    room.SellingRate = roomModel.SellingRate;
                    room.FinalPrice = roomModel.FinalPrice;
                    room.Breakfast = roomModel.IsBreakfast;

                    room.ObjectState = ObjectState.Modified;
                    _roomControlService.Update(room);
                }
                else
                {
                    room = new RoomControl
                    {
                        RoomId = roomModel.RoomId,
                        AutoTopUp = 0,
                        CloseOutRegular = false,
                        Guaranteed = 0,
                        Regular = 0,
                        UseRegular = 0,
                        TotalAvailable = 0,
                        UseGuaranteed = 0,
                        RoomDate = roomModel.Date,
                        TaRate = roomModel.TaRate,
                        Profit = roomModel.Profit,
                        Breakfast = roomModel.IsBreakfast,
                        CompulsoryMeal = roomModel.CompulsoryMeal,
                        Surcharge1 = roomModel.Surcharge1,
                        Surcharge2 = roomModel.Surcharge2,
                        SellingRate = roomModel.SellingRate,
                        FinalPrice = roomModel.FinalPrice,
                        ObjectState = ObjectState.Added
                    };
                    _roomControlService.Insert(room);
                }
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectResult.Status = (int)Web.Framework.Utilities.Status.Active;
                objectResult.Message = string.Format("All rates control is updated!");
            }
            catch (Exception ex)
            {
                objectResult.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectResult.Message = string.Format("Update rates control is error! " + ex.Message + ex.InnerException);
            }
            return objectResult;
        }
        #endregion

        #region Room 
        [HttpPost]
        public ActionResult GetPersonDetailByTourId(int TourId)
        {
            var tourPrice = _tourRateService.GetTourRatesByTourId(TourId);
            return Json(tourPrice);
        }

        [HttpPost]
        public ActionResult GetRoomDetailByHotelId(int HotelId)
        {
            //var room = _roomService.GetRoom(objectModel.IntParam1) ?? new Room();
            var roomList = _roomService.GetRoomList(HotelId);
            return Json(roomList);
        }

        [HttpPost]
        public ActionResult GetRoom(ObjectModel objectModel)
        {
            ViewBag.RoomViews = _catDetailService.GetCategoriesDetail(15);
            var room = _roomService.GetRoom(objectModel.IntParam1) ?? new Room();
            ViewBag.RoomFacilities = _catDetailService.CategoriesList(room.RoomFacilities ?? "", 3);
            ViewBag.Photos = _hotelService.GetRoomImagesByHotelId(objectModel.Id);
            return PartialView("_RoomForm", room);
        }

        [HttpPost]
        public ActionResult AddRoom(ObjectModel objectModel)
        {
            var room = new Room
            {
                Name = objectModel.StrParam1,
                HotelId = objectModel.Id,
                ObjectState = ObjectState.Added
            };
            _roomService.AddRoom(room);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.IntParam1 = room.Id;
                objectModel.Message = string.Format("Room is added!");
            }
            catch (Exception)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Add room is error!");
            }
            return Json(objectModel);
        }
        #endregion

        #region Package
        [HttpPost]
        public ActionResult AddPackage(HotelPackage package)
        {
            var cancellations = _cancellationService.GetCancellationList();
            var cancellation = cancellations.Find(a => a.Id == package.CancellationId);
            var room = new HotelPackage()
            {
                RoomName = package.RoomName,
                RoomNameVN = package.RoomNameVN,
                HotelId = package.HotelId,
                Price = package.Price,
                FromDate = package.FromDate,
                CancellationNameVn = cancellation != null ? cancellation.DescriptionVn : "",
                ToDate = package.ToDate,
                CancellationId = package.CancellationId,
                CancellationName = package.CancellationName,
                Including = package.Including,
                ImageUrl = package.ImageUrl,
                Night = package.Night,
                IncludingValue = package.IncludingValue,
                IncludingValueVN = package.IncludingValueVN
            };
            _hotelPackageService.Insert(room);
            if (package.IsPromotion.HasValue && package.IsPromotion.Value)
            {
                var priceFake = package.Price;

                if (package.DiscountValue.HasValue && package.DiscountValue.Value > 0)
                {
                    var price = package.Price - (priceFake * package.DiscountValue.Value / 100);
                    var roomPromotion = new HotelPackage()
                    {
                        RoomName = package.RoomName,
                        HotelId = package.HotelId,
                        PriceFake = package.Price,
                        Price = price,
                        FromDate = package.FromDate,
                        ToDate = package.ToDate,
                        CancellationId = 72,
                        CancellationName = "Non Refundable",
                        CancellationNameVn = cancellation != null ? cancellation.DescriptionVn : "",
                        Including = package.Including,
                        Night = package.Night,
                        IsPromotion = package.IsPromotion,
                        DiscountValue = package.DiscountValue,
                        IncludingValue = package.IncludingValue
                    };
                    _hotelPackageService.Insert(roomPromotion);
                }
            }
            ObjectModel objectModel = new ObjectModel();
            try
            {
                _unitOfWorkAsync.SaveChanges();

                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.IntParam1 = room.Id;
                objectModel.Message = string.Format("Package is added!");
            }
            catch (Exception ex)
            {

                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Add Package is error!" + ex.Message);
            }
            return Json(objectModel);
        }

        [HttpPost]
        public ActionResult UpdatePackage(HotelPackage package)
        {
            var packageUpdate = _hotelPackageService.ODataQueryable().FirstOrDefault(a => a.Id == package.Id);
            var cancellations = _cancellationService.GetCancellationList();
            if (packageUpdate != null)
            {
                var cancellation = cancellations.Find(a => a.Id == package.CancellationId);
                packageUpdate.RoomName = package.RoomName;
                packageUpdate.RoomName = package.RoomName;
                packageUpdate.RoomNameVN = package.RoomNameVN;
                packageUpdate.Price = package.Price;
                packageUpdate.HotelId = package.HotelId;
                packageUpdate.HotelId = package.HotelId;
                packageUpdate.FromDate = package.FromDate;
                packageUpdate.ToDate = package.ToDate;
                packageUpdate.CancellationId = package.CancellationId;
                packageUpdate.ImageUrl = package.ImageUrl;
                packageUpdate.CancellationName = package.CancellationName;
                packageUpdate.CancellationNameVn = cancellation != null ? cancellation.DescriptionVn : "";
                packageUpdate.Including = package.Including;
                packageUpdate.Night = package.Night;
                packageUpdate.IncludingValue = package.IncludingValue;
                packageUpdate.IncludingValueVN = package.IncludingValueVN;
                packageUpdate.ObjectState = ObjectState.Modified;
            }

            ObjectModel objectModel = new ObjectModel();
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.IntParam1 = packageUpdate.Id;
                objectModel.Message = string.Format("Package is updated!");
            }
            catch (Exception ex)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Add Package is error!" + ex.Message);
            }
            return Json(objectModel);
        }

        [HttpPost]
        public ActionResult DeletePackage(HotelPackage package)
        {
            var packageDelete = _hotelPackageService.ODataQueryable().FirstOrDefault(a => a.Id == package.Id);
            ObjectModel objectModel = new ObjectModel();
            if (packageDelete != null)
            {
                var surcharge = _hotelPackageSurchargeService.ODataQueryable().Where(a => a.Package_Id == package.Id);
                if (surcharge.Any())
                {
                    foreach (var item in surcharge)
                    {
                        _hotelPackageSurchargeService.Delete(item);
                    }
                    _unitOfWorkAsync.SaveChanges();
                }

                _hotelPackageService.Delete(packageDelete);
                try
                {
                    _unitOfWorkAsync.SaveChanges();
                    objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                    objectModel.Message = string.Format("Package is Deleted!");
                }
                catch (Exception ex)
                {
                    objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                    objectModel.Message = string.Format("Delete Package is error + !" + ex.Message);
                }
            }
            return Json(objectModel);
        }

        [HttpPost]
        public ActionResult GetHotelPackage(int? id)
        {
            var package = _hotelPackageService.ODataQueryable().FirstOrDefault(a => a.Id == id);

            ViewBag.hotelPackage = package;
            ViewBag.PromotionTypes = _catDetailService.GetCategoriesDetail(17);
            ViewBag.Cancellations = _cancellationService.GetCancellationList();
            ViewBag.Photos = _hotelService.GetRoomImagesByHotelId(package.HotelId);
            if (package != null)
                ViewBag.HotelCancellation = _hotelCancellationService.GetCancellationHotelById(package.CancellationId);
            return PartialView("_HotelPackageEdit", package);
        }


        #endregion

        #region Room Facility

        [HttpPost]
        public ActionResult UpdateFacility(ObjectModel objectModel)
        {
            var room = _roomService.GetRoom(objectModel.IntParam1);
            if (room != null)
            {

                room.RoomFacilities = UpdateCategory(room.RoomFacilities, objectModel.Id, objectModel.StrParam1);
                room.ObjectState = ObjectState.Modified;
                _roomService.UpdateRoom(room);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Facility is updated!");
            }
            catch (Exception)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Facility is error!");
            }
            ViewBag.RoomFacilities = _catDetailService.CategoriesList(room.RoomFacilities, 3);
            return PartialView("_RoomFacilities");
        }
        private static string UpdateCategory(string hotelCategory, int categoryId, string type)
        {
            if (!type.Contains("Remove")) return $"{hotelCategory},{categoryId}";
            var result = string.Empty;
            var categories = Web.Framework.Utilities.GetSelectedItem(hotelCategory);
            return categories.Where(category => category.Value != categoryId).Aggregate(result, (current, category) => current + $"{category.Value},");
        }
        [HttpPost]
        public ActionResult UpdateRoom(Room hotelDetail)
        {
            var room = _roomService.GetRoom(hotelDetail.Id);

            if (hotelDetail.ImageUrl != null)
            {
                room.ImageUrl = hotelDetail.ImageUrl;
            }

            if (room != null)
            {
                room.Name = hotelDetail.Name;
                room.View = hotelDetail.View;
                room.RoomSize = hotelDetail.RoomSize;
                room.Sort = hotelDetail.Sort;
                room.BreakfastSurcharge = hotelDetail.BreakfastSurcharge;
                room.ExtraBed = hotelDetail.ExtraBed;
                room.ExtraBedPrice = hotelDetail.ExtraBedPrice;
                room.AdultNumber = hotelDetail.AdultNumber;
                room.ChildrenNumber = hotelDetail.ChildrenNumber;
                room.ChildrenAge = hotelDetail.ChildrenAge;
                room.MaxOccupancy = hotelDetail.MaxOccupancy;
                room.ObjectState = ObjectState.Modified;
                _roomService.UpdateRoom(room);
            }
            var objectModel = new ObjectModel();
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Room is updated!");
            }
            catch (Exception)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Update room is error!");
            }
            return Json(objectModel);
        }
        [HttpPost]
        public ActionResult DeleteRoom(Room hotelDetail)
        {
            var room = _roomService.GetRoom(hotelDetail.Id);
            if (room != null)
            {
                room.Status = false;
                room.ObjectState = ObjectState.Modified;
                _roomService.UpdateRoom(room);
            }
            var objectModel = new ObjectModel();
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Room is updated!");
            }
            catch (Exception)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Update room is error!");
            }
            return Json(objectModel);
        }

        #endregion

        #region Promotion
        //Promotion
        [HttpGet]
        public ActionResult GetPromotionForm(ObjectModel objectModel)
        {
            ViewBag.PromotionTypes = _catDetailService.GetCategoriesDetail(17);
            ViewBag.Cancellations = _cancellationService.GetCancellationList();
            ViewBag.HotelCancellation = _hotelService.GetHotelCancellationPoliciesByHotelId(objectModel.Id);
            ViewBag.RoomTypes = _hotelService.GetRoomByHotelId(objectModel.Id);
            return PartialView("_Promotion");
        }
        public ActionResult InsertPromotion(Promotion promotionModel)
        {
            var objectModel = new ObjectModel();
            if (promotionModel == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Add promotion is error!";
                return Json(objectModel);
            }
            promotionModel.CreateDate = DateTime.Now;
            promotionModel.ObjectState = ObjectState.Added;
            promotionModel.Status = true;
            promotionModel.BookingDateFrom = promotionModel.BookingDateFrom ?? DateTime.Now;
            promotionModel.BookingDateTo = promotionModel.BookingDateTo ?? DateTime.Now;
            promotionModel.CheckIn = promotionModel.CheckIn ?? DateTime.Now;
            promotionModel.CheckOut = promotionModel.CheckOut ?? DateTime.Now;

            //Insert promotion
            _promotionService.Insert(promotionModel);

            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Promotion is added!");
            }
            catch (Exception)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Add promotion is error!");
            }

            return Json(objectModel);
        }
        public ActionResult DeletePromotion(ObjectModel objectModel)
        {
            //delete promotion
            var promotion = _promotionService.GetPromotionId(objectModel.Id);
            if (promotion == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Delete is error!");
                return Json(objectModel);
            }
            promotion.ObjectState = ObjectState.Deleted;
            _promotionService.Delete(promotion);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Delete this record is successfully!");

            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Delete is error!");
                throw;
            }
            return Json(objectModel);
        }
        #endregion

        #region TourReview
        public ActionResult TourReview()
        {
            var list = _tourReviewService.Queryable().ToList();
            return View(list);
        }

        [HttpPost]
        public ActionResult DeleteTourReview(TourReview couponCode)
        {
            var deleteItem = _tourReviewService.Queryable().FirstOrDefault(a => a.Id == couponCode.Id);
            var objectModel = new ObjectModel();
            if (deleteItem == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Delete TourReview is error!";
                return Json(objectModel);
            }
            _tourReviewService.Delete(deleteItem);

            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("TourReview is Deleted!");
            }
            catch (Exception)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Delete TourReview is error!");
            }

            return Json(objectModel);
        }

        #endregion

        #region Tour_Surcharge
        [HttpPost]
        public ActionResult InsertTourSurcharge(ObjectModel objectModel)
        {
            var checkIn = DateTime.ParseExact(objectModel.StrParam1, "MM/dd/yyyy", null);
            var checkOut = DateTime.ParseExact(objectModel.StrParam2, "MM/dd/yyyy", null);
            var surcharge = new Tour_Surcharge
            {
                TourId = objectModel.Id,
                DateOfWeek = objectModel.StrParam3,
                Price = objectModel.DecParam1,
                Type = objectModel.IntParam1,
                StayDateFrom = checkIn,
                StayDateTo = checkOut,
                SurchargeName = objectModel.StrParam4
            };
            _tourSurchargeService.Insert(surcharge);

            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Add surcharge is successfully!");
            }
            catch
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Add surcharge is not successfully!");
            }
            // UpdateRates(checkIn, checkOut, objectModel.IntParam1);
            return Json(objectModel);
        }
        [HttpPost]
        public ActionResult GetTourSurcharge(ObjectModel objectModel)
        {
            var surcharges = _tourSurchargeService.GetSurchargesByTourId(objectModel.Id);
            return PartialView("_TourSurcharge", surcharges);
        }
        [HttpPost]
        public ActionResult DeleteTourSurcharge(ObjectModel objectModel)
        {
            var surcharge = _tourSurchargeService.GetSurcharge(objectModel.IntParam1);
            if (surcharge != null)
            {
                surcharge.ObjectState = ObjectState.Deleted;
                _tourSurchargeService.Delete(surcharge);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Delete this record is successfully!");

            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Deleta this record is error!");
                throw;
            }
            return Json(objectModel);
        }


        #endregion

        #region Tour_promotion
        [HttpPost]
        public ActionResult InsertTourPromotion(Tour_Promotion promotionModel)
        {
            var objectModel = new ObjectModel();
            if (promotionModel == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Add promotion is error!";
                return Json(objectModel);
            }
            promotionModel.CreateDate = DateTime.Now;
            promotionModel.ObjectState = ObjectState.Added;
            promotionModel.Status = true;
            promotionModel.BookingDateFrom = promotionModel.BookingDateFrom ?? DateTime.Now;
            promotionModel.BookingDateTo = promotionModel.BookingDateTo ?? DateTime.Now;
            promotionModel.CheckIn = promotionModel.CheckIn ?? DateTime.Now;
            promotionModel.CheckOut = promotionModel.CheckOut ?? DateTime.Now;

            //Insert promotion
            _tourPromotionService.Insert(promotionModel);

            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Promotion is added!");
            }
            catch (Exception)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Add promotion is error!");
            }

            return Json(objectModel);
        }

        public ActionResult TourPromotion(int id)
        {
            var tour = _tourService.Queryable().FirstOrDefault(t => t.Id == id);
            ViewBag.Category = _categoryService.GetCategoryById(8);
            var promotions = _tourPromotionService.GetPromotionsByTourId(id);
            ViewBag.Promotions = promotions;
            ViewBag.PromotionTypes = _catDetailService.GetCategoriesDetail(17);
            ViewBag.Cancellations = _cancellationService.GetCancellationList();
            ViewBag.HotelCancellation = _hotelCancellationService.GetCancellationHotelById(id);
            ViewBag.RoomTypes = _hotelService.GetRoomByHotelId(id);

            return View(tour);
        }

        public ActionResult DeleteTourPromotion(ObjectModel objectModel)
        {
            //delete promotion
            var promotion = _tourPromotionService.GetPromotionId(objectModel.Id);
            if (promotion == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Delete is error!");
                return Json(objectModel);
            }
            promotion.ObjectState = ObjectState.Deleted;
            _tourPromotionService.Delete(promotion);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Delete this record is successfully!");

            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Delete is error!");
                throw;
            }
            return Json(objectModel);
        }
        #endregion

        #region Tour Price Option
        [HttpPost]
        public ActionResult InsertTourPriceOption(TourRateOptions promotionModel)
        {
            var objectModel = new ObjectModel();
            if (promotionModel == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Add promotion is error!";
                return Json(objectModel);
            }
            promotionModel.ObjectState = ObjectState.Added;
            //Insert promotion
            _tourrateOptionsService.Insert(promotionModel);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Promotion is added!");
            }
            catch (Exception)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Add promotion is error!");
            }

            return Json(objectModel);
        }

        public ActionResult TourPriceOption(int id)
        {
            var tour = _tourService.Queryable().FirstOrDefault(t => t.Id == id);
            var tourRateOption = _tourrateOptionsService.Queryable().Where(a => a.Tour_Id == id).ToList();
            ViewBag.TourRateOptions = tourRateOption;
            return View(tour);
        }

        public ActionResult DeleteTourPriceOption(ObjectModel objectModel)
        {
            //delete promotion
            var promotion = _tourrateOptionsService.Queryable().FirstOrDefault(a => a.Id == objectModel.Id);
            if (promotion == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Delete is error!");
                return Json(objectModel);
            }
            promotion.ObjectState = ObjectState.Deleted;
            _tourrateOptionsService.Delete(promotion);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Delete this record is successfully!");

            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Delete is error!");
                throw;
            }
            return Json(objectModel);
        }
        #endregion
    }
}