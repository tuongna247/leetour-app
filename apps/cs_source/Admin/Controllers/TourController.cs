using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;
using Vinaday.Data.Models;
using Vinaday.Data.Models.Extention;
using Vinaday.Services;
using Vinaday.Web.Framework;
namespace Vinaday.Admin.Controllers
{
   // [Authorize(Roles = "Admin")]
    public class TourController : Controller
    {
        private readonly ITourService _tourService;
        private readonly ITourDetailService _tourDetailService;
        private readonly IRegionService _regionService;
        private readonly ICountryService _countryService;
        private readonly ICancellationService _cancellationService;
        private readonly ICategoryDetailService _categoryDetailService;
        private readonly ICategoryService _categoryService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly IMediaService _mediaService;
        private readonly ITourRateService _tourRateService;
        private readonly ISeoService _seoService;
        private readonly ICityService _cityService;
        public TourController(ITourService tourService,
            IRegionService regionService,
            ICountryService countryService,
            IUnitOfWorkAsync unitOfWorkAsync,
            ICancellationService cancellationService,
            ICategoryDetailService categoryDetailService,
            IMediaService mediaService,
            ITourDetailService tourDetailService,
            ITourRateService tourRateService,
            ICategoryService categoryService, ISeoService seoService, ICityService cityService)
        {
            _tourService = tourService;
            _regionService = regionService;
            _countryService = countryService;
            _unitOfWorkAsync = unitOfWorkAsync;
            _cancellationService = cancellationService;
            _categoryDetailService = categoryDetailService;
            _mediaService = mediaService;
            _tourDetailService = tourDetailService;
            _tourRateService = tourRateService;
            _categoryService = categoryService;
            _seoService = seoService;
            _cityService = cityService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Management()
        {
            ViewBag.Tours = _tourService.GetTours();
            return View();
        }

        public ActionResult BasicInformation(int? id)
        {
            ViewBag.Regions = _regionService.GetRegionList().ToList();
            ViewBag.Countries = _countryService.GetCountryList().ToList();
            Tour tour = ViewBag.Tour = _tourService.Queryable().FirstOrDefault(t => t.Id == id);

            var category = _categoryService.GetCategories().FirstOrDefault(c => c.KeyCode == Web.Framework.Utilities.TravelStyle);
            if (category == null) return View();
            var categoryStr = tour != null ? tour.TravelStyle : string.Empty;
            ViewBag.TravelStyles = Web.Framework.Utilities.GetCategoryList(categoryStr,
                _categoryDetailService.GetCategoriesDetail(category.Id));


            var cities = _cityService.GetCities();
            var citiStr = string.Empty;
            if (tour != null){
                citiStr = tour.Cities;
            }
            ViewBag.Cities = Web.Framework.Utilities.GetCityList(citiStr, cities);
            return View();
        }

        public ActionResult Detail(int? id)
        {
            ViewBag.Tour = _tourService.Queryable().FirstOrDefault(t => t.Id == id);
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
            return View();
        }

        public ActionResult Itinerary(int? id)
        {
            Tour tour = ViewBag.Tour = _tourService.Queryable().FirstOrDefault(t => t.Id == id);
            var categoryMeals = _categoryService.GetCategories().FirstOrDefault(c => c.KeyCode == Web.Framework.Utilities.Meals);
            var categoryTransports = _categoryService.GetCategories().FirstOrDefault(c => c.KeyCode == Web.Framework.Utilities.Transportations);

            var categoryMealStr = string.Empty;
            var categoryTransportStr = string.Empty;

            if (tour != null && tour.Type == (int)Web.Framework.Utilities.TourType.Tour)
            {
                ViewBag.TourDetails = GetTourDetailsByTourId(tour.Id);

            }
            else
            {
                var tourDetail = ViewBag.TourDetail = _tourDetailService.GetDetailTours().FirstOrDefault(d => d.TourId == id);
                categoryMealStr = tourDetail != null ? tourDetail.Meal : categoryMealStr;
                categoryTransportStr = tourDetail != null ? tourDetail.Transport : categoryTransportStr;
            }

            if (categoryMeals == null) return View();
            ViewBag.Meals = Web.Framework.Utilities.GetCategoryList(categoryMealStr,
                _categoryDetailService.GetCategoriesDetail(categoryMeals.Id));


            if (categoryTransports == null) return View();
            ViewBag.Transports = Web.Framework.Utilities.GetCategoryList(categoryTransportStr,
                _categoryDetailService.GetCategoriesDetail(categoryTransports.Id));

            return View();
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
            return tourDetails;
        }

        public ActionResult Promotion()
        {
            return View();
        }

        public ActionResult Price(int? id)
        {
            ViewBag.Rates = _tourRateService.GetTourRates().Where(d => d.TourId == id).OrderBy(d => d.PersonNo).ToList();
            var tour = ViewBag.Tour = _tourService.Queryable().FirstOrDefault(t => t.Id == id);
            var tourDetail =
                ViewBag.TourDetails = _tourDetailService.GetDetailTours().Where(d => d.TourId == id).ToList();
            var categoryMeals = _categoryService.GetCategories().FirstOrDefault(c => c.KeyCode == Web.Framework.Utilities.Meals);
            if (categoryMeals == null) return View();
            ViewBag.Meals = Web.Framework.Utilities.GetCategoryList(null,
                _categoryDetailService.GetCategoriesDetail(categoryMeals.Id));
            var categoryTransports = _categoryService.GetCategories().FirstOrDefault(c => c.KeyCode == Web.Framework.Utilities.Transportations);
            if (categoryTransports == null) return View();
            ViewBag.Transports = Web.Framework.Utilities.GetCategoryList(null,
                _categoryDetailService.GetCategoriesDetail(categoryTransports.Id));
            return View();
        }
        [HttpPost]
        public ActionResult InsertPrice(Rate rate)
        {
            var objectModel = new ObjectModel();
            //Added comlunm

            rate.CreatedDate = DateTime.Now;
            rate.ModifiedDate = DateTime.Now;
            rate.Status = (int)Web.Framework.Utilities.Status.Active;
            //Inser rate
            rate.ObjectState = ObjectState.Added;
            _tourRateService.Insert(rate);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                ViewBag.Rates = _tourRateService.GetTourRates().Where(d => d.TourId == rate.TourId).ToList();
                return PartialView("_TourPrice");
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Insert rate is error!");
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
                    //Inser rate
                    rate.ObjectState = ObjectState.Added;
                    _tourRateService.Insert(rate);
                }


            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
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
        public ActionResult InsertBasicInformation(Tour tour)
        {
            var objectModel = new ObjectModel();
            //Added comlunm

            tour.CreatedDate = DateTime.Now;
            tour.ModifiedDate = DateTime.Now;
            tour.OperatorId = 1;
            tour.Status = (int)Web.Framework.Utilities.Status.Active;
            tour.CommissionRate = 15;
            tour.TourCode = Web.Framework.Utilities.GetRandomString(7);
            //Inser tour
            tour.ObjectState = ObjectState.Added;
            _tourService.Insert(tour);
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
                EntityName = Enum.GetName(typeof(Web.Framework.Utilities.Product), tour.Type),
                ProductType = tour.Type,
                Slug = Web.Framework.Utilities.GenerateSlug(tour.Name, 200),
                IsActive = true,
                Title = string.Format("{0} ({1}) | vinaday.com", tour.Name, country != null ? country.Name : string.Empty),
                Description = tour.Description,
                Keyword = tour.Name,
                ObjectState = ObjectState.Added
            };
            _seoService.Insert(seo);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Url = string.Format("~/Tour/Detail/{0}", tour.Id);
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
        public ActionResult SaveItinerary(Detail detail)
        {
            var objectModel = new ObjectModel();
            //Added comlunm

            detail.CreatedDate = DateTime.Now;
            detail.ModifiedDate = DateTime.Now;
            detail.Status = (int)Web.Framework.Utilities.Status.Active;
            detail.Meal = detail.Meal;
            detail.Content = detail.Content;
            detail.SortOrder = detail.SortOrder;
            //Inser tour
            detail.ObjectState = ObjectState.Added;
            _tourDetailService.Insert(detail);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                //objectModel.Status = (int)Utilities.Status.Active;
                //objectModel.Url = string.Format("~/Tour/Itinerary/{0}", detail.TourId);
                //ViewBag.TourDetails = _tourDetailService.GetDetailTours().Where(d => d.TourId == detail.TourId).ToList();
                ViewBag.TourDetails = GetTourDetailsByTourId(detail.TourId);
                return PartialView("_TourItinerary");
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Insert itinerary is error!");
                throw;
            }
        }

