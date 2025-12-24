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
	public class RoomControlService : Service<RoomControl>, IRoomControlService, IService<RoomControl>
	{
		private readonly IRepositoryAsync<RoomControl> _roomControlRepository;

		private readonly IRepositoryAsync<Surcharge> _surchargeRepository;

		public RoomControlService(IRepositoryAsync<RoomControl> roomControlRepository, IRepositoryAsync<Surcharge> surchargeRepository) : base(roomControlRepository)
		{
			this._roomControlRepository = roomControlRepository;
			this._surchargeRepository = surchargeRepository;
		}

		public RoomControl GetRoomControlByRoomId(int id)
		{
			return this._roomControlRepository.GetRoomControlById(id);
		}

		public List<RoomControlModel> GetRoomControlByRoomId(int roomId, DateTime checkIn, DateTime checkOut, int hotelId)
		{
			DateTime now;
			decimal? nullable;
			decimal? nullable1;
			decimal? nullable2;
			List<RoomControl> roomListCheckInOut = this._roomControlRepository.GetRoomListCheckInOut(roomId, checkIn, checkOut);
			List<RoomControlModel> roomControlModels = new List<RoomControlModel>();
			while (checkIn < checkOut)
			{
				RoomControlModel roomControlModel = new RoomControlModel();
				DateTime dateTime = checkIn;
				RoomControl roomControl = roomListCheckInOut.FirstOrDefault<RoomControl>((RoomControl a) => a.RoomDate.Equals(dateTime));
				if (roomControl == null)
				{
					roomControlModel.Surcharge1 = decimal.Zero;
					roomControlModel.Surcharge2 = decimal.Zero;
					roomControlModel.CompulsoryMeal = decimal.Zero;
					roomControlModel.IsBreakfast = true;
					roomControlModel.Date = checkIn;
					DateTime date = checkIn.Date;
					now = DateTime.Now;
					roomControlModel.IsPast = date < now.Date;
				}
				else
				{
					RoomControlModel closeOutRegular = new RoomControlModel()
					{
						Id = roomControl.Id
					};
					int? autoTopUp = roomControl.AutoTopUp;
					closeOutRegular.AutoTopUp = (autoTopUp.HasValue ? autoTopUp.GetValueOrDefault() : 0);
					closeOutRegular.CloseOutRegular = roomControl.CloseOutRegular;
					closeOutRegular.Date = roomControl.RoomDate;
					autoTopUp = roomControl.Guaranteed;
					closeOutRegular.Guaranteed = (autoTopUp.HasValue ? autoTopUp.GetValueOrDefault() : 0);
					DateTime date1 = roomControl.RoomDate.Date;
					now = DateTime.Now;
					closeOutRegular.IsPast = date1 < now.Date;
					closeOutRegular.Regular = roomControl.Regular;
					closeOutRegular.RoomId = roomControl.RoomId;
					closeOutRegular.TotalAvailable = roomControl.TotalAvailable;
					autoTopUp = roomControl.UseGuaranteed;
					closeOutRegular.UseGuaranteed = (autoTopUp.HasValue ? autoTopUp.GetValueOrDefault() : 0);
					autoTopUp = roomControl.UseRegular;
					closeOutRegular.UseRegular = (autoTopUp.HasValue ? autoTopUp.GetValueOrDefault() : 0);
					decimal? compulsoryMeal = roomControl.CompulsoryMeal;
					closeOutRegular.CompulsoryMeal = (compulsoryMeal.HasValue ? compulsoryMeal.GetValueOrDefault() : decimal.Zero);
					decimal? sellingRate = roomControl.SellingRate;
					decimal? compulsoryMeal1 = roomControl.CompulsoryMeal;
					if (sellingRate.HasValue & compulsoryMeal1.HasValue)
					{
						nullable = new decimal?(sellingRate.GetValueOrDefault() + compulsoryMeal1.GetValueOrDefault());
					}
					else
					{
						nullable = null;
					}
					decimal? nullable3 = nullable;
					decimal? surcharge1 = roomControl.Surcharge1;
					if (nullable3.HasValue & surcharge1.HasValue)
					{
						nullable1 = new decimal?(nullable3.GetValueOrDefault() + surcharge1.GetValueOrDefault());
					}
					else
					{
						compulsoryMeal1 = null;
						nullable1 = compulsoryMeal1;
					}
					decimal? nullable4 = nullable1;
					decimal? surcharge2 = roomControl.Surcharge2;
					if (nullable4.HasValue & surcharge2.HasValue)
					{
						nullable2 = new decimal?(nullable4.GetValueOrDefault() + surcharge2.GetValueOrDefault());
					}
					else
					{
						surcharge1 = null;
						nullable2 = surcharge1;
					}
					compulsoryMeal = nullable2;
					closeOutRegular.FinalPrice = (compulsoryMeal.HasValue ? compulsoryMeal.GetValueOrDefault() : decimal.Zero);
					compulsoryMeal = roomControl.Profit;
					closeOutRegular.Profit = (compulsoryMeal.HasValue ? compulsoryMeal.GetValueOrDefault() : decimal.Zero);
					compulsoryMeal = roomControl.SellingRate;
					closeOutRegular.SellingRate = (compulsoryMeal.HasValue ? compulsoryMeal.GetValueOrDefault() : decimal.Zero);
					compulsoryMeal = roomControl.Surcharge1;
					closeOutRegular.Surcharge1 = (compulsoryMeal.HasValue ? compulsoryMeal.GetValueOrDefault() : decimal.Zero);
					compulsoryMeal = roomControl.Surcharge2;
					closeOutRegular.Surcharge2 = (compulsoryMeal.HasValue ? compulsoryMeal.GetValueOrDefault() : decimal.Zero);
					compulsoryMeal = roomControl.TaRate;
					closeOutRegular.TaRate = (compulsoryMeal.HasValue ? compulsoryMeal.GetValueOrDefault() : decimal.Zero);
					bool? breakfast = roomControl.Breakfast;
					closeOutRegular.IsBreakfast = (breakfast.HasValue ? breakfast.GetValueOrDefault() : false);
					roomControlModel = closeOutRegular;
				}
				roomControlModels.Add(roomControlModel);
				checkIn = checkIn.AddDays(1);
			}
			return roomControlModels;
		}

		public RoomControl GetRoomControlByRoomIdRoomDate(int roomId, DateTime dt)
		{
			RoomControl roomControl = this._roomControlRepository.GetRoomsControlById(roomId).FirstOrDefault<RoomControl>((RoomControl a) => a.RoomDate == dt);
			return roomControl;
		}

		public List<RoomControl> GetRoomControls()
		{
			return this._roomControlRepository.GetRoomControls().ToList<RoomControl>();
		}

		public RoomControl GetRoomControlSingleByIdOrderBySellingRate(int id)
		{
			return this._roomControlRepository.GetRoomControlSingleByIdOrderBySellingRate(id);
		}

		public List<RoomControl> GetRoomListCheckInOut(int roomId, DateTime fromDate, DateTime toDate)
		{
			List<RoomControl> list = this._roomControlRepository.GetRoomListCheckInOut(roomId, fromDate, toDate).ToList<RoomControl>();
			return list;
		}

		public RoomControl GetSingleRoomCheckInOut(int id, DateTime checkIn, DateTime checkOut)
		{
			return this._roomControlRepository.GetRoomSingleCheckInOut(id, checkIn, checkOut);
		}

		public RoomControl GetSingleRoomControlByDateRate(int id, DateTime dt)
		{
			RoomControl roomControl = this._roomControlRepository.GetRoomsControlById(id).FirstOrDefault<RoomControl>((RoomControl a) => (a.RoomDate != dt ? false : a.SellingRate.HasValue));
			return roomControl;
		}

		public List<Surcharge> GetSurchargesByDate(DateTime date, int roomId, int hotelId)
		{
			List<Surcharge> list;
			List<Surcharge> surcharges = this._surchargeRepository.GetSurchargesByRoomId(date, roomId, hotelId).ToList<Surcharge>();
			if (roomId == -1)
			{
				surcharges = this._surchargeRepository.GetSurchargesByHotelId(date, hotelId).ToList<Surcharge>();
			}
			if (surcharges.Count > 0)
			{
				string str = date.ToString("ddd");
				list = (
					from surcharge in surcharges
					where surcharge != null
					let array = surcharge.DateOfWeek.Split(new char[] { ',' })
					where array.Contains<string>(str)
					select surcharge).ToList<Surcharge>();
			}
			else
			{
				list = new List<Surcharge>();
			}
			return list;
		}
	}
}