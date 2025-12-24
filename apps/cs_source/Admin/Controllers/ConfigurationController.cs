using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Runtime.Versioning;
using System.Web;
using System.Web.Mvc;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;
using Vinaday.Data.Models;
using Vinaday.Data.Models.Extention;
using Vinaday.Services;
using Vinaday.Web.Framework;
using Region = System.Drawing.Region;
using Utilities = Vinaday.Services.Utilities;

namespace Vinaday.Admin.Controllers
{
    //    [Authorize(Roles = "Admin")]
    public class ConfigurationController : Controller
    {
        private readonly ICategoryDetailService _categoryDetailService;
        private readonly ISeoService _seoService;
        private readonly ITourService _tourService;
        private readonly ICountryService _countryService;
        private readonly ICategoryService _categoryService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly IMediaService _mediaService;
        private readonly IFeaturedService _featuredService;
        private readonly ITourTopSiteService _tourtopSiteService;
        private readonly IFeaturedHotelService _featuredHotelService;
        private readonly IHotelService _hotelService;
        private readonly IRoomReguestService _roomReguestService;
        private readonly ICityService _cityService;
        private readonly IRegionService _regionService;
        private readonly ICatDetailService _catDetailService;
        private readonly ICancellationService _cancellationService;
        private readonly IRateExchangeService _rateExchangeService;
        public ConfigurationController(
            ICategoryDetailService categoryDetailService,
            ICategoryService categoryService,
            IUnitOfWorkAsync unitOfWorkAsync,
            ISeoService seoService,
            ITourService tourService,
            ITourTopSiteService tourtopSiteService,
            ICountryService countryService,
            IMediaService mediaService,
            IFeaturedService featuredService,
            IFeaturedHotelService featuredHotelService,
            IHotelService hotelService,
            IRoomReguestService roomReguestService,
            ICityService cityService,
            IRegionService regionService,
            ICatDetailService catDetailService,
            ICancellationService cancellationService,
            IRateExchangeService rateExchangeService)
        {
            _categoryDetailService = categoryDetailService;
            _categoryService = categoryService;
            _unitOfWorkAsync = unitOfWorkAsync;
            _seoService = seoService;
            _tourService = tourService;
            _countryService = countryService;
            _tourtopSiteService = tourtopSiteService;
            _mediaService = mediaService;
            _featuredService = featuredService;
            _featuredHotelService = featuredHotelService;
            _hotelService = hotelService;
            _roomReguestService = roomReguestService;
            _cityService = cityService;
            _regionService = regionService;
            _catDetailService = catDetailService;
            _cancellationService = cancellationService;
            _rateExchangeService = rateExchangeService;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Category()
        {
            var categories = _categoryService.GetCategories();
            return View(categories);
        }
        public ActionResult DeleteCategory(Category category)
        {
            var objectModel = new ObjectModel();
            if (category == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Delete this item is error!");
                return Json(objectModel);
            }
            var categoryDelete = _categoryService.Queryable().FirstOrDefault(t => t.Id == category.Id);
            //Added comlunm
            if (categoryDelete != null)
            {
                var categoryDetails = _categoryDetailService.GetCategoriesDetail(category.Id);
                foreach (var categoryDetail in categoryDetails.Where(categoryDetail => categoryDetail != null))
                {
                    categoryDetail.ObjectState = ObjectState.Deleted;
                    _categoryDetailService.Delete(categoryDetail);
                    _unitOfWorkAsync.SaveChanges();
                }
                categoryDelete.ObjectState = ObjectState.Deleted;
                _categoryService.Delete(categoryDelete);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Url = string.Format("~/Configuration/Category");

            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Delete this item is error!");
                throw;
            }
            return Json(objectModel);
        }
        public ActionResult InsertCategory(Category category)
        {
            var objectModel = new ObjectModel();
            //Added comlunm

            category.CreatedDate = DateTime.Now;
            category.ModifiedDate = DateTime.Now;
            category.Status = true;
            category.KeyCode = category.Name.ToUpper().Replace(" ", "");
            //Inser category
            category.ObjectState = ObjectState.Added;
            _categoryService.Insert(category);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Url = string.Format("~/Configuration/Category");
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Insert category is error!");
                throw;
            }
            return Json(objectModel);
        }
        public ActionResult UpdateCategory(Category category)
        {
            var objectModel = new ObjectModel();
            //Added comlunm
            var categoryModel = _categoryService.GetCategory(category.Id);

            if (categoryModel == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Insert category is error!");
            }
            else
            {
                categoryModel.ModifiedDate = DateTime.Now;
                categoryModel.Name = category.Name;
                categoryModel.Description = category.Description;

                //Inser category
                category.ObjectState = ObjectState.Modified;
                _categoryService.Update(categoryModel);
                try
                {
                    _unitOfWorkAsync.SaveChangesAsync();
                    objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                    objectModel.Url = string.Format("~/Configuration/Category");
                }
                catch (DbUpdateConcurrencyException)
                {
                    objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                    objectModel.Message = string.Format("Insert category detail is error!");
                    throw;
                }
            }

            return Json(objectModel);
        }
        public ActionResult CategoryDetail(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            return View(category);
        }
        public ActionResult DeleteCategoryDetail(CategoryDetail category)
        {

            var objectModel = new ObjectModel();
            if (category == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Delete this item is error!");
                return Json(objectModel);
            }
            var categoryDetailDelete = _categoryDetailService.Queryable().FirstOrDefault(t => t.Id == category.Id);
            //Added comlunm
            if (categoryDetailDelete != null)
            {
                categoryDetailDelete.ObjectState = ObjectState.Deleted;
                _categoryDetailService.Delete(categoryDetailDelete);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                if (categoryDetailDelete != null)
                    objectModel.Url = string.Format("~/Configuration/CategoryDetail/{0}", categoryDetailDelete.CategoryId);
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Delete this item is error!");
                throw;
            }
            return Json(objectModel);
        }
        public ActionResult InsertCategoryDetail(CategoryDetail category)
        {
            var objectModel = new ObjectModel();
            //Added comlunm

            category.CreatedDate = DateTime.Now;
            category.ModifiedDate = DateTime.Now;
            category.Status = true;
            //Inser category
            category.ObjectState = ObjectState.Added;
            _categoryDetailService.Insert(category);
            try
            {
                _unitOfWorkAsync.SaveChangesAsync();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Url = string.Format("~/Configuration/CategoryDetail/{0}", category.CategoryId);
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Insert category detail is error!");
                throw;
            }
            return Json(objectModel);
        }
        public ActionResult UpdateCategoryDetail(CategoryDetail category)
        {
            var objectModel = new ObjectModel();
            //Added comlunm
            var categoryModel = _categoryDetailService.GetCategoryDetail(category.Id);

            if (categoryModel == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Insert category detail is error!");
            }
            else
            {
                categoryModel.ModifiedDate = DateTime.Now;
                categoryModel.Name = category.Name;
                categoryModel.Description = category.Description;
                categoryModel.DescriptionVn = category.DescriptionVn;
                categoryModel.DescriptionFr = category.DescriptionFr;
                categoryModel.DescriptionDe = category.DescriptionDe;
                categoryModel.DescriptionGe = category.DescriptionGe;
                //Inser category
                category.ObjectState = ObjectState.Modified;
                _categoryDetailService.Update(categoryModel);
                try
                {
                    _unitOfWorkAsync.SaveChangesAsync();
                    objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                    objectModel.Url = string.Format("~/Configuration/CategoryDetail/{0}", categoryModel.CategoryId);
                }
                catch (DbUpdateConcurrencyException)
                {
                    objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                    objectModel.Message = string.Format("Insert category detail is error!");
                    throw;
                }
            }

            return Json(objectModel);
        }
        public ActionResult InsertSeo()
        {
            var tours = _tourService.GetTours();
            if (tours.Count > 0)
            {
                foreach (var seo in from tour in tours
                                    where tour != null
                                    let country = _countryService.GetCountryList().FirstOrDefault(c => c.CountryId == tour.CountryId)
                                    select new Seo
                                    {
                                        EntityId = tour.Id,
                                        EntityName = Enum.GetName(typeof(Web.Framework.Utilities.Product), tour.Type),
                                        ProductType = tour.Type,
                                        Slug = Web.Framework.Utilities.GenerateSlug(tour.Name, 200),
                                        IsActive = true,
                                        Title = string.Format("{0} ({1}) | vinaday.com", tour.Name, country != null ? country.Name : string.Empty),
                                        Description = tour.Name,
                                        Keyword = tour.Name,
                                        ObjectState = ObjectState.Added
                                    })
                {
                    _seoService.Insert(seo);
                }
            }
            _unitOfWorkAsync.SaveChanges();
            return RedirectToAction("Seo");
        }
        public ActionResult InsertSearchKey()
        {
            var hotels = _hotelService.GetHotels().ToList();
            if (hotels.Count > 0)
            {
                foreach (var hotel in hotels)
                {
                    if (hotel != null)
                    {
                        var facilities = string.Empty;
                        var star = string.Format("[star-{0}]", hotel.StartRating);
                        var regionId = hotel.RegionId ?? -1;
                        var region = _regionService.GetRegionList().FirstOrDefault(r => r.Id == regionId);
                        if (!string.IsNullOrEmpty(hotel.HotelFacilities))
                        {
                            var facility = hotel.HotelFacilities.Split(',');

                            for (var j = 0; j < facility.Count(); j++)
                            {
                                var f = Web.Framework.Utilities.ConvertToInt(facility[j]);
                                var category = _catDetailService.ODataQueryable().FirstOrDefault(c => c.Id == f);
                                if (category != null)
                                {
                                    if (!string.IsNullOrEmpty(category.Name))
                                    {
                                        facilities += string.Format("[facility-{0}],", Web.Framework.Utilities.GenerateSlug(category.Name));
                                    }

                                }
                            }
                        }

                        var searchKey = facilities + star;


                        if (region != null)
                        {
                            searchKey = searchKey + "," + string.Format("[region-{0}]", Web.Framework.Utilities.GenerateSlug(region.Name));
                        }
                        if (!string.IsNullOrEmpty(hotel.HotelStyleName))
                        {
                            searchKey = searchKey + "," + string.Format("[style-{0}]", Web.Framework.Utilities.GenerateSlug(hotel.HotelStyleName));
                        }

                        //Update tour
                        hotel.KeyValue = searchKey;
                        hotel.ObjectState = ObjectState.Modified;
                        _hotelService.Update(hotel);

                        try
                        {
                            _unitOfWorkAsync.SaveChanges();

                        }
                        catch (DbUpdateConcurrencyException)
                        {

                        }
                    }
                }
            }
            _unitOfWorkAsync.SaveChanges();
            return RedirectToAction("Seo");
        }

