using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;
using Vinaday.Data.Models.Extention;

namespace Vinaday.Services
{
	public class RoomService : Service<Room>, IRoomService, IService<Room>
	{
		private readonly IRepositoryAsync<Room> _roomRepository;

		private readonly IRepositoryAsync<Surcharge> _surchargeRepository;

		private readonly IRepositoryAsync<Promotion> _promotionRepository;

		private readonly IRepositoryAsync<RoomControl> _roomControlRepository;

		private readonly IRepositoryAsync<CancellationPolicy> _cancellationPolicyControlRepository;

		public RoomService(IRepositoryAsync<Room> roomRepository, IRepositoryAsync<Promotion> promotionRepository, IRepositoryAsync<RoomControl> roomControlRepository, IRepositoryAsync<CancellationPolicy> cancellationPolicyControlRepository, IRepositoryAsync<Surcharge> surchargeRepository) : base(roomRepository)
		{
			this._roomRepository = roomRepository;
			this._promotionRepository = promotionRepository;
			this._roomControlRepository = roomControlRepository;
			this._cancellationPolicyControlRepository = cancellationPolicyControlRepository;
			this._surchargeRepository = surchargeRepository;
		}

		public Room AddRoom(Room room)
		{
			this._roomRepository.Insert(room);
			return room;
		}

