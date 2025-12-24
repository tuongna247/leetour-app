using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using Vinaday.Data.Models;
using Vinaday.Data.Models.Extention;

namespace Vinaday.Web.Framework
{
    public class Utilities
    {
        public const string CookieName = "vinaday.user";

        public static readonly string Admin;

        public static readonly string Affiliate;

        public static readonly string Contract;

        public static readonly string SalesManager;

        public static readonly string Account;

        public static readonly string CountryManager;

        public static readonly string Sales;

        public static readonly string Guest;

        public static readonly string Seo;

        public static readonly string HotelSupplier;

        public static readonly string High;

        public static readonly string Middle;

        public static readonly string Slow;

        public static readonly string StarHigh;

        public static readonly string StarSlow;

        public static readonly string ReviewCore;

        public static readonly string HotelName;

        public static readonly string TourDuration;

        public static readonly string GroupSize;

        public static readonly string Meals;

        public static readonly string Transportations;

        public static readonly string TravelStyle;

        public static readonly string TheCollection;

        public static readonly string BookingSession;

        public static readonly string NoneSslUrl;

        public static readonly string SslUrl;

        private static bool _razorInitialized;

        static Utilities()
        {
            Utilities.Admin = "Admin";
            Utilities.Affiliate = "Affiliate";
            Utilities.Contract = "Contract";
            Utilities.SalesManager = "Sales Manager";
            Utilities.Account = "Account";
            Utilities.CountryManager = "Country Manager";
            Utilities.Sales = "Sales";
            Utilities.Guest = "Guest";
            Utilities.Seo = "SEO";
            Utilities.HotelSupplier = "Hotel Supplier";
            Utilities.High = "#c93838";
            Utilities.Middle = "#ff572d";
            Utilities.Slow = "#61c419";
            Utilities.StarHigh = "star-high";
            Utilities.StarSlow = "star-slow";
            Utilities.ReviewCore = "review-core";
            Utilities.HotelName = "hotel-name";
            Utilities.TourDuration = "TOURDURATION";
            Utilities.GroupSize = "GROUPSIZE";
            Utilities.Meals = "MEALS";
            Utilities.Transportations = "TRANSPORTATIONS";
            Utilities.TravelStyle = "TRAVELSTYLE";
            Utilities.TheCollection = "THECOLLECTION";
            Utilities.BookingSession = "BookingSession";
            Utilities.NoneSslUrl = "https://admin.goreise.com";
            Utilities.SslUrl = "https://admin.goreise.com";
        }

        public Utilities()
        {
        }

        public static bool CheckEmail(string email)
        {
            Regex regex = new Regex("^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$");
            return (string.IsNullOrEmpty(email) ? false : regex.IsMatch(email));
        }

        public static string CleanPhone(string phoneNumber)
        {
            string str;
            str = (!string.IsNullOrWhiteSpace(phoneNumber) ? (new Regex("[^\\d;]")).Replace(phoneNumber, "") : string.Empty);
            return str;
        }

        public static string ConvertDateTime2String(DateTime? dt)
        {
            string empty = string.Empty;
            if (dt.HasValue)
            {
                empty = dt.Value.ToString("dd/MM/yyyy");
            }
            return empty;
        }

        public static DateTime ConvertStringToDate(string str)
        {
            DateTime dateTime;
            DateTime today = DateTime.Today;
            if (!string.IsNullOrEmpty(str))
            {
                DateTime dateTime1 = DateTime.Parse(str);
                str = dateTime1.ToString(CultureInfo.GetCultureInfo("en-US").DateTimeFormat.ShortDatePattern);
                DateTime.TryParse(str, out today);
                dateTime = today;
            }
            else
            {
                dateTime = today;
            }
            return dateTime;
        }

        public static DateTime ConvertStringToDateTime(string str)
        {
            DateTime dateTime;
            DateTime today = DateTime.Today;
            if (!string.IsNullOrEmpty(str))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en");
                DateTime.TryParse(str, out today);
                dateTime = today;
            }
            else
            {
                dateTime = today;
            }
            return dateTime;
        }