        [HttpPost]
        public ActionResult UpdateItinerary(Detail detail)
        {
            var objectModel = new ObjectModel();
            if (detail == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Updated tour is error!");
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
                //Update tour detail
                tourDetailUpdate.ObjectState = ObjectState.Modified;
                _tourDetailService.Update(tourDetailUpdate);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Url = string.Format("~/Tour/Price/{0}", tourDetailUpdate != null ? tourDetailUpdate.TourId.ToString() : "");

            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Update tour detail is error!");
                throw;
            }
            return Json(objectModel);
        }

        public ActionResult DeleteItinerary(Detail detail)
        {
            var objectModel = new ObjectModel();
            if (detail == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Updated tour detail is error!");
                return Json(objectModel);
            }
            var detailDelete = _tourDetailService.Queryable().FirstOrDefault(t => t.TourId == detail.TourId);
            //Added comlunm
            if (detailDelete != null)
            {
                detailDelete.ObjectState = ObjectState.Deleted;
                _tourDetailService.Delete(detailDelete);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                //objectModel.Status = (int)Utilities.Status.Active;
                //objectModel.Url = string.Format("~/Tour/Itinerary/{0}", detail.TourId);
                //ViewBag.TourDetails = _tourDetailService.GetDetailTours().Where(d => d.TourId == detail.TourId).ToList();
                ViewBag.TourDetails = GetTourDetailsByTourId(detail.TourId);
                return PartialView("_TourItinerary");

            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Update tour detail is error!");
                throw;
            }
        }
        [HttpPost]
        public ActionResult UpdateBasicInformation(Tour tour)
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
                tourUpdate.Name = tour.Name;
                tourUpdate.Type = tour.Type;
                tourUpdate.CountryId = tour.CountryId;
                tourUpdate.Description = tour.Description;
                tourUpdate.Location = tour.Location;
                tourUpdate.TravelStyle = tour.TravelStyle;
                tourUpdate.Overview = tour.Overview;
                tourUpdate.Cities = tour.Cities;
                if (!string.IsNullOrEmpty(tour.TourTitle))
                {
                    tourUpdate.TourTitle = tour.TourTitle;
                }
                if (string.IsNullOrEmpty(tourUpdate.TourTitle))
                {
                    tourUpdate.TourTitle = tour.Name;
                }