		public bool CheckRoomAvailable(int roomId, DateTime checkIn)
		{
			bool flag = true;
			RoomControl roomSingleByIdDate = this._roomControlRepository.GetRoomSingleByIdDate(roomId, checkIn);
			if (roomSingleByIdDate != null)
			{
				if (roomSingleByIdDate.CloseOutRegular)
				{
					flag = false;
				}
				if (roomSingleByIdDate.TotalAvailable <= 0)
				{
					flag = false;
				}
				decimal? sellingRate = roomSingleByIdDate.SellingRate;
				decimal num = new decimal();
				if ((sellingRate.GetValueOrDefault() <= num ? sellingRate.HasValue : false))
				{
					flag = false;
				}
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		public RoomModel GetEnRoomModel(RoomModel room, Promotion promotion, DateTime checkIn, DateTime checkOut)
		{
			DateTime now;
			double? get;
			int? applyOn;
			RoomModel roomModel;
			double? nullable;
			bool flag;
			bool flag1;
			bool flag2;
			double? nullable1;
			double? nullable2;
			TimeSpan date = checkOut - checkIn;
			double totalDays = date.TotalDays;
			RoomModel description = new RoomModel()
			{
				Id = room.Id,
				AdultNumber = room.AdultNumber,
				Name = room.Name,
				ExtraBed = room.ExtraBed,
				ExtraBedPrice = room.ExtraBedPrice,
				ImageUrl = room.ImageUrl,
				ChildrenAge = room.ChildrenAge,
				ChildrenNumber = room.ChildrenNumber,
				RoomSize = room.RoomSize,
				MaxOccupancy = room.MaxOccupancy,
				MaxExtrabed = room.MaxExtrabed,
				RackRate = room.RackRate,
				IsBreakfast = room.IsBreakfast,
				SellingRate = room.SellingRate,
				TotalAvailable = room.TotalAvailable,
				CloseOutRegular = room.CloseOutRegular,
				View = room.View,
				RoomFacilities = room.RoomFacilities,
				PromotionId = promotion.Id
			};
			int? promotionType = promotion.PromotionType;
			int valueOrDefault = 244;
			if ((promotionType.GetValueOrDefault() == valueOrDefault ? !promotionType.HasValue : true))
			{
				flag = false;
			}
			else
			{
				DateTime? bookingDateFrom = promotion.BookingDateFrom;
				now = DateTime.Now;
				if ((bookingDateFrom.HasValue ? bookingDateFrom.GetValueOrDefault() > now : true))
				{
					flag = false;
				}
				else
				{
					bookingDateFrom = promotion.BookingDateTo;
					now = DateTime.Now;
					flag = (bookingDateFrom.HasValue ? bookingDateFrom.GetValueOrDefault() >= now : false);
				}
			}
			if (!flag)
			{
				promotionType = promotion.PromotionType;
				valueOrDefault = 243;
				if ((promotionType.GetValueOrDefault() == valueOrDefault ? !promotionType.HasValue : true))
				{
					flag1 = false;
				}
				else
				{
					DateTime dateTime = checkOut.Date;
					now = DateTime.Now;
					date = dateTime - now.Date;
					double num = date.TotalDays;
					promotionType = promotion.MinimumDayAdvance;
					if (promotionType.HasValue)
					{
						nullable2 = new double?((double)promotionType.GetValueOrDefault());
					}
					else
					{
						nullable = null;
						nullable2 = nullable;
					}
					get = nullable2;
					flag1 = (num >= get.GetValueOrDefault() ? get.HasValue : false);
				}
				if (!flag1)
				{
					promotionType = promotion.PromotionType;
					valueOrDefault = 245;
					if ((promotionType.GetValueOrDefault() == valueOrDefault ? !promotionType.HasValue : true))
					{
						flag2 = false;
					}
					else
					{
						double num1 = totalDays;
						promotionType = promotion.MinimumStay;
						if (promotionType.HasValue)
						{
							nullable1 = new double?((double)promotionType.GetValueOrDefault());
						}
						else
						{
							nullable = null;
							nullable1 = nullable;
						}
						get = nullable1;
						flag2 = (num1 >= get.GetValueOrDefault() ? get.HasValue : false);
					}
					if (!flag2)
					{
						roomModel = new RoomModel();
					}
					else
					{
						promotionType = promotion.Cancelation;
						if (promotionType.HasValue)
						{
							IRepositoryAsync<CancellationPolicy> repositoryAsync = this._cancellationPolicyControlRepository;
							promotionType = promotion.Cancelation;
							description.Cancelation = repositoryAsync.GetCancellationPolicyById(promotionType.Value).Description;
						}
						promotionType = promotion.DiscountType;
						valueOrDefault = 4;
						if ((promotionType.GetValueOrDefault() == valueOrDefault ? promotionType.HasValue : false))
						{
							promotionType = promotion.ApplyOn;
							if (promotionType.HasValue)
							{
								valueOrDefault = promotionType.GetValueOrDefault();
								if (valueOrDefault == 2)
								{
									description.PromotionText = string.Concat(new object[] { "Stay ", promotion.MinimumStay, " nights get ", promotion.Get, " night(s) free on first night!" });
									//goto Label0;
								}
								else
								{
									if (valueOrDefault != 3)
									{
										goto Label7;
									}
									description.PromotionText = string.Concat(new object[] { "Stay ", promotion.MinimumStay, " nights get ", promotion.Get, " night(s) free on last night!" });
									//goto Label0;
								}
							}
						Label7:
							object[] minimumStay = new object[] { "Stay ", promotion.MinimumStay, " nights get ", promotion.Get, " night(s) free!" };
							description.PromotionText = string.Concat(minimumStay);
                        //Label0: ;
						}
						roomModel = description;
					}
				}
				else
				{
					promotionType = promotion.Cancelation;
					if (promotionType.HasValue)
					{
						IRepositoryAsync<CancellationPolicy> repositoryAsync1 = this._cancellationPolicyControlRepository;
						promotionType = promotion.Cancelation;
						description.Cancelation = repositoryAsync1.GetCancellationPolicyById(promotionType.Value).Description;
					}
					promotionType = promotion.DiscountType;
					if (promotionType.HasValue)
					{
						valueOrDefault = promotionType.GetValueOrDefault();
						switch (valueOrDefault)
						{
							case 1:
							{
								get = promotion.Get;
								if ((!get.HasValue ? false : description.SellingRate > decimal.Zero))
								{
									decimal sellingRate = description.SellingRate;
									decimal sellingRate1 = description.SellingRate;
									get = promotion.Get;
									description.SellingRate = sellingRate - ((sellingRate1 * (decimal)get.Value) / new decimal(100));
								}
								description.PromotionText = string.Concat("Special Rate includes ", promotion.Get, "% discount!");
								goto Label2;
							}
							case 2:
							{
								description.PromotionText = string.Concat("Get $", promotion.Get, " discount per booking!");
								goto Label2;
							}
							case 3:
							{
								applyOn = promotion.ApplyOn;
								if (applyOn.HasValue)
								{
									valueOrDefault = applyOn.GetValueOrDefault();
									switch (valueOrDefault)
									{
										case 1:
										{
											description.PromotionText = string.Concat("Get $ ", promotion.Get, " discount on every night!");
											goto Label3;
										}
										case 2:
										{
											description.PromotionText = string.Concat("Get $ ", promotion.Get, " discount on first night!");
											goto Label3;
										}
										case 3:
										{
											description.PromotionText = string.Concat("Get $ ", promotion.Get, " discount on last night!");
											goto Label3;
										}
									}
								}
								description.PromotionText = string.Concat("Get $", promotion.Get, " discount!");
							Label3:
								RoomModel value = description;
								decimal sellingRate2 = description.SellingRate;
								get = promotion.Get;
								get = ((get.GetValueOrDefault() > 0 ? get.HasValue : false) ? promotion.Get : new double?(0));
								value.SellingRate = sellingRate2 - (decimal)get.Value;
								goto Label2;
							}
						}
					}
					applyOn = promotion.ApplyOn;
					valueOrDefault = 1;
					if ((applyOn.GetValueOrDefault() == valueOrDefault ? !applyOn.HasValue : true))
					{
						applyOn = promotion.ApplyOn;
						valueOrDefault = 2;
						if ((applyOn.GetValueOrDefault() == valueOrDefault ? !applyOn.HasValue : true))
						{
							applyOn = promotion.ApplyOn;
							valueOrDefault = 3;
							if ((applyOn.GetValueOrDefault() == valueOrDefault ? !applyOn.HasValue : true))
							{
								description.PromotionText = string.Concat(new object[] { "Special Rate includes stay ", promotion.MinimumStay, " get ", promotion.Get, "free" });
							}
							else
							{
								description.PromotionText = string.Concat(new object[] { "Special Rate includes stay ", promotion.MinimumStay, " get ", promotion.Get, "free on last night!" });
							}
						}
						else
						{
							description.PromotionText = string.Concat(new object[] { "Special Rate includes stay ", promotion.MinimumStay, " get ", promotion.Get, "free on first night!" });
						}
					}
					else
					{
						description.PromotionText = string.Concat(new object[] { "Special Rate includes stay ", promotion.MinimumStay, " get ", promotion.Get, "free on every night!" });
					}
				Label2:
					roomModel = description;
				}
			}
			else
			{
				promotionType = promotion.Cancelation;
				if (promotionType.HasValue)
				{
					IRepositoryAsync<CancellationPolicy> repositoryAsync2 = this._cancellationPolicyControlRepository;
					promotionType = promotion.Cancelation;
					description.Cancelation = repositoryAsync2.GetCancellationPolicyById(promotionType.Value).Description;
				}
				promotionType = promotion.DiscountType;
				if (promotionType.HasValue)
				{
					valueOrDefault = promotionType.GetValueOrDefault();
					switch (valueOrDefault)
					{
						case 1:
						{
							get = promotion.Get;
							if ((!get.HasValue ? false : description.SellingRate > decimal.Zero))
							{
								decimal num2 = description.SellingRate;
								decimal sellingRate3 = description.SellingRate;
								get = promotion.Get;
								description.SellingRate = num2 - ((sellingRate3 * (decimal)get.Value) / new decimal(100));
							}
							description.PromotionText = string.Concat("Special Rate includes ", promotion.Get, "% discount!");
							goto Label4;
						}
						case 2:
						{
							description.PromotionText = string.Concat("Get $", promotion.Get, " discount per booking!");
							goto Label4;
						}
						case 3:
						{
							applyOn = promotion.ApplyOn;
							if (applyOn.HasValue)
							{
								valueOrDefault = applyOn.GetValueOrDefault();
								switch (valueOrDefault)
								{
									case 1:
									{
										description.PromotionText = string.Concat("Get $", promotion.Get, " discount on every night!");
										goto Label5;
									}
									case 2:
									{
										description.PromotionText = string.Concat("Get $", promotion.Get, " discount on first night!");
										goto Label5;
									}
									case 3:
									{
										description.PromotionText = string.Concat("Get $", promotion.Get, " discount on last night!");
										goto Label5;
									}
								}
							}
							description.PromotionText = string.Concat("Get $", promotion.Get, " discount on every night!!");
						Label5:
							RoomModel value1 = description;
							decimal num3 = description.SellingRate;
							get = promotion.Get;
							get = ((get.GetValueOrDefault() > 0 ? get.HasValue : false) ? promotion.Get : new double?(0));
							value1.SellingRate = num3 - (decimal)get.Value;
							goto Label4;
						}
					}
				}
				applyOn = promotion.ApplyOn;
				if (applyOn.HasValue)
				{
					valueOrDefault = applyOn.GetValueOrDefault();
					switch (valueOrDefault)
					{
						case 1:
						{
							description.PromotionText = string.Concat(new object[] { "Special Rate includes stay ", promotion.MinimumStay, " get ", promotion.Get, "free on every night!" });
							goto Label6;
						}
						case 2:
						{
							description.PromotionText = string.Concat(new object[] { "Special Rate includes stay ", promotion.MinimumStay, " get ", promotion.Get, "free on first night!" });
							goto Label6;
						}
						case 3:
						{
							description.PromotionText = string.Concat(new object[] { "Special Rate includes stay ", promotion.MinimumStay, " get ", promotion.Get, "free on last night!" });
							goto Label6;
						}
					}
				}
				description.PromotionText = string.Concat(new object[] { "Special Rate includes stay ", promotion.MinimumStay, " get ", promotion.Get, "free" });
			Label6:
			Label4:
				roomModel = description;
			}
			return roomModel;
		}

		public List<RoomModel> GetPromotions(RoomModel room, int hotelId, int roomId, DateTime checkIn, DateTime checkOut)
		{
			Func<Promotion, RoomModel> func = null;
			DateTime dateTime = checkIn;
			DateTime dateTime1 = checkOut;
			List<RoomModel> roomModels = new List<RoomModel>();
			while (dateTime < dateTime1)
			{
				string str = dateTime.ToString("ddd");
				List<Promotion> list = this._promotionRepository.GetPromotions(hotelId, roomId, checkIn, checkOut, str).ToList<Promotion>();
				if (list.Any<Promotion>())
				{
					List<RoomModel> roomModels1 = roomModels;
					List<Promotion> promotions = list;
					Func<Promotion, RoomModel> func1 = func;
					if (func1 == null)
					{
						Func<Promotion, RoomModel> enRoomModel = (Promotion promotion) => this.GetEnRoomModel(room, promotion, checkIn, checkOut);
						Func<Promotion, RoomModel> func2 = enRoomModel;
						func = enRoomModel;
						func1 = func2;
					}
					roomModels1.AddRange(promotions.Select<Promotion, RoomModel>(func1));
				}
				dateTime = dateTime.AddDays(1);
			}
			List<RoomModel> list1 = (
				from p in roomModels
				orderby p.FinalPrice descending
				select p into x
				group x by x.PromotionId into g
				select g.First<RoomModel>()).ToList<RoomModel>();
			return list1;
		}

		public List<PromotionModel> GetPromotionsList(int id, int language)
		{
			string str;
			string str1;
			int? discountType;
			double? get;
			double num;
			int? applyOn;
			DateTime value;
			string str2;
			string str3;
			object shortDateString;
			object empty;
			object obj;
			object shortDateString1;
			object empty1;
			object obj1;
			object shortDateString2;
			object empty2;
			List<PromotionModel> promotionModels = new List<PromotionModel>();
			foreach (Room list in this._roomRepository.GetRoomList(id).ToList<Room>())
			{
				List<Promotion> promotions = this._promotionRepository.GetPromotionsByHotelIdOrRoomId(id, list.Id).ToList<Promotion>();
				if (promotions.Any<Promotion>())
				{
					List<PromotionModel> promotionModels1 = new List<PromotionModel>();
					if (language != 1)
					{
						foreach (Promotion promotion in promotions)
						{
							discountType = promotion.DiscountType;
							if (discountType.HasValue)
							{
								switch (discountType.GetValueOrDefault())
								{
									case 1:
									{
										str2 = string.Concat("Special Rate includes ", promotion.Get, "% discount!");
										goto Label0;
									}
									case 2:
									{
										str2 = string.Concat("Get $", promotion.Get, " discount per booking!");
										goto Label0;
									}
									case 3:
									{
										applyOn = promotion.ApplyOn;
										if (applyOn.HasValue)
										{
											switch (applyOn.GetValueOrDefault())
											{
												case 1:
												{
													str2 = string.Concat("Get $", promotion.Get, " discount on every night!");
													goto Label0;
												}
												case 2:
												{
													str2 = string.Concat("Get $", promotion.Get, " discount on first night!");
													goto Label0;
												}
												case 3:
												{
													str2 = string.Concat("Get $", promotion.Get, " discount on last night!");
													goto Label0;
												}
											}
										}
										str2 = string.Concat("Get $", promotion.Get, " discount on every night!!");
										goto Label0;
									}
								}
							}
							applyOn = promotion.ApplyOn;
							if (applyOn.HasValue)
							{
								switch (applyOn.GetValueOrDefault())
								{
									case 1:
									{
										str2 = string.Concat(new object[] { "Special Rate includes stay ", promotion.MinimumStay, " get ", promotion.Get, " free on every night!" });
										goto Label2;
									}
									case 2:
									{
										str2 = string.Concat(new object[] { "Special Rate includes stay ", promotion.MinimumStay, " get ", promotion.Get, " free on first night!" });
										goto Label2;
									}
									case 3:
									{
										str2 = string.Concat(new object[] { "Special Rate includes stay ", promotion.MinimumStay, " get ", promotion.Get, " free on last night!" });
										goto Label2;
									}
								}
							}
							str2 = string.Concat(new object[] { "Special Rate includes stay ", promotion.MinimumStay, " get ", promotion.Get, " free" });
						Label2:
						Label0:
							discountType = promotion.PromotionType;
							if ((discountType.GetValueOrDefault() == 243 ? !discountType.HasValue : true))
							{
								if (promotion.BookingDateFrom.HasValue)
								{
									value = promotion.BookingDateFrom.Value;
									shortDateString = value.ToShortDateString();
								}
								else
								{
									shortDateString = string.Empty;
								}
								if (promotion.BookingDateTo.HasValue)
								{
									value = promotion.BookingDateTo.Value;
									empty = value.ToShortDateString();
								}
								else
								{
									empty = string.Empty;
								}
								str3 = string.Format("Booking {0} to {1}", shortDateString, empty);
							}
							else
							{
								object minimumDayAdvance = promotion.MinimumDayAdvance;
								discountType = promotion.MinimumDayAdvance;
								str3 = string.Format("Booking before {0} {1}", minimumDayAdvance, ((discountType.GetValueOrDefault() > 1 ? discountType.HasValue : false) ? "days" : "day"));
							}
							PromotionModel promotionModel = new PromotionModel()
							{
								BookingDate = str3
							};
							if (promotion.CheckIn.HasValue)
							{
								value = promotion.CheckIn.Value;
								obj = value.ToShortDateString();
							}
							else
							{
								obj = string.Empty;
							}
							if (promotion.CheckOut.HasValue)
							{
								value = promotion.CheckOut.Value;
								shortDateString1 = value.ToShortDateString();
							}
							else
							{
								shortDateString1 = string.Empty;
							}
							promotionModel.Stay = string.Format("Stay {0} to {1}", obj, shortDateString1);
							promotionModel.Description = str2;
							promotionModels1.Add(promotionModel);
						}
					}
					else
					{
						foreach (Promotion promotion1 in promotions)
						{
							discountType = promotion1.DiscountType;
							if (discountType.HasValue)
							{
								switch (discountType.GetValueOrDefault())
								{
									case 1:
									{
										str = string.Concat("Giá đặc biệt giảm ", promotion1.Get, "%!");
										goto Label3;
									}
									case 2:
									{
										get = promotion1.Get;
										num = Math.Round((get.HasValue ? get.GetValueOrDefault() : 0) * 22000, 0);
										str = string.Concat("Giá đặc biệt giảm ", num.ToString("#,###").Replace(",", "."), "đ trên từng booking!");
										goto Label3;
									}
									case 3:
									{
										applyOn = promotion1.ApplyOn;
										if (applyOn.HasValue)
										{
											switch (applyOn.GetValueOrDefault())
											{
												case 1:
												{
													get = promotion1.Get;
													num = Math.Round((get.HasValue ? get.GetValueOrDefault() : 0) * 22000, 0);
													str = string.Concat("Giá đặc biệt giảm ", num.ToString("#,###").Replace(",", "."), "đ cho mỗi đêm!");
													goto Label3;
												}
												case 2:
												{
													get = promotion1.Get;
													num = Math.Round((get.HasValue ? get.GetValueOrDefault() : 0) * 22000, 0);
													str = string.Concat("Giá đặc biệt giảm ", num.ToString("#,###").Replace(",", "."), "đ cho đêm đầu tiên!");
													goto Label3;
												}
												case 3:
												{
													get = promotion1.Get;
													num = Math.Round((get.HasValue ? get.GetValueOrDefault() : 0) * 22000, 0);
													str = string.Concat("Giá đặc biệt giảm ", num.ToString("#,###").Replace(",", "."), "đ cho đêm cuối cùng!");
													goto Label3;
												}
											}
										}
										get = promotion1.Get;
										num = Math.Round((get.HasValue ? get.GetValueOrDefault() : 0) * 22000, 0);
										str = string.Concat("Giá đặc biệt giảm ", num.ToString("#,###").Replace(",", "."), "đ cho mỗi đêm!");
										goto Label3;
									}
								}
							}
							applyOn = promotion1.ApplyOn;
							if (applyOn.HasValue)
							{
								switch (applyOn.GetValueOrDefault())
								{
									case 1:
									{
										str = string.Concat(new object[] { "Giá đặc biệt ở ", promotion1.MinimumStay, " đêm, nhận ", promotion1.Get, " đêm miễn phí!" });
										goto Label5;
									}
									case 2:
									{
										str = string.Concat(new object[] { "Giá đặc biệt ở ", promotion1.MinimumStay, " đêm, nhận ", promotion1.Get, " đêm miễn phí cho đêm đầu tiên." });
										goto Label5;
									}
									case 3:
									{
										str = string.Concat(new object[] { "Ở", promotion1.MinimumStay, " đêm, nhận ", promotion1.Get, " đêm miễn phí cho đêm cuối cùng." });
										goto Label5;
									}
								}
							}
							str = string.Concat(new object[] { "Ở ", promotion1.MinimumStay, " đêm, nhận ", promotion1.Get, " đêm miễn phí!" });
						Label5:
						Label3:
							discountType = promotion1.PromotionType;
							if ((discountType.GetValueOrDefault() == 243 ? !discountType.HasValue : true))
							{
								if (promotion1.BookingDateFrom.HasValue)
								{
									value = promotion1.BookingDateFrom.Value;
									empty1 = value.ToShortDateString();
								}
								else
								{
									empty1 = string.Empty;
								}
								if (promotion1.BookingDateTo.HasValue)
								{
									value = promotion1.BookingDateTo.Value;
									obj1 = value.ToShortDateString();
								}
								else
								{
									obj1 = string.Empty;
								}
								str1 = string.Format("Đặt phòng từ {0} đến {1}", empty1, obj1);
							}
							else
							{
								str1 = string.Format("Đặt phòng trước {0} ngày", promotion1.MinimumDayAdvance);
							}
							PromotionModel promotionModel1 = new PromotionModel()
							{
								BookingDate = str1
							};
							if (promotion1.CheckIn.HasValue)
							{
								value = promotion1.CheckIn.Value;
								shortDateString2 = value.ToShortDateString();
							}
							else
							{
								shortDateString2 = string.Empty;
							}
							if (promotion1.CheckOut.HasValue)
							{
								value = promotion1.CheckOut.Value;
								empty2 = value.ToShortDateString();
							}
							else
							{
								empty2 = string.Empty;
							}
							promotionModel1.Stay = string.Format("Ở {0} đến {1}", shortDateString2, empty2);
							promotionModel1.Description = str;
							promotionModels1.Add(promotionModel1);
						}
					}
					promotionModels.Add(new PromotionModel()
					{
						Name = list.Name,
						Promotions = promotionModels1
					});
				}
			}
			return promotionModels;
		}

		public List<RoomModel> GetPromotionsVn(RoomModel room, int hotelId, int roomId, DateTime checkIn, DateTime checkOut)
		{
			Func<Promotion, RoomModel> func = null;
			DateTime dateTime = checkIn;
			DateTime dateTime1 = checkOut;
			List<RoomModel> roomModels = new List<RoomModel>();
			while (dateTime < dateTime1)
			{
				string str = dateTime.ToString("ddd");
				List<Promotion> list = this._promotionRepository.GetVnPromotions(hotelId, roomId, checkIn, checkOut, str).ToList<Promotion>();
				if (list.Any<Promotion>())
				{
					List<RoomModel> roomModels1 = roomModels;
					List<Promotion> promotions = list;
					Func<Promotion, RoomModel> func1 = func;
					if (func1 == null)
					{
						Func<Promotion, RoomModel> roomModel = (Promotion promotion) => this.GetRoomModel(room, promotion, checkIn, checkOut);
						Func<Promotion, RoomModel> func2 = roomModel;
						func = roomModel;
						func1 = func2;
					}
					roomModels1.AddRange(promotions.Select<Promotion, RoomModel>(func1));
				}
				dateTime = dateTime.AddDays(1);
			}
			List<RoomModel> list1 = (
				from p in roomModels
				orderby p.FinalPrice descending
				select p into x
				group x by x.PromotionId into g
				select g.First<RoomModel>()).ToList<RoomModel>();
			return list1;
		}

		public Room GetRoom(int id)
		{
			return this._roomRepository.GetRoom(id);
		}

		public List<Room> GetRoomList(int id)
		{
			List<Room> list = this._roomRepository.GetRoomList(id).ToList<Room>();
			return list;
		}

		public RoomModel GetRoomModel(RoomModel room, Promotion promotion, DateTime checkIn, DateTime checkOut)
		{
			double? get;
			double num;
			int? applyOn;
			RoomModel roomModel;
			double? nullable;
			bool flag;
			bool flag1;
			double? nullable1;
			double? nullable2;
			TimeSpan timeSpan = checkOut - checkIn;
			double totalDays = timeSpan.TotalDays;
			RoomModel descriptionVn = new RoomModel()
			{
				Id = room.Id,
				AdultNumber = room.AdultNumber,
				Name = room.Name,
				ExtraBed = room.ExtraBed,
				ExtraBedPrice = room.ExtraBedPrice,
				ImageUrl = room.ImageUrl,
				ChildrenAge = room.ChildrenAge,
				ChildrenNumber = room.ChildrenNumber,
				RoomSize = room.RoomSize,
				MaxOccupancy = room.MaxOccupancy,
				MaxExtrabed = room.MaxExtrabed,
				RackRate = room.RackRate,
				IsBreakfast = room.IsBreakfast,
				SellingRate = room.SellingRate,
				TotalAvailable = room.TotalAvailable,
				CloseOutRegular = room.CloseOutRegular,
				ViewVn = room.ViewVn,
				RoomFacilities = room.RoomFacilities,
				PromotionId = promotion.Id
			};
			int? promotionType = promotion.PromotionType;
			int valueOrDefault = 244;
			if ((promotionType.GetValueOrDefault() == valueOrDefault ? !promotionType.HasValue : true))
			{
				promotionType = promotion.PromotionType;
				valueOrDefault = 243;
				if ((promotionType.GetValueOrDefault() == valueOrDefault ? !promotionType.HasValue : true))
				{
					flag = false;
				}
				else
				{
					timeSpan = checkIn - DateTime.Now;
					double totalDays1 = timeSpan.TotalDays;
					promotionType = promotion.MinimumDayAdvance;
					if (promotionType.HasValue)
					{
						nullable2 = new double?((double)promotionType.GetValueOrDefault());
					}
					else
					{
						nullable = null;
						nullable2 = nullable;
					}
					get = nullable2;
					flag = (totalDays1 >= get.GetValueOrDefault() ? get.HasValue : false);
				}
				if (!flag)
				{
					promotionType = promotion.PromotionType;
					valueOrDefault = 245;
					if ((promotionType.GetValueOrDefault() == valueOrDefault ? !promotionType.HasValue : true))
					{
						flag1 = false;
					}
					else
					{
						double num1 = totalDays;
						promotionType = promotion.MinimumStay;
						if (promotionType.HasValue)
						{
							nullable1 = new double?((double)promotionType.GetValueOrDefault());
						}
						else
						{
							nullable = null;
							nullable1 = nullable;
						}
						get = nullable1;
						flag1 = (num1 >= get.GetValueOrDefault() ? get.HasValue : false);
					}
					if (!flag1)
					{
						roomModel = new RoomModel();
					}
					else
					{
						promotionType = promotion.Cancelation;
						if (promotionType.HasValue)
						{
							IRepositoryAsync<CancellationPolicy> repositoryAsync = this._cancellationPolicyControlRepository;
							promotionType = promotion.Cancelation;
							descriptionVn.Cancelation = repositoryAsync.GetCancellationPolicyById(promotionType.Value).DescriptionVn;
						}
						promotionType = promotion.DiscountType;
						valueOrDefault = 4;
						if ((promotionType.GetValueOrDefault() == valueOrDefault ? promotionType.HasValue : false))
						{
							promotionType = promotion.ApplyOn;
							if (promotionType.HasValue)
							{
								valueOrDefault = promotionType.GetValueOrDefault();
								if (valueOrDefault == 2)
								{
									descriptionVn.PromotionText = string.Concat(new object[] { "Ở ", promotion.MinimumStay, " đêm, nhận ", promotion.Get, " đêm miễn phí cho đêm đầu tiên!" });
									//goto Label0;
								}
								else
								{
									if (valueOrDefault != 3)
									{
										goto Label8;
									}
									descriptionVn.PromotionText = string.Concat(new object[] { "Ở ", promotion.MinimumStay, " đêm, nhận ", promotion.Get, " đêm miễn phí cho đêm đêm cuối cùng!" });
									//goto Label0;
								}
							}
						Label8:
							object[] minimumStay = new object[] { "Ở ", promotion.MinimumStay, " đêm, nhận ngay ", promotion.Get, " đêm miễn phí." };
							descriptionVn.PromotionText = string.Concat(minimumStay);
						//Label0:
						}
						roomModel = descriptionVn;
					}
				}
				else
				{
					promotionType = promotion.Cancelation;
					if (promotionType.HasValue)
					{
						IRepositoryAsync<CancellationPolicy> repositoryAsync1 = this._cancellationPolicyControlRepository;
						promotionType = promotion.Cancelation;
						descriptionVn.Cancelation = repositoryAsync1.GetCancellationPolicyById(promotionType.Value).DescriptionVn;
					}
					promotionType = promotion.DiscountType;
					if (promotionType.HasValue)
					{
						valueOrDefault = promotionType.GetValueOrDefault();
						switch (valueOrDefault)
						{
							case 1:
							{
								get = promotion.Get;
								if ((!get.HasValue ? false : descriptionVn.SellingRate > decimal.Zero))
								{
									decimal sellingRate = descriptionVn.SellingRate;
									decimal sellingRate1 = descriptionVn.SellingRate;
									get = promotion.Get;
									descriptionVn.SellingRate = sellingRate - ((sellingRate1 * (decimal)get.Value) / new decimal(100));
								}
								descriptionVn.PromotionText = string.Concat("Giá đặc biệt giảm ", promotion.Get, "%!");
								goto Label2;
							}
							case 2:
							{
								RoomModel roomModel1 = descriptionVn;
								get = promotion.Get;
								num = Math.Round((get.HasValue ? get.GetValueOrDefault() : 0) * 22000, 0);
								roomModel1.PromotionText = string.Concat("Giá đặc biệt giảm ", num.ToString("#,###").Replace(",", "."), "đ trên từng booking!");
								goto Label2;
							}
							case 3:
							{
								applyOn = promotion.ApplyOn;
								if (applyOn.HasValue)
								{
									valueOrDefault = applyOn.GetValueOrDefault();
									switch (valueOrDefault)
									{
										case 1:
										{
											RoomModel roomModel2 = descriptionVn;
											get = promotion.Get;
											num = Math.Round((get.HasValue ? get.GetValueOrDefault() : 0) * 22000, 0);
											roomModel2.PromotionText = string.Concat("Giá đặc biệt giảm ", num.ToString("#,###").Replace(",", "."), "đ cho mỗi đêm!");
											decimal sellingRate2 = descriptionVn.SellingRate;
											get = promotion.Get;
											descriptionVn.SellingRate = sellingRate2 - (decimal)get.Value;
											goto Label2;
										}
										case 2:
										{
											RoomModel roomModel3 = descriptionVn;
											get = promotion.Get;
											num = Math.Round((get.HasValue ? get.GetValueOrDefault() : 0) * 22000, 0);
											roomModel3.PromotionText = string.Concat("Giá đặc biệt giảm ", num.ToString("#,###").Replace(",", "."), "đ cho đêm đầu tiên!");
											goto Label2;
										}
										case 3:
										{
											RoomModel roomModel4 = descriptionVn;
											get = promotion.Get;
											num = Math.Round((get.HasValue ? get.GetValueOrDefault() : 0) * 22000, 0);
											roomModel4.PromotionText = string.Concat("Giá đặc biệt giảm ", num.ToString("#,###").Replace(",", "."), "đ cho đêm cuối cùng!");
											goto Label2;
										}
									}
								}
								RoomModel roomModel5 = descriptionVn;
								get = promotion.Get;
								num = Math.Round((get.HasValue ? get.GetValueOrDefault() : 0) * 22000, 0);
								roomModel5.PromotionText = string.Concat("Giá đặc biệt giảm ", num.ToString("#,###").Replace(",", "."), "đ!");
								decimal num2 = descriptionVn.SellingRate;
								get = promotion.Get;
								descriptionVn.SellingRate = num2 - (decimal)get.Value;
								goto Label2;
							}
						}
					}
					applyOn = promotion.ApplyOn;
					if (applyOn.HasValue)
					{
						valueOrDefault = applyOn.GetValueOrDefault();
						switch (valueOrDefault)
						{
							case 1:
							{
								descriptionVn.PromotionText = string.Concat(new object[] { "Giá đặc biệt ở ", promotion.MinimumStay, " đêm, nhận ", promotion.Get, "đêm cho tất cả các đêm!" });
								goto Label4;
							}
							case 2:
							{
								descriptionVn.PromotionText = string.Concat(new object[] { "Giá đặc biệt ở ", promotion.MinimumStay, " đêm, nhận ", promotion.Get, " đêm áp dụng cho đêm đầu tiên !" });
								goto Label4;
							}
							case 3:
							{
								descriptionVn.PromotionText = string.Concat(new object[] { "Giá đặc biệt ở ", promotion.MinimumStay, " đêm, nhận ", promotion.Get, " đêm áp dụng cho đêm cuối cùng." });
								goto Label4;
							}
						}
					}
					descriptionVn.PromotionText = string.Concat(new object[] { "Giá đặc biệt ở ", promotion.MinimumStay, " đêm, nhận ngay ", promotion.Get, "đêm miễn phí." });
				Label4:
				Label2:
					roomModel = descriptionVn;
				}
			}
			else
			{
				promotionType = promotion.Cancelation;
				if (promotionType.HasValue)
				{
					IRepositoryAsync<CancellationPolicy> repositoryAsync2 = this._cancellationPolicyControlRepository;
					promotionType = promotion.Cancelation;
					descriptionVn.Cancelation = repositoryAsync2.GetCancellationPolicyById(promotionType.Value).DescriptionVn;
				}
				promotionType = promotion.DiscountType;
				if (promotionType.HasValue)
				{
					valueOrDefault = promotionType.GetValueOrDefault();
					switch (valueOrDefault)
					{
						case 1:
						{
							get = promotion.Get;
							if ((!get.HasValue ? false : descriptionVn.SellingRate > decimal.Zero))
							{
								decimal sellingRate3 = descriptionVn.SellingRate;
								decimal num3 = descriptionVn.SellingRate;
								get = promotion.Get;
								descriptionVn.SellingRate = sellingRate3 - ((num3 * (decimal)get.Value) / new decimal(100));
							}
							descriptionVn.PromotionText = string.Concat("Giá đặc biệt giảm ", promotion.Get, "%!");
							goto Label5;
						}
						case 2:
						{
							RoomModel roomModel6 = descriptionVn;
							get = promotion.Get;
							num = Math.Round((get.HasValue ? get.GetValueOrDefault() : 0) * 22000, 0);
							roomModel6.PromotionText = string.Concat("Giá đặc biệt giảm ", num.ToString("#,###").Replace(",", "."), "đ trên từng booking!");
							goto Label5;
						}
						case 3:
						{
							applyOn = promotion.ApplyOn;
							if (applyOn.HasValue)
							{
								valueOrDefault = applyOn.GetValueOrDefault();
								switch (valueOrDefault)
								{
									case 1:
									{
										RoomModel roomModel7 = descriptionVn;
										get = promotion.Get;
										num = Math.Round((get.HasValue ? get.GetValueOrDefault() : 0) * 22000, 0);
										roomModel7.PromotionText = string.Concat("Giá đặc biệt giảm ", num.ToString("#,###").Replace(",", "."), "đ cho mỗi đêm!");
										decimal sellingRate4 = descriptionVn.SellingRate;
										get = promotion.Get;
										descriptionVn.SellingRate = sellingRate4 - Utilities.ConvertToDecimal(get.ToString());
										goto Label5;
									}
									case 2:
									{
										RoomModel roomModel8 = descriptionVn;
										get = promotion.Get;
										num = Math.Round((get.HasValue ? get.GetValueOrDefault() : 0) * 22000, 0);
										roomModel8.PromotionText = string.Concat("Giá đặc biệt giảm ", num.ToString("#,###").Replace(",", "."), "đ cho đêm đầu tiên!");
										goto Label5;
									}
									case 3:
									{
										RoomModel roomModel9 = descriptionVn;
										get = promotion.Get;
										num = Math.Round((get.HasValue ? get.GetValueOrDefault() : 0) * 22000, 0);
										roomModel9.PromotionText = string.Concat("Giá đặc biệt giảm ", num.ToString("#,###").Replace(",", "."), "đ cho đêm cuối cùng!");
										goto Label5;
									}
								}
							}
							RoomModel roomModel10 = descriptionVn;
							get = promotion.Get;
							num = Math.Round((get.HasValue ? get.GetValueOrDefault() : 0) * 22000, 0);
							roomModel10.PromotionText = string.Concat("Giá đặc biệt giảm ", num.ToString("#,###").Replace(",", "."), "đ cho mỗi đêm!");
							decimal num4 = descriptionVn.SellingRate;
							get = promotion.Get;
							descriptionVn.SellingRate = num4 - Utilities.ConvertToDecimal(get.ToString());
							goto Label5;
						}
					}
				}
				applyOn = promotion.ApplyOn;
				if (applyOn.HasValue)
				{
					valueOrDefault = applyOn.GetValueOrDefault();
					switch (valueOrDefault)
					{
						case 1:
						{
							descriptionVn.PromotionText = string.Concat(new object[] { "Giá đặc biệt ở ", promotion.MinimumStay, " đêm, nhận ", promotion.Get, " đêm miễn phí!" });
							goto Label7;
						}
						case 2:
						{
							descriptionVn.PromotionText = string.Concat(new object[] { "Giá đặc biệt ở ", promotion.MinimumStay, " đêm, nhận ", promotion.Get, " đêm miễn phí cho đêm đầu tiên." });
							goto Label7;
						}
						case 3:
						{
							descriptionVn.PromotionText = string.Concat(new object[] { "đêm miễn phí! ", promotion.MinimumStay, " đêm, nhận ", promotion.Get, " đêm miễn phí cho đêm cuối cùng." });
							goto Label7;
						}
					}
				}
				descriptionVn.PromotionText = string.Concat(new object[] { "đêm miễn phí! ", promotion.MinimumStay, " đêm, nhận ", promotion.Get, " đêm miễn phí!" });
			Label7:
			Label5:
				roomModel = descriptionVn;
			}
			return roomModel;
		}

		public Room UpdateRoom(Room room)
		{
			this._roomRepository.Update(room);
			return room;
		}
	}
}