using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Repository.Pattern.UnitOfWork;
using Vinaday.Services;
using Vinaday.Web.Framework;

namespace Vinaday.Admin.Controllers
{
    [Authorize(Roles = "Master Admin, Content Editor,Salesperson,Accountant,Master SEO")]
    public class DashboardController : Controller
    {
        private readonly ICategoryDetailService _categoryDetailService;
        private readonly ISeoService _seoService;
        private readonly ITourService _tourService;
        private readonly ICountryService _countryService;
        private readonly ICategoryService _categoryService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly IMediaService _mediaService;
        private readonly IFeaturedService _featuredService;
        private readonly IFeaturedHotelService _featuredHotelService;
        private readonly IHotelService _hotelService;
        private readonly IRoomReguestService _roomReguestService;
        private readonly ICityService _cityService;
        private readonly IRegionService _regionService;
        private readonly ICatDetailService _catDetailService;
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly ICustomerService _customerService;
        public DashboardController() {

        }
        public DashboardController(IOrderService orderService, ICustomerService customerService)
        {
            _orderService = orderService;
            _customerService = customerService;
        }

        public ActionResult Index()
        {
            var item = _orderService.GetDashboardItem();
            return View(item);
        }
    }
}