using Repository.Pattern.UnitOfWork;
using Service.Pattern;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using Vinaday.Data.Models;
using Vinaday.Services;
using Vinaday.Web.Framework;

namespace Vinaday.Admin.Controllers
{
	public class DealsController : Controller
	{
		private readonly IUnitOfWorkAsync _unitOfWorkAsync;

		private readonly IDealService _dealService;

		private readonly IHotelService _hotelService;

		private readonly IImageService _imageService;

		public DealsController(IUnitOfWorkAsync unitOfWorkAsync, IDealService dealService, IHotelService hotelService, IImageService imageService)
		{
			this._unitOfWorkAsync = unitOfWorkAsync;
			this._dealService = dealService;
			this._hotelService = hotelService;
			this._imageService = imageService;
		}

		private static void DefaultCompressionPng(Image original, string imagePath, string imageName)
		{
			MemoryStream memoryStream = new MemoryStream();
			original.Save(memoryStream, ImageFormat.Png);
			Bitmap bitmap = new Bitmap(memoryStream);
			memoryStream.Close();
			string str = Path.Combine(imagePath, string.Format("{0}.png", imageName));
			bitmap.Save(str, ImageFormat.Png);
		}

		public ActionResult Index()
		{
			List<Deals> allDeals = this._dealService.GetAllDeals();
			((dynamic)base.ViewBag).Hotels = this._hotelService.GetHotels();
			return base.View(allDeals);
		}

		public ActionResult SaveUploadedPictures(FormCollection forms)
		{
			ActionResult actionResult;
			ObjectModel objectModel = new ObjectModel();
			int num = Vinaday.Web.Framework.Utilities.ConvertToInt(forms["id"]);
			string str = "~/uploads/DealImages";
			if (num > 0)
			{
				Deals deal = this._dealService.GetDeal(num);
				if (deal != null)
				{
					foreach (string file in base.Request.Files)
					{
						HttpPostedFileBase item = base.Request.Files[file];
						if (item != null)
						{
							string str1 = base.Server.MapPath(str);
							if (!Directory.Exists(str1))
							{
								Directory.CreateDirectory(str1);
							}
							string randomString = Vinaday.Web.Framework.Utilities.GetRandomString(5);
							Image image = Image.FromStream(item.InputStream);
							string str2 = string.Format("{0}-{1}", randomString, Vinaday.Web.Framework.Utilities.GenerateSlug(deal.Name, 100));
							DealsController.DefaultCompressionPng(image, str1, str2);
							string str3 = string.Format("https://admin.goreise.com{0}/{1}.png", str.Substring(1), str2);
							deal.ImageUrl = str3;
							this._dealService.Update(deal);
							image.Dispose();
						}
					}
					try
					{
						this._unitOfWorkAsync.SaveChanges();
						objectModel.Status = 2;
						objectModel.Message = "Add all images are successfully!";
					}
					catch (Exception exception)
					{
						objectModel.Status = 1;
						objectModel.Message = "Add all images are not successfully!";
					}
					actionResult = base.Json(objectModel);
				}
				else
				{
					actionResult = this.Redirect("/deals/");
				}
			}
			else
			{
				actionResult = this.Redirect("/deals/");
			}
			return actionResult;
		}

		[HttpPost]
		public ActionResult UpdatedDeal(Deals dealModel)
		{
			ActionResult actionResult;
			ObjectModel objectModel = new ObjectModel();
			Deals deal = this._dealService.GetDeal(dealModel.Id);
			if (deal != null)
			{
				deal.Name = dealModel.Name;
				deal.Description = dealModel.Description;
				deal.Status = dealModel.Status;
				deal.Url = dealModel.Url;
				deal.Countdown = dealModel.Countdown;
				deal.Modified = DateTime.Now;
				deal.Type = 1;
				this._dealService.Update(deal);
				try
				{
					this._unitOfWorkAsync.SaveChanges();
					objectModel.Status = 2;
					objectModel.Message = "Add all images are successfully!";
				}
				catch (Exception exception)
				{
					objectModel.Status = 1;
					objectModel.Message = "Add all images are not successfully!";
				}
				actionResult = base.Json(objectModel);
			}
			else
			{
				actionResult = this.Redirect("/deals/");
			}
			return actionResult;
		}

		[HttpPost]
		public ActionResult UpdatedDeals(ObjectModel objectModel)
		{
			ActionResult actionResult;
			string str;
			Deals deal = this._dealService.GetDeal(objectModel.Id);
			if (deal != null)
			{
				if (objectModel.IntParam2 != -1)
				{
					Hotel hotelSingle = this._hotelService.GetHotelSingle(objectModel.IntParam2);
					string str1 = string.Format("http://hotel.vinaday.com/{0}/{1}/{2}", hotelSingle.Country.ToLower(), hotelSingle.Id, Vinaday.Web.Framework.Utilities.GenerateSlug(hotelSingle.Name, 100));
					HotelImages hotelImage = this._imageService.GetImageListByHotelId(hotelSingle.Id).FirstOrDefault<HotelImages>((HotelImages i) => i.PictureType == Constant.ImageRestaurant);
					Deals deal1 = deal;
					if (hotelImage == null)
					{
						str = "/Content/images/demo/general/no-image.jpg";
					}
					else
					{
						str = (!string.IsNullOrEmpty(hotelImage.ImageThumbnail) ? string.Concat("https://admin.goreise.com", base.Url.Content(string.Format("{0}-90.jpeg", hotelImage.ImageThumbnail))) : "/Content/images/demo/general/no-image.jpg");
					}
					deal1.ImageUrl = str;
					deal.Url = str1;
				}
				deal.Name = objectModel.StrParam1;
				deal.Description = objectModel.StrParam2;
				deal.Status = objectModel.IntParam1;
				deal.Countdown = DateTime.Now;
				deal.Modified = DateTime.Now;
				deal.Type = 1;
				this._dealService.Update(deal);
				try
				{
					this._unitOfWorkAsync.SaveChanges();
					objectModel.Status = 2;
					objectModel.Message = "Updated deal are successfully!";
				}
				catch (Exception exception)
				{
					objectModel.Status = 1;
					objectModel.Message = "Updated deal are not successfully!";
				}
				actionResult = base.Json(objectModel);
			}
			else
			{
				actionResult = this.Redirect("/deals/");
			}
			return actionResult;
		}
	}
}