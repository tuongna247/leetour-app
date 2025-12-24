using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class Utilities
	{
		public Utilities()
		{
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

		public static bool ConvertToBoolean(string str)
		{
			bool flag;
			bool.TryParse(str, out flag);
			return flag;
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

		public static string GenerateSlug(string phrase, int maxLength = 100)
		{
			string lower = phrase.ToLower();
			lower = Regex.Replace(lower, "[^a-z0-9\\s-]", "");
			lower = Regex.Replace(lower, "[\\s-]+", " ").Trim();
			lower = lower.Substring(0, (lower.Length <= maxLength ? lower.Length : maxLength)).Trim();
			lower = Regex.Replace(lower, "\\s", "-");
			return lower;
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

		public enum ApplyOn
		{
			Notspecific,
			EveryNight,
			FirstNight,
			LastNight
		}

		public enum DiscountType
		{
			Percent = 1,
			AmountDiscountPerBooking = 2,
			AmountDiscountPerNight = 3,
			FreeNight = 4
		}

		public enum ExpandRateType
		{
			ExpandRate = 1,
			Surcharge = 2,
			Promotion = 3
		}

		public enum Language
		{
			Defautl,
			Vietnam,
			English
		}

		public enum ProductType
		{
			Hotel = 1,
			Tour = 2,
			Other = 3
		}

		public enum PromotionType
		{
			EarlyBird = 243,
			CustomizedPromotion = 244,
			BonusNight = 245,
			HotDeal = 246
		}

		public enum RequestType
		{
			RoomRequest = 1,
			ChangeBookingRequest = 2,
			Other = 3
		}

		public enum SpecialRateType
		{
			Defautl,
			Surcharge,
			Promotion
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
	}
}