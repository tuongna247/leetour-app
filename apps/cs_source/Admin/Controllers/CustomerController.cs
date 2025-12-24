using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Xml;
using DataTables.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;
using Vinaday.Admin.Models;
using Vinaday.Data.Models;
using Vinaday.Data.Models.Extention;
using Vinaday.Services;
using Vinaday.Web.Framework;
using Utilities = Vinaday.Web.Framework.Utilities;

namespace Vinaday.Admin.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IBusinessCardService _businessCardService;
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        public CustomerController(IBusinessCardService businessCardService, IUnitOfWorkAsync unitOfWorkAsync, ICustomerService customerService, IOrderService orderService)
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new SecurityContext())), businessCardService, unitOfWorkAsync, customerService, orderService)
        { }
        public CustomerController(UserManager<ApplicationUser> userManager, IBusinessCardService businessCardService, IUnitOfWorkAsync unitOfWorkAsync, ICustomerService customerService, IOrderService orderService)
        {
            UserManager = userManager;
            _businessCardService = businessCardService;
            _unitOfWorkAsync = unitOfWorkAsync;
            _customerService = customerService;
            _orderService = orderService;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }
        // GET: Customer
        public ActionResult Index()
        {
            ViewBag.Users = GetRolesToUsers();
            ViewBag.CurrentUser = ((ClaimsIdentity)User.Identity).Name;
            //var businessCards = _businessCardService.GetBusinessCardsByUser(userName);

            return View();
        }
        [HttpPost]
        public ActionResult GetCustomer(ObjectModel objectModel)
        {
            var orders = new List<OrderModel>();
            var customer = _businessCardService.GetBusinessCard(objectModel.Id);
            if (customer != null)
            {
                var user = _customerService.GetCustomerByEmail(customer.Email);
                if (user != null)
                {
                    orders = _orderService.GetOrdersByCustomerId(user.CustomerId).Where(s => s.Status == (int)Utilities.BookingStatus.Paid).ToList();
                }
            }
            ViewBag.OrderHistories = orders;
            return customer == null ? PartialView("_Business", new BusinessCard()) : PartialView("_Business", customer);
        }
        [HttpPost]
        public ActionResult SaveCustomer(BusinessCard businessCard)
        {
            var objectResult = new ObjectModel();
            var customer = _businessCardService.GetBusinessCard(businessCard.Id);
            if (customer == null)
            {
                objectResult.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectResult.Message = "Process voucher is error!";
                return Json(objectResult);
            }
            //Update customer
            customer.Note = businessCard.Note;
            customer.Address = customer.FullName;
            customer.FullName = businessCard.FullName;
            customer.IsCall = businessCard.IsCall;
            customer.WrongNumber = businessCard.WrongNumber;
            customer.PhoneSecond = businessCard.PhoneSecond;
            customer.EmailSecond = businessCard.EmailSecond;
            customer.Disconnected = businessCard.Disconnected;
            customer.ObjectState = ObjectState.Modified;

            _businessCardService.Update(customer);

            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectResult.Status = (int)Web.Framework.Utilities.Status.Active;
                objectResult.Message = "Customer is resolved!";
            }
            catch (Exception)
            {
                objectResult.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectResult.Message = "Process update customer is error!";
            }

            return Json(objectResult);
        }
        public List<ApplicationUser> GetRolesToUsers()
        {
            var context = new SecurityContext();
            var users = context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains("1fdd1eac-7191-441c-8c13-679c4d6fa1a3")).ToList();
            return users;
        }
        [HttpPost]
        public ActionResult ImportExcel(HttpPostedFileBase file)
        {
            var ds = new DataSet();
            var postedFileBase = Request.Files["file"];
            if (postedFileBase != null && postedFileBase.ContentLength <= 0) return View("Index");
            var fileBase = Request.Files["file"];
            if (fileBase != null)
            {
                var fileExtension =
                    System.IO.Path.GetExtension(fileBase.FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    var fileLocation = Server.MapPath("~/Content/") + fileBase.FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {

                        System.IO.File.Delete(fileLocation);
                    }
                    fileBase.SaveAs(fileLocation);
                    var excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                                fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    //connection String for xls file format.
                    switch (fileExtension)
                    {
                        case ".xls":
                            excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                                                    fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                            break;
                        case ".xlsx":
                            excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                                    fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                            break;
                    }
                    //Create Connection to Excel work book and add oledb namespace
                    var excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();

                    var dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null)
                    {
                        return null;
                    }

                    var excelSheets = new string[dt.Rows.Count];
                    var t = 0;
                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                        t++;
                    }
                    var excelConnection1 = new OleDbConnection(excelConnectionString);
                    var query = $"Select * from [{excelSheets[0]}]";
                    using (var dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }
                }
                if (fileExtension != null && fileExtension.ToString().ToLower().Equals(".xml"))
                {
                    var httpPostedFileBase = Request.Files["FileUpload"];
                    if (httpPostedFileBase != null)
                    {
                        var fileLocation = Server.MapPath("~/Content/") + httpPostedFileBase.FileName;
                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }

                        httpPostedFileBase.SaveAs(fileLocation);
                        var xmlreader = new XmlTextReader(fileLocation);
                        // DataSet ds = new DataSet();
                        ds.ReadXml(xmlreader);
                        xmlreader.Close();
                    }
                }
            }

            for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (ds.Tables[0].Rows == null)
                {
                    continue;
                }
                var customer = new BusinessCard
                {
                    FullName = ds.Tables[0].Rows[i][1].ToString(),
                    Email = ds.Tables[0].Rows[i][0].ToString(),
                    Phone = ds.Tables[0].Rows[i][2].ToString(),
                    UserAssignmentId = "Avil",
                    ObjectState = ObjectState.Added
                };
                _businessCardService.Insert(customer);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Json(new { success = false, error = "Error when processing Import Excel" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, error = "Import Excel is compalted" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AssignCustomer(string userAssign, int[] users)
        {
            var objectModel = new ObjectModel();

            foreach (var image in users.Select(id => _businessCardService.GetBusinessCard(id)).Where(user => user != null))
            {

                image.UserAssignmentId = userAssign;
                image.ObjectState = ObjectState.Modified;
                _businessCardService.Update(image);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                objectModel.Message = string.Format("Assing all record is successfully!");

            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectModel.Message = string.Format("Assing all record is not successfully!");
                throw;
            }
            return Json(objectModel);
        }
        [ScriptMethod(UseHttpGet = true)]
        public JsonResult DataTableGet([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var userName = ((ClaimsIdentity)User.Identity).Name;
            List<BusinessCard> businessCards;
            var role = ((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).FirstOrDefault();
            if (role != null && role.Contains("Master Admin"))
            {
                businessCards = _businessCardService.GetBusinessCards();
            }
            else
            {
                businessCards = _businessCardService.GetBusinessCardsByUser(userName);
            }
            var totalCount = businessCards.Count();

            // Apply filters
            if (requestModel.Search.Value != String.Empty)
            {
                var value = requestModel.Search.Value.Trim();
                businessCards = businessCards.Where(p => p.FullName != null && p.FullName.Contains(value) || (p.Email != null && p.Email.Contains(value)) || (p.UserAssignmentId != null && p.UserAssignmentId.Contains(value))).ToList();
            }
            string sortPhone = requestModel.Columns.ToList()[3].SortDirection.ToString();
            if (sortPhone.Contains("Ascendant"))
            {
                businessCards = businessCards.OrderBy(b => b.WrongNumber).ToList();
            }
            else
            {
                businessCards = businessCards.OrderByDescending(b => b.WrongNumber).ToList();
            }

            string sortIsCall = requestModel.Columns.ToList()[6].SortDirection.ToString();
            if (sortIsCall.Contains("Ascendant"))
            {
                businessCards = businessCards.OrderBy(b => b.IsCall).ToList();
            }
            else
            {
                businessCards = businessCards.OrderByDescending(b => b.IsCall).ToList();

            }
            var filteredCount = businessCards.Count();

            // Sort
            //var sortedColumns = requestModel.Columns.GetSortedColumns();
            //var orderByString = String.Empty;

            //foreach (var column in sortedColumns)
            //{
            //    orderByString += orderByString != String.Empty ? "," : "";
            //    orderByString += column.Data + " " + column.SortDirection.ToString().Substring(0, 3);
            //}

            //businessCards = businessCards.OrderBy(orderByString == String.Empty ? "fullname asc" : orderByString).ToList();

            // Paging
            businessCards = businessCards.Skip(requestModel.Start).Take(requestModel.Length).ToList();
            var data = businessCards.Select(p => new
            {
                Id = p.Id,
                FullName = p.FullName,
                Email = (role != null && role.Contains("Master Admin")) ? p.Email : $"xxxxx{p.Email.Substring(p.Email.LastIndexOf('@'))}",
                Phone = p.WrongNumber ? "<span class=\'label label-danger\'>Wrong number</span>" : ((role != null && role.Contains("Master Admin")) ? p.Phone : $"(xxx) xxx-x{p.Phone.Substring(p.Phone.Length - 3)}"),
                Note = p.Note,
                IsCall = p.IsCall,
                User = p.UserAssignmentId
            }).ToList();

            return Json(new DataTablesResponse(requestModel.Draw, data, filteredCount, totalCount), JsonRequestBehavior.AllowGet);
        }
    }
}