        [HttpPost]
        public ActionResult GetCityById(int id)
        {
            var city = _cityService.GetCities().FirstOrDefault(a => a.CityId == id);
            return Json(city, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Seo()
        {
            return View();
        }


        public ActionResult BestTour()
        {
            var featureds = _featuredService.GetFeatureds().OrderBy(f => f.Priority);
            var tourModels = new List<TourModel>();
            foreach (var featured in featureds)
            {
                var media =
                    _mediaService.Queryable()
                        .FirstOrDefault(t => t.OwnerId == featured.TourId && t.MediaType == (int)Web.Framework.Utilities.MediaType.Banner);
                var tour = _tourService.GetTourById(featured.TourId);
                var tourModel = new TourModel
                {
                    Name = tour.Name,
                    Id = tour.Id,
                    ImageUrl = (media != null && media.ThumbnailPath != null)
                            ? string.Format("{0}", Url.Content(string.Format("https://admin.goreise.com/{0}", media.ThumbnailPath.Substring(2))))
                            : string.Format("~/Content/img/no-image.jpg")
                };
                tourModels.Add(tourModel);
            }
            ViewBag.Tours = _tourService.GetTours();
            ViewBag.Featureds = tourModels;
            return View();
        }
        public ActionResult TourTopSite()
        {
            var featureds = _tourtopSiteService.GetTourTopSites().OrderBy(f => f.Priority);
            var tourModels = new List<TourModel>();
            foreach (var featured in featureds)
            {
                var media =
                    _mediaService.Queryable()
                        .FirstOrDefault(t => t.OwnerId == featured.TourId && t.MediaType == (int)Web.Framework.Utilities.MediaType.Banner);
                var tour = _tourService.GetTourById(featured.TourId);
                var tourModel = new TourModel
                {
                    Name = tour.Name,
                    Id = tour.Id,
                    ImageUrl = (media != null && media.ThumbnailPath != null)
                            ? string.Format("{0}", Url.Content(string.Format("https://admin.goreise.com/{0}", media.ThumbnailPath.Substring(2))))
                            : string.Format("~/Content/img/no-image.jpg")
                };
                tourModels.Add(tourModel);
            }
            ViewBag.Tours = _tourService.GetTours();
            ViewBag.Featureds = tourModels;
            return View();
        }
        public ActionResult TourFeatured()
        {
            var featureds = _featuredService.GetFeatureds().OrderBy(f => f.Priority);
            var tourModels = new List<TourModel>();
            foreach (var featured in featureds)
            {
                var media =
                    _mediaService.Queryable()
                        .FirstOrDefault(t => t.OwnerId == featured.TourId && t.MediaType == (int)Web.Framework.Utilities.MediaType.Banner);
                var tour = _tourService.GetTourById(featured.TourId);
                var tourModel = new TourModel
                {
                    Name = tour.Name,
                    Id = tour.Id,
                    ImageUrl = (media != null && media.ThumbnailPath != null)
                            ? string.Format("{0}", Url.Content(string.Format("https://admin.goreise.com/{0}", media.ThumbnailPath.Substring(2))))
                            : string.Format("~/Content/img/no-image.jpg")
                };
                tourModels.Add(tourModel);
            }
            ViewBag.Tours = _tourService.GetTours();
            ViewBag.Featureds = tourModels;
            return View();
        }
        public ActionResult HotelFeatured()
        {
            var featureds = _featuredHotelService.GetHotelFeatureds().OrderBy(f => f.Priority);
            var hotelModels = new List<HotelModel>();
            foreach (var featured in featureds)
            {

                var hotel = _hotelService.GetHotelSingle(featured.HotelId);
                var hotelModel = new HotelModel
                {
                    Name = hotel.Name,
                    Id = hotel.Id,
                    ImageUrl = !string.IsNullOrEmpty(hotel.HotelPicture1)
                            ? string.Format("{0}", string.Format("https://goreise.com{0}", Url.Content(hotel.HotelPicture1)))
                            : string.Format("~/Content/img/no-image.jpg")
                };
                hotelModels.Add(hotelModel);
            }
            ViewBag.Hotels = _hotelService.GetHotels();
            ViewBag.Featureds = hotelModels;
            return View();
        }
        [HttpPost]
        public ActionResult UpdateTourTopSite(ObjectModel objectModel)
        {
            if (objectModel == null)
            {
                objectModel = new ObjectModel
                {
                    Status = (int)Web.Framework.Utilities.Status.Inactive,
                    Message = string.Format("Updated tour top site is error!")
                };
                return Json(objectModel);
            }
            var featured = _tourtopSiteService.GetTourTopSites().FirstOrDefault(f => f.Priority == objectModel.IntParam1);
            //Added comlunm
            if (featured != null)
            {
                featured.ModifiedDate = DateTime.Now;
                featured.TourId = objectModel.Id;
                //Update tour
                featured.ObjectState = ObjectState.Modified;
                _tourtopSiteService.Update(featured);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Url = string.Format("~/Configuration/TourTopSite");

            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Updated tour top site is error!");
                throw;
            }
            return Json(objectModel);
        }
        
       [HttpPost]
        public ActionResult UpdateTourFeatured(ObjectModel objectModel)
        {
            if (objectModel == null)
            {
                objectModel = new ObjectModel
                {
                    Status = (int)Web.Framework.Utilities.Status.Inactive,
                    Message = string.Format("Updated tour featured is error!")
                };
                return Json(objectModel);
            }
            var featured = _featuredService.GetFeatureds().FirstOrDefault(f => f.Priority == objectModel.IntParam1);
            //Added comlunm
            if (featured != null)
            {
                featured.ModifiedDate = DateTime.Now;
                featured.TourId = objectModel.Id;
                //Update tour
                featured.ObjectState = ObjectState.Modified;
                _featuredService.Update(featured);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Url = string.Format("~/Configuration/TourFeatured");

            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Updated tour featured is error!");
                throw;
            }
            return Json(objectModel);
        }
        [HttpPost]
        public ActionResult UpdateHotelFeatured(ObjectModel objectModel)
        {
            if (objectModel == null)
            {
                objectModel = new ObjectModel
                {
                    Status = (int)Web.Framework.Utilities.Status.Inactive,
                    Message = string.Format("Updated hotel featured is error!")
                };
                return Json(objectModel);
            }
            var featured = _featuredHotelService.GetHotelFeatureds().FirstOrDefault(f => f.Priority == objectModel.IntParam1);
            //Added comlunm
            if (featured != null)
            {
                featured.ModifiedDate = DateTime.Now;
                featured.HotelId = objectModel.Id;
                //Update tour
                featured.ObjectState = ObjectState.Modified;
                _featuredHotelService.Update(featured);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Url = string.Format("~/Configuration/HotelFeatured");

            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Updated hotel featured is error!");
                throw;
            }
            return Json(objectModel);
        }
        public ActionResult RequestHotel()
        {
            var reguests = _roomReguestService.GetRoomReguests();

            return View(reguests);
        }
        #region Cancellation
        [HttpPost]
        public ActionResult DeleteCancellation(int cancellationId)
        {
            var objectModel = new ObjectModel();
            if (cancellationId <= 0)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Delete this item is error!";
                return Json(objectModel);
            }
            var cancellation = _cancellationService.GetCancellation(cancellationId);
            //Added comlunm
            if (cancellation != null)
            {
                cancellation.ObjectState = ObjectState.Modified;
                cancellation.Status = (int)Utilities.Status.Inactive;
                _cancellationService.Update(cancellation);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Delete this item is error!";
                throw;
            }
            return Json(objectModel);
        }
        [HttpPost]
        public ActionResult GetCancellation(int cancellationId)
        {
            var cancellation = _cancellationService.GetCancellation(cancellationId);
            return PartialView("_Cancellation", cancellation ?? new CancellationPolicy());
        }
        [HttpPost]
        public ActionResult SaveCancellation(ObjectModel objectModel)
        {
            var cancellationPolicy = new CancellationPolicy
            {
                Code = objectModel.StrParam1,
                Name = objectModel.StrParam2,
                Description = objectModel.StrParam3,
                DescriptionVn = objectModel.StrParam4
            };
            if (objectModel.Id > 0)
            {
                cancellationPolicy = _cancellationService.GetCancellation(objectModel.Id);
                if (cancellationPolicy == null)
                {
                    objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                    objectModel.Message = "Save this item is error!";
                    return Json(objectModel);
                }
                cancellationPolicy.Code = objectModel.StrParam1;
                cancellationPolicy.Name = objectModel.StrParam2;
                cancellationPolicy.Description = objectModel.StrParam3;
                cancellationPolicy.DescriptionVn = objectModel.StrParam4;
                cancellationPolicy.ObjectState = ObjectState.Modified;
                _cancellationService.Update(cancellationPolicy);
            }
            else
            {
                cancellationPolicy.ObjectState = ObjectState.Added;
                _cancellationService.Insert(cancellationPolicy);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Url = "Saved successfully!";
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Saved Unsuccessfully!";
                throw;
            }
            return Json(objectModel);
        }
        public ActionResult Cancellation()
        {
            var cancellations = _cancellationService.GetCancellationList();
            return View(cancellations);
        }

        #endregion

        #region City

        public ActionResult City()
        {
            var countries = _countryService.GetCountryList();
            return View(countries);
        }
        [HttpPost]
        public ActionResult GetCities(ObjectModel objectModel)
        {
            var cities = _cityService.GetCitiesByCountryId(objectModel.Id);
            return PartialView("_TabCities", cities);
        }
        public ActionResult UpdateCity(City cityModel)
        {
            var objectModel = new ObjectModel();
            //Added comlunm
            var city = _cityService.GetCityById(cityModel.CityId);

            if (City() == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Update city is error!";
            }
            else
            {
                city.Name = cityModel.Name;
                city.Description = cityModel.Description;
                city.Priority = cityModel.Priority;
                city.SEO_Meta = cityModel.SEO_Meta;
                city.SEO_Keyword = cityModel.SEO_Keyword;
                city.SEO_Description = cityModel.SEO_Description;
                city.SEO_Title_VN = cityModel.SEO_Title_VN;
                city.Seo_Title = cityModel.Seo_Title;
                city.SEO_Keyword_VN = cityModel.SEO_Keyword_VN;
                city.SEO_Description_VN = cityModel.SEO_Description_VN;
                //Inser city
                _cityService.Update(city);
                try
                {
                    _unitOfWorkAsync.SaveChangesAsync();
                    objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                    objectModel.Message = "City is updated!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                    objectModel.Message = "Update city is error!";
                    throw;
                }
            }
            return Json(objectModel);
        }
        public ActionResult InsertCity(City cityModel)
        {
            var objectModel = new ObjectModel();
            //Added comlunm
            cityModel.ObjectState = ObjectState.Added;
            _cityService.Insert(cityModel);
            try
            {
                _unitOfWorkAsync.SaveChangesAsync();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = "This record is inserted!";
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Insert city detail is error!";
                throw;
            }
            return Json(objectModel);
        }
        public ActionResult DeleteCity(City cityModel)
        {
            var objectModel = new ObjectModel();
            var city = _cityService.GetCityById(cityModel.CityId);
            //Added comlunm
            city.ObjectState = ObjectState.Deleted;
            _cityService.Delete(city);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = "This record is deleted!";
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Delete this item is error!";
                throw;
            }
            return Json(objectModel);
        }
        #endregion

