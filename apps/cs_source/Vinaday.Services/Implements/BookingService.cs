using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;
using Vinaday.Data.Models.Extention;

namespace Vinaday.Services
{
    public class BookingService : Service<Booking>, IBookingService, IService<Booking>
    {
        private readonly IRepositoryAsync<Booking> _bookingRepository;

        private readonly IRepositoryAsync<Hotel> _hotelRepository;
        private readonly IRepositoryAsync<HotelPackage> _hotelPackageRepository;
        private readonly IRepositoryAsync<HotelPackage_Surcharge> _hotelPackageSurchargeRepository;

        private readonly IRepositoryAsync<HotelImages> _imageRepository;

        private readonly IRepositoryAsync<RoomControl> _roomControlRepository;

        private readonly IRepositoryAsync<Room> _roomRepository;

        private readonly IRepositoryAsync<HotelCancellation> _hotelCancellationRepository;

        private readonly IRepositoryAsync<CancellationPolicy> _cancellationPolicyRepository;

        private readonly IRepositoryAsync<Promotion> _promotionRepository;

        private readonly IRateExchangeService _exchangeService;
        private readonly HotelCouponService _hotelCouponService;

        public BookingService(IRepositoryAsync<Booking> bookingRepository, IRepositoryAsync<RoomControl> roomControlRepository, IRepositoryAsync<Room> roomRepository, IRepositoryAsync<Hotel> hotelRepository, IRepositoryAsync<HotelImages> imageRepository, IRepositoryAsync<HotelCancellation> hotelCancellationRepository, IRepositoryAsync<CancellationPolicy> cancellationPolicyRepository, IRepositoryAsync<Promotion> promotionRepository, IRateExchangeService exchangeService, IRepositoryAsync<HotelPackage> hotelPackageRepository, IRepositoryAsync<HotelPackage_Surcharge> hotelPackageSurchargeRepository, HotelCouponService hotelCouponService) : base(bookingRepository)
        {
            _hotelPackageRepository = hotelPackageRepository;
            _bookingRepository = bookingRepository;
            _roomControlRepository = roomControlRepository;
            _roomRepository = roomRepository;
            _hotelRepository = hotelRepository;
            _imageRepository = imageRepository;
            _hotelCancellationRepository = hotelCancellationRepository;
            _cancellationPolicyRepository = cancellationPolicyRepository;
            _promotionRepository = promotionRepository;
            _exchangeService = exchangeService;
            _hotelCouponService = hotelCouponService;

            _hotelPackageSurchargeRepository = hotelPackageSurchargeRepository;
        }

        public string GeneratePnrNumber()
        {
            int num = -1;
            string empty = string.Empty;
            while (num == -1)
            {
                empty = Utilities.GetRandomString(7);
                if (_bookingRepository.GetBookingsByPnr(empty).ToList<Booking>().Count <= 0)
                {
                    num = 0;
                }
            }
            return empty;
        }

        public List<Booking> GetGetBookings()
        {
            List<Booking> list = (
                from p in _bookingRepository.GetBookings()
                orderby p.Date descending
                select p into g
                group g by g.RoomId into p
                select p.FirstOrDefault<Booking>()).Take<Booking>(15).ToList<Booking>();
            return list;
        }


