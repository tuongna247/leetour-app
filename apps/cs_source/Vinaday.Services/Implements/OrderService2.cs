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
	public class OrderService2 : Service<Order2>, IOrderService2, IService<Order2>
	{
		private readonly IRepositoryAsync<PaymentOrder2> _paymentOrderRepository;

		private readonly IRepositoryAsync<RateExchange> _rateExchangeRepository;

		private readonly IRepositoryAsync<Order2> _orderRepository;

		private readonly IRepositoryAsync<OrderInformation2s> _orderInformationRepository;

		private readonly IRepositoryAsync<Tour> _tourRepository;

		private readonly IRepositoryAsync<Room> _roomRepository;

		private readonly IRepositoryAsync<Customer> _customerRepository;

		private readonly IRepositoryAsync<Medium> _mediaRepository;

		private readonly IRepositoryAsync<Hotel> _hotelRepository;

		private readonly IRepositoryAsync<Nationality> _nationalityRepository;

		private readonly IRepositoryAsync<OrderDetail> _orderDetailRepository;

		private readonly IRepositoryAsync<RoomReguest> _roomReguestRepository;

		private readonly IRepositoryAsync<Notify> _notifyRepository;

		private readonly IRepositoryAsync<Review> _reviewRepository;

		public OrderService2(IRepositoryAsync<Order2> orderRepository, IRepositoryAsync<Tour> tourRepository, IRepositoryAsync<Room> roomRepository, IRepositoryAsync<Customer> customerRepository, IRepositoryAsync<Nationality> nationalityRepository, IRepositoryAsync<Medium> mediaRepository, IRepositoryAsync<OrderDetail> orderDetailRepository, IRepositoryAsync<OrderInformation2s> orderInformationRepository, IRepositoryAsync<RoomReguest> roomReguestRepository, IRepositoryAsync<Notify> notifyRepository, IRepositoryAsync<Review> reviewRepository, IRepositoryAsync<Hotel> hotelRepository, IRepositoryAsync<PaymentOrder2> paymentOrderRepository, IRepositoryAsync<RateExchange> rateExchangeRepository) : base(orderRepository)
		{
			this._orderRepository = orderRepository;
			this._tourRepository = tourRepository;
			this._roomRepository = roomRepository;
			this._customerRepository = customerRepository;
			this._nationalityRepository = nationalityRepository;
			this._mediaRepository = mediaRepository;
			this._orderDetailRepository = orderDetailRepository;
			this._orderInformationRepository = orderInformationRepository;
			this._roomReguestRepository = roomReguestRepository;
			this._notifyRepository = notifyRepository;
			this._reviewRepository = reviewRepository;
			this._paymentOrderRepository = paymentOrderRepository;
			this._rateExchangeRepository = rateExchangeRepository;
			this._hotelRepository = hotelRepository;
		}

		public int Add(Vinaday.Data.Models.Order2 Order2)
		{
			this._orderRepository.Insert(Order2);
			return Order2.Id;
		}

		public DashboardModel GetDashboardItem()
		{
			List<Order2> list = this._orderRepository.GetOrdersMonthly().ToList<Order2>();
			this._orderRepository.GetOrders().ToList<Order2>();
			List<Customer> customers = this._customerRepository.GetCustomers().ToList<Customer>();
			decimal? nullable = (
				from o in list
				where o.Status == 4
				select o).Sum<Order2>((Order2 o) => o.Amount);
			List<RoomReguest> roomReguests = this._roomReguestRepository.GetMonthlyReguests().ToList<RoomReguest>();
			List<RoomReguest> list1 = this._roomReguestRepository.GetRoomReguestsNoneRead().ToList<RoomReguest>();
			List<RoomReguest> roomReguests1 = this._roomReguestRepository.GetReguests().ToList<RoomReguest>();
			int num = (list1.Count > 0 ? list1.Count : 0);
			List<Notify> notifies = this._notifyRepository.GetNotifies().ToList<Notify>();
			DashboardModel dashboardModel = new DashboardModel()
			{
				TotalOrder = list.Count
			};
			decimal? nullable1 = nullable;
			dashboardModel.TotalRevenue = (nullable1.HasValue ? nullable1.GetValueOrDefault() : decimal.Zero);
			dashboardModel.TotalUser = customers.Count;
			dashboardModel.TotalEmail = roomReguests.Count;
			dashboardModel.RoomReguests = list1;
			dashboardModel.TotalEmailNoneRead = num;
			dashboardModel.Emails = roomReguests1.Take<RoomReguest>(10).ToList<RoomReguest>();
			dashboardModel.TotalNotifyNoneRead = notifies.Count;
			dashboardModel.Notifies = notifies;
			return dashboardModel;
		}

		public List<Order2> GetHotelOrders()
		{
			return this._orderRepository.GetHotelOrders().ToList<Order2>();
		}

		public OrderModel GetOrder2(int id)
		{
			Order2 orderByOrderId = this._orderRepository.GetOrder(id) ?? this._orderRepository.GetOrderByOrderId(id);
			IRepositoryAsync<Customer> repositoryAsync = this._customerRepository;
			int? customerId = orderByOrderId.CustomerId;
			Customer customer = repositoryAsync.GetCustomer((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
			List<OrderDetail> list = this._orderDetailRepository.GetOrderDetails(orderByOrderId.TourOrderId).ToList<OrderDetail>();
			Customer nationality = customer;
			IRepositoryAsync<Nationality> repositoryAsync1 = this._nationalityRepository;
			customerId = customer.NationalId;
			nationality.Nationality = repositoryAsync1.GetNationality((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
			IRepositoryAsync<Review> repositoryAsync2 = this._reviewRepository;
			string memberId = customer.MemberId;
			customerId = orderByOrderId.ProductId;
			bool flag = repositoryAsync2.IsReview(memberId, (customerId.HasValue ? customerId.GetValueOrDefault() : -1));
			OrderModel orderModel = new OrderModel()
			{
				Id = orderByOrderId.Id,
				Pnr = orderByOrderId.Pnr,
				OrderId = orderByOrderId.TourOrderId,
				StartDate = orderByOrderId.StartDate.ToString("MM/dd/yyyy"),
				StartDateCompare = orderByOrderId.StartDate,
				EndDate = orderByOrderId.EndDate.ToShortDateString(),
				Status = orderByOrderId.Status,
				CreatedDate = orderByOrderId.CreatedDate.ToShortDateString(),
				Customer = customer,
				Price = orderByOrderId.Price,
				Quantity = orderByOrderId.Quantity,
				CustomerId = orderByOrderId.CustomerId,
				Amount = orderByOrderId.Amount,
				CancelFee = orderByOrderId.CancelFee,
				TotalRefund = orderByOrderId.TotalRefund,
				OrderDetails = list,
				PaymentMethod = orderByOrderId.PaymentMethod,
				CardNumber = orderByOrderId.CardNumber
			};
			customerId = orderByOrderId.ProductId;
			orderModel.ProductId = (customerId.HasValue ? customerId.GetValueOrDefault() : 0);
			orderModel.SpecialRequest = (!string.IsNullOrEmpty(orderByOrderId.SpecialRequest) ? orderByOrderId.SpecialRequest : string.Empty);
			orderModel.Night = orderByOrderId.Night;
			orderModel.CancellationPolicy = orderByOrderId.CancellationPolicy;
			decimal? discount = orderByOrderId.Discount;
			decimal num = new decimal();
			orderModel.Discount = ((discount.GetValueOrDefault() > num ? discount.HasValue : false) ? string.Format("{0}", orderByOrderId.Discount) : "N/A");
			discount = orderByOrderId.TaxFee;
			num = new decimal();
			orderModel.TaxFee = ((discount.GetValueOrDefault() > num ? discount.HasValue : false) ? string.Format("{0}", orderByOrderId.TaxFee) : "N/A");
			discount = orderByOrderId.SurchargeFee;
			num = new decimal();
			orderModel.SurchargeFee = ((discount.GetValueOrDefault() > num ? discount.HasValue : false) ? string.Format("{0}", orderByOrderId.SurchargeFee) : "N/A");
			orderModel.SurchargeName = orderByOrderId.SurchargeName;
			orderModel.ProductLink = orderByOrderId.ProductName;
			orderModel.DiscountName = orderByOrderId.DiscountName;
			discount = orderByOrderId.ExtraBed;
			orderModel.ExtraBed = new decimal?((discount.HasValue ? discount.GetValueOrDefault() : decimal.Zero));
			discount = orderByOrderId.ThirdPersonFee;
			orderModel.ThirdPersonFee = new decimal?((discount.HasValue ? discount.GetValueOrDefault() : decimal.Zero));
			orderModel.IsReview = new bool?(flag);
			orderModel.MemberId = orderByOrderId.MemberId;
			orderModel.ProductName = orderByOrderId.ProductName;
			orderModel.Type = orderByOrderId.Type;
			orderModel.LocalType = orderByOrderId.LocalType;
			orderModel.CardId = orderByOrderId.CardId;
			orderModel.Note = orderByOrderId.Note;
			orderModel.RateExchange = orderByOrderId.RateExchange;
			orderModel.Management = orderByOrderId.Management;
			orderModel.GuestFirstName = orderByOrderId.GuestFirstName;
			orderModel.GuestCountry = orderByOrderId.GuestCountry;
			orderModel.Deposit = orderByOrderId.Deposit;
			orderModel.TourOrderId = orderByOrderId.TourOrderId;
			orderModel.GuestLastName = orderByOrderId.GuestLastName;
			orderModel.IsRead = orderByOrderId.IsRead;
			OrderModel list1 = orderModel;
			list1.OrderInformation2s = this._orderInformationRepository.GetOrderInformation(id).ToList<OrderInformation2s>();
			switch (orderByOrderId.Type)
			{
				case 1:
				{
					IRepositoryAsync<Room> repositoryAsync3 = this._roomRepository;
					customerId = orderByOrderId.ProductId;
					Room room = repositoryAsync3.GetRoom((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
					Medium mediaById = this._mediaRepository.GetMediaById((room != null ? room.Id : 0));
					list1.Avatar = (mediaById != null ? string.Format("https://admin.goreise.com/{0}", mediaById.OriginalPath.Substring(2)) : string.Format("/Content/img/no-image.jpg", new object[0]));
					list1.ProductName = (!string.IsNullOrEmpty(list1.ProductName) || room == null ? list1.ProductName : room.Name);
					break;
				}
				case 2:
				{
					IRepositoryAsync<Tour> repositoryAsync4 = this._tourRepository;
					customerId = orderByOrderId.ProductId;
					Tour tour = repositoryAsync4.GetTour((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
					Medium medium = this._mediaRepository.GetMediaById((tour != null ? tour.Id : 0));
					list1.Avatar = (medium != null ? string.Format("https://admin.goreise.com/{0}", medium.OriginalPath.Substring(2)) : string.Format("/Content/img/no-image.jpg", new object[0]));
					list1.ProductName = (!string.IsNullOrEmpty(list1.ProductName) || tour == null ? list1.ProductName : tour.Name);
					break;
				}
				case 3:
				{
					IRepositoryAsync<Tour> repositoryAsync5 = this._tourRepository;
					customerId = orderByOrderId.ProductId;
					Tour tour1 = repositoryAsync5.GetTour((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
					Medium mediaById1 = this._mediaRepository.GetMediaById((tour1 != null ? tour1.Id : 0));
					list1.Avatar = (mediaById1 != null ? string.Format("https://admin.goreise.com/{0}", mediaById1.OriginalPath.Substring(2)) : string.Format("/Content/img/no-image.jpg", new object[0]));
					break;
				}
			}
			return list1;
		}

		public Order2 GetOrderById(int id)
		{
			Order2 order2 = this._orderRepository.GetOrders().FirstOrDefault<Order2>((Order2 o) => o.Id == id);
			return order2;
		}

		public Order2 GetOrderByOrderId(int id)
		{
			Order2 order2 = this._orderRepository.GetOrders().FirstOrDefault<Order2>((Order2 o) => o.TourOrderId == id);
			return order2;
		}

		public List<OrderModel> GetOrders()
		{
			List<OrderModel> orderModels;
			IEnumerable<Order2> orders = this._orderRepository.GetOrders();
			Order2[] array = orders as Order2[] ?? orders.ToArray<Order2>();
			if (array.Any<Order2>())
			{
				List<OrderModel> orderModels1 = new List<OrderModel>();
				Order2[] order2Array = array;
				for (int i = 0; i < (int)order2Array.Length; i++)
				{
					Order2 order2 = order2Array[i];
					OrderModel orderModel = new OrderModel()
					{
						Id = order2.Id,
						Pnr = order2.Pnr,
						StartDate = order2.StartDate.ToShortDateString(),
						StartDateCompare = order2.StartDate,
						Status = order2.Status,
						Type = order2.Type,
						CreatedDate = order2.CreatedDate.ToShortDateString()
					};
					IRepositoryAsync<Customer> repositoryAsync = this._customerRepository;
					int? customerId = order2.CustomerId;
					orderModel.Customer = repositoryAsync.GetCustomer((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
					orderModel.SpecialRequest = order2.SpecialRequest;
					orderModel.ProductName = order2.ProductName;
					orderModel.Management = order2.Management;
					OrderModel orderModel1 = orderModel;
					switch (order2.Type)
					{
						case 1:
						{
							IRepositoryAsync<Room> repositoryAsync1 = this._roomRepository;
							customerId = order2.ProductId;
							Room room = repositoryAsync1.GetRoom((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
							orderModel1.ProductName = (room != null ? room.Name : string.Empty);
							break;
						}
						case 2:
						{
							IRepositoryAsync<Tour> repositoryAsync2 = this._tourRepository;
							customerId = order2.ProductId;
							Tour tour = repositoryAsync2.GetTour((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
							orderModel1.ProductName = (tour != null ? tour.Name : string.Empty);
							break;
						}
						case 3:
						{
							break;
						}
						default:
						{
							goto case 3;
						}
					}
					orderModels1.Add(orderModel1);
				}
				orderModels = orderModels1;
			}
			else
			{
				orderModels = new List<OrderModel>();
			}
			return orderModels;
		}

		public List<OrderModel> GetOrdersByCustomerId(string custId)
		{
			List<OrderModel> list;
			List<Vinaday.Data.Models.Order2> order2s = this._orderRepository.GetOrdersByCustomerId(custId).ToList<Vinaday.Data.Models.Order2>();
			if (order2s.Count > 0)
			{
				List<OrderModel> orderModels = (
					from Order2 in order2s
					where Order2 != null
					select this.GetOrder2(Order2.Id)).ToList<OrderModel>();
				list = (
					from o in orderModels
					orderby o.CreatedDate
					select o).ToList<OrderModel>();
			}
			else
			{
				list = new List<OrderModel>();
			}
			return list;
		}

		public List<OrderModel> GetOrdersByCustomerId(int custId)
		{
			List<OrderModel> list;
			List<Vinaday.Data.Models.Order2> order2s = this._orderRepository.GetOrdersByCustomerId(custId).ToList<Vinaday.Data.Models.Order2>();
			if (order2s.Count > 0)
			{
				List<OrderModel> orderModels = (
					from Order2 in order2s
					where Order2 != null
					select this.GetOrder2(Order2.Id)).ToList<OrderModel>();
				list = (
					from o in orderModels
					orderby o.CreatedDate
					select o).ToList<OrderModel>();
			}
			else
			{
				list = new List<OrderModel>();
			}
			return list;
		}

		public List<OrderModel> GetOrdersByDate(DateTime startDate, DateTime endDate)
		{
			List<OrderModel> orderModels;
			decimal? nullable;
			decimal? nullable1;
			decimal? nullable2;
			decimal? price;
			decimal? nullable3;
			string str;
			int value;
			IEnumerable<Order2> ordersByDate = this._orderRepository.GetOrdersByDate(startDate, endDate);
			decimal? currentPrice = this._rateExchangeRepository.GetRateExchangeSingle(3).CurrentPrice;
			decimal num = Math.Round((currentPrice.HasValue ? currentPrice.GetValueOrDefault() : decimal.Zero), 0);
			Order2[] array = ordersByDate as Order2[] ?? ordersByDate.ToArray<Order2>();
			if (array.Any<Order2>())
			{
				List<OrderModel> orderModels1 = new List<OrderModel>();
				Order2[] order2Array = array;
				for (int i = 0; i < (int)order2Array.Length; i++)
				{
					Order2 order2 = order2Array[i];
					decimal? amount = order2.Amount;
					decimal? extraBed = order2.ExtraBed;
					decimal num1 = (extraBed.HasValue ? extraBed.GetValueOrDefault() : decimal.Zero);
					if (amount.HasValue)
					{
						nullable = new decimal?(amount.GetValueOrDefault() + num1);
					}
					else
					{
						extraBed = null;
						nullable = extraBed;
					}
					decimal? discount = nullable;
					amount = order2.ThirdPersonFee;
					decimal num2 = (amount.HasValue ? amount.GetValueOrDefault() : decimal.Zero);
					if (discount.HasValue)
					{
						nullable1 = new decimal?(discount.GetValueOrDefault() + num2);
					}
					else
					{
						amount = null;
						nullable1 = amount;
					}
					currentPrice = nullable1;
					discount = order2.Discount;
					decimal num3 = (discount.HasValue ? discount.GetValueOrDefault() : decimal.Zero);
					if (currentPrice.HasValue)
					{
						nullable2 = new decimal?(currentPrice.GetValueOrDefault() - num3);
					}
					else
					{
						discount = null;
						nullable2 = discount;
					}
					decimal? nullable4 = nullable2;
					PaymentOrder2 paymentOrderByOrderId = this._paymentOrderRepository.GetPaymentOrderByOrderId(order2.Id);
					IRepositoryAsync<Customer> repositoryAsync = this._customerRepository;
					int? customerId = order2.CustomerId;
					Customer customer = repositoryAsync.GetCustomer((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
					if (customer != null)
					{
						OrderModel orderModel = new OrderModel()
						{
							Id = order2.Id,
							Pnr = order2.Pnr,
							StartDate = order2.StartDate.ToShortDateString(),
							StartDateCompare = order2.StartDate,
							Status = order2.Status,
							Type = order2.Type,
							OrderId = order2.TourOrderId,
							CreatedDate = order2.CreatedDate.ToShortDateString(),
							Customer = customer,
							SpecialRequest = order2.SpecialRequest,
							ProductName = order2.ProductName,
							Management = (!string.IsNullOrEmpty(order2.Management) ? order2.Management : "admin")
						};
						currentPrice = order2.Price;
						num3 = new decimal();
						if ((currentPrice.GetValueOrDefault() > num3 ? currentPrice.HasValue : false))
						{
							price = order2.Price;
						}
						else
						{
							num3 = new decimal();
							price = new decimal?(num3);
						}
						orderModel.Price = price;
						orderModel.EndDate = order2.EndDate.ToShortDateString();
						currentPrice = nullable4;
						num3 = new decimal();
						if ((currentPrice.GetValueOrDefault() > num3 ? currentPrice.HasValue : false))
						{
							nullable3 = nullable4;
						}
						else
						{
							num3 = new decimal();
							nullable3 = new decimal?(num3);
						}
						orderModel.Total = nullable3;
						currentPrice = order2.Discount;
						num3 = new decimal();
						if ((currentPrice.GetValueOrDefault() > num3 ? currentPrice.HasValue : false))
						{
							currentPrice = order2.Discount;
							str = currentPrice.ToString();
						}
						else
						{
							str = "0";
						}
						orderModel.Discount = str;
						num3 = new decimal();
						orderModel.Balance = new decimal?(num3);
						num3 = new decimal();
						orderModel.Deposit = new decimal?(num3);
						orderModel.Quantity = order2.Quantity;
						orderModel.IsRead = order2.IsRead;
						orderModel.OutCome = new decimal?(Math.Round((paymentOrderByOrderId != null ? paymentOrderByOrderId.Outcome : decimal.Zero), 0));
						orderModel.RateExchange = new decimal?(num);
						orderModel.FullName = string.Format("{0} {1}", customer.Firstname, customer.Lastname);
						orderModel.InCome = new decimal?(Math.Round((paymentOrderByOrderId != null ? paymentOrderByOrderId.Income : decimal.Zero), 0));
						OrderModel empty = orderModel;
						switch (order2.Type)
						{
							case 1:
							{
								IRepositoryAsync<Room> repositoryAsync1 = this._roomRepository;
								customerId = order2.ProductId;
								Room room = repositoryAsync1.GetRoom((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
								IRepositoryAsync<Hotel> repositoryAsync2 = this._hotelRepository;
								if (room != null)
								{
									customerId = room.HotelId;
									value = customerId.Value;
								}
								else
								{
									value = -1;
								}
								Hotel hotelSingle = repositoryAsync2.GetHotelSingle(value);
								if ((hotelSingle != null ? false : room == null))
								{
									empty.ProductName = string.Empty;
								}
								else
								{
									if (hotelSingle != null)
									{
										empty.ProductName = hotelSingle.Name;
									}
									if (room != null)
									{
										OrderModel orderModel1 = empty;
										orderModel1.ProductName = string.Concat(orderModel1.ProductName, " - ", room.Name);
									}
								}
								break;
							}
							case 2:
							{
								IRepositoryAsync<Tour> repositoryAsync3 = this._tourRepository;
								customerId = order2.ProductId;
								Tour tour = repositoryAsync3.GetTour((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
								empty.ProductName = (tour != null ? tour.Name : string.Empty);
								break;
							}
							case 3:
							{
								break;
							}
							default:
							{
								goto case 3;
							}
						}
						orderModels1.Add(empty);
					}
				}
				orderModels = orderModels1;
			}
			else
			{
				orderModels = new List<OrderModel>();
			}
			return orderModels;
		}

		public List<OrderModel> GetOrdersByUserName(string userName)
		{
			List<OrderModel> orderModels;
			IEnumerable<Order2> ordersByUserName = this._orderRepository.GetOrdersByUserName(userName);
			Order2[] array = ordersByUserName as Order2[] ?? ordersByUserName.ToArray<Order2>();
			if (array.Any<Order2>())
			{
				List<OrderModel> orderModels1 = new List<OrderModel>();
				Order2[] order2Array = array;
				for (int i = 0; i < (int)order2Array.Length; i++)
				{
					Order2 order2 = order2Array[i];
					OrderModel orderModel = new OrderModel()
					{
						Id = order2.Id,
						Pnr = order2.Pnr,
						StartDate = order2.StartDate.ToShortDateString(),
						StartDateCompare = order2.StartDate,
						Status = order2.Status,
						Type = order2.Type,
						CreatedDate = order2.CreatedDate.ToShortDateString()
					};
					IRepositoryAsync<Customer> repositoryAsync = this._customerRepository;
					int? customerId = order2.CustomerId;
					orderModel.Customer = repositoryAsync.GetCustomer((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
					orderModel.SpecialRequest = order2.SpecialRequest;
					orderModel.ProductName = order2.ProductName;
					orderModel.Management = order2.Management;
					OrderModel orderModel1 = orderModel;
					switch (order2.Type)
					{
						case 1:
						{
							IRepositoryAsync<Room> repositoryAsync1 = this._roomRepository;
							customerId = order2.ProductId;
							Room room = repositoryAsync1.GetRoom((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
							orderModel1.ProductName = (room != null ? room.Name : string.Empty);
							break;
						}
						case 2:
						{
							IRepositoryAsync<Tour> repositoryAsync2 = this._tourRepository;
							customerId = order2.ProductId;
							Tour tour = repositoryAsync2.GetTour((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
							orderModel1.ProductName = (tour != null ? tour.Name : string.Empty);
							break;
						}
						case 3:
						{
							break;
						}
						default:
						{
							goto case 3;
						}
					}
					orderModels1.Add(orderModel1);
				}
				orderModels = orderModels1;
			}
			else
			{
				orderModels = new List<OrderModel>();
			}
			return orderModels;
		}

		public List<OrderModel> GetOrdersByUserNameDate(string userName, DateTime startDate, DateTime endDate)
		{
			List<OrderModel> orderModels;
			decimal? nullable;
			decimal? nullable1;
			decimal? nullable2;
			decimal? price;
			decimal? nullable3;
			string str;
			IEnumerable<Order2> ordersByUserDate = this._orderRepository.GetOrdersByUserDate(userName, 4, startDate, endDate);
			decimal? currentPrice = this._rateExchangeRepository.GetRateExchangeSingle(3).CurrentPrice;
			decimal num = Math.Round((currentPrice.HasValue ? currentPrice.GetValueOrDefault() : decimal.Zero), 0);
			Order2[] array = ordersByUserDate as Order2[] ?? ordersByUserDate.ToArray<Order2>();
			if (array.Any<Order2>())
			{
				List<OrderModel> orderModels1 = new List<OrderModel>();
				Order2[] order2Array = array;
				for (int i = 0; i < (int)order2Array.Length; i++)
				{
					Order2 order2 = order2Array[i];
					decimal? amount = order2.Amount;
					decimal? extraBed = order2.ExtraBed;
					decimal num1 = (extraBed.HasValue ? extraBed.GetValueOrDefault() : decimal.Zero);
					if (amount.HasValue)
					{
						nullable = new decimal?(amount.GetValueOrDefault() + num1);
					}
					else
					{
						extraBed = null;
						nullable = extraBed;
					}
					decimal? discount = nullable;
					amount = order2.ThirdPersonFee;
					decimal num2 = (amount.HasValue ? amount.GetValueOrDefault() : decimal.Zero);
					if (discount.HasValue)
					{
						nullable1 = new decimal?(discount.GetValueOrDefault() + num2);
					}
					else
					{
						amount = null;
						nullable1 = amount;
					}
					currentPrice = nullable1;
					discount = order2.Discount;
					decimal num3 = (discount.HasValue ? discount.GetValueOrDefault() : decimal.Zero);
					if (currentPrice.HasValue)
					{
						nullable2 = new decimal?(currentPrice.GetValueOrDefault() - num3);
					}
					else
					{
						discount = null;
						nullable2 = discount;
					}
					decimal? nullable4 = nullable2;
					PaymentOrder2 paymentOrderByOrderId = this._paymentOrderRepository.GetPaymentOrderByOrderId(order2.Id);
					IRepositoryAsync<Customer> repositoryAsync = this._customerRepository;
					int? customerId = order2.CustomerId;
					Customer customer = repositoryAsync.GetCustomer((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
					OrderModel orderModel = new OrderModel()
					{
						Id = order2.Id,
						Pnr = order2.Pnr,
						StartDate = order2.StartDate.ToShortDateString(),
						StartDateCompare = order2.StartDate,
						Status = order2.Status,
						Type = order2.Type,
						CreatedDate = order2.CreatedDate.ToShortDateString(),
						Customer = customer,
						SpecialRequest = order2.SpecialRequest,
						ProductName = order2.ProductName,
						Management = (!string.IsNullOrEmpty(order2.Management) ? order2.Management : "admin")
					};
					currentPrice = order2.Price;
					num3 = new decimal();
					if ((currentPrice.GetValueOrDefault() > num3 ? currentPrice.HasValue : false))
					{
						price = order2.Price;
					}
					else
					{
						num3 = new decimal();
						price = new decimal?(num3);
					}
					orderModel.Price = price;
					orderModel.EndDate = order2.EndDate.ToShortDateString();
					currentPrice = nullable4;
					num3 = new decimal();
					if ((currentPrice.GetValueOrDefault() > num3 ? currentPrice.HasValue : false))
					{
						nullable3 = nullable4;
					}
					else
					{
						num3 = new decimal();
						nullable3 = new decimal?(num3);
					}
					orderModel.Total = nullable3;
					currentPrice = order2.Discount;
					num3 = new decimal();
					if ((currentPrice.GetValueOrDefault() > num3 ? currentPrice.HasValue : false))
					{
						currentPrice = order2.Discount;
						str = currentPrice.ToString();
					}
					else
					{
						str = "0";
					}
					orderModel.Discount = str;
					num3 = new decimal();
					orderModel.Balance = new decimal?(num3);
					num3 = new decimal();
					orderModel.Deposit = new decimal?(num3);
					orderModel.Quantity = order2.Quantity;
					orderModel.OutCome = new decimal?(Math.Round((paymentOrderByOrderId != null ? paymentOrderByOrderId.Outcome : decimal.Zero), 0));
					orderModel.RateExchange = new decimal?(num);
					orderModel.FullName = string.Format("{0} {1}", customer.Firstname, customer.Lastname);
					orderModel.InCome = new decimal?(Math.Round((paymentOrderByOrderId != null ? paymentOrderByOrderId.Income : decimal.Zero), 0));
					OrderModel orderModel1 = orderModel;
					int type = order2.Type;
					if (type == 1)
					{
						IRepositoryAsync<Room> repositoryAsync1 = this._roomRepository;
						customerId = order2.ProductId;
						Room room = repositoryAsync1.GetRoom((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
						orderModel1.ProductName = (room != null ? room.Name : string.Empty);
					}
					else if (type == 2)
					{
						IRepositoryAsync<Tour> repositoryAsync2 = this._tourRepository;
						customerId = order2.ProductId;
						Tour tour = repositoryAsync2.GetTour((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
						orderModel1.ProductName = (tour != null ? tour.Name : string.Empty);
					}
					orderModels1.Add(orderModel1);
				}
				orderModels = orderModels1;
			}
			else
			{
				orderModels = new List<OrderModel>();
			}
			return orderModels;
		}

		public List<Order2> GetOrdersForTest()
		{
			return this._orderRepository.GetOrders().ToList<Order2>();
		}

        public OrderModel GetOrderVietnam(int id)
        {
            var order = _orderRepository.GetOrderByOrderId(id);
            if (order == null)
            {
                order = _orderRepository.GetOrder(id);
            }
            var customer = _customerRepository.GetCustomer(order.CustomerId ?? 0);
            var orderDetail = _orderDetailRepository.GetOrderDetails(order.Id).ToList();
            customer.Nationality =
                _nationalityRepository.GetNationality(customer.NationalId ?? 0);
            var isReview = _reviewRepository.IsReview(customer.MemberId, order.ProductId ?? -1);
            var orderModel = new OrderModel
            {

                Id = order.Id,
                Pnr = order.Pnr,
                StartDate = order.StartDate.ToString("MM/dd/yyyy"),
                StartDateCompare = order.StartDate,
                EndDate = order.EndDate.ToShortDateString(),
                Status = order.Status,
                CreatedDate = order.CreatedDate.ToShortDateString(),
                Customer = customer,
                Price = order.Price ,
                Quantity = order.Quantity,
                CustomerId = order.CustomerId,
                Amount = order.Amount ,
                OrderDetails = orderDetail,
                PaymentMethod = order.PaymentMethod,
                CardNumber = order.CardNumber,
                ProductId = order.ProductId ?? 0,
                SpecialRequest = !string.IsNullOrEmpty(order.SpecialRequest) ? order.SpecialRequest : string.Empty,
                Night = order.Night,
                CancellationPolicy = order.CancellationPolicy,
                CancelFee = order.CancelFee * order.RateExchange,
                TotalRefund = order.TotalRefund * order.RateExchange,
                Discount = order.Discount > 0 ? $"{order.Discount}" : "N/A",
                TaxFee = order.TaxFee > 0 ? $"{order.TaxFee}" : "N/A",
                SurchargeFee = order.SurchargeFee > 0 ? $"{order.SurchargeFee }" : "N/A",
                SurchargeName = order.SurchargeName,
                ProductLink = order.ProductName,
                DiscountName = order.DiscountName,
                ExtraBed = (order.ExtraBed * order.RateExchange ?? 0) ,
                ThirdPersonFee = (order.ThirdPersonFee ?? 0) ,
                IsReview = isReview,
                MemberId = order.MemberId,
                ProductName = order.ProductName,
                Type = order.Type,
                LocalType = order.LocalType,
                CardId = order.CardId,
                Note = order.Note,
                OrderInformation2s = _orderInformationRepository.GetOrderInformation(id).ToList(),
                RateExchange = order.RateExchange,
                TourOrderId = order.TourOrderId,
                Management = order.Management

            };
            switch (order.Type)
            {
                case (int)Utilities.ProductType.Hotel:
                    {
                        var room = _roomRepository.GetRoom(order.ProductId ?? 0);
                        var media = _mediaRepository.GetMediaById(room != null ? room.Id : 0);
                        orderModel.Avatar = media != null
                            ? string.Format("https://admin.goreise.com/{0}", media.OriginalPath.Substring(2))
                            : string.Format("/Content/img/no-image.jpg");
                        orderModel.ProductName = (string.IsNullOrEmpty(orderModel.ProductName) && room != null) ? room.Name : orderModel.ProductName;
                    }
                    break;
                case (int)Utilities.ProductType.Tour:
                    {
                        var tour = _tourRepository.GetTour(order.ProductId ?? 0);
                        var media = _mediaRepository.GetMediaById(tour != null ? tour.Id : 0);
                        orderModel.Avatar = media != null
                            ? string.Format("https://admin.goreise.com/{0}", media.OriginalPath.Substring(2))
                            : string.Format("/Content/img/no-image.jpg");
                        //orderModel.ProductName = (string.IsNullOrEmpty(orderModel.ProductName) && tour != null) ? tour.Name : orderModel.ProductName;
                        orderModel.ProductName = (string.IsNullOrEmpty(orderModel.ProductName) && tour != null) ? tour.Name : orderModel.ProductName;
                    }
                    break;
                case (int)Utilities.ProductType.Other:
                    {
                        var tour = _tourRepository.GetTour(order.ProductId ?? 0);
                        var media = _mediaRepository.GetMediaById(tour != null ? tour.Id : 0);
                        orderModel.Avatar = media != null
                            ? string.Format("https://admin.goreise.com/{0}", media.OriginalPath.Substring(2))
                            : string.Format("/Content/img/no-image.jpg");
                        // orderModel.ProductName = tour != null ? tour.Name : string.Empty;
                    }
                    break;
                default:
                    {
                        // var tour = _tourRepository.GetTour(Order2.ProductId ?? 0);
                        // orderModel.ProductName = tour != null ? tour.Name : string.Empty;
                    }
                    break;
            }
            return orderModel;
        }

        public List<OrderModel> GetPaidOrders()
		{
			List<OrderModel> orderModels;
			IEnumerable<Order2> ordersByStatus = this._orderRepository.GetOrdersByStatus(4);
			Order2[] array = ordersByStatus as Order2[] ?? ordersByStatus.ToArray<Order2>();
			if (array.Any<Order2>())
			{
				List<OrderModel> orderModels1 = new List<OrderModel>();
				Order2[] order2Array = array;
				for (int i = 0; i < (int)order2Array.Length; i++)
				{
					Order2 order2 = order2Array[i];
					OrderModel orderModel = new OrderModel()
					{
						Id = order2.Id,
						Pnr = order2.Pnr,
						StartDate = order2.StartDate.ToShortDateString(),
						StartDateCompare = order2.StartDate,
						Status = order2.Status,
						Type = order2.Type,
						CreatedDate = order2.CreatedDate.ToShortDateString()
					};
					IRepositoryAsync<Customer> repositoryAsync = this._customerRepository;
					int? customerId = order2.CustomerId;
					orderModel.Customer = repositoryAsync.GetCustomer((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
					orderModel.SpecialRequest = order2.SpecialRequest;
					orderModel.ProductName = order2.ProductName;
					orderModel.Management = order2.Management;
					orderModel.Price = order2.Price;
					orderModel.EndDate = order2.EndDate.ToShortDateString();
					OrderModel orderModel1 = orderModel;
					switch (order2.Type)
					{
						case 1:
						{
							IRepositoryAsync<Room> repositoryAsync1 = this._roomRepository;
							customerId = order2.ProductId;
							Room room = repositoryAsync1.GetRoom((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
							orderModel1.ProductName = (room != null ? room.Name : string.Empty);
							break;
						}
						case 2:
						{
							IRepositoryAsync<Tour> repositoryAsync2 = this._tourRepository;
							customerId = order2.ProductId;
							Tour tour = repositoryAsync2.GetTour((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
							orderModel1.ProductName = (tour != null ? tour.Name : string.Empty);
							break;
						}
						case 3:
						{
							break;
						}
						default:
						{
							goto case 3;
						}
					}
					orderModels1.Add(orderModel1);
				}
				orderModels = orderModels1;
			}
			else
			{
				orderModels = new List<OrderModel>();
			}
			return orderModels;
		}

		public List<Order2> GetTourOrders()
		{
			return this._orderRepository.GetTourOrders().ToList<Order2>();
		}
	}
}