using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;
using Vinaday.Data.Models;
using Vinaday.Services;
using Vinaday.Web.Framework;

namespace Vinaday.Admin.Controllers
{
 //   [Authorize(Roles = "Master Admin")]
    public class SettingController : Controller
    {
        private readonly IImageService _imageService;
        private readonly ICategoryDetailService _categoryDetailService;
        private readonly ITourService _tourService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly IHotelService _hotelService;
        private readonly IRegionService _regionService;
        private readonly ICountryService _countryService;
        private readonly ISeoService _seoService;
        public SettingController(IImageService imageService,
            IUnitOfWorkAsync unitOfWorkAsync,
            ITourService tourService,
            ICategoryDetailService categoryDetailService,
            IHotelService hotelService, IRegionService regionService, ICountryService countryService, ISeoService seoService)
        {
            _imageService = imageService;
            _unitOfWorkAsync = unitOfWorkAsync;
            _tourService = tourService;
            _categoryDetailService = categoryDetailService;
            _hotelService = hotelService;
            _regionService = regionService;
            _countryService = countryService;
            _seoService = seoService;
        }

        public ActionResult Index()
        {
            ViewBag.Image = _imageService.GetHotelImageSetting();
            return View();
        }
        [HttpPost]
        public ActionResult UpdateImageQuality(ObjectModel objectModel)
        {
            var images = _imageService.GetHotelImages();
            if (objectModel.Id <= 0)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Updated images is error!";
                return Json(objectModel);
            }
            //Add image for update list
            foreach (var image in images)
            {
                image.ImageQuanlity = objectModel.Id;
                image.ObjectState = ObjectState.Modified;
                _imageService.Update(image);
            }
            //Update list images.
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Message = "Updated all quality of images are successfully!";
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Update images is error!";
                throw;
            }
            return Json(objectModel);
        }
        public ActionResult UpdateSearchKeyTour()
        {
            var objectModel = new ObjectModel();
            var tours = _tourService.GetTours().ToList();

            if (tours.Count <= 0) return Json(objectModel);
            foreach (var tour in tours)
            {
                if (tour == null) continue;
                var travelStyles = string.Empty;
                var groupSize = $"[{Web.Framework.Utilities.GenerateSlug(tour.GroupSize)}]";
                var duration = $"[{Web.Framework.Utilities.GenerateSlug(tour.Duration)}]";
                if (!string.IsNullOrEmpty(tour.TravelStyle))
                {
                    var travelStyle = tour.TravelStyle.Split(',');

                    for (var j = 0; j < travelStyle.Count(); j++)
                    {
                        var f = Web.Framework.Utilities.ConvertToInt(travelStyle[j]);
                        var category = _categoryDetailService.GetCategoryDetail(f);
                        if (!string.IsNullOrEmpty(category?.Name))
                        {
                            travelStyles += $"[{Web.Framework.Utilities.GenerateSlug(category.Name)}],";
                        }
                    }
                }
                var filterKey = groupSize + "," + travelStyles + duration;
                //Update tour
                tour.Filter = filterKey;
                tour.ObjectState = ObjectState.Modified;
                _tourService.Update(tour);
            }
            var tourseoList = _seoService.Queryable().Where(a => a.ProductType == 2).ToList();
            foreach (var seo in tourseoList)
            {
                _seoService.Delete(seo);
                
            }
            _unitOfWorkAsync.SaveChanges();
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

            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = "All tour filter key is updated!";
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Update tour filter key is error!";
                throw;
            }
            return Json(objectModel);
        }
        public FileResult SiteMapHotelLocal()
        {
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var hotels = _hotelService.GetHotels();
            var cities = _hotelService.GetCitiesEn();
            //For home page
            var sitemapHome = new XElement(ns + "url",
                                         new XElement(ns + "loc",
                                            "http://vinaday.vn"),
                                        new XElement(ns + "lastmod", $"{DateTime.Now:yyyy-MM-dd}"),
                                        new XElement(ns + "changefreq", "daily"),
                                        new XElement(ns + "priority", "1.00"));
            //For spa page
            var sitemapSpa = new XElement(ns + "url",
                                         new XElement(ns + "loc",
                                            "http://vinaday.vn/spa-hotel-collection"),
                                        new XElement(ns + "lastmod", $"{DateTime.Now:yyyy-MM-dd}"),
                                        new XElement(ns + "changefreq", "daily"),
                                        new XElement(ns + "priority", "1.00"));

            var sitemapCities = from city in cities
                                where city != null && city.CityId > 0 && !string.IsNullOrEmpty(city.Country) && !string.IsNullOrEmpty(city.Name)
                                select new XElement(ns + "url",
                                         new XElement(ns + "loc",
                                            $"http://vinaday.vn/{Web.Framework.Utilities.GenerateSlug(city.Country.ToLower())}/{city.CityId}-{Web.Framework.Utilities.GenerateSlug(city.Name.ToLower())}"),
                                        new XElement(ns + "lastmod", $"{DateTime.Now:yyyy-MM-dd}"),
                                        new XElement(ns + "changefreq", "daily"),
                                        new XElement(ns + "priority", "0.5"));
            var sitemapHotels = from hotel in hotels
                                where
                                    hotel != null && hotel.Id > 0 && !string.IsNullOrEmpty(hotel.Country) &&
                                    !string.IsNullOrEmpty(hotel.Name)
                                select
                                    new XElement(ns + "url",
                                        new XElement(ns + "loc",
                                            $"http://vinaday.vn/{hotel.Country.ToLower()}/{hotel.Id}/{Web.Framework.Utilities.GenerateSlug(hotel.Name)}"),
                                        new XElement(ns + "lastmod", $"{DateTime.Now:yyyy-MM-dd}"),
                                        new XElement(ns + "changefreq", "daily"),
                                        new XElement(ns + "priority", "0.9"));

            var sitemap = new XDocument(
                            new XDeclaration("1.0", "utf-8", "yes"),
                            new XElement(ns + "urlset", sitemapHome, sitemapSpa, sitemapHotels, sitemapCities));


            var stream = new MemoryStream();
            var writer = XmlWriter.Create(stream);
            writer.WriteRaw(sitemap.ToString());
            stream.Position = 0;
            var fileStreamResult = File(stream, "application/octet-stream", "sitemap.xml");
            return fileStreamResult;


            //return Content("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + sitemap, "text/xml");
        }
        public FileResult SiteMapHotelEnglish()
        {
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var hotels = _hotelService.GetHotels();
            var cities = _hotelService.GetCitiesEn();
            //For home page
            var sitemapHome = new XElement(ns + "url",
                                         new XElement(ns + "loc",
                                            "http://hotel.vinaday.com"),
                                        new XElement(ns + "lastmod", $"{DateTime.Now:yyyy-MM-dd}"),
                                        new XElement(ns + "changefreq", "daily"),
                                        new XElement(ns + "priority", "1.00"));

            var sitemapCities = from city in cities
                                where city != null && city.CityId > 0 && !string.IsNullOrEmpty(city.Country) && !string.IsNullOrEmpty(city.Name)
                                select new XElement(ns + "url",
                                         new XElement(ns + "loc",
                                            $"http://hotel.vinaday.com/{Web.Framework.Utilities.GenerateSlug(city.Country.ToLower())}/{city.CityId}-{Web.Framework.Utilities.GenerateSlug(city.Name.ToLower())}"),
                                        new XElement(ns + "lastmod", $"{DateTime.Now:yyyy-MM-dd}"),
                                        new XElement(ns + "changefreq", "daily"),
                                        new XElement(ns + "priority", "0.5"));
            var sitemapHotels = from hotel in hotels
                                where
                                    hotel != null && hotel.Id > 0 && !string.IsNullOrEmpty(hotel.Country) &&
                                    !string.IsNullOrEmpty(hotel.Name)
                                select
                                    new XElement(ns + "url",
                                        new XElement(ns + "loc",
                                            $"http://hotel.vinaday.com/{hotel.Country.ToLower()}/{hotel.Id}/{Web.Framework.Utilities.GenerateSlug(hotel.Name)}"),
                                        new XElement(ns + "lastmod", $"{DateTime.Now:yyyy-MM-dd}"),
                                        new XElement(ns + "changefreq", "daily"),
                                        new XElement(ns + "priority", "0.9"));

            var sitemap = new XDocument(
                            new XDeclaration("1.0", "utf-8", "yes"),
                            new XElement(ns + "urlset", sitemapHome, sitemapHotels, sitemapCities));


            var stream = new MemoryStream();
            var writer = XmlWriter.Create(stream);
            writer.WriteRaw(sitemap.ToString());
            stream.Position = 0;
            var fileStreamResult = File(stream, "application/octet-stream", "sitemap.xml");
            return fileStreamResult;


            //return Content("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + sitemap, "text/xml");
        }
        public ActionResult UpdateHotelKey()
        {
            var hotels = _hotelService.GetHotels().ToList();
            if (hotels.Count > 0)
            {
                foreach (var hotel in hotels)
                {
                    var searchKey = _hotelService.GenerateSlugHotelKey(hotel);
                    //Update hotel
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
            _unitOfWorkAsync.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult UpdateTourMeta()
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
                                        Title = $"{tour.Name} ({(country != null ? country.Name : string.Empty)}) | vinaday.com",
                                        Description = tour.Name,
                                        Keyword = tour.Name,
                                        ObjectState = ObjectState.Added
                                    })
                {
                    _seoService.Insert(seo);
                }
            }
            _unitOfWorkAsync.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}