        public ProductModel GetHotelPackage(int id, string strCheckIn, int totalRoom)
        {
            var checkIn = Utilities.ConvertToDateTime(strCheckIn);
            var details = new List<ProductDetailModel>();
            var hotelPackage = _hotelPackageRepository.Queryable().FirstOrDefault(a=>a.Id == id);
            var hotel = _hotelRepository.Queryable().FirstOrDefault(a=>a.Id == hotelPackage.HotelId);
            if (hotelPackage == null)
            {
                return new ProductModel();
            }

            var nights = hotelPackage.Night;
            var checkOut = checkIn.Date.AddDays(nights);
            var stay =
                $"{checkIn.ToString("MMM dd, yyyy")} - {checkOut.ToString("MMM dd, yyyy")} | {nights} {(nights >= 2 ? "nights" : "night")} ";

            var cancelationPolicy = new CancellationPolicy();
            if (hotelPackage.CancellationId > 0)
            {
                cancelationPolicy = _cancellationPolicyRepository.GetCancellationPolicyById(hotelPackage.CancellationId);
            }

            var image = _imageRepository.GetImageSingleByHotelId(hotelPackage.HotelId );
            string imageUlr = image != null
                ? !string.IsNullOrEmpty(image.ImageThumbnail)
                    ? "https://admin.goreise.com" + $"{image.ImageThumbnail.Substring(1)}-original.jpg"
                    : "/Content/images/demo/general/no-image.jpg"
                : "/Content/images/demo/general/no-image.jpg";
            var detail = new ProductDetailModel
            {
                Date = checkIn,
                Night = nights.ToString(),
                Quantity = totalRoom,
                PriceRoom = hotelPackage.Price ,
                PriceSurcharge = 0,
            };
            details.Add(detail);
            var promotion = new PromotionModel
            {
                CancelationId = hotelPackage.CancellationId,
                Name = "Special Sale! Discount " + hotelPackage.DiscountValue + '%'
            };
            var surchargedbListTemp =
                _hotelPackageSurchargeRepository.Queryable()
                    .Where(a => a.Package_Id == hotelPackage.Id && a.FromDate <= checkIn && a.ToDate >= checkIn).ToList();
            var surchargeList = new List<HotelPackage_Surcharge>();
            List<DateTime> dateList = new List<DateTime> ();
            var startdate = checkIn;
            while (startdate<checkOut)
            {
                
                dateList.Add(startdate);
                startdate = startdate.AddDays(1);
            }
            


            foreach (var hotelPackageSurcharge in surchargedbListTemp)
            {
                List<DateTime> dateList_2 = new List<DateTime>();
                var startdate2 = hotelPackageSurcharge.FromDate;
                while (startdate2 < hotelPackageSurcharge.ToDate)
                {
                    dateList_2.Add(startdate2);
                    startdate2 = startdate2.AddDays(1);
                }
                foreach (var dateItem in dateList_2)
                {
                    bool isValid;
                    var str = dateItem.ToString("ddd");
                    isValid = hotelPackageSurcharge.DateOfWeek.Split(',').Contains(str);
                    if (isValid)
                    {
                        surchargeList.Add(hotelPackageSurcharge);
                    }
                }
               
            }
            var surcharge = surchargeList.Sum(item => item.Price);
            var price = hotelPackage.PriceFake.HasValue ? hotelPackage.PriceFake.Value : hotelPackage.Price;
            var model = new ProductModel
            {
                Id = hotelPackage.Id,
                ProductId = hotelPackage.Id,
                CheckIn = checkIn,
                CheckOut = checkOut,
                Stay = stay,
                Name = hotel!=null ? hotel.Name:"",
                Rating = hotel != null ? (hotel.StartRating ?? 0):0,
                Location = hotel != null ? hotel.StreetAddressEnglish:"",
                ImageUrl = imageUlr,
                Include =hotelPackage.IncludingValue,
                FinalPrice = (hotelPackage.Price * totalRoom)+ surcharge ,
                DetailName = hotelPackage.RoomName,
                ProductUrl =
                    $"/{hotel.Country.ToLower()}/{hotelPackage.Id}/{Utilities.GenerateSlug(hotelPackage.RoomName)}",
                TotalPrice = (price * totalRoom) ,
                CancellationPolicy = cancelationPolicy,
                TotalSurcharge = surcharge,
                TotalTaxeFee = 0,
                Details = details,
                Promotion = hotelPackage.IsPromotion.HasValue && hotelPackage.IsPromotion.Value? promotion:null,
              
                Night = nights,
                Quantity = totalRoom
            };
            if (hotelPackage.PriceFake.HasValue)
            {
                model.TotalSave = (decimal) (hotelPackage.PriceFake - hotelPackage.Price);
            }
            
            return model;
        }

