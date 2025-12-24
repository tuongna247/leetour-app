using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.ModelBinding;
using System.Web.Mvc;
using System.Web.UI;
using System.Xml;
using System.Xml.Serialization;
using DocumentFormat.OpenXml.Packaging;
using iTextSharp.text;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.Data.OData.Query.SemanticAst;
using MvcRazorToPdf;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;
using Vinaday.Data.Models;
using Vinaday.Data.Models.Extention;
using Vinaday.Services;
using Vinaday.Web.Framework;
using Vinaday.Admin;
using Vinaday.Admin.Models;
using Vinaday.Web.Framework.EmailHelpers;
using Utilities = Vinaday.Web.Framework.Utilities;
using DocumentFormat.OpenXml.Extensions;
using AutoMapper;
using Vinaday.Admin.Models.VinadayDBF;

//using System.IO.Packaging;


namespace Vinaday.Admin.Controllers
{
    //   [Authorize(Roles = "Master Admin, Content Editor")]
    public partial class BookingController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ITourOperatorsService _tourOperatorsService;
        private readonly IOrderService2 _orderService2;
        private readonly IAccountantOrderService _accountantOrderService;
        private readonly IAccountantOrderDetailService _accountantOrderDetailService;
        private readonly IPaymentOrderService _paymentOrderService;
        private readonly IPaymentOrderService2 _paymentOrderService2;
        private readonly IPaymentOrderDetailService _paymentOrderDetailService;
        private readonly IPaymentOrderDetailService2 _paymentOrderDetailService2;
        private readonly IOrderDetailService _orderDetailService;
        private readonly ICustomerService _customerService;
        private readonly ITourService _tourService;
        private readonly IHotelService _hotelService;
        private readonly IRoomService _roomService;
        private readonly IMediaService _mediaService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly INationalityService _nationalityService;
        private readonly IRoomControlService _roomControlService;
        private readonly ICatDetailService _catDetailService;
        private readonly ITourRateService _tourRateService;
        private readonly IBookingService _bookingService;
        private readonly IOrderInformationService _orderInformationService;
        private readonly IOrderInformationService2 _orderInformationService2;
        private readonly ICancellationService _cancellationService;
        private readonly ICityService _cityService;
        private readonly IVoucherService _voucherService;
        private readonly ICreditCardService _creditCardService;
        private readonly IRateExchangeService _rateExchangeService;
        private readonly IImageService _imageService;
        public BookingController(IOrderService orderService,
            IOrderService2 orderService2,
            ICustomerService customerService,
            ITourService tourService,
            IMediaService mediaService,
             IImageService imageService,
            IUnitOfWorkAsync unitOfWorkAsync,
            IOrderDetailService orderDetailService,
            INationalityService nationalityService,
            IHotelService hotelService,
            IRoomService roomService,
            IRoomControlService roomControlService,
            ICatDetailService catDetailService,
            ITourRateService tourRateService,
            IBookingService bookingService,
            IOrderInformationService orderInformationService,
            IOrderInformationService2 orderInformationService2,
            ICancellationService cancellationService,
            ICityService cityService,
            IVoucherService voucherService,
            ICreditCardService creditCardService,
            IRateExchangeService rateExchangeService,
            IPaymentOrderService paymentOrderService, IPaymentOrderService2 paymentOrderService2, IPaymentOrderDetailService2 paymentOrderDetailService2,
            IPaymentOrderDetailService paymentOrderDetailService, IAccountantOrderService accountantOrderService, IAccountantOrderDetailService accountantOrderDetailService, ITourOperatorsService tourOperatorsService)
        {
            _tourOperatorsService = tourOperatorsService;
            _imageService = imageService;
            _orderService = orderService;
            _orderService2 = orderService2;
            _customerService = customerService;
            _tourService = tourService;
            _mediaService = mediaService;
            _unitOfWorkAsync = unitOfWorkAsync;
            _orderDetailService = orderDetailService;
            _nationalityService = nationalityService;
            _hotelService = hotelService;
            _roomService = roomService;
            _roomControlService = roomControlService;
            _catDetailService = catDetailService;
            _tourRateService = tourRateService;
            _bookingService = bookingService;
            _orderInformationService = orderInformationService;
            _orderInformationService2 = orderInformationService2;
            _cancellationService = cancellationService;
            _cityService = cityService;
            _voucherService = voucherService;
            _creditCardService = creditCardService;
            _rateExchangeService = rateExchangeService;
            _paymentOrderService = paymentOrderService;
            _paymentOrderDetailService = paymentOrderDetailService;
            _accountantOrderService = accountantOrderService;
            _accountantOrderDetailService = accountantOrderDetailService;
            _paymentOrderService2 = paymentOrderService2;
            _paymentOrderDetailService2 = paymentOrderDetailService2;
        }

