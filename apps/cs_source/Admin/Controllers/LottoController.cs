using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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

namespace Vinaday.Admin.Controllers
{
   // [Authorize(Roles = "Master Admin")]
    public class LottoController : Controller
    {
        private readonly ILottoService _lottoService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;

        public LottoController(ILottoService lottoService, 
            IUnitOfWorkAsync unitOfWorkAsync)
        {
            _lottoService = lottoService;
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        protected override void HandleUnknownAction(string actionName)
        {
            this.View(actionName).ExecuteResult(this.ControllerContext);
            //base.HandleUnknownAction(actionName);
        }
        public ActionResult Index()
        {
            var lottos = _lottoService.GetLottos();
            return View(lottos);
        }
        public ActionResult Detail(int id)
        {
            var lottoDetails = _lottoService.GetLottoDetails(id);
            return View(lottoDetails);
        }
        [HttpPost]
        public ActionResult Insert(ObjectModel objectModel)
        {
            var lotto = new Lotto
            {
               Name = objectModel.StrParam1,
               Created = DateTime.Now,
               Status = true,
               Type = objectModel.IntParam1,
               Total = objectModel.IntParam1,
               ObjectState = ObjectState.Added,
               HashLink = $"http://vinaday.vn/mini-game/{Web.Framework.Utilities.GenerateSlug(objectModel.StrParam1)}"
            };
            _lottoService.Insert(lotto);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = "Add Lotto is successfully!";
            }
            catch
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = "Add Lotto is not successfully!";
            }
            return Json(objectModel);
        }

        [HttpPost]
        public ActionResult UpdateStatus(ObjectModel objectModel)
        {
            var lotto = _lottoService.GetLotto(objectModel.Id);
            var objectResult = new ObjectModel();
            bool status;
            bool.TryParse(objectModel.StrParam1, out status);
            //Update hotel
            if (lotto != null)
            {
                lotto.Status = status;
                lotto.ObjectState = ObjectState.Modified;
                _lottoService.Update(lotto);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = "Lotto status is updated!";
            }
            catch (Exception)
            {
                objectResult.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectResult.Message = "Update lotto status is error!";
            }
            return Json(objectModel);
        }
    }
}