        public ProductModel GetHotelPackageVN(int id, string strCheckIn, int totalRoom)
        {
            var exchange = _exchangeService.GetRateExchangeById(3);
            var checkIn = Utilities.ConvertToDateTime(strCheckIn);
            var details = new List<ProductDetailModel>();
            var hotelPackage = _hotelPackageRepository.Queryable().FirstOrDefault(a => a.Id == id);
            var hotel = _hotelRepository.Queryable().FirstOrDefault(a => a.Id == hotelPackage.HotelId);
            if (hotelPackage == null)
            {
                return new ProductModel();
            }

            var nights = hotelPackage.Night;
            var checkOut = checkIn.Date.AddDays(nights);
            var stay =
                $"{checkIn.ToString("MMM dd, yyyy")} - {checkOut.ToString("MMM dd, yyyy")} | {nights} {(nights >= 2 ? "nights" : "night")} ";

            var cancelationPolicy = new CancellationPolicy();
            if (hotelPackage.CancellationId > 0)
            {
                cancelationPolicy = _cancellationPolicyRepository.GetCancellationPolicyById(hotelPackage.CancellationId);
            }

            var image = _imageRepository.GetImageSingleByHotelId(hotelPackage.HotelId);
            string imageUlr = image != null
                ? !string.IsNullOrEmpty(image.ImageThumbnail)
                    ? "https://admin.goreise.com" + $"{image.ImageThumbnail.Substring(1)}-original.jpg"
                    : "/Content/images/demo/general/no-image.jpg"
                : "/Content/images/demo/general/no-image.jpg";
            var detail = new ProductDetailModel
            {
                Date = checkIn,
                Night = nights.ToString(),
                Quantity = totalRoom,
                PriceRoom = hotelPackage.Price * exchange.CurrentPrice ?? 1,
                PriceSurcharge = 0,
            };
            details.Add(detail);
            var promotion = new PromotionModel
            {
                CancelationId = hotelPackage.CancellationId,
                Name = "Special Sale! Discount " + hotelPackage.DiscountValue + '%'
            };
            var surchargedbListTemp =
                _hotelPackageSurchargeRepository.Queryable()
                    .Where(a => a.Package_Id == hotelPackage.Id && a.FromDate <= checkIn && a.ToDate >= checkIn).ToList();
            var surchargeList = new List<HotelPackage_Surcharge>();
            List<DateTime> dateList = new List<DateTime>();
            var startdate = checkIn;
            while (startdate < checkOut)
            {

                dateList.Add(startdate);
                startdate = startdate.AddDays(1);
            }



            foreach (var hotelPackageSurcharge in surchargedbListTemp)
            {
                List<DateTime> dateList2 = new List<DateTime>();
                var startdate2 = hotelPackageSurcharge.FromDate;
                while (startdate2 < hotelPackageSurcharge.ToDate)
                {

                    dateList2.Add(startdate);
                    startdate2 = startdate2.AddDays(1);
                }
                foreach (var dateItem in dateList2)
                {
                    bool isValid;
                    var str = dateItem.ToString("ddd");
                    isValid = hotelPackageSurcharge.DateOfWeek.Split(',').Contains(str);
                    if (isValid)
                    {
                        surchargeList.Add(hotelPackageSurcharge);
                    }
                }

            }
            var surcharge = surchargeList.Sum(item => item.Price);
            var price = hotelPackage.PriceFake.HasValue ? hotelPackage.PriceFake.Value : hotelPackage.Price;
            var model = new ProductModel
            {
                Id = hotelPackage.Id,
                ProductId = hotelPackage.Id,
                CheckIn = checkIn,
                CheckOut = checkOut,
                Stay = stay,
                Name = hotel != null ? hotel.Name : "",
                Rating = hotel != null ? (hotel.StartRating ?? 0) : 0,
                Location = hotel != null ? hotel.StreetAddressEnglish : "",
                ImageUrl = imageUlr,
                Include = hotelPackage.IncludingValueVN,
                FinalPrice =((hotelPackage.Price * totalRoom) + surcharge) * exchange.CurrentPrice ?? 1,
                DetailName = hotelPackage.RoomName,
                ProductUrl =
                    $"/{hotel.Country.ToLower()}/{hotelPackage.Id}/{Utilities.GenerateSlug(hotelPackage.RoomName)}",
                TotalPrice = (price * totalRoom) * exchange.CurrentPrice ?? 1,
                CancellationPolicy = cancelationPolicy,
                TotalSurcharge = (surcharge) * exchange.CurrentPrice ?? 1,
                TotalTaxeFee = 0,
                Details = details,
                Promotion = hotelPackage.IsPromotion.HasValue && hotelPackage.IsPromotion.Value ? promotion : null,

                Night = nights,
                Quantity = totalRoom
            };
            if (hotelPackage.PriceFake.HasValue)
            {
                model.TotalSave = (decimal)(hotelPackage.PriceFake - hotelPackage.Price) * exchange.CurrentPrice ?? 1;
            }

            return model;
        }