                //Update tour
                tourUpdate.ObjectState = ObjectState.Modified;
                _tourService.Update(tourUpdate);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Url = string.Format("~/Tour/Detail/{0}", tour.Id);

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
        public ActionResult UpdateDetail(Tour tour)
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
                tourUpdate.Duration = tour.Duration;
                tourUpdate.Notes = tour.Notes;
                tourUpdate.CancelationPolicy = tour.CancelationPolicy;
                //Update tour
                tourUpdate.ObjectState = ObjectState.Modified;
                _tourService.Update(tourUpdate);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Url = string.Format("~/Tour/Photo/{0}", tour.Id);

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
                objectModel.Url = string.Format("~/Tour/Photo/{0}", media.OwnerId);

            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Update media is error!");
                throw;
            }
            return Json(objectModel);
        }
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
                mediaDelete.ObjectState = ObjectState.Deleted;
                _mediaService.Delete(mediaDelete);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Url = string.Format("~/Tour/Photo/{0}", media.OwnerId);

            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Update media is error!");
                throw;
            }
            return Json(objectModel);
        }
        public ActionResult DeleteTour(Tour tour)
        {
            var objectModel = new ObjectModel();
            if (tour == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("delete image is error!");
                return Json(objectModel);
            }
            var tourDelete = _tourService.Queryable().FirstOrDefault(t => t.Id == tour.Id);

            //Added comlunm
            if (tourDelete != null)
            {
                //Delete rate of tour
                var rates = _tourRateService.Queryable().Where(t => t.TourId == tourDelete.Id).ToList();
                foreach (var rate in rates.Where(rate => rate != null))
                {
                    rate.ObjectState = ObjectState.Deleted;
                    _tourRateService.Delete(rate);
                }
                //Delete tour detail of tour
                var details = _tourDetailService.Queryable().Where(t => t.TourId == tourDelete.Id).ToList();
                foreach (var detail in details.Where(detail => detail != null))
                {
                    detail.ObjectState = ObjectState.Deleted;
                    _tourDetailService.Delete(detail);
                }
                //Delete this tour
                tourDelete.ObjectState = ObjectState.Deleted;
                _tourService.Delete(tourDelete);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Url = string.Format("~/Tour/Management");

            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Update media is error!");
                throw;
            }
            return Json(objectModel);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWorkAsync.Dispose();
            }
            base.Dispose(disposing);
        }
        private bool CustomerExists(int id)
        {
            return _tourService.Query(e => e.Id == id).Select().Any();
        }

        public ActionResult Photo(int id)
        {
            ViewBag.TourId = id;
            ViewBag.Images = _mediaService.Queryable().Where(t => t.OwnerId == id).ToList();
            ViewBag.Tour = _tourService.Queryable().FirstOrDefault(t => t.Id == id);
            return View();
        }

        [HttpPost]
        public ActionResult Uploads(HttpPostedFileBase[] files, FormCollection forms)
        {
            var id = Web.Framework.Utilities.ConvertToInt(forms["id"]);
            var baseUrl = string.Format("~/Uploads/TourImages/");
            if (id <= 0) return RedirectToAction("BasicInformation");
            var tour = _tourService.Queryable().FirstOrDefault(t => t.Id == id);

            if (tour == null) return RedirectToAction("BasicInformation");
            try
            {

                // Get the data
                foreach (var file in files)
                {
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
                    var thumbnail = Web.Framework.Utilities.ScaleBySize(original, 300);

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

                return RedirectToAction("Photo", new { id });
            }
            catch
            {
                Response.StatusCode = 500;
                Response.Write("An error occured");
                Response.End();
            }
            return RedirectToAction("BasicInformation");
        }
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase[] files, FormCollection forms)
        {
            var id = Web.Framework.Utilities.ConvertToInt(forms["id"]);
            var baseUrl = string.Format("~/Uploads/TourImages/");
            if (id <= 0) return RedirectToAction("BasicInformation");
            var tour = _tourService.Queryable().FirstOrDefault(t => t.Id == id);

            if (tour == null) return RedirectToAction("BasicInformation");
            try
            {

                // Get the data
                foreach (var file in files)
                {
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
                        MediaType = (int)Web.Framework.Utilities.MediaType.Banner,
                        ObjectState = ObjectState.Added
                    };
                    //Inser media
                    _mediaService.Insert(media);
                    _unitOfWorkAsync.SaveChanges();
                    //Clear up
                    original.Dispose();
                    thumbnail.Dispose();

                }

                return RedirectToAction("Photo", new { id });
            }
            catch
            {
                Response.StatusCode = 500;
                Response.Write("An error occured");
                Response.End();
            }
            return RedirectToAction("BasicInformation");
        }
    }
}