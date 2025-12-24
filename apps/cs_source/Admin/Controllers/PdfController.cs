
using System;
using System.Web.Mvc;
using iTextSharp.text;
using MvcRazorToPdf;
using Vinaday.Data.Models;


namespace Vinaday.Admin.Controllers
{
    public class PdfController : Controller
    {
        //
        // GET: /Pdf/
        public ActionResult Voucher(Voucher voucherModel)
        {
            //var model = new Voucher();
            
            return new PdfActionResult(voucherModel, (writer, document) =>
            {
                document.SetPageSize(new Rectangle(500f, 500f, 90));
                document.NewPage();
            })
            {
                FileDownloadName = $"{DateTime.Now.ToShortDateString()}-{voucherModel.Guest}.pdf"
            };
        }
	}
}