        public ProductModel GetHotelProduct(int type, int id, string strCheckIn, string strCheckOut, int total, int promotionId)
        {
            var checkIn = Utilities.ConvertToDateTime(strCheckIn);
            var checkOut = Utilities.ConvertToDateTime(strCheckOut);
            var details = new List<ProductDetailModel>();
            var surcharges = new List<ProductDetailModel>();
            var days = (checkOut - checkIn).Days;
            var stay = String.Format("{0} - {1} | {2} {3} ", (object)checkIn.ToString("MMM dd, yyyy"),
               (object)checkOut.ToString("MMM dd, yyyy"), (object)days,
               days >= 2 ? (object)"nights" : (object)"night");
            var room = _roomRepository.GetRoom(id);
            if (room == null)
            {
                return new ProductModel();
            }
            var hotel = _hotelRepository.GetHotelSingle(room.HotelId ?? 0);
            if (hotel == null)
            {
                return new ProductModel();
            }
            var cancelationPolicy = new CancellationPolicy();
            //var image = _imageRepository.GetImageSingleByHotelId(room.HotelId ?? 0);
            string imageUlr = room.ImageUrl;
            //var totalPrice = _roomControlRepository.GetTotalPrice(room.Id, checkIn, checkOut);
            var cancelationHotel = _hotelCancellationRepository.GetHotelCancellationPolicyById(room.HotelId ?? 0, (int)Utilities.Status.Active);
            if (cancelationHotel != null)
            {
                cancelationPolicy = _cancellationPolicyRepository.GetCancellationPolicyById(cancelationHotel.CancellationID);
                //if (cancelationPolicy != null)
                //{
                //    cancelation = cancelationPolicy.Description;
                //}
            }
            var roomControls = _roomControlRepository.GetRoomListCheckInOut(room.Id, checkIn, checkOut);
            if (roomControls == null)
            {
                return new ProductModel();
            }
            var promotion = GetPromotionModelById(promotionId, roomControls);
            var totalSave = promotion != null ? promotion.Price : 0;
            var night = 1;
            var surchargePrice = ((roomControls.Sum(r => r.Surcharge1) +
                                    roomControls.Sum(r => r.Surcharge2) +
                                    roomControls.Sum(r => r.CompulsoryMeal)) * total);
            var taxFeePrice = ((roomControls.Sum(r => r.SellingRate) - roomControls.Sum(r => r.SellingRate) / 1.15m) * total);
            var sellingRatePrice = (roomControls.Sum(r => r.SellingRate) / 1.15m * total);
            var totalPrice = (Math.Round(surchargePrice ?? 0, 2) + Math.Round(sellingRatePrice ?? 0, 2) + Math.Round(taxFeePrice ?? 0, 2) - Math.Round(totalSave, 2));

            foreach (var roomControl in roomControls)
            {
                if (roomControl == null)
                {
                    continue;
                }
                var detail = new ProductDetailModel
                {
                    Date = roomControl.RoomDate,
                    Night = night.ToString(CultureInfo.InvariantCulture),
                    Quantity = total,
                    PriceRoom = Math.Round((roomControl.SellingRate / 1.15m ?? 0), 2),
                    PriceSurcharge = Math.Round((roomControl.Surcharge1 + roomControl.Surcharge2 + roomControl.CompulsoryMeal) ?? 0, 2),
                };
                details.Add(detail);
                night++;
            }

            var model = new ProductModel
            {
                Id = room.Id,
                ProductId = hotel.Id,
                CheckIn = checkIn,
                CheckOut = checkOut,
                Stay = stay,
                Name = hotel.Name,
                Rating = hotel.StartRating ?? 0,
                Location = hotel.StreetAddressEnglish,
                ImageUrl = imageUlr != null ? "http://admin.goreise.com" + imageUlr : "/Content/images/demo/general/no-image.jpg",

                FinalPrice = totalPrice,
                DetailName = room.Name,
                ProductUrl = String.Format("/{0}/{1}/{2}", hotel.Country.ToLower(), hotel.Id, Utilities.GenerateSlug(hotel.Name)),
                TotalPrice = Math.Round((sellingRatePrice ?? 0), 2),
                CancellationPolicy = cancelationPolicy,
                TotalSurcharge = Math.Round((surchargePrice ?? 0), 2),
                TotalTaxeFee = Math.Round((taxFeePrice ?? 0), 2),
                Details = details,
                Promotion = promotion,
                TotalSave = totalSave,
                Night = days,
                Quantity = total
            };

            return model;
        }

