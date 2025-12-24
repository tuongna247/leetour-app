using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Drawing.Imaging;
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
    public class DealsController : Controller
    {

        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly IDealService _dealService;
        private readonly IHotelService _hotelService;
        private readonly IImageService _imageService;
        public DealsController(IUnitOfWorkAsync unitOfWorkAsync,
            IDealService dealService,
            IHotelService hotelService, IImageService imageService)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            _dealService = dealService;
            _hotelService = hotelService;
            _imageService = imageService;
        }

        public ActionResult Index()
        {
            var deals = _dealService.GetAllDeals();
            ViewBag.Hotels = _hotelService.GetHotels();
            return View(deals);
        }
        static void DefaultCompressionPng(Image original, string imagePath, string imageName)
        {
            var ms = new MemoryStream();
            original.Save(ms, ImageFormat.Png);
            var compressed = new Bitmap(ms);
            ms.Close();

            var fileOutPng = Path.Combine(imagePath, $"{imageName}.png");
            compressed.Save(fileOutPng, ImageFormat.Png);
        }
        public ActionResult SaveUploadedPictures(FormCollection forms)
        {
            var objectModel = new ObjectModel();
            //Get Id
            var id = Web.Framework.Utilities.ConvertToInt(forms["id"]);
            //Create founder path
            var baseUrl = $"~/uploads/DealImages";
            if (id <= 0) return Redirect($"/deals/");
            //Get hotel by hotel Id
            var deal = _dealService.GetDeal(id);

            if (deal == null) return Redirect($"/deals/");
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
                var original = Image.FromStream(file.InputStream);
                var imageName = $"{imageCode}-{Web.Framework.Utilities.GenerateSlug(deal.Name)}";
                DefaultCompressionPng(original, virtualPath, imageName);
                //Save on data
                var absOriginalUrl = $"https://admin.goreise.com{baseUrl.Substring(1)}/{imageName}.png";

                //Inser media
                deal.ImageUrl = absOriginalUrl;
                _dealService.Update(deal);
                //Clear up
                original.Dispose();
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
        [HttpPost]
        public ActionResult UpdatedDeal(Deals dealModel)
        {
            var objectModel = new ObjectModel();
            var deal = _dealService.GetDeal(dealModel.Id);
            if (deal == null)
            {
                return Redirect($"/deals/");
            }

            deal.Name = dealModel.Name;
            deal.Description = dealModel.Description;
            deal.Status = dealModel.Status;
            deal.Url = dealModel.Url;
            deal.Countdown = dealModel.Countdown;
            deal.Modified = DateTime.Now;
            deal.Type = (int)Web.Framework.Utilities.Product.Hotel;
            _dealService.Update(deal);

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
        [HttpPost]
        public ActionResult UpdatedDeals(ObjectModel objectModel)
        {
            var deal = _dealService.GetDeal(objectModel.Id);
            if (deal == null)
            {
                return Redirect($"/deals/");
            }
            if (objectModel.IntParam2!=-1)
            {
                var hotel = _hotelService.GetHotelSingle(objectModel.IntParam2);
                var hotelUrl = $"http://hotel.vinaday.com/{hotel.Country.ToLower()}/{hotel.Id}/{Web.Framework.Utilities.GenerateSlug(hotel.Name)}";
                var image = _imageService.GetImageListByHotelId(hotel.Id).FirstOrDefault(i => i.PictureType == Constant.ImageRestaurant);
                deal.ImageUrl = image == null ? "/Content/images/demo/general/no-image.jpg" : !string.IsNullOrEmpty(image.ImageThumbnail) ? "https://admin.goreise.com" + @Url.Content($"{image.ImageThumbnail}-90.jpeg") : "/Content/images/demo/general/no-image.jpg";
                deal.Url = hotelUrl;
            }

            deal.Name = objectModel.StrParam1;
            deal.Description = objectModel.StrParam2;
            deal.Status = objectModel.IntParam1;
            deal.Countdown = DateTime.Now;
            deal.Modified = DateTime.Now;
            deal.Type = (int)Web.Framework.Utilities.Product.Hotel;
        
            _dealService.Update(deal);

            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = "Updated deal are successfully!";
            }
            catch (Exception ex)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Updated deal are not successfully!";
            }
            return Json(objectModel);
        }
    }
}