        #region Tour Order
        [Authorize(Roles = "Admin")]
        public ActionResult TourOrder()
        {
            var orders = _orderService.GetTourOrders();
            var orderModels = (from order in orders
                               where order != null
                               let tour = _tourService.GetTourById(order.ProductId ?? 0)
                               let customer = _customerService.GetCustomerById(order.CustomerId ?? 0)
                               select new OrderModel
                               {
                                   Id = order.Id,
                                   Pnr = order.Pnr,
                                   StartDate = order.StartDate.ToString("d"),
                                   StartDateCompare = order.StartDate,
                                   Status = order.Status,
                                   CreatedDate = order.CreatedDate.ToShortDateString(),
                                   Tour = tour,
                                   Customer = customer,
                                   SpecialRequest = order.SpecialRequest
                               }).ToList();
            ViewBag.Orders = orderModels;
            return View();
        }
        public ActionResult HotelOrder()
        {
            var orders = _orderService.GetHotelOrders();
            var orderModels = (from order in orders
                               where order != null
                               let room = _roomService.GetRoom(order.ProductId ?? 0) //_tourService.GetTourById(order.ProductId ?? 0)
                               let customer = _customerService.GetCustomerById(order.CustomerId ?? 0)
                               select new OrderModel
                               {
                                   Id = order.Id,
                                   Pnr = order.Pnr,
                                   StartDate = order.StartDate.ToShortDateString(),
                                   StartDateCompare = order.StartDate,
                                   Status = order.Status,
                                   CreatedDate = order.CreatedDate.ToShortDateString(),
                                   Room = room,
                                   Customer = customer,
                                   SpecialRequest = order.SpecialRequest
                               }).ToList();
            ViewBag.Orders = orderModels;
            return View();
        }
        public ActionResult TourOrderDetail(int id)
        {
            Order order = _orderService.GetOrderById(id);
            if (order == null)
            {
                return RedirectToAction("TourOrder");
            }
            var customer = _customerService.GetCustomerById(order.CustomerId ?? 0);
            var tour = _tourService.GetTourById(order.ProductId ?? 0);
            var media = _mediaService.GetMediaByTourId(tour != null ? tour.Id : 0);
            var orderDetail = _orderDetailService.GetOrderDetails(order.Id);
            customer.Nationality = _nationalityService.GetNationality(customer.NationalId != null ? (int)customer.NationalId : 0);

            var orderModel = new OrderModel
            {
                Id = order.Id,
                Pnr = order.Pnr,
                StartDate = order.StartDate.ToShortDateString(),
                StartDateCompare = order.StartDate,
                Status = order.Status,
                CreatedDate = order.CreatedDate.ToShortDateString(),
                Tour = tour,
                Customer = customer,
                Price = order.Price,
                Quantity = order.Quantity,
                Amount = order.Amount,
                Avatar = media != null ? media.ThumbnailPath : string.Format("~/Content/Images/no-image.jpg"),
                OrderDetails = orderDetail,
                PaymentMethod = order.PaymentMethod,
                CardNumber = order.CardNumber,
                ProductId = order.ProductId ?? 0,
                SpecialRequest = order.SpecialRequest

            };
            ViewBag.Order = orderModel;
            return View();
        }
        public ActionResult HotelOrderDetail(int id)
        {
            Order order = _orderService.GetOrderById(id);
            if (order == null)
            {
                return RedirectToAction("HotelOrder");
            }
            var customer = _customerService.GetCustomerById(order.CustomerId ?? 0);
            var room = _roomService.GetRoom(order.ProductId ?? 0);
            //var media = _mediaService.GetMediaByTourId(hotel != null ? hotel.Id : 0);
            var orderDetail = _orderDetailService.GetOrderDetails(order.Id);
            customer.Nationality = _nationalityService.GetNationality(customer.NationalId != null ? (int)customer.NationalId : 0);

            var orderModel = new OrderModel
            {
                Id = order.Id,
                Pnr = order.Pnr,
                StartDate = order.StartDate.ToShortDateString(),
                StartDateCompare = order.StartDate,
                EndDate = order.EndDate.ToShortDateString(),
                Status = order.Status,
                CreatedDate = order.CreatedDate.ToShortDateString(),
                Room = room,
                Customer = customer,
                Price = order.Price,
                Quantity = order.Quantity,
                Amount = order.Amount,
                Avatar = room != null ? string.Format("https://goreise.com{0}", Url.Content(room.ImageUrl)) : "~/Content/img/no-image.jpg",
                OrderDetails = orderDetail,
                PaymentMethod = order.PaymentMethod,
                CardNumber = order.CardNumber,
                ProductId = order.ProductId ?? 0,
                SpecialRequest = !string.IsNullOrEmpty(order.SpecialRequest) ? order.SpecialRequest : string.Empty,
                Night = order.Night,
                CancellationPolicy = order.CancellationPolicy,
                Discount = order.Discount > 0 ? string.Format("{0}<sup>USD</sup>", order.Discount) : "N/A",
                TaxFee = order.TaxFee > 0 ? string.Format("{0}<sup>USD</sup>", order.TaxFee) : "N/A",
                SurchargeFee = order.SurchargeFee > 0 ? string.Format("{0}<sup>USD</sup>", order.SurchargeFee) : "N/A",
                SurchargeName = order.SurchargeName,
                ProductName = order.ProductName,
                DiscountName = order.DiscountName,
                OrderInformations = _orderDetailService.GetOrderInformations(id)

            };
            ViewBag.Order = orderModel;
            return View();
        }
        public ActionResult UpdateStatus(ObjectModel objectModel)
        {
            if (objectModel == null)
            {
                objectModel = new ObjectModel
                {
                    Status = (int)Utilities.Status.Inactive,
                    Message = "Updated order is error!"
                };
                return Json(objectModel);
            }
            if (objectModel.Status == (int)Utilities.BookingStatus.Paid)
            {
                var rateExchange = _rateExchangeService.GetRateExchangeById(3);
                var order = _orderService.GetOrderById(objectModel.Id);
                if (order != null)
                {
                    //Add change for OrderDetail table
                    var orderDetail = new OrderDetail
                    {
                        UserId = GetCurrentUserName(),
                        CreatedDate = DateTime.Now,
                        Note = "Booking is Approved",
                        ChangedName = "Approve booking",
                        ChangedValue = "Approved",
                        OrderId = objectModel.Id,

                        ObjectState = ObjectState.Added
                    };
                    if (order.Type == (int)Utilities.Product.Hotel)
                    {
                        //Add change for OrderDetail table
                        var orderDetailHotel = new OrderDetail
                        {
                            UserId = GetCurrentUserName(),
                            CreatedDate = DateTime.Now,
                            Note = "Booking is Approved, please send email for company",
                            ChangedName = "Company Request Booking",
                            ChangedValue = "CompanyRequestBooking",
                            OrderId = objectModel.Id,
                            ObjectState = ObjectState.Added
                        };
                        _orderDetailService.Insert(orderDetailHotel);
                    }

                    _orderDetailService.Insert(orderDetail);
                    //Added comlunm for changed of Order table
                    order.Status = (int)Utilities.BookingStatus.Paid;
                    order.ModifiedDate = DateTime.Now;
                    order.ObjectState = ObjectState.Modified;
                    order.PaymentMethod = objectModel.IntParam1;
                    order.CardNumber = objectModel.StrParam1;
                    order.RateExchange = rateExchange != null ? rateExchange.CurrentPrice : 0;
                    _orderService.Update(order);
                    //Added for Payment Order
                    var paymentOrder = new PaymentOrder
                    {
                        OrderId = objectModel.Id,
                        Status = (int)Utilities.Status.Active,
                        ModifiedDate = DateTime.Now,
                        CreatedDate = DateTime.Now,
                        OrderDate = DateTime.Now,
                        ObjectState = ObjectState.Added
                    };
                    _paymentOrderService.Insert(paymentOrder);
                    var order2 = _orderService2.GetOrderByOrderId(objectModel.Id);
                    if (order2 != null)
                    {
                        order2.Status = (int)Utilities.BookingStatus.Paid;
                        order2.ModifiedDate = DateTime.Now;
                        order2.ObjectState = ObjectState.Modified;
                        order2.PaymentMethod = objectModel.IntParam1;
                        order2.CardNumber = objectModel.StrParam1;
                        order2.RateExchange = rateExchange != null ? rateExchange.CurrentPrice : 0;
                        _orderService2.Update(order2);
                    }

                    try
                    {
                        _unitOfWorkAsync.SaveChanges();
                        objectModel.Status = (int)Utilities.Status.Active;
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        objectModel.Status = (int)Utilities.Status.Inactive;
                        objectModel.Message = "Update order is error!";
                        throw;
                    }
                }
            }
            else if (objectModel.Status == (int)Utilities.BookingStatus.Cancelled)
            {
                var order = _orderService.GetOrderById(objectModel.Id);
                if (order != null)
                {
                    //Add change for OrderDetail table
                    var orderDetail = new OrderDetail
                    {
                        UserId = GetCurrentUserName(),
                        CreatedDate = DateTime.Now,
                        Note = objectModel.StrParam1,
                        ChangedName = "Cancel booking",
                        ChangedValue = "Cancelled",
                        OrderId = objectModel.Id,

                        ObjectState = ObjectState.Added
                    };
                    _orderDetailService.Insert(orderDetail);
                    //Added comlunm for changed of Order table
                    order.CancelFee = objectModel.DecParam1;
                    if (order.CancelFee < order.Amount && order.CancelFee > 0)
                    {
                        order.Status = (int)Utilities.BookingStatus.CancelledFee;
                        order.TotalRefund = order.Amount + order.ThirdPersonFee + order.ExtraBed - order.Deposit - order.CancelFee;
                    }
                    else
                        order.Status = (int)Utilities.BookingStatus.Cancelled;


                    order.ModifiedDate = DateTime.Now;
                    order.ObjectState = ObjectState.Modified;
                    _orderService.Update(order);

                    var order2 = _orderService2.GetOrderByOrderId(objectModel.Id);
                    if (order2 != null)
                    {
                        order2.CancelFee = objectModel.DecParam1;
                        if (order.CancelFee < order.Amount)
                        {
                            order2.Status = (int)Utilities.BookingStatus.CancelledFee;
                            order2.Amount = order.Amount;
                            order2.TotalRefund = order2.Amount + order2.ThirdPersonFee + order2.ExtraBed - order2.Deposit - order2.CancelFee;
                        }
                        else
                            order2.Status = (int)Utilities.BookingStatus.Cancelled;
                        order2.Status = (int)Utilities.BookingStatus.Cancelled;
                        order2.ModifiedDate = DateTime.Now;
                        order2.ObjectState = ObjectState.Modified;
                        _orderService2.Update(order2);
                    }
                    try
                    {
                        _unitOfWorkAsync.SaveChanges();
                        objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                        objectModel.Message = string.Format("Update order is error!");
                        throw;
                    }
                }
            }
            else if (objectModel.Status == (int)Web.Framework.Utilities.BookingStatus.Refunded)
            {
                var order = _orderService.GetOrderById(objectModel.Id);
                if (order != null)
                {
                    //Add change for OrderDetail table
                    var orderDetail = new OrderDetail
                    {
                        UserId = GetCurrentUserName(),
                        CreatedDate = DateTime.Now,
                        Note = objectModel.StrParam1,
                        ChangedName = string.Format("Refund booking"),
                        ChangedValue = string.Format("Refunded ${0}", objectModel.DecParam1),
                        OrderId = objectModel.Id,

                        ObjectState = ObjectState.Added
                    };
                    _orderDetailService.Insert(orderDetail);
                    //Added comlunm for changed of Order table
                    //order.Status = (int)Web.Framework.Utilities.BookingStatus.Refunded;
                    order.IsRefund = true;
                    order.TotalRefund = objectModel.DecParam1;
                    order.ModifiedDate = DateTime.Now;
                    order.ObjectState = ObjectState.Modified;
                    _orderService.Update(order);
                    var order2 = _orderService2.GetOrderByOrderId(objectModel.Id);
                    if (order2 != null)
                    {
                        order.IsRefund = true;
                        order.TotalRefund = objectModel.DecParam1;
                        order.ModifiedDate = DateTime.Now;
                        order.ObjectState = ObjectState.Modified;
                        _orderService2.Update(order2);
                    }
                    try
                    {
                        _unitOfWorkAsync.SaveChanges();
                        objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                        objectModel.Message = string.Format("Update order is error!");
                        throw;
                    }
                }
            }
            else if (objectModel.Status == (int)Web.Framework.Utilities.BookingStatus.VatInvoice)
            {
                var order = _orderService.GetOrderById(objectModel.Id);
                if (order != null)
                {
                    //Add change for OrderDetail table
                    var orderDetail = new OrderDetail
                    {
                        UserId = GetCurrentUserName(),
                        CreatedDate = DateTime.Now,
                        Note = objectModel.StrParam1,
                        ChangedName ="VAT Invoice",
                        ChangedValue = "VAT Invoice",
                        OrderId = objectModel.Id,
                        ObjectState = ObjectState.Added
                    };
                    _orderDetailService.Insert(orderDetail);
                    try
                    {
                        _unitOfWorkAsync.SaveChanges();
                        objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                        objectModel.Message = string.Format("Update order is error!");
                        throw;
                    }
                }
            }
            else if (objectModel.Status == (int)Web.Framework.Utilities.BookingStatus.Amended)
            {
                var order = _orderService.GetOrderById(objectModel.Id);
                if (order != null)
                {
                    //Add change for OrderDetail table
                    var orderDetail = new OrderDetail
                    {
                        UserId = GetCurrentUserName(),
                        CreatedDate = DateTime.Now,
                        Note = objectModel.StrParam1,
                        ChangedName = string.Format("Amend booking"),
                        ChangedValue = string.Format("Amended date from {0} to {1}", objectModel.IntParam2, objectModel.StrParam3),
                        OrderId = objectModel.Id,

                        ObjectState = ObjectState.Added
                    };
                    _orderDetailService.Insert(orderDetail);



                    //Added comlunm for changed of Order table
                    //on case hotel just allow add surcharge, extra bed.
                    //on other case allow change start date.
                    DateTime dateTmpl;
                    DateTime.TryParse(objectModel.StrParam2, out dateTmpl);
                    DateTime endDate;
                    DateTime.TryParse(objectModel.StrParam3, out endDate);
                    var spanTime = order.EndDate - order.StartDate;
                    var spanTimeStart = dateTmpl - order.StartDate;
                    if (order.Type == (int)Web.Framework.Utilities.Product.Hotel)
                    {
                        order.StartDate = dateTmpl;
                        order.EndDate = order.StartDate.Add(spanTime);
                        var order001 = _orderService.GetOrder(order.Id);
                        foreach (var detail in order001.OrderInformations)
                        {
                            DateTime dateModify = DateTime.Parse(detail.Date);
                            detail.Date = dateModify.Add(spanTimeStart).ToString("MM/dd/yyyy");
                            _orderInformationService.Update(detail);
                        }

                    }
                    else
                    {
                        order.StartDate = dateTmpl;
                        if (endDate > DateTime.MinValue)
                        {
                            order.EndDate = endDate;
                        }
                    }


                    order.ModifiedDate = DateTime.Now;
                    order.ObjectState = ObjectState.Modified;
                    _orderService.Update(order);
                    try
                    {
                        _unitOfWorkAsync.SaveChanges();
                        objectModel.Status = (int)Web.Framework.Utilities.Status.Active;
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        objectModel.Status = (int)Web.Framework.Utilities.Status.Inactive;
                        objectModel.Message = string.Format("Update order is error!");
                        throw;
                    }
                }
            }
            return Json(objectModel);
        }
        public ActionResult Deposit(ObjectModel objectModel)
        {
            if (objectModel == null)
            {
                objectModel = new ObjectModel
                {
                    Status = (int)Utilities.Status.Inactive,
                    Message = "Updated order is error!"
                };
                return Json(objectModel);
            }
            var order = _orderService.GetOrderById(objectModel.Id);
            if (order != null)
            {
                //Add change for OrderDetail table
                var orderDetail = new OrderDetail
                {
                    UserId = GetCurrentUserName(),
                    CreatedDate = DateTime.Now,
                    Note = "Deposit booking ",
                    ChangedName = $"Deposit: ${objectModel.DecParam1}<br/>Date: {DateTime.Now}",
                    ChangedValue = "Deposit",
                    Value = objectModel.StrParam1,
                    OrderId = objectModel.Id,

                    ObjectState = ObjectState.Added
                };
                _orderDetailService.Insert(orderDetail);
                //Added comlunm for changed of Order table
                if (order.Deposit == null)
                {
                    order.Deposit = 0;
                }
                var deposit = order.Deposit + objectModel.DecParam1;
                order.ModifiedDate = DateTime.Now;
                order.ObjectState = ObjectState.Modified;
                order.Status = (int)Utilities.BookingStatus.Deposit;
                order.Deposit = deposit;
                _orderService.Update(order);
                try
                {
                    _unitOfWorkAsync.SaveChanges();
                    objectModel.Status = (int)Utilities.Status.Active;
                }
                catch (DbUpdateConcurrencyException)
                {
                    objectModel.Status = (int)Utilities.Status.Inactive;
                    objectModel.Message = "Update order is error!";
                    throw;
                }
            }
            return Json(objectModel);
        }
        public ActionResult SendVoucherEmail(ObjectModel objectModel)
        {

            if (objectModel == null)
            {
                objectModel = new ObjectModel
                {
                    Status = (int)Utilities.Status.Inactive,
                    Message = "Cannot send email for customer!"
                };
                return Json(objectModel);
            }


            var order = _orderService.GetOrderById(objectModel.Id);
            var room = _roomService.GetRoom(order.ProductId ?? -1);
            var hotel = _hotelService.GetHotelSingle(room.HotelId ?? -1);
            //Send email request for vinaday team.
            var customer = _customerService.GetCustomerById(order.CustomerId ?? -1);
            try
            {
                dynamic myObject = new
                {
                    DateTime = DateTime.Now.ToString("D", CultureInfo.CreateSpecificCulture("en-US")),
                    CustomerFullName = $"{customer.Firstname ?? ""} {customer.Lastname ?? ""}",
                    ServiceName = order.ProductName,
                    Pnr = order.Pnr,
                    Address = hotel.StreetAddressEnglish,
                    Country = Utilities.UppercaseWords(hotel.Country.Replace("-", " ")),
                    ArrivalDate = order.StartDate.ToString("MMMM dd, yyyy"),
                    DepartureDate = order.EndDate.ToString("MMMM dd, yyyy"),
                    Nights = order.Night,
                    Rooms = order.Quantity,
                    RoomType = room.Name,
                    SpecialNotes = order.SpecialRequest,
                    Breakfast = room.BreakfastInclude == true ? "Included" : "Not Included",
                    TotalRoomCharge = order.Amount + order.ExtraBed + order.ThirdPersonFee - order.Discount,

                };
                string bookingEmailResult = Utilities.ParseTemplate("ConfirmBooking", myObject);
                var subject = $"VINADAY goREISE Booking #{order.Pnr}";
                //MailClient.RequestBookingHotel(customer.Email, bookingEmailResult, subject);
            }
            catch (Exception ex)
            {
                objectModel = new ObjectModel
                {
                    Status = (int)Utilities.Status.Inactive,
                    Message = "Cannot send email for customer!"
                };
                return Json(objectModel);
            }
            //Send email function here
            var orderDetail = _orderDetailService.GetOrderDetail(objectModel.IntParam1);
            if (orderDetail == null)
            {
                objectModel = new ObjectModel
                {
                    Status = (int)Utilities.Status.Inactive,
                    Message = "Cannot send email for customer!"
                };
                return Json(objectModel);
            }
            orderDetail.IsSend = true;
            orderDetail.ObjectState = ObjectState.Modified;
            _orderDetailService.Update(orderDetail);
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Utilities.Status.Inactive;
                objectModel.Message = "Update send email is error!";
                throw;
            }
            objectModel.Status = (int)Utilities.Status.Active;
            objectModel.Message = "Send email is successfully!";
            return Json(objectModel);
        }
        public ActionResult SendEmailRequest(ObjectModel objectModel)
        {
            if (objectModel == null)
            {
                objectModel = new ObjectModel
                {
                    Status = (int)Utilities.Status.Inactive,
                    Message = "Cannot send email for customer!"
                };
                return Json(objectModel);
            }
            var order = _orderService.GetOrderById(objectModel.Id);
            var room = _roomService.GetRoom(order.ProductId ?? -1);
            var hotel = _hotelService.GetHotelSingle(room.HotelId ?? -1);
            //Send email request for vinaday team.
            var customer = _customerService.GetCustomerById(order.CustomerId ?? -1);
            try
            {
                dynamic myObject = new
                {
                    DateTime = DateTime.Now.ToString("D", CultureInfo.CreateSpecificCulture("en-US")),
                    Company = $"{customer.Firstname ?? ""} {customer.Lastname ?? ""}",
                    ServiceName = hotel.Name,
                    Pnr = order.Pnr,
                    Country = Utilities.UppercaseWords(hotel.Country.Replace("-", " ")),
                    CheckIn = order.StartDate.ToString("MMMM dd, yyyy"),
                    CheckOut = order.EndDate.ToString("MMMM dd, yyyy"),
                    Nights = order.Night,
                    Rooms = order.Quantity,
                    RoomType = room.Name,
                    SpecialNotes = order.SpecialRequest,
                    Breakfast = room.BreakfastInclude == true ? "Included" : "Not Included",
                    TotalRoomCharge = order.Amount + order.ExtraBed + order.ThirdPersonFee - order.Discount,

                };
                string bookingEmailResult = Utilities.ParseTemplate("CompanyRequestBooking", myObject);
                var subject = $"Vinaday Booking ID 00{order.Id} - CONFIRMED Check-in {order.StartDate.ToString("MMMM dd, yyyy")}";
                MailClient.RequestBookingForHotel(bookingEmailResult, subject, objectModel.StrParam1);
            }
            catch (Exception ex)
            {
                objectModel = new ObjectModel
                {
                    Status = (int)Utilities.Status.Inactive,
                    Message = "Cannot send email for customer!"
                };
                return Json(objectModel);
            }
            //Send email function here
            var orderDetail = _orderDetailService.GetOrderDetail(objectModel.IntParam1);
            if (orderDetail == null)
            {
                objectModel = new ObjectModel
                {
                    Status = (int)Utilities.Status.Inactive,
                    Message = "Cannot send email for customer!"
                };
                return Json(objectModel);
            }
            orderDetail.IsSend = true;
            orderDetail.ObjectState = ObjectState.Modified;
            _orderDetailService.Update(orderDetail);
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Utilities.Status.Inactive;
                objectModel.Message = "Update send email is error!";
                throw;
            }
            objectModel.Status = (int)Utilities.Status.Active;
            objectModel.Message = "Send email is successfully!";
            return Json(objectModel);
        }
        public ActionResult SendEmail(ObjectModel objectModel)
        {

            if (objectModel == null)
            {
                objectModel = new ObjectModel
                {
                    Status = (int)Utilities.Status.Inactive,
                    Message = "Cannot send email for customer!"
                };
                return Json(objectModel);
            }


            var order = _orderService.GetOrderById(objectModel.Id);

            //Send email request for vinaday team.
            var customer = _customerService.GetCustomerById(order.CustomerId ?? -1);
            try
            {
                dynamic myObject = new
                {
                    DateTime = DateTime.Now.ToString("D", CultureInfo.CreateSpecificCulture("en-US")),
                    CustomerFullName = $"{customer.Firstname ?? ""} {customer.Lastname ?? ""}",
                    ServiceName = order.ProductName,
                    Pnr = order.Pnr,
                    Price = order.Price,
                    Total = order.Amount,
                    Cancellation = order.CancellationPolicy
                };
                string bookingEmailResult = Utilities.ParseTemplate("ConfirmBooking", myObject);
                var subject = $"VINADAY goREISE Booking #{order.Pnr}";
                //MailClient.RequestBookingHotel(customer.Email, bookingEmailResult, subject);
            }
            catch (Exception ex)
            {
                objectModel = new ObjectModel
                {
                    Status = (int)Utilities.Status.Inactive,
                    Message = "Cannot send email for customer!"
                };
                return Json(objectModel);
            }
            //Send email function here
            var orderDetail = _orderDetailService.GetOrderDetail(objectModel.IntParam1);
            if (orderDetail == null)
            {
                objectModel = new ObjectModel
                {
                    Status = (int)Utilities.Status.Inactive,
                    Message = "Cannot send email for customer!"
                };
                return Json(objectModel);
            }
            orderDetail.IsSend = true;
            orderDetail.ObjectState = ObjectState.Modified;
            _orderDetailService.Update(orderDetail);
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Utilities.Status.Inactive;
                objectModel.Message = "Update send email is error!";
                throw;
            }
            objectModel.Status = (int)Utilities.Status.Active;
            objectModel.Message = "Send email is successfully!";
            return Json(objectModel);
        }
        //public ActionResult SendEmail(ObjectModel objectModel)
        //{

        //    if (objectModel == null){
        //        objectModel = new ObjectModel
        //        {
        //            Status = (int)Utilities.Status.Inactive,
        //            Message = "Cannot send email for customer!"
        //        };
        //        return Json(objectModel);
        //    }
        //    //Send email function here
        //    var orderDetail = _orderDetailService.GetOrderDetail(objectModel.Id);
        //    if (orderDetail == null) return Json(objectModel);
        //    _orderDetailService.Insert(orderDetail);
        //    //Added comlunm for changed of Order table
        //    orderDetail.IsSend = true;
        //    orderDetail.ObjectState = ObjectState.Modified;
        //    _orderDetailService.Update(orderDetail);
        //    try
        //    {
        //        _unitOfWorkAsync.SaveChanges();
        //        objectModel.Status = (int)Utilities.Status.Active;
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        objectModel.Status = (int)Utilities.Status.Inactive;
        //        objectModel.Message = "Update send email is error!";
        //        throw;
        //    }

        //    return Json(objectModel);
        //}
        public ActionResult Order()
        {

            DateTime fromDate = DateTime.Now.AddMonths(-1);
            DateTime toDate = DateTime.Now.AddMonths(1);
            var orders = _orderService.GetOrdersByDate(fromDate, toDate, false);
            var orderUpComming = new List<SP_GetAllNewBooking_Result>();
            using (vinadaydbEntities db = new vinadaydbEntities())
            {
                orderUpComming = db.SP_GetAllNewBooking().ToList();
            }
            ViewBag.UpCommingOrder = orderUpComming;
            // var orderBys = _orderService.GetOrdersByDate(fromDate, toDate, false);
            ViewBag.ByCreatedDate = true;
            ViewBag.ByCheckInDate = false;
            ViewBag.Orders = orders.OrderBy(a => a.CreatedDate).ToList();
            ViewBag.Users = GetRolesToUsers();
            ViewBag.ByCreatedDate = true;
            return View();
        }

        [HttpPost]
        public ActionResult Order(FormCollection form)
        {
            string fromdate = form["fromdate"];
            string todate = form["todate"];
            bool searchByCreatedDate = form["SearchBy"] == "by-created-date";
            DateTime fromDate = Utilities.ConvertStringToDateTime(fromdate);
            DateTime toDate = Utilities.ConvertStringToDateTime(todate); ;
            var orders = _orderService.GetOrdersByDate(fromDate, toDate, searchByCreatedDate);
            ViewBag.FromDate = fromdate;
            ViewBag.ToDate = todate;
            ViewBag.ByCreatedDate = searchByCreatedDate;
            ViewBag.ByCheckInDate = form["SearchBy"] == "by-check-in";
            ViewBag.Orders = orders.OrderBy(a => a.CreatedDate).ToList();
            ViewBag.Users = GetRolesToUsers();
            return View();
        }


        public ActionResult OrderOfAccountant()
        {
            //ViewBag.Orders = _orderService.GetPaidOrders();
            ViewBag.Exchange = _rateExchangeService.GetRateExchangeById(3);
            return View();
        }
        public ActionResult OrderForUser()
        {
            //var userName = GetCurrentUserName();
            //ViewBag.Orders = _orderService.GetOrdersByUserName(userName);
            ViewBag.Exchange = _rateExchangeService.GetRateExchangeById(3);
            return View();
        }

        private string GetCurrentUserName()
        {
            var context = new SecurityContext();
            var currentUserId = User.Identity.GetUserId();
            var currentUser = context.Users.FirstOrDefault(x => x.Id == currentUserId);
            return currentUser != null ? currentUser.UserName : "N/A";
        }
        [HttpPost]
        public ActionResult UpdateBookingManagement(ObjectModel objectModel)
        {
            if (objectModel == null)
            {
                objectModel = new ObjectModel
                {
                    Status = (int)Utilities.Status.Inactive,
                    Message = "A assigned for user is error!"
                };
                return Json(objectModel);
            }
            var order = _orderService.GetOrderById(objectModel.Id);
            //Added comlunm
            if (order != null)
            {
                order.ModifiedDate = DateTime.Now;
                order.Management = objectModel.StrParam1;
                //Update tour
                order.ObjectState = ObjectState.Modified;
                _orderService.Update(order);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Utilities.Status.Active;
                objectModel.Url = "A assigned for user is successfully!";

            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Utilities.Status.Inactive;
                objectModel.Message = "A assigned for user is error!";
                throw;
            }
            return Json(objectModel);
        }
        public List<ApplicationUser> GetRolesToUsers()
        {
            var context = new SecurityContext();
            var users = context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains("1fdd1eac-7191-441c-8c13-679c4d6fa1a3")).ToList();
            return users;
        }
        /// <summary>
        /// Get order by id 
        /// on this order we could easy get order by type
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult OrderDetail(int id)
        {

            OrderModel order = _orderService.GetOrder(id);
            var exchanges = _rateExchangeService.GetRateExchangeById(3);
            ViewBag.Exchange = exchanges;
            if (order != null)
            {
                if (!order.RateExchange.HasValue)
                {
                    order.RateExchange = exchanges.CurrentPrice;
                    var book = _orderService.GetOrderById(id);
                    book.RateExchange = exchanges.CurrentPrice;
                    _orderService.Update(book);
                    _unitOfWorkAsync.SaveChanges();
                }

            }
            ViewBag.Order = order;
            Hotel hotel = ViewBag.Hotel = _hotelService.GetHotelSingleByRoomId(order?.ProductId ?? -1);
            var images =
               _imageService.GetImageListByHotelId(hotel.Id)
                   .Where(i => i.PictureType == Constant.ImageMaptype)
                   .ToList();
            ViewBag.HotelImage = images.FirstOrDefault();

            if (order.Type == 2)
            {
                ViewBag.TourImage = _mediaService.Queryable().FirstOrDefault(t => t.OwnerId == order.ProductId && t.MediaType == 4);
            }

            ViewBag.HotelImage = images.FirstOrDefault();
            ViewBag.RateExchange = _rateExchangeService.GetRateExchangeById(3);
            string emailList = string.Empty;
            if (hotel != null)
            {
                var mainEmail = _hotelService.GetContact(hotel.MaincontractId);
                var marketingEmail = _hotelService.GetContact(hotel.MarketingcontractId ?? -1);
                var reservationEmail = _hotelService.GetContact(hotel.ReservationcontractId ?? -1);

                if (mainEmail?.EMAILADDRESS != null && !emailList.Contains(mainEmail.EMAILADDRESS))
                {
                    emailList += $"{mainEmail.EMAILADDRESS},";
                }
                if (marketingEmail?.EMAILADDRESS != null && !emailList.Contains(marketingEmail.EMAILADDRESS))
                {
                    emailList += $"{marketingEmail.EMAILADDRESS},";
                }
                if (reservationEmail?.EMAILADDRESS != null && !emailList.Contains(reservationEmail.EMAILADDRESS))
                {
                    emailList += $"{reservationEmail.EMAILADDRESS},";
                }
            }
            ViewBag.ContactStr = emailList;
            return View();
        }

        /// <summary>
        /// Get order by id 
        /// on this order we could easy get order by type
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult OrderDetailAccountant(int id)
        {
            ViewBag.Exchange = _rateExchangeService.GetRateExchangeById(3);
            var order = _orderService2.GetOrder2(id);
            order.TourOrderId = id;
            ViewBag.Order = order;
            Hotel hotel = ViewBag.Hotel = _hotelService.GetHotelSingleByRoomId(order != null ? order.ProductId : -1);
            ViewBag.RateExchange = _rateExchangeService.GetRateExchangeById(3);
            string emailList = string.Empty;
            if (hotel != null)
            {
                var mainEmail = _hotelService.GetContact(hotel.MaincontractId);
                var marketingEmail = _hotelService.GetContact(hotel.MarketingcontractId ?? -1);
                var reservationEmail = _hotelService.GetContact(hotel.ReservationcontractId ?? -1);

                if (mainEmail?.EMAILADDRESS != null && !emailList.Contains(mainEmail.EMAILADDRESS))
                {
                    emailList += $"{mainEmail.EMAILADDRESS},";
                }
                if (marketingEmail?.EMAILADDRESS != null && !emailList.Contains(marketingEmail.EMAILADDRESS))
                {
                    emailList += $"{marketingEmail.EMAILADDRESS},";
                }
                if (reservationEmail?.EMAILADDRESS != null && !emailList.Contains(reservationEmail.EMAILADDRESS))
                {
                    emailList += $"{reservationEmail.EMAILADDRESS},";
                }
            }
            ViewBag.ContactStr = emailList;
            return View();
        }
        public ActionResult OrderDetailOfAccountant(int id)
        {
            var order = _accountantOrderService.GetOrder(id);
            if (order == null)
            {
                return View(new AccountantOrders());
            }
            ViewBag.OrderInformations = _orderDetailService.GetAccountantOrderDetails(id);
            ViewBag.Exchange = _rateExchangeService.GetRateExchangeById(3);
            return View(order);
        }
        public ActionResult UpdateOrderForAccountant()
        {
            var orders = _orderService.GetOrdersForTest().ToList();

            foreach (var order in orders)
            {
                if (order == null) { continue; }
                var description = string.Empty;
                switch (order.Type)
                {
                    case (int)Services.Utilities.ProductType.Hotel:
                        {
                            var room = _roomService.GetRoom(order.ProductId ?? 0);

                            description = room != null ? room.Name : string.Empty;
                        }
                        break;
                    case (int)Services.Utilities.ProductType.Tour:
                        {
                            var tour = _tourService.GetTourById(order.ProductId ?? 0);
                            description = tour != null ? tour.Name : string.Empty;
                        }
                        break;
                    default:
                        {
                            // var tour = _tourRepository.GetTour(order.ProductId ?? 0);
                            // orderModel.ProductName = tour != null ? tour.Name : string.Empty;
                        }
                        break;
                }
                var customer = _customerService.GetCustomerById(order.CustomerId ?? -1);

                if (customer != null)
                {
                    var nationality = _nationalityService.GetNationality(customer.NationalId ?? 0);
                    AccountantOrders accountantOrder = new AccountantOrders()
                    {
                        ParentId = order.Id,
                        Name = order.ProductName,
                        Description = description,
                        Cancellation = order.CancellationPolicy,
                        CheckIn = order.StartDate,
                        CheckOut = order.EndDate,
                        Deposit = order.Deposit,
                        Discount = order.Discount,
                        ExtraBedFee = order.ExtraBed,
                        GuestCountry = nationality.Name,
                        GuestFullName = customer.Firstname + " " + customer.Lastname,
                        GuestPhone = customer.PhoneNumber,
                        GuestEmail = customer.Email,
                        Method = Enum.GetName(typeof(Utilities.PaymentMethod), order.PaymentMethod),
                        Pnr = order.Pnr,
                        SurchargeFee = order.SurchargeFee,
                        TaxFee = order.TaxFee,
                        ThirdPersonFee = order.ThirdPersonFee,
                        Type = order.Type,
                        ObjectState = ObjectState.Added,
                        RateExchange = order.RateExchange ?? 21000
                    };
                    _accountantOrderService.Insert(accountantOrder);
                }
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Json(new { success = false, error = "Error when processing booking" }, JsonRequestBehavior.AllowGet);
            }
            return Content(" ");
        }
        //Clear Order
        public ActionResult ClearOrders()
        {
            var orders = _orderService.GetOrdersForTest().Where(o => o.Status == 5).ToList();

            foreach (var order in orders)
            {
                if (order == null) { continue; }
                var orderDetails = _orderDetailService.GetOrderDetails(order.Id);
                foreach (var orderDetail in orderDetails.Where(orderDetail => orderDetail != null))
                {
                    orderDetail.ObjectState = ObjectState.Deleted;
                    _orderDetailService.Delete(orderDetail);
                }
                order.ObjectState = ObjectState.Deleted;
                _orderService.Delete(order);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Json(new { success = false, error = "Error when processing booking" }, JsonRequestBehavior.AllowGet);
            }
            return Content(" ");
        }
        public ActionResult UpdateOrderForAccountant1()
        {
            var orders = _orderService.GetOrdersForTest().ToList();

            foreach (var order in orders)
            {
                if (order == null || order.Type == (int)Services.Utilities.ProductType.Hotel) { continue; }

                var customer = _customerService.GetCustomerById(order.CustomerId ?? -1);

                if (customer != null)
                {
                    AccountantOrderDetails accountantOrder = new AccountantOrderDetails()
                    {

                        Discount = order.Discount,
                        Amount = order.Amount,
                        OrderId = order.Id,
                        Date = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                        CreatedDate = DateTime.Now,
                        ObjectState = ObjectState.Added,
                        Price = order.Price,
                        DiscountName = order.DiscountName,
                        Surcharge = order.SurchargeFee,
                        SurchargeName = order.SurchargeName,
                        Quantity = order.Quantity ?? 1,
                        ModifiedDate = DateTime.Now,
                        Status = 2
                    };
                    _accountantOrderDetailService.Insert(accountantOrder);
                }
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Json(new { success = false, error = "Error when processing booking" }, JsonRequestBehavior.AllowGet);
            }
            return Content(" ");
        }
        public ActionResult OrderCreate()
        {
            ViewBag.Exchange = _rateExchangeService.GetRateExchangeById(3);
            ViewBag.Hotels = _hotelService.GetHotels();
            return View();
        }
        public List<RoomModel> GetRooms(int hotelId, DateTime checkIn, DateTime checkOut)
        {
            var roomsModel = new List<RoomModel>();
            var rooms = _roomService.GetRoomList(hotelId);
            if (rooms.Count > 0)
            {
                foreach (var room in rooms)
                {
                    var checkInTemp = checkIn;
                    var checkOutTemp = checkOut;
                    var roomModel = new RoomModel
                    {
                        Id = room.Id,
                        AdultNumber = room.AdultNumber ?? 0,
                        Name = room.Name,
                        ExtraBed = room.ExtraBed ?? 0,
                        ExtraBedPrice = room.ExtraBedPrice ?? 0,
                        ImageUrl = room.ImageUrl,
                        ChildrenAge = room.ChildrenAge ?? 0,
                        ChildrenNumber = room.ChildrenNumber ?? 0,
                        RoomSize = room.RoomSize ?? 0,
                        MaxOccupancy = room.MaxOccupancy ?? 0,
                        MaxExtrabed = room.MaxExtrabed ?? 0,
                        RackRate = room.RackRate ?? 0,
                        Cancelation = _hotelService.GetCancellationHotelVn(hotelId),
                        IsBreakfast = false,
                        SellingRate = 0,
                        TotalAvailable = 0,
                        CloseOutRegular = false

                    };
                    int viewId;
                    int.TryParse(room.View, out viewId);
                    var firstOrDefault = _catDetailService.GetCategoryDetail(viewId);
                    if (firstOrDefault != null)
                    {
                        roomModel.ViewVn = firstOrDefault.DescriptionVn;
                    }
                    var isRoom = true;
                    while (checkInTemp < checkOutTemp)
                    {
                        var checkInTemp1 = DateTime.Parse(checkIn.ToShortDateString());
                        var roomTemp = _roomControlService.GetSingleRoomControlByDateRate(room.Id,
                            checkInTemp1);
                        if (roomTemp == null)
                        {
                            isRoom = false;
                        }
                        else
                        {
                            if (roomTemp.CloseOutRegular)
                            {
                                isRoom = false;
                            }
                            if (roomTemp.TotalAvailable < 0)
                            {
                                isRoom = false;
                            }
                            if (roomTemp.SellingRate < 0)
                            {
                                isRoom = false;
                            }
                        }
                        checkInTemp = checkInTemp.AddDays(1);
                    }
                    if (isRoom)
                    {
                        var checkInTemp2 = DateTime.Parse(checkIn.ToShortDateString());
                        var checkOutTemp2 = DateTime.Parse(checkOut.ToShortDateString());
                        var roomControl = _roomControlService.GetSingleRoomCheckInOut(room.Id, checkInTemp2,
                            checkOutTemp2);
                        if (roomControl != null)
                        {
                            roomModel.IsBreakfast = roomControl.Breakfast ?? false;
                            roomModel.SellingRate = roomControl.SellingRate / 1.15m ?? 0;
                            roomModel.TotalAvailable = roomControl.TotalAvailable;
                            roomModel.CloseOutRegular = roomControl.CloseOutRegular;

                        }
                        var checkInDate = Web.Framework.Utilities.ConvertToDateTime(checkIn.ToString(CultureInfo.InvariantCulture));
                        var checkOutDate = Web.Framework.Utilities.ConvertToDateTime(checkOut.ToString(CultureInfo.InvariantCulture));
                        var promotions = _roomService.GetPromotionsVn(roomModel, hotelId, room.Id, checkInDate, checkOutDate);

                        if (promotions != null && promotions.Count > 0)
                        {
                            roomsModel.AddRange(promotions);
                        }
                    }

                    roomsModel.Add(roomModel);
                }
            }
            return roomsModel;
        }
        [HttpPost]
        public ActionResult GetRooms(ObjectModel objectModel)
        {
            var checkIn = Web.Framework.Utilities.ConvertToDateTime(objectModel.StrParam1);
            var checkOut = Web.Framework.Utilities.ConvertToDateTime(objectModel.StrParam2);

            var rooms = GetRooms(objectModel.Id, checkIn, checkOut);
            return PartialView("_Room", rooms);
        }
        [HttpPost]
        public ActionResult GetHotelProduct()
        {
            var hotels = _hotelService.GetHotels();
            return PartialView("_ProductHotel", hotels);
        }
        [HttpPost]
        public ActionResult GetFlightTicketProduct()
        {
            var cities = _cityService.GetCities();
            return PartialView("_ProductFlightTicket", cities);
        }

        [HttpPost]
        public ActionResult GetRates(ObjectModel objectModel)
        {
            var tourModel = new TourModel
            {
                Rates = _tourRateService.GetTourRatesById(objectModel.Id),
                Tour = _tourService.GetTourById(objectModel.Id)
            };
            return PartialView("_TourRate", tourModel);
        }
        [HttpPost]
        public ActionResult GetTourProduct()
        {
            var tours = _tourService.GetTours();
            return PartialView("_ProductTour", tours);
        }
        [HttpPost]
        public ActionResult SetUnRead(int id)
        {
            var book = _orderService.GetOrderById(id);
            book.IsRead = false;
            _orderService.Update(book);
            _unitOfWorkAsync.SaveChanges();
            return Json(new { status = 1, message = "Completed" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SetAccountaintRead(int id)
        {
            var book = _orderService2.GetOrderByOrderId(id);
            book.IsRead = true;
            _orderService2.Update(book);
            _unitOfWorkAsync.SaveChanges();
            return Json(new { status = 1, message = "Completed" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SetAccountaintUnRead(int id)
        {
            var book = _orderService2.GetOrderByOrderId(id);
            book.IsRead = false;
            _orderService2.Update(book);
            _unitOfWorkAsync.SaveChanges();
            return Json(new { status = 1, message = "Completed" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SetRead(int id)
        {
            var book = _orderService.GetOrderById(id);
            book.IsRead = true;
            _orderService.Update(book);
            _unitOfWorkAsync.SaveChanges();
            return Json(new { status = 1, message = "Completed" }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GetHotelBooking(ObjectModel objectModel)
        {
            //Add customer
            var customer = new Customer();
            var existCustomer = _customerService.GetCustomerByEmail(objectModel.StrParam6);
            if (existCustomer == null)
            {
                customer.Firstname = objectModel.StrParam3;
                customer.Lastname = objectModel.StrParam4;
                customer.PhoneNumber = objectModel.StrParam5;
                customer.Email = objectModel.StrParam6;
                customer.NationalId = objectModel.IntParam2;
                customer.ObjectState = ObjectState.Added;
                _customerService.Insert(customer);
                try
                {
                    _unitOfWorkAsync.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Json(new { success = false, error = "Error when processing booking" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                existCustomer.Firstname = objectModel.StrParam3;
                existCustomer.Lastname = objectModel.StrParam4;
                existCustomer.PhoneNumber = objectModel.StrParam5;
                existCustomer.Email = objectModel.StrParam6;
                existCustomer.NationalId = objectModel.IntParam2;
                existCustomer.ObjectState = ObjectState.Modified;
                _customerService.Update(existCustomer);
                try
                {
                    _unitOfWorkAsync.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Json(new { success = false, error = "Error when processing booking" }, JsonRequestBehavior.AllowGet);
                }
                customer = existCustomer;
            }
            customer.Street = objectModel.Message;//Rendering as special request property
            var product = _bookingService.GetHotelProduct(1, objectModel.Id, objectModel.StrParam1, objectModel.StrParam2, objectModel.IntParam1, 0);
            product.TotalSave = objectModel.DecParam1;
            ViewBag.Customer = customer;
            if (!string.IsNullOrEmpty(objectModel.StrParam7))
            {
                var guestNational = _nationalityService.GetNationality(objectModel.IntParam3);
                ViewBag.Nationality = objectModel.StrParam7 + " - " + guestNational.Name;
            }
            else
            {
                ViewBag.Nationality = _nationalityService.GetNationality(customer.NationalId ?? 0).Name;
            }

            return PartialView("_HotelBooking", product);
        }



        [HttpPost]
        public ActionResult GetTourBooking(ObjectModel objectModel)
        {
            var id = objectModel.Id;//Pick product id value (hotel or tour)
            var total = objectModel.IntParam1;//Pick total product fr booking
            var checkIn = objectModel.StrParam1;//Pick check in or depart date

            //DecParam1
            //Add customer
            var customer = new Customer();
            var existCustomer = _customerService.GetCustomerByEmail(objectModel.StrParam6);
            if (existCustomer == null)
            {
                customer.Firstname = objectModel.StrParam3;
                customer.Lastname = objectModel.StrParam4;
                customer.PhoneNumber = objectModel.StrParam5;
                customer.Email = objectModel.StrParam6;
                customer.NationalId = objectModel.IntParam2;
                customer.ObjectState = ObjectState.Added;
                _customerService.Insert(customer);
                try
                {
                    _unitOfWorkAsync.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Json(new { success = false, error = "Error when processing booking" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                existCustomer.Firstname = objectModel.StrParam3;
                existCustomer.Lastname = objectModel.StrParam4;
                existCustomer.PhoneNumber = objectModel.StrParam5;
                existCustomer.Email = objectModel.StrParam6;
                existCustomer.NationalId = objectModel.IntParam2;
                existCustomer.ObjectState = ObjectState.Modified;
                _customerService.Update(existCustomer);
                try
                {
                    _unitOfWorkAsync.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Json(new { success = false, error = "Error when processing booking" }, JsonRequestBehavior.AllowGet);
                }
                customer = existCustomer;
            }
            customer.Street = objectModel.Message;//Rendering as special request property
            ViewBag.Customer = customer;
            //ViewBag.Nationality = _nationalityService.GetNationality(customer.NationalId ?? 0);
            if (!string.IsNullOrEmpty(objectModel.StrParam7))
            {
                var guestNational = _nationalityService.GetNationality(objectModel.IntParam3);
                ViewBag.Nationality = objectModel.StrParam7 + " - " + guestNational.Name;
            }
            else
            {
                ViewBag.Nationality = _nationalityService.GetNationality(customer.NationalId ?? 0).Name;
            }
            //Add Order
            var product = new ProductModel();
            var tour = _tourService.GetTourById(id);
            if (tour == null) return PartialView("_TourBooking", product);

            var rate = _tourRateService.GetTourRatesByIdPersion(id, total);
            var depart = Web.Framework.Utilities.ConvertStringToDateTime(checkIn);
            var details = new List<ProductDetailModel>();
            var detail = new ProductDetailModel
            {
                Date = depart,
                Quantity = rate.PersonNo ?? 1,
                PriceRoom = decimal.Parse(rate.RetailRate.ToString())
            };
            details.Add(detail);
            var cancellation = _cancellationService.GetCancellationList().FirstOrDefault(c => c.Id == tour.CancelationPolicy);
            product = new ProductModel
            {
                Name = tour.Name,
                CheckIn = depart,
                Details = details,
                TotalTaxeFee = Math.Round(decimal.Parse((((rate.RetailRate * tour.CommissionRate / 100)) * rate.PersonNo).ToString()), 2),
                FinalPrice = Math.Round(decimal.Parse((
                    ((rate.RetailRate + (rate.RetailRate * tour.CommissionRate / 100)) *
                                            rate.PersonNo) - objectModel.DecParam1
                                            ).ToString()), 2),
                CancellationPolicy = cancellation,
                TotalSave = objectModel.DecParam1
            };
            return PartialView("_TourBooking", product);
        }
        [HttpPost]
        public ActionResult GetCustomerBooking(ObjectModel objectModel)
        {
            ViewBag.Nationalities = _nationalityService.GetNationalityList();
            if (!string.IsNullOrEmpty(objectModel.StrParam2))
            {
                var customer = _customerService.GetCustomerByPhone(objectModel.StrParam2) ?? new Customer()
                {
                    PhoneNumber = objectModel.StrParam2
                };
                return PartialView("_CustomerBooking", customer);
            }
            if (string.IsNullOrEmpty(objectModel.StrParam1)) return null;
            {
                var customer = _customerService.GetCustomerByEmail(objectModel.StrParam1) ?? new Customer()
                {
                    Email = objectModel.StrParam1
                };
                return PartialView("_CustomerBooking", customer);
            }
        }
        [HttpPost]
        public JsonResult CreateHotelOrder(ObjectModel objectModel)
        {
            var email = objectModel.StrParam3;//Pick email value
            var productId = objectModel.Id;//Pick product id value (hotel or tour)
            var checkIn = objectModel.StrParam1;//Pick check in or depart date
            var checkOut = objectModel.StrParam2;//Pick check out date
            var total = objectModel.IntParam1;//Pick total product fr booking
            var status = objectModel.Status;//Pick status (paid or Hoding)
            var specialRequest = objectModel.Message;//rendering for special request
            const int promotionId = 0;//Create promotion code
            var pnr = Web.Framework.Utilities.GetRandomString(7).ToUpper();//Random PNR order
            var userId = GetCurrentUserName();
            //Get customer by email address.
            var customer = _customerService.GetCustomerByEmail(email);
            //Get product by product id.
            var product = _bookingService.GetHotelProduct(1, productId, checkIn, checkOut, total, promotionId);
            var rateExchange = _rateExchangeService.GetRateExchangeById(3);
            var guestNational = _nationalityService.GetNationality(objectModel.IntParam2);
            //Create order.
            var order = new Order
            {
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                GuestFirstName = objectModel.StrParam4,
                GuestLastName = objectModel.StrParam5,
                GuestCountry = guestNational != null ? guestNational.Name : string.Empty,
                Status = status,
                Quantity = product.Quantity,
                CustomerId = customer.CustomerId,
                Pnr = pnr,
                TaxFee = product.TotalTaxeFee,
                Price = product.TotalPrice,
                ProductId = product.Id,
                Amount = product.FinalPrice - objectModel.DecParam1,
                Type = (int)Web.Framework.Utilities.Product.Hotel,
                StartDate = product.CheckIn,
                EndDate = product.CheckOut,
                Night = product.Night,
                SurchargeFee = product.TotalSurcharge,
                IpLocation = Web.Framework.Utilities.GetUserIp(Request),
                SpecialRequest = specialRequest,
                Management = userId,
                //Discount = product.TotalSave,
                Discount = objectModel.DecParam1,
                CancellationPolicy = product.CancellationPolicy != null ? product.CancellationPolicy.Description : string.Empty,
                ProductName = product.Name,
                //DiscountName = product.Promotion != null ? string.Format("{0}<br/>{1}", product.Promotion.Name, product.Promotion.Description) : string.Empty,
                DiscountName = "Special Sale",
                ObjectState = ObjectState.Added,
                RateExchange = rateExchange != null ? rateExchange.CurrentPrice : 0
            };
            //Insert hotel order
            _orderService.Add(order);
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Json(new { success = false, error = "Error when processing booking" }, JsonRequestBehavior.AllowGet);
            }
            var order2 = new Order2
            {
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                GuestFirstName = objectModel.StrParam4,
                GuestLastName = objectModel.StrParam5,
                GuestCountry = guestNational != null ? guestNational.Name : string.Empty,
                Status = status,
                Quantity = product.Quantity,
                CustomerId = customer.CustomerId,
                Pnr = pnr,
                TaxFee = product.TotalTaxeFee,
                Price = product.TotalPrice,
                ProductId = product.Id,
                Amount = product.FinalPrice - objectModel.DecParam1,
                Type = (int)Web.Framework.Utilities.Product.Hotel,
                StartDate = product.CheckIn,
                EndDate = product.CheckOut,
                Night = product.Night,
                SurchargeFee = product.TotalSurcharge,
                IpLocation = Web.Framework.Utilities.GetUserIp(Request),
                SpecialRequest = specialRequest,
                Management = userId,
                //Discount = product.TotalSave,
                Discount = objectModel.DecParam1,
                CancellationPolicy = product.CancellationPolicy != null ? product.CancellationPolicy.Description : string.Empty,
                ProductName = product.Name,
                //DiscountName = product.Promotion != null ? string.Format("{0}<br/>{1}", product.Promotion.Name, product.Promotion.Description) : string.Empty,
                DiscountName = "Special Sale",
                ObjectState = ObjectState.Added,
                RateExchange = rateExchange != null ? rateExchange.CurrentPrice : 0,
                TourOrderId = order.Id

            };
            //Insert hotel order
            _orderService2.Add(order2);


            if (status == (int)Web.Framework.Utilities.BookingStatus.Paid)
            {
                //Add change for OrderDetail table
                var orderDetail = new OrderDetail
                {
                    UserId = GetCurrentUserName(),
                    CreatedDate = DateTime.Now,
                    Note = string.Format("Booking is Approved"),
                    ChangedName = string.Format("Approve booking"),
                    ChangedValue = string.Format("Approved"),
                    OrderId = order.Id,

                    ObjectState = ObjectState.Added
                };
                _orderDetailService.Insert(orderDetail);
                //Added comlunm for changed of Order table
                order.ModifiedDate = DateTime.Now;
                order.PaymentMethod = (int)Web.Framework.Utilities.PaymentMethod.Cash;
                //Added for Payment Order
                var paymentOrder = new PaymentOrder
                {
                    OrderId = order.Id,
                    Status = (int)Utilities.Status.Active,
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    OrderDate = DateTime.Now,
                    ObjectState = ObjectState.Added
                };
                _paymentOrderService.Insert(paymentOrder);
                //Added for Payment Order
                var paymentOrder2 = new PaymentOrder2
                {
                    OrderId = order.Id,
                    Status = (int)Utilities.Status.Active,
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    OrderDate = DateTime.Now,
                    ObjectState = ObjectState.Added
                };
                _paymentOrderService2.Insert(paymentOrder2);
            }

            //Add orders information
            var productDetails = product.Details;
            if (productDetails.Count > 0)
            {
                foreach (var informationOrder in productDetails.Select(detail => new OrderInformations()
                {
                    OrderId = order.Id,
                    Price = detail.PriceRoom,
                    Date = detail.Date.ToString("MM/dd/yyyy"),
                    Quantity = product.Quantity ?? 0,
                    Discount = product.TotalSave,
                    DiscountName = product.Promotion != null ? string.Format("{0}<br/>{1}", product.Promotion.Name, product.Promotion.Description) : string.Empty,
                    Surcharge = detail.PriceSurcharge,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    Status = (int)Web.Framework.Utilities.Status.Active,
                    ObjectState = ObjectState.Added
                }))
                {
                    //Insert information order
                    _orderInformationService.Insert(informationOrder);
                }

                // save for accountant bill
                foreach (var informationOrder in productDetails.Select(detail => new OrderInformation2s()
                {
                    OrderId = order.Id,
                    Price = detail.PriceRoom,
                    Date = detail.Date.ToString("MM/dd/yyyy"),
                    Quantity = product.Quantity ?? 0,
                    Discount = product.TotalSave,
                    DiscountName = product.Promotion != null ? string.Format("{0}<br/>{1}", product.Promotion.Name, product.Promotion.Description) : string.Empty,
                    Surcharge = detail.PriceSurcharge,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    Status = (int)Web.Framework.Utilities.Status.Active,
                    ObjectState = ObjectState.Added
                }))
                {
                    //Insert information order
                    _orderInformationService2.Insert(informationOrder);
                }
            }

            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Json(new { success = false, error = "Error when processing booking" }, JsonRequestBehavior.AllowGet);
            }
            //Return json result of booking
            var resutls = new
            {
                success = true,
                bookingLink = string.Format("/rc/Booking/OrderDetail/{0}", order.Id),
                pnr
            };

            var jsonResult = Json(resutls, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult CreateTourOrder(ObjectModel objectModel)
        {
            var email = objectModel.StrParam2;//Pick email value
            var productId = objectModel.Id;//Pick product id value (hotel or tour)
            var depart = Web.Framework.Utilities.ConvertStringToDateTime(objectModel.StrParam1);//Pick check in or depart date
            var status = objectModel.Status;//Pick status (paid or Hoding)
            var specialRequest = objectModel.Message;//rendering for special request
            var pnr = Web.Framework.Utilities.GetRandomString(7).ToUpper();//Random PNR order
            var total = objectModel.IntParam1;//Pick total product fr booking
            //Get customer by email address.
            var customer = _customerService.GetCustomerByEmail(email);
            //Get product by product id.
            var product = _tourService.GetTourById(productId);
            //Get Rate 
            var rate = _tourRateService.GetTourRatesByIdPersion(productId, total);
            //Get cancellation
            var cancellation = _cancellationService.GetCancellationList().FirstOrDefault(c => c.Id == product.CancelationPolicy);
            var rateExchange = _rateExchangeService.GetRateExchangeById(3);
            var guestNational = _nationalityService.GetNationality(objectModel.IntParam2);
            //Create order.
            var userId = GetCurrentUserName();
            var order = new Order
            {
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                GuestFirstName = objectModel.StrParam4,
                GuestLastName = objectModel.StrParam5,
                GuestCountry = guestNational != null ? guestNational.Name : string.Empty,
                Status = status,
                Quantity = rate.PersonNo,
                CustomerId = customer.CustomerId,
                Pnr = pnr,
                Management = userId,
                TaxFee = Math.Round(decimal.Parse((((rate.RetailRate * product.CommissionRate / 100)) * rate.PersonNo).ToString()), 2),
                Price = rate.RetailRate,
                ProductId = product.Id,
                Amount = Math.Round(decimal.Parse(((rate.RetailRate +
                                            (rate.RetailRate * product.CommissionRate / 100)) *
                                           rate.PersonNo).ToString()) - objectModel.DecParam1, 2),
                Type = (int)Web.Framework.Utilities.Product.Tour,
                StartDate = depart,
                EndDate = depart,
                IpLocation = Web.Framework.Utilities.GetUserIp(Request),
                SpecialRequest = specialRequest,
                CancellationPolicy = cancellation != null ? cancellation.Description : string.Empty,
                ObjectState = ObjectState.Added,
                RateExchange = rateExchange != null ? rateExchange.CurrentPrice : 0,
                Discount = objectModel.DecParam1,
                DiscountName = "Special Sale",
            };
            //Insert hotel order
            _orderService.Add(order);
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Json(new { success = false, error = "Error when processing booking" }, JsonRequestBehavior.AllowGet);
            }
            var order2 = new Order2
            {
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                GuestFirstName = objectModel.StrParam4,
                GuestLastName = objectModel.StrParam5,
                GuestCountry = guestNational != null ? guestNational.Name : string.Empty,
                Status = status,
                Quantity = rate.PersonNo,
                CustomerId = customer.CustomerId,
                Pnr = pnr,
                Management = userId,
                TaxFee = Math.Round(decimal.Parse((((rate.RetailRate * product.CommissionRate / 100)) * rate.PersonNo).ToString()), 2),
                Price = rate.RetailRate,
                ProductId = product.Id,
                Amount = Math.Round(decimal.Parse(((rate.RetailRate +
                                            (rate.RetailRate * product.CommissionRate / 100)) *
                                           rate.PersonNo).ToString()) - objectModel.DecParam1, 2),
                Type = (int)Web.Framework.Utilities.Product.Tour,
                StartDate = depart,
                EndDate = depart,
                IpLocation = Web.Framework.Utilities.GetUserIp(Request),
                SpecialRequest = specialRequest,
                CancellationPolicy = cancellation != null ? cancellation.Description : string.Empty,
                ObjectState = ObjectState.Added,
                RateExchange = rateExchange != null ? rateExchange.CurrentPrice : 0,
                Discount = objectModel.DecParam1,
                DiscountName = "Special Sale",
                TourOrderId = order.Id

            };
            //Insert hotel order
            _orderService2.Add(order2);

            if (status == (int)Web.Framework.Utilities.BookingStatus.Paid)
            {
                //Add change for OrderDetail table
                var orderDetail = new OrderDetail
                {
                    UserId = GetCurrentUserName(),
                    CreatedDate = DateTime.Now,
                    Note = string.Format("Booking is Approved"),
                    ChangedName = string.Format("Approve booking"),
                    ChangedValue = string.Format("Approved"),
                    OrderId = order.Id,

                    ObjectState = ObjectState.Added
                };
                _orderDetailService.Insert(orderDetail);
                //Added comlunm for changed of Order table
                order.ModifiedDate = DateTime.Now;
                order.PaymentMethod = (int)Web.Framework.Utilities.PaymentMethod.Cash;
                //Added for Payment Order
                var paymentOrder = new PaymentOrder
                {
                    OrderId = order.Id,
                    Status = (int)Utilities.Status.Active,
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    OrderDate = DateTime.Now,
                    ObjectState = ObjectState.Added
                };
                _paymentOrderService.Insert(paymentOrder);

                var paymentOrder2 = new PaymentOrder2
                {
                    OrderId = order.Id,
                    Status = (int)Utilities.Status.Active,
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    OrderDate = DateTime.Now,
                    ObjectState = ObjectState.Added
                };
                _paymentOrderService2.Insert(paymentOrder2);
            }

            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Json(new { success = false, error = "Error when processing booking" }, JsonRequestBehavior.AllowGet);
            }
            //Return json result of booking
            var resutls = new
            {
                success = true,
                bookingLink = string.Format("/rc/Booking/OrderDetail/{0}", order.Id),
                pnr
            };

            var jsonResult = Json(resutls, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult UpdateReceipt2(List<PaymentOrderDetail2> paymentOrderDetails, Order2 paymentOrder)
        {
            var objectModel = new ObjectModel();
            //var order = _orderService2.GetOrderById(paymentOrder.TourOrderId);
            //if(order == null)
            //{
            //    order = _orderService2.GetOrderByOrderId(paymentOrder.TourOrderId);
            //}
            var order = _orderService2.GetOrderByOrderId(paymentOrder.TourOrderId);
            if (order == null)
            {
                order = _orderService2.GetOrderById(paymentOrder.TourOrderId);
            }
            if (order != null && paymentOrder != null)
            {
                order.ProductName = paymentOrder.ProductName;
                order.TaxFee = paymentOrder.TaxFee;
                order.Discount = paymentOrder.Discount;
                order.StartDate = paymentOrder.StartDate;
                order.EndDate = paymentOrder.EndDate;
                order.Amount = paymentOrder.Amount;
                order.ThirdPersonFee = paymentOrder.ThirdPersonFee;
                order.Quantity = paymentOrder.Quantity;
                if (!string.IsNullOrEmpty(paymentOrder.Note))
                {
                    order.Note = paymentOrder.Note;
                }

                order.Price = paymentOrder.Price;
                _orderService2.Update(order);
                try
                {
                    _unitOfWorkAsync.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Json(new { success = false, error = "Error when processing booking" }, JsonRequestBehavior.AllowGet);
                }
            }
            //
            OrderModel orderModel = _orderService2.GetOrder2(paymentOrder.TourOrderId);
            if (orderModel != null)
            {
                //orderModel.StartDate = paymentOrder.StartDate.ToString("MM/dd/yyyy");
                var orderInformation2S = orderModel.OrderInformation2s;
                if (paymentOrderDetails != null && paymentOrderDetails.Count > 0 && paymentOrderDetails.Count == orderInformation2S.Count)
                {
                    for (var i = 0; i < orderInformation2S.Count; i++)
                    {
                        var oi = _orderInformationService2.Find(orderInformation2S[i].Id);
                        //var oi = OrderInformation2s[i];
                        oi.Amount = paymentOrderDetails[i].Amount;
                        oi.CreatedDate = paymentOrder.StartDate;
                        oi.Quantity = paymentOrderDetails[i].Quantity;
                        oi.Price = paymentOrderDetails[i].Price;
                        _orderInformationService2.Update(oi);
                        try
                        {
                            _unitOfWorkAsync.SaveChanges();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            return Json(new { success = false, error = "Error when processing booking" }, JsonRequestBehavior.AllowGet);
                        }
                    }

                }
            }



            objectModel.Status = (int)Utilities.Status.Active;
            objectModel.Message = "Update successful";
            return Json(objectModel);
        }
        [HttpPost]
        public ActionResult GetVoucher(ObjectModel objectModel)
        {
            var voucher = _voucherService.GetVoucherByBookingId(objectModel.Id) ?? new Voucher();
            if (!string.IsNullOrEmpty(objectModel.StrParam1))
                voucher.HotelPhone = objectModel.StrParam1;
            if (string.IsNullOrEmpty(voucher.Description))
                voucher.Description = "Empty";
            if (string.IsNullOrEmpty(voucher.Localtion))
                voucher.Localtion = "Empty";
            voucher.Type = objectModel.IntParam1;
            return PartialView("_Voucher", voucher);
        }
        [HttpPost]
        public ActionResult GetPo(ObjectModel objectModel)
        {

            var paymentOrder = _paymentOrderService.GetPaymentOrderByOrderId(objectModel.Id);
            var order = ViewBag.Order = _orderService.GetOrder(objectModel.Id);
            //ViewBag.Exchange = _rateExchangeService.GetRateExchangeById(3);
            if (paymentOrder == null)
            {
                //paymentOrder = new PaymentOrder();
                //paymentOrder.OrderId = objectModel.Id;
                //_paymentOrderService.Insert(paymentOrder);
                //_unitOfWorkAsync.SaveChanges();

                var paymentOrderModel2 = new PaymentOrderModel
                {
                    PaymentOrder = new PaymentOrder() { Id = objectModel.Id },
                    PaymentOrderDetails = new List<PaymentOrderDetail>()
                };
                return PartialView("_Po", paymentOrderModel2);
            }
            var paymentOrderDetail = _paymentOrderDetailService.GetPaymentOrderDetails(paymentOrder.Id);
            var paymentOrderModel = new PaymentOrderModel
            {
                PaymentOrder = paymentOrder,
                PaymentOrderDetails = paymentOrderDetail
            };

            //var voucher = _voucherService.GetVoucherByBookingId(objectModel.Id) ?? new Voucher();
            return PartialView("_Po", paymentOrderModel);
        }

        //
        [HttpPost]
        public ActionResult GetPo2(int id)
        {

            var paymentOrder = _paymentOrderService2.GetPaymentOrderByOrderId(id);
            var order = ViewBag.Order = _orderService2.GetOrder2(id);
            //ViewBag.Exchange = _rateExchangeService.GetRateExchangeById(3);
            if (paymentOrder == null)
            {
                return PartialView("_Po2", new PaymentOrderModel2());
            }
            var paymentOrderDetail = _paymentOrderDetailService2.GetPaymentOrderDetails(paymentOrder.OrderId);
            var paymentOrderModel = new PaymentOrderModel2
            {
                PaymentOrder = paymentOrder,
                PaymentOrderDetails = paymentOrderDetail
            };

            //var voucher = _voucherService.GetVoucherByBookingId(objectModel.Id) ?? new Voucher();
            return PartialView("_Po2", paymentOrderModel);
        }


        [HttpPost]
        public ActionResult InsertPo(List<PaymentOrderDetail> paymentOrderDetails, PaymentOrder paymentOrder)
        {
            var objectModel = new ObjectModel();
            var paymentOrderTemp = _paymentOrderService.GetPaymentOrderByOrderId(paymentOrder.OrderId);
            if (paymentOrderTemp == null)
            {
                paymentOrderTemp = new PaymentOrder { OrderId = paymentOrder.OrderId };
                //objectModel.Status = (int)Utilities.Status.Inactive;
                //objectModel.Message = "Insert rate is error!";
                //return Json(objectModel);
            }
            var paymentDetails = _paymentOrderDetailService.GetPaymentOrderDetails(paymentOrderTemp.Id);
            if (paymentDetails != null && paymentDetails.Count > 0)
            {
                foreach (var paymentDetail in paymentDetails.Where(paymentDetail => paymentDetail != null))
                {
                    paymentDetail.ObjectState = ObjectState.Deleted;
                    _paymentOrderDetailService.Delete(paymentDetail);
                }
            }


            //decimal outCome = 0;
            //Added comlunm
            if (paymentOrderDetails == null) return Json(objectModel);
            foreach (var rate in paymentOrderDetails.Where(rate => rate != null))
            {
                rate.ObjectState = ObjectState.Added;
                rate.PaymentId = paymentOrderTemp.Id;
                //outCome += rate.Amount;
                _paymentOrderDetailService.Insert(rate);
            }
            paymentOrderTemp.Outcome = paymentOrder.Outcome;
            paymentOrderTemp.Income = paymentOrder.Income;
            paymentOrderTemp.IssueBy = paymentOrder.IssueBy;
            paymentOrderTemp.Profit = paymentOrder.Profit;
            paymentOrderTemp.Ratio = paymentOrder.Ratio;
            paymentOrderTemp.CreatedBy = paymentOrder.CreatedBy;
            paymentOrderTemp.ModifiedDate = DateTime.Now;
            paymentOrderTemp.CreatedDate = DateTime.Now;
            paymentOrderTemp.OrderDate = DateTime.Now;
            paymentOrderTemp.ObjectState = ObjectState.Modified;
            if (paymentOrderTemp.Id == 0)
                _paymentOrderService.Insert(paymentOrderTemp);
            else
                _paymentOrderService.Update(paymentOrderTemp);

            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Utilities.Status.Active;
                objectModel.Message = "Record is added successfully!";
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Utilities.Status.Inactive;
                objectModel.Message = "Insert rate is error!";
                throw;
            }

            //#region copy to payment2
            //var paymentOrderTemp2 = _paymentOrderService2.GetPaymentOrderByOrderId(paymentOrder.OrderId);
            //if (paymentOrderTemp2 == null)
            //{
            //    objectModel.Status = (int)Utilities.Status.Inactive;
            //    objectModel.Message = "Insert rate is error!";
            //    return Json(objectModel);
            //}
            //var paymentDetails2 = _paymentOrderDetailService2.GetPaymentOrderDetails(paymentOrderTemp2.Id);
            //if (paymentDetails2.Count > 0)
            //{
            //    foreach (var paymentDetail in paymentDetails2.Where(paymentDetail => paymentDetail != null))
            //    {
            //        paymentDetail.ObjectState = ObjectState.Deleted;
            //        _paymentOrderDetailService2.Delete(paymentDetail);
            //    }
            //}


            ////decimal outCome = 0;
            ////Added comlunm
            //if (paymentOrderDetails == null) return Json(objectModel);
            //var paymentOrderDetails2 = new List<PaymentOrderDetail2>();
            //foreach (PaymentOrderDetail po in paymentOrderDetails)
            //{
            //    var modelView = new PaymentOrderDetail2();
            //    Mapper.Map(po, modelView, typeof(PaymentOrderDetail), typeof(PaymentOrderDetail2));
            //    paymentOrderDetails2.Add(modelView);
            //}
            //foreach (var rate in paymentOrderDetails2.Where(rate => rate != null))
            //{
            //    rate.ObjectState = ObjectState.Added;
            //    rate.PaymentId = paymentOrderTemp2.Id;
            //    //outCome += rate.Amount;
            //    _paymentOrderDetailService2.Insert(rate);
            //}
            //paymentOrderTemp2.Outcome = paymentOrder.Outcome;
            //paymentOrderTemp2.Income = paymentOrder.Income;
            //paymentOrderTemp2.IssueBy = paymentOrder.IssueBy;
            //paymentOrderTemp2.Profit = paymentOrder.Profit;
            //paymentOrderTemp2.Ratio = paymentOrder.Ratio;
            //paymentOrderTemp2.CreatedBy = paymentOrder.CreatedBy;
            //paymentOrderTemp2.ModifiedDate = DateTime.Now;
            //paymentOrderTemp2.CreatedDate = DateTime.Now;
            //paymentOrderTemp2.OrderDate = DateTime.Now;
            //paymentOrderTemp2.ObjectState = ObjectState.Modified;
            //_paymentOrderService2.Update(paymentOrderTemp2);

            //try
            //{
            //    _unitOfWorkAsync.SaveChanges();
            //    objectModel.Status = (int)Utilities.Status.Active;
            //    objectModel.Message = "Record is added successfully!";
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    objectModel.Status = (int)Utilities.Status.Inactive;
            //    objectModel.Message = "Insert rate is error!";
            //    throw;
            //}

            //#endregion

            objectModel.Status = (int)Utilities.Status.Active;
            return Json(objectModel);
        }

        [HttpPost]
        public ActionResult InsertPo2(List<PaymentOrderDetail2> paymentOrderDetails, PaymentOrder2 paymentOrder)
        {
            var objectModel = new ObjectModel();
            var paymentOrderTemp = _paymentOrderService2.GetPaymentOrderByOrderId(paymentOrder.OrderId);
            if (paymentOrderTemp == null)
            {
                objectModel.Status = (int)Utilities.Status.Inactive;
                objectModel.Message = "Insert rate is error!";
                return Json(objectModel);
            }

            var paymentDetails = _paymentOrderDetailService2.GetPaymentOrderDetails(paymentOrder.OrderId);
            if (paymentDetails.Count > 0)
            {
                foreach (var paymentDetail in paymentDetails.Where(paymentDetail => paymentDetail != null))
                {
                    paymentDetail.ObjectState = ObjectState.Deleted;
                    _paymentOrderDetailService2.Delete(paymentDetail);
                }
            }


            //decimal outCome = 0;
            //Added comlunm
            if (paymentOrderDetails == null) return Json(objectModel);
            foreach (var rate in paymentOrderDetails.Where(rate => rate != null))
            {
                rate.ObjectState = ObjectState.Added;
                rate.PaymentId = paymentOrder.OrderId;
                //outCome += rate.Amount;
                _paymentOrderDetailService2.Insert(rate);
            }
            paymentOrderTemp.Outcome = paymentOrder.Outcome;
            paymentOrderTemp.Income = paymentOrder.Income;
            if (paymentOrderTemp.OrderId == 0)
                paymentOrderTemp.OrderId = paymentOrder.OrderId;
            //paymentOrderTemp.IssueBy = paymentOrder.IssueBy;
            paymentOrderTemp.Profit = paymentOrder.Profit;
            paymentOrderTemp.Ratio = paymentOrder.Ratio;
            paymentOrderTemp.CreatedBy = paymentOrder.CreatedBy;
            paymentOrderTemp.ModifiedDate = DateTime.Now;
            paymentOrderTemp.CreatedDate = paymentOrder.CreatedDate;
            if (paymentOrderTemp.CreatedDate == DateTime.MinValue)
            {
                paymentOrderTemp.CreatedDate = DateTime.Now;
            }
            if (paymentOrderTemp.OrderDate == DateTime.MinValue)
                paymentOrderTemp.OrderDate = DateTime.Now;
            paymentOrderTemp.ObjectState = ObjectState.Modified;

            if (paymentOrderTemp.Id > 0)
                _paymentOrderService2.Update(paymentOrderTemp);
            else
                _paymentOrderService2.Insert(paymentOrderTemp);
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectModel.Status = (int)Utilities.Status.Active;
                objectModel.Message = "Record is added successfully!";
            }
            catch (DbUpdateConcurrencyException ee)
            {
                objectModel.Status = (int)Utilities.Status.Inactive;
                objectModel.Message = "Insert rate is error!";
                throw;
            }
            objectModel.Status = (int)Utilities.Status.Active;
            return Json(objectModel);
        }
        [HttpPost]
        public ActionResult SaveVoucher(Voucher voucherModel)
        {
            var objectResult = new ObjectModel();
            var voucher = _voucherService.GetVoucher(voucherModel.Id);

            //Update voucher
            if (voucher != null)
            {
                voucher.Quantity = voucherModel.Quantity;
                voucher.Type = voucherModel.Type;//(int)Utilities.Product.Tour 
                voucher.Extra = voucherModel.Extra;
                voucher.Adult = voucherModel.Adult;
                voucher.Children = voucherModel.Children;
                voucher.Promotion = voucherModel.Promotion;
                voucher.Description = voucherModel.Description;
                voucher.HotelPhone = voucherModel.HotelPhone;
                voucher.Meal = voucherModel.Meal;
                voucher.CheckIn = voucherModel.CheckIn;
                voucher.CheckOut = voucherModel.CheckOut;
                voucher.Cancellation = voucherModel.Cancellation;
                voucher.Name = voucherModel.Name;
                //voucher.RoomType = voucherModel.RoomType;
                voucher.Localtion = voucherModel.Localtion;
                voucher.ObjectState = ObjectState.Modified;
                objectResult.Message = "Voucher of booking is updated!";
                objectResult.Id = voucher.Id;
                if (voucher.CheckOut == DateTime.MinValue)
                {
                    voucher.CheckOut = voucher.CheckIn;
                }
                _voucherService.Update(voucher);
            }
            else
            {
                if (voucherModel.Type == (int)Utilities.Product.Tour)
                {
                    voucherModel.CheckOut = voucherModel.CheckIn;
                }
                voucherModel.ObjectState = ObjectState.Added;
                objectResult.Message = "Voucher of booking is added!";
                _voucherService.Insert(voucherModel);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                objectResult.Status = (int)Web.Framework.Utilities.Status.Active;
                objectResult.Id = voucherModel.Id;
            }
            catch (Exception ex)
            {
                objectResult.Status = (int)Web.Framework.Utilities.Status.Inactive;
                objectResult.Message = ex.Message;
            }

            return Json(objectResult);
        }

        #endregion

        #region Tour Operator

        public ActionResult TourOperator()
        {
            DateTime fromDate = DateTime.Now.AddMonths(-1);
            DateTime toDate = DateTime.Now.AddMonths(1);
            var orders = _tourOperatorsService.Queryable().ToList();//(fromDate, toDate, false);
            // var orderBys = _orderService.GetOrdersByDate(fromDate, toDate, false);
            ViewBag.ByCreatedDate = false;
            ViewBag.ByCheckInDate = true;
            ViewBag.Orders = orders.OrderBy(a => a.BookedDate).ToList();
            ViewBag.Users = GetRolesToUsers();
            ViewBag.ByCreatedDate = true;
            return View();
        }

        #endregion

        #region Receipt
        [HttpGet]
        public ActionResult PrintPoView(int id)
        {
            var paymentOrder = _paymentOrderService.GetPaymentOrderByOrderId(id);
            var paymentDetails = _paymentOrderDetailService.GetPaymentOrderDetails(paymentOrder.Id);
            var paymentOrderModel = new PaymentOrderModel
            {
                PaymentOrder = paymentOrder,
                PaymentOrderDetails = paymentDetails
            };
            ViewBag.PaymentOrder = paymentOrderModel;

            var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter
            {
                Size = NReco.PdfGenerator.PageSize.A4
            };
            var date = Utilities.GenerateSlug(DateTime.Now.ToString("yyyyMMddhhmm"));
            var fileName = $"Payment-Order-{date}-{id}.pdf";

            var fullPath = Server.MapPath($"~/PdfFiles/{fileName}");
            var server = Request.Url.GetLeftPart(UriPartial.Authority) + "/rc/Booking/PrintPo/";
            htmlToPdf.GeneratePdfFromFile(server + id, null, fullPath);
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            return File(fullPath, "application/pdf");
        }

        [HttpGet]
        public ActionResult PrintPo2View(int id)
        {
            var paymentOrder = _paymentOrderService2.GetPaymentOrderByOrderId(id);
            var paymentDetails = _paymentOrderDetailService2.GetPaymentOrderDetails(paymentOrder.Id);
            var paymentOrderModel = new PaymentOrderModel2
            {
                PaymentOrder = paymentOrder,
                PaymentOrderDetails = paymentDetails
            };
            ViewBag.PaymentOrder = paymentOrderModel;

            var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter
            {
                Size = NReco.PdfGenerator.PageSize.A4
            };
            var date = Utilities.GenerateSlug(DateTime.Now.ToString("yyyyMMddhhmm"));
            var fileName = $"Payment-Order-{date}-{id}.pdf";

            var fullPath = Server.MapPath($"~/PdfFiles/{fileName}");
            var server = Request.Url.GetLeftPart(UriPartial.Authority) + "/rc/Booking/PrintPo2/";
            htmlToPdf.GeneratePdfFromFile(server + id, null, fullPath);
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            return File(fullPath, "application/pdf");
        }

        public ActionResult PrintPo(int? id)
        {

            if (id == null) return View();
            var paymentOrder = _paymentOrderService.GetPaymentOrderByOrderId(id.Value);
            var paymentDetails = _paymentOrderDetailService.GetPaymentOrderDetails(paymentOrder.Id);
            var paymentOrderModel = new PaymentOrderModel
            {
                PaymentOrder = paymentOrder,
                PaymentOrderDetails = paymentDetails
            };

            return View(paymentOrderModel);
        }

        public ActionResult PrintPo2(int? id)
        {

            if (id == null) return View();
            var paymentOrder = _paymentOrderService2.GetPaymentOrderByOrderId(id.Value);
            var paymentDetails = _paymentOrderDetailService2.GetPaymentOrderDetails(paymentOrder.OrderId);
            var paymentOrderModel = new PaymentOrderModel2
            {
                PaymentOrder = paymentOrder,
                PaymentOrderDetails = paymentDetails
            };

            return View(paymentOrderModel);
        }

        [HttpGet]
        public ActionResult PrintVoucherView(int id)
        {
            var voucher = ViewBag.Voucher = _voucherService.GetVoucher(id);
            var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter
            {
                Size = NReco.PdfGenerator.PageSize.A4
            };
            var date = Utilities.GenerateSlug(DateTime.Now.ToString("yyyyMMddhhmm"));
            var fileName = $"voucher-{date}-{id}.pdf";
            var fullPath = Server.MapPath($"~/PdfFiles/{fileName}");
            var server = Request.Url.GetLeftPart(UriPartial.Authority) + "/rc/Booking/PrintVoucher/";
            //var server = Server.MapPath($"~/Booking/PrintVoucher/");
            htmlToPdf.GeneratePdfFromFile(server + id, null, fullPath);
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            return File(fullPath, "application/pdf");
        }

        public ActionResult PrintReceiptView1(int? id)
        {
            var objectResult = new ObjectModel();
            objectResult.Id = id.Value;
            return Json(objectResult);
        }

        [HttpGet]
        public ActionResult PrintReceiptView(Voucher voucherModel)
        {
            OrderModel order = _orderService.GetOrder(voucherModel.Id);
            var exchanges = _rateExchangeService.GetRateExchangeById(3);
            ViewBag.Exchange = exchanges;
            if (order != null)
            {
                if (!order.RateExchange.HasValue)
                {
                    order.RateExchange = exchanges.CurrentPrice;
                }

            }
            ViewBag.Order = order;


            var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter
            {
                Size = NReco.PdfGenerator.PageSize.A4
            };
            var guest = order.CustomerId;
            var date = Utilities.GenerateSlug(DateTime.Now.ToString("yyyyMMddhhmm"));
            var fileName = $"receipt-{date}{voucherModel.Id}{guest}.pdf";
            var fullPath = Server.MapPath($"~/PdfFiles/{fileName}");
            //var server = Server.MapPath($"~/Booking/PrintReceipt/");
            var server = Request.Url.GetLeftPart(UriPartial.Authority) + "/rc/Booking/PrintReceipt/";
            htmlToPdf.GeneratePdfFromFile(server + voucherModel.Id, null, fullPath);
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            return File(fullPath, "application/pdf");
        }
        [HttpGet]
        public ActionResult PrintReceiptViewVietnam(Voucher voucherModel)
        {
            OrderModel order = _orderService.GetOrder(voucherModel.Id);
            var exchanges = _rateExchangeService.GetRateExchangeById(3);
            ViewBag.Exchange = exchanges;
            if (order != null)
            {
                if (!order.RateExchange.HasValue)
                {
                    order.RateExchange = exchanges.CurrentPrice;
                }

            }
            ViewBag.Order = order;
            var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
            var guest = order.CustomerId;
            var date = Utilities.GenerateSlug(DateTime.Now.ToString("yyyyMMddhhmm"));
            var fileName = $"receipt-{date}{voucherModel.Id}{guest}.pdf";
            var fullPath = Server.MapPath($"~/PdfFiles/{fileName}");
            //var server = Server.MapPath($"~/Booking/PrintReceiptVietnam/");
            var currencies = _rateExchangeService.GetRateExchanges().FirstOrDefault(a => a.Name == "VND");
            ViewBag.VND = currencies;
            var server = Request.Url.GetLeftPart(UriPartial.Authority) + "/rc/Booking/PrintReceiptVietnam/";
            htmlToPdf.GeneratePdfFromFile(server + voucherModel.Id, null, fullPath);
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            return File(fullPath, "application/pdf");
        }

        [HttpGet]
        public ActionResult PrintReceipt2ViewVietnam(Voucher voucherModel)
        {
            //var objectResult = new ObjectModel();


            OrderModel order = _orderService2.GetOrder2(voucherModel.Id);

            ViewBag.Order = order;// _orderService2.GetOrder2(voucherModel.Id);

            var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
            var guest = order.CustomerId;
            var date = Utilities.GenerateSlug(DateTime.Now.ToString("yyyyMMddhhmm"));
            var fileName = $"receipt-{date}{voucherModel.Id}{guest}.pdf";
            var fullPath = Server.MapPath($"~/PdfFiles/{fileName}");
            //var server = Server.MapPath($"~/Booking/PrintReceiptVietnam/");
            var currencies = _rateExchangeService.GetRateExchanges().FirstOrDefault(a => a.Name == "VND");
            ViewBag.VND = currencies;
            var server = Request.Url.GetLeftPart(UriPartial.Authority) + "/rc/Booking/PrintReceiptVietnam2/";
            htmlToPdf.GeneratePdfFromFile(server + voucherModel.Id, null, fullPath);
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            return File(fullPath, "application/pdf");
        }
        [HttpGet]
        public ActionResult PrintPdf(int id)
        {
            var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
            htmlToPdf.GeneratePdfFromFile("/Booking/TestPrint/21", null, "PdfFiles/export.pdf");
            return null;
        }

        public ActionResult PrintVoucher(int? id)
        {
            if (id == null) return View();
            var voucher = _voucherService.GetVoucher(id.Value);
            var order = _orderService.GetOrderById(voucher.BookingId);
            var hotel = voucher.Type == 1 && order.ProductId.HasValue
                ? _hotelService.GetHotelSingleByRoomId(order.ProductId.Value)
                : null;
            if (hotel != null)
            {
                var images =
             _imageService.GetImageListByHotelId(hotel.Id)
                 .Where(i => i.PictureType == Constant.ImageMaptype)
                 .ToList();
                ViewBag.HotelImage = images.FirstOrDefault();
            }

            if (order.Type == 2)
            {
                ViewBag.TourImage = _mediaService.Queryable().FirstOrDefault(t => t.OwnerId == order.ProductId && t.MediaType == 4);
            }

            ViewBag.Pnr = order != null ? order.Pnr : "N/A";
            return View(voucher);
        }

        public ActionResult PrintReceiptVietnam(int? id)
        {
            if (id != null)
            {
                OrderModel order = _orderService.GetOrderVietnam(id.Value);
                var exchanges = _rateExchangeService.GetRateExchangeById(3);
                ViewBag.Exchange = exchanges;
                if (order != null)
                {
                    if (!order.RateExchange.HasValue)
                    {
                        order.RateExchange = exchanges.CurrentPrice;
                    }
                }
                ViewBag.Order = order;
                // OrderModel order = ViewBag.Order = _orderService.GetOrderVietnam(id.Value);

                var context = new SecurityContext();
                var currentUser = context.Users.FirstOrDefault(x => x.UserName.Contains(order.Management));
                ViewBag.FullName = currentUser != null ? $"{currentUser.FirstName} {currentUser.LastName}" : "N/A";

                return View(order);
            }
            return View();
        }

        public ActionResult PrintReceiptVietnam2(int? id)
        {
            if (id != null)
            {
                OrderModel order = ViewBag.Order = _orderService2.GetOrderVietnam(id.Value);

                var context = new SecurityContext();
                var currentUser = context.Users.FirstOrDefault(x => x.UserName.Contains(order.Management));
                ViewBag.FullName = currentUser != null ? $"{currentUser.FirstName} {currentUser.LastName}" : "N/A";

                return View(order);
            }
            return View();
        }
        public ActionResult PrintReceipt(int? id)
        {
            if (id != null)
            {
                OrderModel order = ViewBag.Order = _orderService.GetOrder(id.Value);
                var context = new SecurityContext();
                var currentUser = context.Users.FirstOrDefault(x => x.UserName.Contains(order.Management));
                ViewBag.FullName = currentUser != null ? $"{currentUser.FirstName} {currentUser.LastName}" : "N/A";
                return View(order);
            }
            return View();
        }
        #endregion

        #region other
        public JsonResult ShowCardConfirm(ObjectModel objectModel)
        {
            var result = new AccountController().UserValid(objectModel.StrParam1);
            if (!result)
            {
                objectModel.Message = "Cannot show this code !.";
                objectModel.Status = (int)Utilities.Status.Inactive;
            }
            else
            {
                var card = _creditCardService.GetCreditCard(objectModel.Id);
                if (card != null)
                {
                    var message = string.Format("<div class='row stats-info'>" +
                                                "<div style='padding: 0px 50px;'>" +
                                                "<ul class='list-unstyled'>" +
                                                "<li style='text-align: left;'>First Name: <div class='pull-right'><b>" + card.FirstName +
                                                "</b></div></li>" +
                                                "<li style='text-align: left;'>Last Name: <div class='pull-right'><b>" + card.LastName +
                                                "</b></div></li>" +
                                                "<li style='text-align: left;'>Credit Number: <div class='pull-right'><b>" + card.CardNumber +
                                                "</b></div></li>" +
                                                "<li style='text-align: left;'>CVV: <div class='pull-right'><b>" + card.Cvv + "</b></div></li>" +
                                                "<li style='text-align: left;'>Exp Month: <div class='pull-right'><b>" + card.ExpMonth +
                                                "</b></div></li>" +
                                                "<li style='text-align: left;'>Exp Year: <div class='pull-right'><b>" + card.ExpYear +
                                                "</b></div></li>" +
                                                "<li style='text-align: left;'>Cardholder Address: <div class='pull-right'><b>" + card.Address +
                                                "</b></div></li>" +
                                                "</ul>" +
                                                "</div>" +
                                                "</div>");
                    objectModel.Message = message;
                    objectModel.Status = (int)Utilities.Status.Active;
                }
                else
                {
                    objectModel.Message = "This card number invalid";
                    objectModel.Status = (int)Utilities.Status.Inactive;
                }
            }
            var jsonResult = Json(objectModel, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public ActionResult EditSurchargeExtraBed(ObjectModel objectModel)
        {
            var order = _orderService.GetOrderById(objectModel.Id);
            if (order == null) return Json(objectModel);
            var orderDetail = new OrderDetail
            {
                UserId = GetCurrentUserName(),
                CreatedDate = DateTime.Now,
                Note = objectModel.StrParam1,
                ChangedName = "Add extra option booking",
                ChangedValue = "ExtraOption",
                OrderId = objectModel.Id,
                IsSend = false,
                ObjectState = ObjectState.Added
            };
            _orderDetailService.Insert(orderDetail);
            order.IsRefund = true;
            order.ExtraBed = objectModel.DecParam1;
            order.ThirdPersonFee = objectModel.DecParam2;
            order.Note = objectModel.StrParam1;

            order.ModifiedDate = DateTime.Now;
            order.ObjectState = ObjectState.Modified;
            _orderService.Update(order);
            try
            {

                objectModel.Status = (int)Utilities.Status.Active;
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                objectModel.Status = (int)Utilities.Status.Inactive;
                objectModel.Message = "Update order is error!";
                throw;
            }
            return Json(objectModel);
        }

        [HttpPost]
        public ActionResult GetOrdersForAccountant(ObjectModel objectModel)
        {
            var startDate = Utilities.ConvertStringToDate(objectModel.StrParam1);
            var endDate = Utilities.ConvertStringToDate(objectModel.StrParam2);
            var orders = _orderService2.GetOrdersByDate(startDate, endDate);

            return PartialView("_OrdersListForAccountant", orders);
        }
        [HttpPost]

        public ActionResult GetOrdersForUser(ObjectModel objectModel)
        {
            var userName = GetCurrentUserName();
            var startDate = Utilities.ConvertStringToDate(objectModel.StrParam1);
            var endDate = Utilities.ConvertStringToDate(objectModel.StrParam2);
            var orders = _orderService.GetOrdersByUserNameDate(userName, startDate, endDate);
            return PartialView("_OrdersListForUser", orders);
        }

        #endregion

        #region Export Excel

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }



        public void ExportToExcel(string start, string end)
        {
            var startDate = Utilities.ConvertStringToDate(start);
            var endDate = Utilities.ConvertStringToDate(end);
            var dataOrder = _orderService.GetOrdersByDate(startDate, endDate, true);
            DataTable dt = ToDataTable(dataOrder);
            ExportExcel(dt);
        }
        public void ExportToExcelForUser(string start, string end)
        {
            var userName = GetCurrentUserName();
            var startDate = Utilities.ConvertStringToDate(start);
            var endDate = Utilities.ConvertStringToDate(end);
            var dataOrder = _orderService.GetOrdersByUserNameDate(userName, startDate, endDate);
            DataTable dt = ToDataTable(dataOrder);
            ExportExcel(dt);
        }
        protected void ExportExcel(DataTable table)
        {
            MemoryStream stream = AbstractReader.Copy(
                $"{Request.PhysicalApplicationPath}\\ExcelFiles\\receipt_report.xlsx");
            SpreadsheetDocument doc = SpreadsheetDocument.Open(stream, true);
            SpreadsheetReader.GetDefaultStyle(doc);
            WorksheetPart ws1 = SpreadsheetReader.GetWorksheetPartByName(doc, "Sheet1");
            WorksheetWriter writer1 = new WorksheetWriter(doc, ws1);
            //Save to the memory stream
            foreach (DataRow row in table.Rows)
            {
                int index = table.Rows.IndexOf(row);
                WriteDataToEachCell(writer1, index, row);
            }
            SpreadsheetWriter.Save(doc);
            //Write to response stream
            Response.Clear();
            Response.AddHeader("content-disposition", $"attachment;filename={"ExportReceipt.xlsx"}");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            stream.WriteTo(Response.OutputStream);
            stream.Close();
            //  doc.Close();
            Response.End();
        }

        private static void WriteDataToEachCell(WorksheetWriter writer, int index, DataRow row)
        {
            int statusInt;
            int.TryParse(row["Status"].ToString(), out statusInt);

            string position = (index + 2).ToString();
            writer.PasteText("A" + position, row["Id"].ToString());
            writer.PasteText("B" + position, row["Pnr"].ToString());
            writer.PasteText("C" + position, row["FullName"].ToString());
            writer.PasteText("D" + position, row["ProductName"].ToString());
            writer.PasteText("E" + position, row["CreatedDate"].ToString());
            writer.PasteText("F" + position, row["StartDate"].ToString());
            writer.PasteText("G" + position, row["EndDate"].ToString());
            writer.PasteText("H" + position, row["Price"].ToString());
            writer.PasteText("I" + position, row["Quantity"].ToString());
            writer.PasteText("J" + position, row["Total"].ToString());
            writer.PasteText("K" + position, row["Deposit"].ToString());
            writer.PasteText("L" + position, row["Discount"].ToString());
            writer.PasteText("M" + position, row["Balance"].ToString());
            writer.PasteText("N" + position, row["RateExchange"].ToString());
            writer.PasteText("O" + position, row["InCome"].ToString());
            writer.PasteText("P" + position, row["OutCome"].ToString());
            writer.PasteText("Q" + position, row["Management"].ToString());
            writer.PasteText("R" + position, Enum.GetName(typeof(Utilities.BookingStatus), statusInt));
        }

        #endregion
    }

}