        public ProductModel GetHotelCoupon(int id, int totalRoom)
        {
            var exchange = _exchangeService.GetRateExchangeById(3);
            var details = new List<ProductDetailModel>();
            
            var coupon = _hotelCouponService.Queryable().First(a => a.Id == id);
            if (coupon != null)
            {
                var hotel = _hotelRepository.GetHotelSingle(coupon.HotelId);
                if (hotel == null)
                {
                    return new ProductModel();
                }

                var cancelationPolicy = new CancellationPolicy();
                decimal totalSave = 0;
                var image = _imageRepository.GetImageSingleByHotelId(coupon.HotelId);
                var night = 1;
                var finalPrice = coupon.Price*exchange.CurrentPrice;
                var totalPrice = finalPrice * totalRoom;
                

                //var room = _roomRepository.GetRoom(hotel.Id);
                //if (room == null)
                //{
                //    return new ProductModel();
                //}

                var model = new ProductModel
                {
                    Id = coupon.Id,
                    ProductId = coupon.HotelId,
                    Stay = "",
                    Name = !string.IsNullOrEmpty(hotel.HotelNameLocal) ? hotel.HotelNameLocal : hotel.Name,
                    Rating = hotel.StartRating ?? 0,
                    Location = hotel.StreetAddressLocal,
                    ImageUrl = image != null ? !string.IsNullOrEmpty(image.ImageThumbnail) ? "https://admin.goreise.com" + $"{image.ImageThumbnail.Substring(1)}-original.jpeg" : "/Content/images/demo/general/no-image.jpg" : "/Content/images/demo/general/no-image.jpg",
                    FinalPrice = finalPrice ?? 1,
                    DetailName = coupon.Name,
                    Include = coupon.Description,
                    ProductUrl = String.Format("/{0}/{1}/{2}", hotel.Country.ToLower(), hotel.Id, Utilities.GenerateSlug(hotel.Name)),
                    TotalPrice = Math.Round(totalPrice ?? 1),
                    CancellationPolicy = cancelationPolicy,
                    TotalSurcharge = 0,
                    TotalTaxeFee =0,
                    Details = details,
                    Promotion = null,
                    TotalSave = totalSave * exchange.CurrentPrice ?? 1,
                    Night = 0,
                    Quantity = totalRoom
                };

                return model;
            }
            return new ProductModel();

        }