        public static int ConvertStringToInt(string str)
        {
            int num;
            int.TryParse(str, out num);
            return num;
        }

        public static bool ConvertToBoolean(string str)
        {
            bool flag;
            bool.TryParse(str, out flag);
            return flag;
        }

        public static string ConvertToDateString(string s)
        {
            string str;
            if (!string.IsNullOrWhiteSpace(s))
            {
                DateTime? nullable = Utilities.ParseExactDateTimeString2(s);
                DateTimeFormatInfo dateTimeFormat = (new CultureInfo("en-US", false)).DateTimeFormat;
                str = (nullable.HasValue ? nullable.Value.ToString("dd MMM .yyyy", dateTimeFormat) : "");
            }
            else
            {
                str = s;
            }
            return str;
        }

        public static DateTime ConvertToDateTime(string str)
        {
            DateTime today = DateTime.Today;
            if (!string.IsNullOrEmpty(str))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en");
                DateTime.TryParse(str, out today);
            }
            return today;
        }

        public static decimal ConvertToDecimal(string str)
        {
            decimal num;
            decimal.TryParse(str, out num);
            return num;
        }

        public static double ConvertToDouble(string str)
        {
            double num;
            double.TryParse(str, out num);
            return num;
        }

        public static double ConvertToDouble(int value)
        {
            return Convert.ToDouble(value);
        }

        public static int ConvertToInt(string str)
        {
            int num;
            int.TryParse(str, out num);
            return num;
        }

        public static long ConvertToLong(string value)
        {
            long num;
            num = (string.IsNullOrEmpty(value) ? (long)0 : Convert.ToInt64(value));
            return num;
        }

        public static string ConvertToVietnamese(string str)
        {
            string str1;
            if (!string.IsNullOrEmpty(str))
            {
                Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
                string str2 = str.Normalize(NormalizationForm.FormD);
                str1 = regex.Replace(str2, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
            }
            else
            {
                str1 = str;
            }
            return str1;
        }

        private static ITemplateService CreateTemplateService()
        {
            var config = new TemplateServiceConfiguration
            {
                BaseTemplateType = typeof(HtmlTemplateBase<>),
            };
            return new TemplateService(config);
        }

        public static Image CropImage(int width, int height, Stream stream)
        {
            Image image = Image.FromStream(stream);
            Rectangle rectangle = new Rectangle(Convert.ToInt32((image.Width - width) / 2), Convert.ToInt32((image.Height - height) / 2), Convert.ToInt32(width), Convert.ToInt32(height));
            Bitmap bitmap = new Bitmap(rectangle.Width, rectangle.Height);
            using (Graphics graphic = Graphics.FromImage(bitmap))
            {
                graphic.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height), rectangle, GraphicsUnit.Pixel);
            }
            return bitmap;
        }

        public static string FormatPhone(string phoneNumber)
        {
            string str;
            str = (!string.IsNullOrWhiteSpace(phoneNumber) ? Regex.Replace(phoneNumber, "(\\d{3})(\\d{3})(\\d{4})", "($1) $2-$3") : string.Empty);
            return str;
        }
        public static Dictionary<string,string> slugKeyAndValue = new Dictionary<string, string>(); 
        public static string GenerateSlug(string phrase, int maxLength = 100)
        {
            if (slugKeyAndValue.ContainsKey(phrase))
            {
                return slugKeyAndValue[phrase];
            }
            string str;
            if (!string.IsNullOrEmpty(phrase))
            {
                string lower = phrase.ToLower();
                lower = Regex.Replace(lower, "[^a-z0-9\\s-]", "");
                lower = Regex.Replace(lower, "[\\s-]+", " ").Trim();
                lower = lower.Substring(0, (lower.Length <= maxLength ? lower.Length : maxLength)).Trim();
                lower = Regex.Replace(lower, "\\s", "-");
                str = lower;
            }
            else
            {
                str = null;
            }
            if (phrase == null) slugKeyAndValue.Add(phrase,str);
            return str;
        }

        public static List<CatDetail> GetCategoryDetailListItem(string str, List<CatDetail> listCategoryDetail)
        {
            Func<CatDetail, bool> func = null;
            List<CatDetail> catDetails = new List<CatDetail>();
            Dictionary<int, int> selectedItem = Utilities.GetSelectedItem(str);
            List<CatDetail> catDetails1 = listCategoryDetail;
            Func<CatDetail, bool> func1 = func;
            if (func1 == null)
            {
                Func<CatDetail, bool> func2 = (CatDetail cat) => selectedItem.ContainsKey(cat.Id);
                Func<CatDetail, bool> func3 = func2;
                func = func2;
                func1 = func3;
            }
            foreach (CatDetail catDetail in catDetails1.Where<CatDetail>(func1))
            {
                catDetail.CheckedItem = "check";
                catDetails.Add(catDetail);
            }
            return catDetails;
        }

        public static Dictionary<int, int> GetCategoryItem(string str)
        {
            Dictionary<int, int> nums;
            int num;
            Dictionary<int, int> nums1 = new Dictionary<int, int>();
            if (!string.IsNullOrEmpty(str))
            {
                string[] strArrays = str.Split(new char[] { ',' });
                for (int i = 0; i < (int)strArrays.Length; i++)
                {
                    int.TryParse(strArrays[i], out num);
                    if (!nums1.ContainsKey(num))
                    {
                        nums1.Add(num, num);
                    }
                }
                nums = nums1;
            }
            else
            {
                nums = nums1;
            }
            return nums;
        }

        public static List<ItemModel> GetCategoryList(string categoryStr, List<CategoryDetail> categories)
        {
            Dictionary<int, int> categoryItem = Utilities.GetCategoryItem(categoryStr);
            List<ItemModel> list = (
                from category in categories
                select new ItemModel()
                {
                    Id = category.Id,
                    Checked = categoryItem.ContainsKey(category.Id),
                    Name = category.Name,
                    Description = category.Description,
                    Status = category.Status
                }).ToList<ItemModel>();
            return list;
        }

        public static List<ItemModel> GetCityList(string categoryStr, List<City> cities)
        {
            Dictionary<int, int> categoryItem = Utilities.GetCategoryItem(categoryStr);
            List<ItemModel> list = (
                from city in cities
                select new ItemModel()
                {
                    Id = city.CityId,
                    Checked = categoryItem.ContainsKey(city.CityId),
                    Name = city.Description,
                    Description = city.Description
                }).ToList<ItemModel>();
            return list;
        }

        public static string GetLast(string source, int length)
        {
            return (length >= source.Length ? source : source.Substring(source.Length - length));
        }

        public static string GetRandomString(int length)
        {
            RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
            int num = (int)Math.Ceiling(length / new decimal(20, 0, 0, false, 1));
            byte[] numArray = new byte[num];
            rNGCryptoServiceProvider.GetBytes(numArray);
            byte[] numArray1 = numArray;
            StringBuilder stringBuilder = new StringBuilder();
            string str = ((IEnumerable<byte>)numArray1).Aggregate<byte, StringBuilder>(stringBuilder, (StringBuilder f, byte s) => f.AppendFormat("{0:X2}", s)).ToString(0, length);
            return str;
        }

        public static Dictionary<int, int> GetSelectedItem(string str)
        {
            Dictionary<int, int> nums;
            int num;
            Dictionary<int, int> nums1 = new Dictionary<int, int>();
            if (!string.IsNullOrEmpty(str))
            {
                string[] strArrays = str.Split(new char[] { ',' });
                for (int i = 0; i < (int)strArrays.Length; i++)
                {
                    int.TryParse(strArrays[i], out num);
                    if (!nums1.ContainsKey(num))
                    {
                        nums1.Add(num, num);
                    }
                }
                nums = nums1;
            }
            else
            {
                nums = nums1;
            }
            return nums;
        }

        public static string GetSeoFriendly(string name)
        {
            string empty = string.Empty;
            if (!string.IsNullOrEmpty(name))
            {
                empty = name.ToLower().Replace("&", "").Replace("\"", "").Replace("\\", "").Replace("/", "").Replace(" ", "-").Replace("--", "-").Replace("---", "-").Replace(":", "");
            }
            return empty;
        }

        public static string GetUserIp(HttpRequestBase request)
        {
            return request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.ServerVariables["REMOTE_ADDR"];
        }

        private static void InitializeRazor()
        {
            if (!Utilities._razorInitialized)
            {
                Utilities._razorInitialized = true;
                Razor.SetTemplateService(Utilities.CreateTemplateService());
            }
        }

        public static DateTime MinDate()
        {
            return DateTime.Now.AddYears(-100);
        }

        public static DateTime? ParseExactDateTimeString2(string date)
        {
            DateTime? nullable;
            if (date != null)
            {
                DateTime dateTime = Utilities.MinDate();
                try
                {
                    int num = date.IndexOf(".");
                    if (num == -1)
                    {
                        dateTime = DateTime.ParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    }
                    else
                    {
                        dateTime = (num == 3 ? DateTime.ParseExact(date, "dd .MM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None) : DateTime.ParseExact(date, "dd MMM .yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None));
                    }
                }
                catch (Exception exception)
                {
                }
                nullable = new DateTime?(dateTime);
            }
            else
            {
                nullable = new DateTime?(DateTime.Now);
            }
            return nullable;
        }

        public static string ParseTemplate(string name, object model)
        {
            Utilities.InitializeRazor();
            string str = string.Format("~/EmailTemplate/{0}.cshtml", name);
            string str1 = File.ReadAllText(HttpContext.Current.Server.MapPath(str));
            return Razor.Parse(str1, model);
        }

        public static long Rounding(decimal? myNum, long roundTo)
        {
            long num;
            decimal? nullable;
            decimal? nullable1;
            decimal? nullable2;
            if (roundTo > (long)0)
            {
                decimal? nullable3 = myNum;
                decimal num1 = roundTo / (long)2;
                if (nullable3.HasValue)
                {
                    nullable = new decimal?(nullable3.GetValueOrDefault() + num1);
                }
                else
                {
                    nullable = null;
                }
                decimal? nullable4 = nullable;
                decimal num2 = roundTo;
                if (nullable4.HasValue)
                {
                    nullable1 = new decimal?(nullable4.GetValueOrDefault() / num2);
                }
                else
                {
                    nullable3 = null;
                    nullable1 = nullable3;
                }
                decimal? nullable5 = nullable1;
                decimal num3 = roundTo;
                if (nullable5.HasValue)
                {
                    nullable2 = new decimal?(nullable5.GetValueOrDefault() * num3);
                }
                else
                {
                    nullable4 = null;
                    nullable2 = nullable4;
                }
                string str = string.Format("{0:N0}", nullable2).Replace(",", "").Replace(".", "");
                num = (str == "" ? (long)0 : Utilities.ConvertToLong(str));
            }
            else
            {
                num = (myNum.HasValue ? (long)myNum.Value : (long)0);
            }
            return num;
        }

        public static string RoundingTo(long myNum, long roundTo)
        {
            string str;
            if (roundTo > (long)0)
            {
                string str1 = string.Format("{0:N0}", (myNum + roundTo / (long)2) / roundTo * roundTo).Replace(",", ".");
                str = str1;
            }
            else
            {
                str = myNum.ToString();
            }
            return str;
        }

        public static long  RoundingToLong(long myNum, long roundTo)
        {
            long str;
            if (roundTo > 0)
            {
                str = (myNum  ) / roundTo * roundTo;
            }
            else
            {
                str = myNum;
            }
            return str;
        }

        public static long RoundingToLong(decimal? myNum, long roundTo)
        {
            decimal str;
            if(!myNum.HasValue)
            {
                return 0;
            }
            else if (roundTo > 0)
            {
                str = (myNum.Value  ) / roundTo * roundTo;
            }
            else
            {
                str = myNum.Value;
            }
            return (long)str;
        }

        public static string RoundingTo(string myNum, int roundTo)
        {
            myNum = myNum.Replace(",", "");
            return Utilities.RoundingTo(Utilities.ConvertStringToInt(myNum), roundTo);
        }

        public static string RoundingTo(int myNum, int roundTo)
        {
            return Utilities.RoundingTo((long)myNum, (long)roundTo);
        }

        public static Image ScaleBySize(Image imgPhoto, int size)
        {
            float single;
            float single1;
            int num = size;
            float width = (float)imgPhoto.Width;
            float height = (float)imgPhoto.Height;
            if (width <= 2f * height)
            {
                int num1 = num / 2;
                single = (float)num1;
                single1 = width * (float)num1 / height;
            }
            else
            {
                single1 = (float)num;
                single = height * (float)num / width;
            }
            Bitmap bitmap = new Bitmap((int)single1, (int)single, PixelFormat.Format32bppPArgb);
            bitmap.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
            Graphics graphic = Graphics.FromImage(bitmap);
            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.DrawImage(imgPhoto, new Rectangle(0, 0, (int)single1, (int)single), new Rectangle(0, 0, (int)width, (int)height), GraphicsUnit.Pixel);
            graphic.Dispose();
            return bitmap;
        }

        public static string UppercaseWords(string value)
        {
            char[] charArray = value.ToCharArray();
            if ((int)charArray.Length >= 1)
            {
                if (char.IsLower(charArray[0]))
                {
                    charArray[0] = char.ToUpper(charArray[0]);
                }
            }
            for (int i = 1; i < (int)charArray.Length; i++)
            {
                if (charArray[i - 1] == ' ')
                {
                    if (char.IsLower(charArray[i]))
                    {
                        charArray[i] = char.ToUpper(charArray[i]);
                    }
                }
            }
            return new string(charArray);
        }

        public enum ApplyOn
        {
            Notspecific,
            EveryNight,
            FirstNight,
            LastNight
        }

        public enum BookingStatus
        {
            Defautl,
            Inactive,
            Active,
            Holding,
            Paid,
            Cancelled,
            Amended,
            Refunded,
            Delete,
            Deposit,
            CancelledFee,
            Failure,
            VatInvoice
        }

        public enum ContactType
        {
            Defautl,
            Reservation,
            Marketing,
            Accountant,
            Main
        }

        public enum Countries
        {
            Default,
            VietName,
            Thailand,
            Cambodia,
            Laos,
            Myanmar
        }

        public enum DiscountType
        {
            Percent = 1,
            AmountDiscountPerBooking = 2,
            AmountDiscountPerNight = 3,
            FreeNight = 4
        }

        public enum SurChargeType
        {
            Price = 1,
            Percent = 2,
        }


        public enum Language
        {
            Default,
            Vietnam,
            English,
            German,
            French,
            Danish
        }

        public enum MediaType
        {
            Defautl,
            Banner,
            Gallery,
            Logo,
            Map
        }

        public enum MemberType
        {
            Default,
            DinningClub
        }

        public enum PaymentMethod
        {
            None,
            Cash,
            Credit,
            Transfer
        }

        public enum Product
        {
            Hotel = 1,
            Tour = 2,
            Daytrip = 3,
            OtherServices = 4,
            HotelPackage = 5,
            HotelVoucher = 6
        }

        public enum PromotionType
        {
            EarlyBird = 215,
            CustomizedPromotion = 216,
            BonusNight = 217,
            HotDeal = 218
        }

        public enum SearchFilter
        {
            TravelStyle,
            Servirces,
            Duration
        }

        public enum Status
        {
            Defautl,
            Inactive,
            Active,
            Pending,
            Approved,
            Delete
        }

        public enum TourType
        {
            Defautl,
            Tour,
            Daytrip
        }

        public enum TransportationType
        {
            Bus = 18,
            Boat = 19,
            Plane = 21
        }
    }
}