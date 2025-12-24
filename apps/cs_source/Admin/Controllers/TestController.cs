using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vinaday.Web.Framework;

namespace Vinaday.Admin.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            //long so, tronToi1000, tronToi500;

            //so = 123456350;

            //tronToi1000 = Utilities.RoundingTo(so, 1000); // 123456000
            //tronToi500 = Utilities.RoundingTo(so, 500); // 123456500


            //string numberpre = "1118875";
            //// số mình cần convert
            //string numberEnd = Convert.ToDecimal(numberpre.Substring(numberpre.Length – 4)).ToString("N0");
            //// khai bao so cuỗi cắt chuỗi từ trái qua 4 số
            //double numberFirst = Convert.ToDouble(numberpre.Substring(0, numberpre.Length–4) + "0");
            ////số đầu cắt 4 số
            //double numberPresent = Math.Round(Convert.ToDouble(numberEnd.Replace(',','.')));
            ////sau đó lấy số cần lấy
            //Response.Write((numberFirst + numberPresent).ToString("N0") + ", 000");

            return Content("");
        }
    }
}