        public ProductModel GetHotelProductVn(int type, int id, string strCheckIn, string strCheckOut, int total, int promotionId)
        {
            var exchange = _exchangeService.GetRateExchangeById(3);
            var checkIn = Utilities.ConvertToDateTime(strCheckIn);
            var checkOut = Utilities.ConvertToDateTime(strCheckOut);
            var details = new List<ProductDetailModel>();
            var days = (checkOut - checkIn).Days;
            var stay = String.Format("{0} đến {1} | {2} {3} ", (object)checkIn.ToString("dd/MM/yyyy"),
               (object)checkOut.ToString("dd/MM/yyyy"), (object)days,
               (object)"đêm");
            var room = _roomRepository.GetRoom(id);
            if (room == null)
            {
                return new ProductModel();
            }
            var hotel = _hotelRepository.GetHotelSingle(room.HotelId ?? 0);
            if (hotel == null)
            {
                return new ProductModel();
            }
            var cancelationPolicy = new CancellationPolicy();
            decimal totalSave = 0;
            var image = _imageRepository.GetImageSingleByHotelId(room.HotelId ?? 0);
            //var totalPrice = _roomControlRepository.GetTotalPrice(room.Id, checkIn, checkOut);

            var roomControls = _roomControlRepository.GetRoomListCheckInOut(room.Id, checkIn, checkOut);
            if (roomControls == null)
            {
                return new ProductModel();
            }
            var promotion = GetVietnamPromotionModelById(promotionId, roomControls);
            if (promotion != null)
            {
                totalSave = promotion.Price;
                cancelationPolicy = _cancellationPolicyRepository.GetCancellationPolicyById(promotion.CancelationId ?? -1);
            }
            else
            {
                var cancelationHotel = _hotelCancellationRepository.GetHotelCancellationPolicyById(room.HotelId ?? 0, (int)Utilities.Status.Active);
                if (cancelationHotel != null)
                {
                    cancelationPolicy = _cancellationPolicyRepository.GetCancellationPolicyById(cancelationHotel.CancellationID);
                }
            }

            var night = 1;
            var surchargePrice = ((roomControls.Sum(r => r.Surcharge1) +
                                    roomControls.Sum(r => r.Surcharge2) +
                                    roomControls.Sum(r => r.CompulsoryMeal)) * total);
            var taxFeePrice = ((roomControls.Sum(r => r.SellingRate) - roomControls.Sum(r => r.SellingRate) / 1.15m) * total);
            var sellingRatePrice = (roomControls.Sum(r => r.SellingRate) / 1.15m * total);
            var totalPrice = (Math.Round(surchargePrice ?? 0, 2) + Math.Round(sellingRatePrice ?? 0, 2) + Math.Round(taxFeePrice ?? 0, 2) - Math.Round(totalSave, 2));

            foreach (var roomControl in roomControls)
            {
                if (roomControl == null)
                {
                    continue;
                }
                var detail = new ProductDetailModel
                {
                    Date = roomControl.RoomDate,
                    Night = night.ToString(CultureInfo.InvariantCulture),
                    Quantity = total,
                    PriceRoom = Math.Round((roomControl.SellingRate / 1.15m ?? 0), 0) * exchange.CurrentPrice ?? 1,
                    PriceSurcharge = Math.Round(((roomControl.Surcharge1 + roomControl.Surcharge2 + roomControl.CompulsoryMeal) ?? 0) * exchange.CurrentPrice ?? 1, 0),
                };
                details.Add(detail);
                night++;
            }

            var model = new ProductModel
            {
                Id = room.Id,
                ProductId = hotel.Id,
                CheckIn = checkIn,
                CheckOut = checkOut,
                Stay = stay,
                Name = !string.IsNullOrEmpty(hotel.HotelNameLocal) ? hotel.HotelNameLocal : hotel.Name,
                Rating = hotel.StartRating ?? 0,
                Location = hotel.StreetAddressLocal,
                ImageUrl = image != null ? !string.IsNullOrEmpty(image.ImageThumbnail) ? "https://admin.goreise.com" + $"{image.ImageThumbnail.Substring(1)}-original.jpeg" : "/Content/images/demo/general/no-image.jpg" : "/Content/images/demo/general/no-image.jpg",
                FinalPrice = totalPrice * exchange.CurrentPrice ?? 1,
                DetailName = room.Name,
                ProductUrl = String.Format("/{0}/{1}/{2}", hotel.Country.ToLower(), hotel.Id, Utilities.GenerateSlug(hotel.Name)),
                TotalPrice = Math.Round((sellingRatePrice ?? 0), 0) * exchange.CurrentPrice ?? 1,
                CancellationPolicy = cancelationPolicy,
                TotalSurcharge = (surchargePrice ?? 0) * exchange.CurrentPrice ?? 1,
                TotalTaxeFee = Math.Round((taxFeePrice ?? 0), 0) * exchange.CurrentPrice ?? 1,
                Details = details,
                Promotion = promotion,
                TotalSave = totalSave * exchange.CurrentPrice ?? 1,
                Night = days,
                Quantity = total
            };

            return model;
        }

        public ProductModel GetOtherServices(int type, int id, string strCheckIn, string strCheckOut, int qty, decimal price, decimal total, decimal discount, string cancellation, string name, string description)
        {
            DateTime dateTime = Utilities.ConvertToDateTime(strCheckIn);
            DateTime dateTime1 = Utilities.ConvertToDateTime(strCheckOut);
            int days = (dateTime1 - dateTime).Days;
            object[] str = new object[] { dateTime.ToString("MMM dd, yyyy"), dateTime1.ToString("MMM dd, yyyy"), days, null };
            str[3] = (days >= 2 ? "nights" : "night");
            string str1 = string.Format("{0} - {1} | {2} {3} ", str);
            return new ProductModel()
            {
                Id = id,
                CheckIn = dateTime,
                CheckOut = dateTime1,
                Stay = str1,
                Name = name,
                ImageUrl = "https://goreise.com/rc/Content/new-ui/images/demo/general/no-image.jpg",
                FinalPrice = price,
                TotalPrice = total,
                CancellationPolicy = new CancellationPolicy()
                {
                    Description = cancellation
                },
                TotalSave = discount,
                Night = new int?(days),
                Quantity = new int?(qty),
                DetailName = description
            };
        }