        #region Country

        public ActionResult Country()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetCountries()
        {
            var countries = _countryService.GetCountryList();
            return PartialView("_TabCountries", countries);
        }
        public ActionResult UpdateCountry(Country countryModel)
        {
            var objectModel = new ObjectModel();
            //Added comlunm
            var country = _countryService.GetCountry(countryModel.CountryId);

            if (country == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Update city is error!";
            }
            else
            {
                country.Name = countryModel.Name;
                country.NameVn = countryModel.NameVn;
                //Inser country
                _countryService.Update(country);
                try
                {
                    _unitOfWorkAsync.SaveChangesAsync();
                    objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                    objectModel.Message = "Country is updated!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                    objectModel.Message = "Update country is error!";
                    throw;
                }
            }
            return Json(objectModel);
        }
        public ActionResult InsertCountry(Country countryModel)
        {
            var objectModel = new ObjectModel();
            //Added comlunm
            countryModel.ObjectState = ObjectState.Added;
            _countryService.Insert(countryModel);
            try
            {
                _unitOfWorkAsync.SaveChangesAsync();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = "This record is inserted!";
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Insert country is error!";
                throw;
            }
            return Json(objectModel);
        }
        public ActionResult DeleteCountry(Country countryModel)
        {
            var objectModel = new ObjectModel();
            var country = _countryService.GetCountry(countryModel.CountryId);
            //Added comlunm
            country.ObjectState = ObjectState.Deleted;
            _countryService.Delete(country);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = "This record is deleted!";
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Delete this item is error!";
                throw;
            }
            return Json(objectModel);
        }
        #endregion

