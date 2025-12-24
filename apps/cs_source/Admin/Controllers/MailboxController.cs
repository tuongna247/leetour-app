using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vinaday.Services;

namespace Vinaday.Admin.Controllers
{
   // [Authorize(Roles = "Admin")]
    public class MailboxController : Controller
    {
        private readonly IRoomReguestService _roomReguestService;

        public MailboxController(IRoomReguestService roomReguestService)
        {
            _roomReguestService = roomReguestService;
        }

        public ActionResult Inbox()
        {
            var reguests = _roomReguestService.GetRoomReguests().ToList();
            return View(reguests);
        }
        public ActionResult Message(int id)
        {
            var reguest = _roomReguestService.GetReguest(id);
            return View(reguest);
        }
        public ActionResult Compose()
        {
            
            return View();
        }
        

    }
}