        public PromotionModel GetPromotionModelById(int id, List<RoomControl> rooms)
        {
            var promotion = _promotionRepository.GetPromotionById(id);

            var promotionModel = new PromotionModel();
            if (promotion == null) return null;
            switch (promotion.PromotionType)
            {
                case (int)Utilities.PromotionType.BonusNight:
                    if (promotion.MinimumStay != null)
                    {
                        if (promotion.Get == null) return promotionModel;
                        var daysFree = (int)(rooms.Count() / promotion.MinimumStay) * (decimal)promotion.Get;

                        var room = rooms.LastOrDefault();
                        if (room == null) return null;

                        promotionModel.Price = room.SellingRate ?? 0 / 1.15m * daysFree;
                        promotionModel.Name = string.Format("Stay {0} get {1} free.", promotion.MinimumStay, promotion.Get);
                        promotionModel.Description = string.Format("Discount ${0} for {1} day(s).",
                            Math.Round(promotionModel.Price, 2), daysFree);
                        return promotionModel;
                    }
                    break;
                case (int)Utilities.PromotionType.EarlyBird:
                case (int)Utilities.PromotionType.CustomizedPromotion:
                    switch (promotion.DiscountType)
                    {
                        case (int)Utilities.DiscountType.Percent:
                            promotionModel.Price = rooms.Sum(room => room.SellingRate != null ? (promotion.Get != null ? (((decimal)room.SellingRate / 1.15m) * (decimal)promotion.Get) / 100 : 0) : 0);
                            promotionModel.Name = string.Format("Special rate includes {0}% discount!", promotion.Get);
                            promotionModel.Description = string.Format("Discount ${0} for {1} day(s).",
                                Math.Round(promotionModel.Price, 2), rooms.Count());
                            return promotionModel;
                        case (int)Utilities.DiscountType.AmountDiscountPerNight:
                            promotionModel.Price = rooms.Where(room => promotion.Get != null).Sum(room => promotion.Get != null ? (decimal)promotion.Get : 0);
                            promotionModel.Name = string.Format("Get ${0} discount per night!", promotion.Get);
                            promotionModel.Description = string.Format("Discount ${0} for {1} day(s).",
                                Math.Round(promotionModel.Price, 2), rooms.Count());
                            return promotionModel;
                        case (int)Utilities.DiscountType.AmountDiscountPerBooking:
                            if (promotion.Get != null) promotionModel.Price = (decimal)promotion.Get;
                            promotionModel.Name = string.Format("Get ${0} discount per booking!", promotion.Get);
                            promotionModel.Description = string.Format("Discount ${0} per booking.",
                                Math.Round(promotionModel.Price, 2));
                            return promotionModel;
                        case (int)Utilities.DiscountType.FreeNight:
                            if (promotion.Get != null)
                            {
                                var roomControlModel = rooms.FirstOrDefault();
                                if (roomControlModel != null)
                                    if (roomControlModel.SellingRate != null)
                                        promotionModel.Price = ((decimal)roomControlModel.SellingRate * (decimal)promotion.Get);
                            }
                            promotionModel.Name = string.Format("Stay {0} get {1} free.", promotion.MinimumStay, promotion.Get);
                            promotionModel.Description = string.Format("Discount ${0} for {1} day(s) free.",
                                Math.Round(promotionModel.Price, 2), promotion.Get);
                            return promotionModel;
                    }
                    break;
            }
            return null;
        }

