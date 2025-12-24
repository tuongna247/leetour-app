using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Repository.Pattern.Infrastructure;
using Vinaday.Data.Models;
using Vinaday.Web.Framework;
using Vinaday.Data.Models.Extention;
using AutoMapper;

namespace Vinaday.Admin.Controllers
{
    public partial class BookingController
    {
        /// <summary>
        /// Create other product booking 
        /// 
        /// </summary>
        /// <param name="objectModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreatePopUpOtherProduct(ObjectModel objectModel)
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
                customer.NationalId = objectModel.IntParam5;
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
                existCustomer.NationalId = objectModel.IntParam5;
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
            var product = _bookingService.GetOtherServices(1, objectModel.Id, objectModel.StrParam1, objectModel.StrParam2, objectModel.IntParam1, objectModel.DecParam1, objectModel.DecParam2, objectModel.DecParam3, objectModel.StrParam8, objectModel.StrParam7, objectModel.StrParam9);
            product.GuestFirstName = objectModel.StrParam10;
            product.GuestLastName = objectModel.StrParam11;
            product.GuestNational = objectModel.StrParam12;
            ViewBag.Customer = customer;
            ViewBag.Nationality = _nationalityService.GetNationality(customer.NationalId ?? 0);
            return PartialView("_OtherServicesBooking", product);
        }

        [HttpPost]
        public JsonResult CreateTourOrder2(ObjectModel objectModel)
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
            var guestNational = _nationalityService.GetNationality(objectModel.IntParam2); ;
            //Create order.
            var userId = GetCurrentUserName();

            var order = new Order2
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
                                            rate.PersonNo).ToString()), 2),
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
            _orderService2.Add(order);
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Json(new { success = false, error = "Error when processing booking" }, JsonRequestBehavior.AllowGet);
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
                bookingLink = string.Format("/rc/Booking//OrderDetail/{0}", order.Id),
                pnr
            };

            var jsonResult = Json(resutls, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult CreateOtherProduct(ObjectModel objectModel)
        {
            var email = objectModel.StrParam3;//Pick email value
            var productId = objectModel.Id;//Pick product id value (hotel or tour)
            var checkIn = objectModel.StrParam1;//Pick check in or depart date
            var checkOut = objectModel.StrParam2;//Pick check out date
            var total = objectModel.IntParam3;//Pick total product fr booking
            var status = objectModel.Status;//Pick status (paid or Hoding)
            var specialRequest = objectModel.Message;//rendering for special request
            const int promotionId = 0;//Create promotion code
            var pnr = Utilities.GetRandomString(7).ToUpper();//Random PNR order
            //Get customer by email address.
            var customer = _customerService.GetCustomerByEmail(email);
            //Get product by product id.
            var product = _bookingService.GetOtherServices(1, objectModel.Id, objectModel.StrParam1, objectModel.StrParam2, objectModel.IntParam1, objectModel.DecParam1, objectModel.DecParam2, objectModel.DecParam3, objectModel.StrParam8, objectModel.StrParam7, objectModel.StrParam9);
            var rateExchange = _rateExchangeService.GetRateExchangeById(3);
            var userId = GetCurrentUserName();
            //Create order.
            var order = new Order
            {
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Status = status,
                Quantity = product.Quantity,
                CustomerId = customer.CustomerId,
                Pnr = pnr,
                TaxFee = product.TotalTaxeFee,
                Price = objectModel.DecParam1,
                Management = userId,
                ProductId = product.Id,
                Amount = objectModel.DecParam2,
                GuestFirstName = objectModel.StrParam10,
                GuestLastName = objectModel.StrParam11,
                GuestCountry = objectModel.StrParam12,
                Type = (int)Utilities.Product.OtherServices,
                StartDate = product.CheckIn,
                EndDate = product.CheckOut,
                Night = product.Night,
                SurchargeFee = product.TotalSurcharge,
                IpLocation = Utilities.GetUserIp(Request),
                SpecialRequest = specialRequest,
                Discount = objectModel.DecParam3,
                CancellationPolicy = objectModel.StrParam8,//product.CancellationPolicy != null ? product.CancellationPolicy.Description : string.Empty,
                ProductName = objectModel.StrParam7,
                DiscountName = product.Promotion != null ?
                    $"{product.Promotion.Name}<br/>{product.Promotion.Description}"
                    : "Special Discount",
                ObjectState = ObjectState.Added,
                Note = objectModel.StrParam9,
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
                Status = status,
                Quantity = product.Quantity,
                CustomerId = customer.CustomerId,
                Pnr = pnr,
                TaxFee = product.TotalTaxeFee,
                Price = objectModel.DecParam1,
                Management = userId,
                ProductId = product.Id,
                Amount = objectModel.DecParam2,
                Type = (int)Utilities.Product.OtherServices,
                StartDate = product.CheckIn,
                EndDate = product.CheckOut,
                Night = product.Night,
                SurchargeFee = product.TotalSurcharge,
                IpLocation = Utilities.GetUserIp(Request),
                SpecialRequest = specialRequest,
                Discount = objectModel.DecParam3,
                CancellationPolicy = objectModel.StrParam8,//product.CancellationPolicy != null ? product.CancellationPolicy.Description : string.Empty,
                ProductName = objectModel.StrParam7,
                DiscountName = product.Promotion != null ?
                    $"{product.Promotion.Name}<br/>{product.Promotion.Description}"
                    : "Special Discount",
                ObjectState = ObjectState.Added,
                Note = objectModel.StrParam9,
                RateExchange = rateExchange != null ? rateExchange.CurrentPrice : 0,
                TourOrderId = order.Id

            };
            //Insert hotel order
            _orderService2.Add(order2);
            if (status == (int)Utilities.BookingStatus.Paid)
            {
                //Add change for OrderDetail table
                var orderDetail = new OrderDetail
                {
                    UserId = GetCurrentUserName(),
                    CreatedDate = DateTime.Now,
                    Note = "Booking is Approved",
                    ChangedName = "Approve booking",
                    ChangedValue = "Approved",
                    OrderId = order.Id,

                    ObjectState = ObjectState.Added
                };
                _orderDetailService.Insert(orderDetail);

                order.ModifiedDate = DateTime.Now;
                order.PaymentMethod = (int)Utilities.PaymentMethod.Cash;

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
                bookingLink = $"/rc/Booking/OrderDetail/{order.Id}",
                pnr
            };

            var jsonResult = Json(resutls, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #region Get JSON
        public JsonResult GetOrderJsonData()
        {
            var orders = _orderService.GetOrders(); //.ToList().Take(5000);
            var list = new List<OrderModelView>();
            foreach (OrderModel order in orders)
            {
                var modelView = new OrderModelView();
                Mapper.Map(order, modelView, typeof(OrderModel), typeof(OrderModelView));
                list.Add(modelView);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}