        #region Region

        public ActionResult Region()
        {
            var cities = _cityService.GetCities();
            return View(cities);
        }
        [HttpPost]
        public ActionResult GetRegions()
        {
            var regions = _regionService.GetRegionList();
            return PartialView("_TabRegions", regions);
        }
        public ActionResult UpdateRegion(Region1 regionModel)
        {
            var objectModel = new ObjectModel();
            //Added comlunm
            var region = _regionService.GetRegion(regionModel.Id);

            if (region == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Update region is error!";
            }
            else
            {
                region.Name = regionModel.Name;
                region.NameVn = regionModel.NameVn;
                region.Description = regionModel.Description;
                region.DescriptionVn = regionModel.DescriptionVn;
                //Inser country
                _regionService.Update(region);
                try
                {
                    _unitOfWorkAsync.SaveChangesAsync();
                    objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                    objectModel.Message = "Region is updated!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                    objectModel.Message = "Update region is error!";
                    throw;
                }
            }
            return Json(objectModel);
        }
        public ActionResult InsertRegion(Region1 regionModel)
        {
            var objectModel = new ObjectModel();
            //Added comlunm
            regionModel.ObjectState = ObjectState.Added;
            regionModel.Slug = Utilities.GenerateSlug(regionModel.Name);
            regionModel.Status = 1;
            regionModel.Type = 1;
            _regionService.Insert(regionModel);
            try
            {
                _unitOfWorkAsync.SaveChangesAsync();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = "This record is inserted!";
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Insert region is error!";
                throw;
            }
            return Json(objectModel);
        }
        public ActionResult DeleteRegion(Region1 regionModel)
        {
            var objectModel = new ObjectModel();
            var region = _regionService.GetRegion(regionModel.Id);
            //Added comlunm
            region.ObjectState = ObjectState.Deleted;
            _regionService.Delete(region);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = "This record is deleted!";
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Delete this item is error!";
                throw;
            }
            return Json(objectModel);
        }
        #endregion

        #region Currency

        public ActionResult Currency()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetCurrencies()
        {
            var currencies = _rateExchangeService.GetRateExchanges();
            return PartialView("_TabCurrencies", currencies);
        }
        public ActionResult UpdateCurrency(RateExchange rateExchangeModel)
        {
            var objectModel = new ObjectModel();
            //Added comlunm
            var rateExchange = _rateExchangeService.GetRateExchangeById(rateExchangeModel.Id);

            if (rateExchange == null)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Update rate exchange is error!";
            }
            else
            {
                rateExchange.CurrentPrice = rateExchangeModel.CurrentPrice;
                //Inser rate
                _rateExchangeService.Update(rateExchange);
                try
                {
                    _unitOfWorkAsync.SaveChangesAsync();
                    objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                    objectModel.Message = "Rate exchange is updated!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                    objectModel.Message = "Update rate exchange is error!";
                    throw;
                }
            }
            return Json(objectModel);
        }
        public ActionResult InsertCurrency(RateExchange rateExchangeModel)
        {
            var objectModel = new ObjectModel();
            //Added comlunm
            rateExchangeModel.ObjectState = ObjectState.Added;
            _rateExchangeService.Insert(rateExchangeModel);
            try
            {
                _unitOfWorkAsync.SaveChangesAsync();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = "This record is inserted!";
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Insert rate exchange is error!";
                throw;
            }
            return Json(objectModel);
        }
        public ActionResult DeleteCurrency(RateExchange rateExchangeModel)
        {
            var objectModel = new ObjectModel();
            var rateExchange = _rateExchangeService.GetRateExchangeById(rateExchangeModel.Id);
            //Added comlunm
            rateExchange.ObjectState = ObjectState.Deleted;
            _rateExchangeService.Delete(rateExchange);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = "This record is deleted!";
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Delete this item is error!";
                throw;
            }
            return Json(objectModel);
        }
        #endregion
    }
}