        public PromotionModel GetVietnamPromotionModelById(int id, List<RoomControl> rooms)
        {
            var exchange = _exchangeService.GetRateExchangeById(3);
            var promotion = _promotionRepository.GetPromotionById(id);

            var promotionModel = new PromotionModel();
            if (promotion == null) return null;
            promotionModel.CancelationId = promotion.Cancelation;
            switch (promotion.PromotionType)
            {
                case (int)Utilities.PromotionType.BonusNight:
                    if (promotion.MinimumStay != null)
                    {
                        if (promotion.Get == null) return promotionModel;
                        var daysFree = (int)(rooms.Count() / promotion.MinimumStay) * (decimal)promotion.Get;
                        var room = rooms.LastOrDefault();
                        if (room == null) return null;
                        promotionModel.Price = room.SellingRate ?? 0 / 1.15m * daysFree;
                        promotionModel.Name = string.Format("Ở {0} đêm nhận ngay {1} đêm miễn phí.", promotion.MinimumStay, promotion.Get);
                        promotionModel.Description = string.Format("Giảm {0}<sup>đ</sup> cho {1} đêm.",
                            Math.Round(promotionModel.Price * exchange.CurrentPrice ?? 1, 0).ToString("##,###").Replace(",", "."), daysFree);
                        return promotionModel;
                    }
                    break;
                case (int)Utilities.PromotionType.EarlyBird:
                case (int)Utilities.PromotionType.CustomizedPromotion:
                    switch (promotion.DiscountType)
                    {
                        case (int)Utilities.DiscountType.Percent:
                            promotionModel.Price = rooms.Sum(room => room.SellingRate != null ? (promotion.Get != null ? (((decimal)room.SellingRate / 1.15m) * (decimal)promotion.Get) / 100 : 0) : 0);
                            promotionModel.Name = string.Format("Giá đặc biệt giảm {0}% !", promotion.Get);
                            promotionModel.Description = string.Format("Giảm {0}<sup>đ</sup> cho {1} đêm.",
                                Math.Round(promotionModel.Price * exchange.CurrentPrice ?? 1, 0).ToString("##,###").Replace(",", "."), rooms.Count());
                            return promotionModel;
                        case (int)Utilities.DiscountType.AmountDiscountPerNight:
                            promotionModel.Price = rooms.Where(room => promotion.Get != null).Sum(room => promotion.Get != null ? (decimal)promotion.Get : 0);
                            promotionModel.Name = string.Format("Giá đặc biệt giảm {0}<sup>đ</sup> cho mỗi đêm ở.", Math.Round(((promotion.Get ?? 0) * (double)(exchange.CurrentPrice ?? 1)), 0).ToString("##,###").Replace(",", "."));
                            promotionModel.Description = string.Format("Giảm {0}<sup>đ</sup> cho {1} đêm.",
                                Math.Round(promotionModel.Price * exchange.CurrentPrice ?? 1, 0).ToString("##,###").Replace(",", "."), rooms.Count());
                            return promotionModel;
                        case (int)Utilities.DiscountType.AmountDiscountPerBooking:
                            if (promotion.Get != null) promotionModel.Price = (decimal)promotion.Get;
                            promotionModel.Name = string.Format("Giá đặc biệt giảm {0}<sup>đ</sup> trên booking!", Math.Round(((promotion.Get ?? 0) * (double)(exchange.CurrentPrice ?? 1)), 0).ToString("##,###").Replace(",", "."));
                            promotionModel.Description = string.Format("Giảm {0}<sup>đ</sup>trên booking.",
                                Math.Round(promotionModel.Price * exchange.CurrentPrice ?? 1, 0).ToString("##,###").Replace(",", "."));
                            return promotionModel;
                        case (int)Utilities.DiscountType.FreeNight:
                            if (promotion.Get != null)
                            {
                                var roomControlModel = rooms.FirstOrDefault();
                                if (roomControlModel != null)
                                    if (roomControlModel.SellingRate != null)
                                        promotionModel.Price = ((decimal)roomControlModel.SellingRate * (decimal)promotion.Get);
                            }
                            promotionModel.Name = string.Format("Ở {0} đêm nhận {1} đêm miễn phí.", promotion.MinimumStay, promotion.Get);
                            promotionModel.Description = string.Format("Giảm {0}<sup>đ</sup> cho {1} đêm miễm phí.",
                                Math.Round(promotionModel.Price * exchange.CurrentPrice ?? 1, 0).ToString("##,###").Replace(",", "."), promotion.Get);
                            return promotionModel;
                    }
                    break;
            }
            return null;
        }
    }
}