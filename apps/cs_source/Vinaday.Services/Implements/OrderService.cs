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
	public class OrderService : Service<Order>, IOrderService, IService<Order>
	{
		private readonly IRepositoryAsync<PaymentOrder> _paymentOrderRepository;

		private readonly IRepositoryAsync<RateExchange> _rateExchangeRepository;

		private readonly IRepositoryAsync<Order> _orderRepository;

		private readonly IRepositoryAsync<OrderInformations> _orderInformationRepository;

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

		public OrderService(IRepositoryAsync<Order> orderRepository, IRepositoryAsync<Tour> tourRepository, IRepositoryAsync<Room> roomRepository, IRepositoryAsync<Customer> customerRepository, IRepositoryAsync<Nationality> nationalityRepository, IRepositoryAsync<Medium> mediaRepository, IRepositoryAsync<OrderDetail> orderDetailRepository, IRepositoryAsync<OrderInformations> orderInformationRepository, IRepositoryAsync<RoomReguest> roomReguestRepository, IRepositoryAsync<Notify> notifyRepository, IRepositoryAsync<Review> reviewRepository, IRepositoryAsync<Hotel> hotelRepository, IRepositoryAsync<PaymentOrder> paymentOrderRepository, IRepositoryAsync<RateExchange> rateExchangeRepository) : base(orderRepository)
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

		public int Add(Order order)
		{
			this._orderRepository.Insert(order);
			return order.Id;
		}

		public DashboardModel GetDashboardItem()
		{
			List<Order> list = this._orderRepository.GetOrdersMonthly().ToList<Order>();
			List<Order> orders = this._orderRepository.GetOrders().ToList<Order>();
			List<Customer> customers = this._customerRepository.GetCustomers().ToList<Customer>();
			decimal? nullable = (
				from o in list
				where o.Status == 4
				select o).Sum<Order>((Order o) => o.Amount);
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
			dashboardModel.Orders = orders;
			dashboardModel.Notifies = notifies;
			return dashboardModel;
		}

		public List<Order> GetHotelOrders()
		{
			return this._orderRepository.GetHotelOrders().ToList<Order>();
		}

		public OrderModel GetOrder(int id)
		{
			Order order = this._orderRepository.GetOrder(id);
			IRepositoryAsync<Customer> repositoryAsync = this._customerRepository;
			int? customerId = order.CustomerId;
			Customer customer = repositoryAsync.GetCustomer((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
			List<OrderDetail> list = this._orderDetailRepository.GetOrderDetails(order.Id).ToList<OrderDetail>();
			Customer nationality = customer;
			IRepositoryAsync<Nationality> repositoryAsync1 = this._nationalityRepository;
			customerId = customer.NationalId;
			nationality.Nationality = repositoryAsync1.GetNationality((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
			IRepositoryAsync<Review> repositoryAsync2 = this._reviewRepository;
			string memberId = customer.MemberId;
			customerId = order.ProductId;
			bool flag = repositoryAsync2.IsReview(memberId, (customerId.HasValue ? customerId.GetValueOrDefault() : -1));
			OrderModel orderModel = new OrderModel()
			{
				Id = order.Id,
				Pnr = order.Pnr,
				StartDate = order.StartDate.ToString("MM/dd/yyyy"),
				StartDateCompare = order.StartDate,
				EndDate = order.EndDate.ToString("MM/dd/yyyy"),
                Status = order.Status,
				CreatedDate = order.CreatedDate.ToString("MM/dd/yyyy"),
				Customer = customer,
				Price = order.Price,
				Quantity = order.Quantity,
				CustomerId = order.CustomerId,
				Amount = order.Amount,
				CancelFee = order.CancelFee,
				TotalRefund = order.TotalRefund,
				OrderDetails = list,
                CouponCode =  order.CouponCode,
				PaymentMethod = order.PaymentMethod,
				CardNumber = order.CardNumber
			};
			customerId = order.ProductId;
		    orderModel.DepartureOption = order.DepartureOption;
            orderModel.GroupType = order.GroupType;
            orderModel.ProductId = (customerId.HasValue ? customerId.GetValueOrDefault() : 0);
			orderModel.SpecialRequest = (!string.IsNullOrEmpty(order.SpecialRequest) ? order.SpecialRequest : string.Empty);
			orderModel.Night = order.Night;
			orderModel.CancellationPolicy = order.CancellationPolicy;
			decimal? discount = order.Discount;
			decimal num = new decimal();
			orderModel.Discount = ((discount.GetValueOrDefault() > num ? discount.HasValue : false) ? string.Format("{0}", order.Discount) : "N/A");
			discount = order.TaxFee;
			num = new decimal();
			orderModel.TaxFee = ((discount.GetValueOrDefault() > num ? discount.HasValue : false) ? string.Format("{0}", order.TaxFee) : "N/A");
			discount = order.SurchargeFee;
			num = new decimal();
			orderModel.SurchargeFee = ((discount.GetValueOrDefault() > num ? discount.HasValue : false) ? string.Format("{0}", order.SurchargeFee) : "N/A");
			orderModel.SurchargeName = order.SurchargeName;
			orderModel.ProductLink = order.ProductName;
			orderModel.DiscountName = order.DiscountName;
			discount = order.ExtraBed;
			orderModel.ExtraBed = new decimal?((discount.HasValue ? discount.GetValueOrDefault() : decimal.Zero));
			discount = order.ThirdPersonFee;
			orderModel.ThirdPersonFee = new decimal?((discount.HasValue ? discount.GetValueOrDefault() : decimal.Zero));
			orderModel.IsReview = new bool?(flag);
			orderModel.MemberId = order.MemberId;
			orderModel.ProductName = order.ProductName;
			orderModel.Type = order.Type;
			orderModel.LocalType = order.LocalType;
			orderModel.CardId = order.CardId;
			orderModel.OrderInformations = this._orderInformationRepository.GetOrderInformation(id).ToList<OrderInformations>();
			orderModel.Note = order.Note;
			orderModel.RateExchange = order.RateExchange;
			orderModel.Management = order.Management;
			orderModel.GuestFirstName = order.GuestFirstName;
			orderModel.GuestCountry = order.GuestCountry;
			orderModel.Deposit = order.Deposit;
			orderModel.IsRead = order.IsRead;
			orderModel.Children = order.Children;
			orderModel.GuestLastName = order.GuestLastName;
			OrderModel orderModel1 = orderModel;
			switch (order.Type)
			{
				case 1:
				{
					IRepositoryAsync<Room> repositoryAsync3 = this._roomRepository;
					customerId = order.ProductId;
					Room room = repositoryAsync3.GetRoom((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
					Medium mediaById = this._mediaRepository.GetMediaById((room != null ? room.Id : 0));
					orderModel1.Avatar = (mediaById != null ? string.Format("https://admin.goreise.com/{0}", mediaById.OriginalPath.Substring(2)) : string.Format("/Content/img/no-image.jpg", new object[0]));
					orderModel1.ProductName = (room != null ? room.Name : string.Empty);
					break;
				}
				case 2:
				{
					IRepositoryAsync<Tour> repositoryAsync4 = this._tourRepository;
					customerId = order.ProductId;
					Tour tour = repositoryAsync4.GetTour((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
					Medium medium = this._mediaRepository.GetMediaById((tour != null ? tour.Id : 0));
					orderModel1.Avatar = (medium != null ? string.Format("https://admin.goreise.com/{0}", medium.OriginalPath.Substring(2)) : string.Format("/Content/img/no-image.jpg", new object[0]));
					orderModel1.ProductName = (tour != null ? tour.Name : string.Empty);
					break;
				}
				case 3:
				{
					IRepositoryAsync<Tour> repositoryAsync5 = this._tourRepository;
					customerId = order.ProductId;
					Tour tour1 = repositoryAsync5.GetTour((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
					Medium mediaById1 = this._mediaRepository.GetMediaById((tour1 != null ? tour1.Id : 0));
					orderModel1.Avatar = (mediaById1 != null ? string.Format("https://admin.goreise.com/{0}", mediaById1.OriginalPath.Substring(2)) : string.Format("/Content/img/no-image.jpg", new object[0]));
					break;
				}
			}
			return orderModel1;
		}

		public Order GetOrderById(int id)
		{
			Order order = this._orderRepository.GetOrders().FirstOrDefault<Order>((Order o) => o.Id == id);
			return order;
		}

		public List<OrderModel> GetOrders()
		{
			List<OrderModel> orderModels;
			IEnumerable<Order> orders = this._orderRepository.GetOrders();
			Order[] array = orders as Order[] ?? orders.ToArray<Order>();
			if (array.Any<Order>())
			{
				List<OrderModel> orderModels1 = new List<OrderModel>();
				Order[] orderArray = array;
				for (int i = 0; i < (int)orderArray.Length; i++)
				{
					Order order = orderArray[i];
					OrderModel orderModel = new OrderModel()
					{
						Id = order.Id,
						Pnr = order.Pnr,
						StartDate = order.StartDate.ToString("MM/dd/yyyy"),
						StartDateCompare = order.StartDate,
						Status = order.Status,
						Type = order.Type,
						CreatedDate = order.CreatedDate.ToString("MM/dd/yyyy")
					};
					IRepositoryAsync<Customer> repositoryAsync = this._customerRepository;
					int? customerId = order.CustomerId;
					orderModel.Customer = repositoryAsync.GetCustomer((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
					orderModel.SpecialRequest = order.SpecialRequest;
					orderModel.ProductName = order.ProductName;
					orderModel.Management = order.Management;
					OrderModel orderModel1 = orderModel;
					switch (order.Type)
					{
						case 1:
						{
							IRepositoryAsync<Room> repositoryAsync1 = this._roomRepository;
							customerId = order.ProductId;
							Room room = repositoryAsync1.GetRoom((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
							orderModel1.ProductName = (room != null ? room.Name : string.Empty);
							break;
						}
						case 2:
						{
							IRepositoryAsync<Tour> repositoryAsync2 = this._tourRepository;
							customerId = order.ProductId;
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
			List<Order> orders = this._orderRepository.GetOrdersByCustomerId(custId).ToList<Order>();
			if (orders.Count > 0)
			{
				List<OrderModel> orderModels = (
					from order in orders
					where order != null
					select this.GetOrder(order.Id)).ToList<OrderModel>();
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
			List<Order> orders = this._orderRepository.GetOrdersByCustomerId(custId).ToList<Order>();
			if (orders.Count > 0)
			{
				List<OrderModel> orderModels = (
					from order in orders
					where order != null
					select this.GetOrder(order.Id)).ToList<OrderModel>();
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

		public List<OrderModel> GetOrdersByDate(DateTime startDate, DateTime endDate, bool searchByCreatedDate)
		{
			List<OrderModel> orderModels;
			decimal? nullable;
			decimal? nullable1;
			decimal? nullable2;
			decimal? price;
			decimal? nullable3;
			string str;
			int value;
			IEnumerable<Order> ordersByDate = this._orderRepository.GetOrdersByDate(startDate, endDate, searchByCreatedDate);
			decimal? currentPrice = this._rateExchangeRepository.GetRateExchangeSingle(3).CurrentPrice;
			decimal num = Math.Round((currentPrice.HasValue ? currentPrice.GetValueOrDefault() : decimal.Zero), 0);
			Order[] array = ordersByDate as Order[] ?? ordersByDate.ToArray<Order>();
			if (array.Any<Order>())
			{
				List<OrderModel> orderModels1 = new List<OrderModel>();
				Order[] orderArray = array;
				for (int i = 0; i < (int)orderArray.Length; i++)
				{
					Order order = orderArray[i];
					decimal? amount = order.Amount;
					decimal? extraBed = order.ExtraBed;
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
					amount = order.ThirdPersonFee;
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
					discount = order.Discount;
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
					PaymentOrder paymentOrderByOrderId = this._paymentOrderRepository.GetPaymentOrderByOrderId(order.Id);
					IRepositoryAsync<Customer> repositoryAsync = this._customerRepository;
					int? customerId = order.CustomerId;
					Customer customer = repositoryAsync.GetCustomer((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
					if (customer != null)
					{
						OrderModel orderModel = new OrderModel()
						{
							Id = order.Id,
							Pnr = order.Pnr,
							StartDate = order.StartDate.ToShortDateString(),
							StartDateCompare = order.StartDate,
							Status = order.Status,
							Type = order.Type,
							CreatedDate = order.CreatedDate.ToString("yyyy/MM/dd hh:mm "),
							Customer = customer,
							SpecialRequest = order.SpecialRequest,
							ProductName = order.ProductName,
							Management = (!string.IsNullOrEmpty(order.Management) ? order.Management : "admin")
						};
						currentPrice = order.Price;
						num3 = new decimal();
						if ((currentPrice.GetValueOrDefault() > num3 ? currentPrice.HasValue : false))
						{
							price = order.Price;
						}
						else
						{
							num3 = new decimal();
							price = new decimal?(num3);
						}
						orderModel.Price = price;
					    orderModel.CouponCode = order.CouponCode;
						orderModel.EndDate = order.EndDate.ToShortDateString();
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
						currentPrice = order.Discount;
						num3 = new decimal();
						if ((currentPrice.GetValueOrDefault() > num3 ? currentPrice.HasValue : false))
						{
							currentPrice = order.Discount;
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
						orderModel.Quantity = order.Quantity;
						orderModel.IsRead = order.IsRead;
						orderModel.OutCome = new decimal?(Math.Round((paymentOrderByOrderId != null ? paymentOrderByOrderId.Outcome : decimal.Zero), 0));
						orderModel.RateExchange = new decimal?(num);
						orderModel.FullName = string.Format("{0} {1}", customer.Firstname, customer.Lastname);
						orderModel.InCome = new decimal?(Math.Round((paymentOrderByOrderId != null ? paymentOrderByOrderId.Income : decimal.Zero), 0));
						OrderModel empty = orderModel;
						switch (order.Type)
						{
							case 1:
							{
								IRepositoryAsync<Room> repositoryAsync1 = this._roomRepository;
								customerId = order.ProductId;
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
								customerId = order.ProductId;
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
			IEnumerable<Order> ordersByUserName = this._orderRepository.GetOrdersByUserName(userName);
			Order[] array = ordersByUserName as Order[] ?? ordersByUserName.ToArray<Order>();
			if (array.Any<Order>())
			{
				List<OrderModel> orderModels1 = new List<OrderModel>();
				Order[] orderArray = array;
				for (int i = 0; i < (int)orderArray.Length; i++)
				{
					Order order = orderArray[i];
					OrderModel orderModel = new OrderModel()
					{
						Id = order.Id,
						Pnr = order.Pnr,
						StartDate = order.StartDate.ToShortDateString(),
						StartDateCompare = order.StartDate,
						Status = order.Status,
						Type = order.Type,
						CreatedDate = order.CreatedDate.ToShortDateString()
					};
					IRepositoryAsync<Customer> repositoryAsync = this._customerRepository;
					int? customerId = order.CustomerId;
					orderModel.Customer = repositoryAsync.GetCustomer((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
					orderModel.SpecialRequest = order.SpecialRequest;
					orderModel.ProductName = order.ProductName;
					orderModel.Management = order.Management;
					OrderModel orderModel1 = orderModel;
					switch (order.Type)
					{
						case 1:
						{
							IRepositoryAsync<Room> repositoryAsync1 = this._roomRepository;
							customerId = order.ProductId;
							Room room = repositoryAsync1.GetRoom((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
							orderModel1.ProductName = (room != null ? room.Name : string.Empty);
							break;
						}
						case 2:
						{
							IRepositoryAsync<Tour> repositoryAsync2 = this._tourRepository;
							customerId = order.ProductId;
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
			IEnumerable<Order> ordersByUserDate = this._orderRepository.GetOrdersByUserDate(userName, 4, startDate, endDate);
			decimal? currentPrice = this._rateExchangeRepository.GetRateExchangeSingle(3).CurrentPrice;
			decimal num = Math.Round((currentPrice.HasValue ? currentPrice.GetValueOrDefault() : decimal.Zero), 0);
			Order[] array = ordersByUserDate as Order[] ?? ordersByUserDate.ToArray<Order>();
			if (array.Any<Order>())
			{
				List<OrderModel> orderModels1 = new List<OrderModel>();
				Order[] orderArray = array;
				for (int i = 0; i < (int)orderArray.Length; i++)
				{
					Order order = orderArray[i];
					decimal? amount = order.Amount;
					decimal? extraBed = order.ExtraBed;
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
					amount = order.ThirdPersonFee;
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
					discount = order.Discount;
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
					PaymentOrder paymentOrderByOrderId = this._paymentOrderRepository.GetPaymentOrderByOrderId(order.Id);
					IRepositoryAsync<Customer> repositoryAsync = this._customerRepository;
					int? customerId = order.CustomerId;
					Customer customer = repositoryAsync.GetCustomer((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
					OrderModel orderModel = new OrderModel()
					{
						Id = order.Id,
						Pnr = order.Pnr,
						StartDate = order.StartDate.ToShortDateString(),
						StartDateCompare = order.StartDate,
						Status = order.Status,
						Type = order.Type,
						CreatedDate = order.CreatedDate.ToShortDateString(),
						Customer = customer,
						SpecialRequest = order.SpecialRequest,
						ProductName = order.ProductName,
						Management = (!string.IsNullOrEmpty(order.Management) ? order.Management : "admin")
					};
					currentPrice = order.Price;
					num3 = new decimal();
					if ((currentPrice.GetValueOrDefault() > num3 ? currentPrice.HasValue : false))
					{
						price = order.Price;
					}
					else
					{
						num3 = new decimal();
						price = new decimal?(num3);
					}
					orderModel.Price = price;
					orderModel.EndDate = order.EndDate.ToShortDateString();
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
					currentPrice = order.Discount;
					num3 = new decimal();
					if ((currentPrice.GetValueOrDefault() > num3 ? currentPrice.HasValue : false))
					{
						currentPrice = order.Discount;
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
					orderModel.Quantity = order.Quantity;
					orderModel.OutCome = new decimal?(Math.Round((paymentOrderByOrderId != null ? paymentOrderByOrderId.Outcome : decimal.Zero), 0));
					orderModel.RateExchange = new decimal?(num);
					orderModel.FullName = string.Format("{0} {1}", customer.Firstname, customer.Lastname);
					orderModel.InCome = new decimal?(Math.Round((paymentOrderByOrderId != null ? paymentOrderByOrderId.Income : decimal.Zero), 0));
					OrderModel orderModel1 = orderModel;
					int type = order.Type;
					if (type == 1)
					{
						IRepositoryAsync<Room> repositoryAsync1 = this._roomRepository;
						customerId = order.ProductId;
						Room room = repositoryAsync1.GetRoom((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
						orderModel1.ProductName = (room != null ? room.Name : string.Empty);
					}
					else if (type == 2)
					{
						IRepositoryAsync<Tour> repositoryAsync2 = this._tourRepository;
						customerId = order.ProductId;
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

		public List<Order> GetOrdersForTest()
		{
			return this._orderRepository.GetOrders().ToList<Order>();
		}

		public OrderModel GetOrderVietnam(int id)
		{
			decimal? nullable;
			decimal? nullable1;
			decimal? nullable2;
			decimal? nullable3;
			decimal? nullable4;
			decimal? nullable5;
			string str;
			decimal? nullable6;
			string str1;
			decimal? nullable7;
			string str2;
			decimal? nullable8;
			decimal? nullable9;
			Order order = this._orderRepository.GetOrder(id);
			IRepositoryAsync<Customer> repositoryAsync = this._customerRepository;
			int? customerId = order.CustomerId;
			Customer customer = repositoryAsync.GetCustomer((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
			List<OrderDetail> list = this._orderDetailRepository.GetOrderDetails(order.Id).ToList<OrderDetail>();
			Customer nationality = customer;
			IRepositoryAsync<Nationality> repositoryAsync1 = this._nationalityRepository;
			customerId = customer.NationalId;
			nationality.Nationality = repositoryAsync1.GetNationality((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
			IRepositoryAsync<Review> repositoryAsync2 = this._reviewRepository;
			string memberId = customer.MemberId;
			customerId = order.ProductId;
			bool flag = repositoryAsync2.IsReview(memberId, (customerId.HasValue ? customerId.GetValueOrDefault() : -1));
			OrderModel orderModel = new OrderModel()
			{
				Id = order.Id,
				Pnr = order.Pnr,
				StartDate = order.StartDate.ToString("MM/dd/yyyy"),
				StartDateCompare = order.StartDate,
				EndDate = order.EndDate.ToString("MM/dd/yyyy"),
				Status = order.Status,
				CreatedDate = order.CreatedDate.ToString("MM/dd/yyyy"),
				Customer = customer
			};
			decimal? price = order.Price;
			decimal? rateExchange = order.RateExchange;
			if (price.HasValue & rateExchange.HasValue)
			{
				nullable1 = new decimal?(price.GetValueOrDefault() * rateExchange.GetValueOrDefault());
			}
			else
			{
				nullable = null;
				nullable1 = nullable;
			}
            orderModel.Deposit = order.Deposit;
			orderModel.Price = nullable1;
			orderModel.Quantity = order.Quantity;
			orderModel.CustomerId = order.CustomerId;
			rateExchange = order.Amount;
			price = order.RateExchange;
			if (rateExchange.HasValue & price.HasValue)
			{
				nullable2 = new decimal?(rateExchange.GetValueOrDefault() * price.GetValueOrDefault());
			}
			else
			{
				nullable = null;
				nullable2 = nullable;
			}
			orderModel.Amount = nullable2;
			orderModel.OrderDetails = list;
			orderModel.PaymentMethod = order.PaymentMethod;
            orderModel.DepartureOption = order.DepartureOption;
            orderModel.GroupType = order.GroupType;
            orderModel.CardNumber = order.CardNumber;
			customerId = order.ProductId;
			orderModel.ProductId = (customerId.HasValue ? customerId.GetValueOrDefault() : 0);
			orderModel.SpecialRequest = (!string.IsNullOrEmpty(order.SpecialRequest) ? order.SpecialRequest : string.Empty);
			orderModel.Night = order.Night;
			orderModel.CancellationPolicy = order.CancellationPolicy;
			price = order.CancelFee;
			rateExchange = order.RateExchange;
			if (price.HasValue & rateExchange.HasValue)
			{
				nullable3 = new decimal?(price.GetValueOrDefault() * rateExchange.GetValueOrDefault());
			}
			else
			{
				nullable = null;
				nullable3 = nullable;
			}
			orderModel.CancelFee = nullable3;
			rateExchange = order.TotalRefund;
			price = order.RateExchange;
			if (rateExchange.HasValue & price.HasValue)
			{
				nullable4 = new decimal?(rateExchange.GetValueOrDefault() * price.GetValueOrDefault());
			}
			else
			{
				nullable = null;
				nullable4 = nullable;
			}
			orderModel.TotalRefund = nullable4;
			price = order.Discount;
			decimal num = new decimal();
			if ((price.GetValueOrDefault() > num ? price.HasValue : false))
			{
				price = order.Discount;
				rateExchange = order.RateExchange;
				if (price.HasValue & rateExchange.HasValue)
				{
					nullable5 = new decimal?(price.GetValueOrDefault() * rateExchange.GetValueOrDefault());
				}
				else
				{
					nullable = null;
					nullable5 = nullable;
				}
				str = string.Format("{0}", nullable5);
			}
			else
			{
				str = "N/A";
			}
			orderModel.Discount = str;
			rateExchange = order.TaxFee;
			num = new decimal();
			if ((rateExchange.GetValueOrDefault() > num ? rateExchange.HasValue : false))
			{
				rateExchange = order.TaxFee;
				price = order.RateExchange;
				if (rateExchange.HasValue & price.HasValue)
				{
					nullable6 = new decimal?(rateExchange.GetValueOrDefault() * price.GetValueOrDefault());
				}
				else
				{
					nullable = null;
					nullable6 = nullable;
				}
				str1 = string.Format("{0}", nullable6);
			}
			else
			{
				str1 = "N/A";
			}
			orderModel.TaxFee = str1;
			price = order.SurchargeFee;
			num = new decimal();
			if ((price.GetValueOrDefault() > num ? price.HasValue : false))
			{
				price = order.SurchargeFee;
				rateExchange = order.RateExchange;
				if (price.HasValue & rateExchange.HasValue)
				{
					nullable7 = new decimal?(price.GetValueOrDefault() * rateExchange.GetValueOrDefault());
				}
				else
				{
					nullable = null;
					nullable7 = nullable;
				}
				str2 = string.Format("{0}", nullable7);
			}
			else
			{
				str2 = "N/A";
			}
			orderModel.SurchargeFee = str2;
			orderModel.SurchargeName = order.SurchargeName;
			orderModel.ProductLink = order.ProductName;
			orderModel.DiscountName = order.DiscountName;
			price = order.ExtraBed;
			num = (price.HasValue ? price.GetValueOrDefault() : decimal.Zero);
			rateExchange = order.RateExchange;
			if (rateExchange.HasValue)
			{
				nullable8 = new decimal?(num * rateExchange.GetValueOrDefault());
			}
			else
			{
				price = null;
				nullable8 = price;
			}
			orderModel.ExtraBed = nullable8;
			price = order.ThirdPersonFee;
			num = (price.HasValue ? price.GetValueOrDefault() : decimal.Zero);
			rateExchange = order.RateExchange;
			if (rateExchange.HasValue)
			{
				nullable9 = new decimal?(num * rateExchange.GetValueOrDefault());
			}
			else
			{
				price = null;
				nullable9 = price;
			}
			orderModel.ThirdPersonFee = nullable9;
			orderModel.IsReview = new bool?(flag);
			orderModel.MemberId = order.MemberId;
			orderModel.ProductName = order.ProductName;
			orderModel.Type = order.Type;
			orderModel.LocalType = order.LocalType;
			orderModel.CardId = order.CardId;
			orderModel.Note = order.Note;
			orderModel.Children = order.Children;
			orderModel.OrderInformations = this._orderInformationRepository.GetOrderInformation(id).ToList<OrderInformations>();
			orderModel.RateExchange = order.RateExchange;
			orderModel.Management = order.Management;
			OrderModel orderModel1 = orderModel;
			switch (order.Type)
			{
				case 1:
				{
					IRepositoryAsync<Room> repositoryAsync3 = this._roomRepository;
					customerId = order.ProductId;
					Room room = repositoryAsync3.GetRoom((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
					Medium mediaById = this._mediaRepository.GetMediaById((room != null ? room.Id : 0));
					orderModel1.Avatar = (mediaById != null ? string.Format("https://admin.goreise.com/{0}", mediaById.OriginalPath.Substring(2)) : string.Format("/Content/img/no-image.jpg", new object[0]));
					orderModel1.ProductName = (room != null ? room.Name : string.Empty);
					break;
				}
				case 2:
				{
					IRepositoryAsync<Tour> repositoryAsync4 = this._tourRepository;
					customerId = order.ProductId;
					Tour tour = repositoryAsync4.GetTour((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
					Medium medium = this._mediaRepository.GetMediaById((tour != null ? tour.Id : 0));
					orderModel1.Avatar = (medium != null ? string.Format("https://admin.goreise.com/{0}", medium.OriginalPath.Substring(2)) : string.Format("/Content/img/no-image.jpg", new object[0]));
					orderModel1.ProductName = (tour != null ? tour.Name : string.Empty);
					break;
				}
				case 3:
				{
					IRepositoryAsync<Tour> repositoryAsync5 = this._tourRepository;
					customerId = order.ProductId;
					Tour tour1 = repositoryAsync5.GetTour((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
					Medium mediaById1 = this._mediaRepository.GetMediaById((tour1 != null ? tour1.Id : 0));
					orderModel1.Avatar = (mediaById1 != null ? string.Format("https://admin.goreise.com/{0}", mediaById1.OriginalPath.Substring(2)) : string.Format("/Content/img/no-image.jpg", new object[0]));
					break;
				}
			}
			return orderModel1;
		}

		public List<OrderModel> GetPaidOrders()
		{
			List<OrderModel> orderModels;
			IEnumerable<Order> ordersByStatus = this._orderRepository.GetOrdersByStatus(4);
			Order[] array = ordersByStatus as Order[] ?? ordersByStatus.ToArray<Order>();
			if (array.Any<Order>())
			{
				List<OrderModel> orderModels1 = new List<OrderModel>();
				Order[] orderArray = array;
				for (int i = 0; i < (int)orderArray.Length; i++)
				{
					Order order = orderArray[i];
					OrderModel orderModel = new OrderModel()
					{
						Id = order.Id,
						Pnr = order.Pnr,
						StartDate = order.StartDate.ToString("MM/dd/yyyy"),
						StartDateCompare = order.StartDate,
						Status = order.Status,
						Type = order.Type,
						CreatedDate = order.CreatedDate.ToString("MM/dd/yyyy")
					};
					IRepositoryAsync<Customer> repositoryAsync = this._customerRepository;
					int? customerId = order.CustomerId;
					orderModel.Customer = repositoryAsync.GetCustomer((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
					orderModel.SpecialRequest = order.SpecialRequest;
					orderModel.ProductName = order.ProductName;
					orderModel.Management = order.Management;
					orderModel.Price = order.Price;
					orderModel.EndDate = order.EndDate.ToString("MM/dd/yyyy");
					OrderModel orderModel1 = orderModel;
					switch (order.Type)
					{
						case 1:
						{
							IRepositoryAsync<Room> repositoryAsync1 = this._roomRepository;
							customerId = order.ProductId;
							Room room = repositoryAsync1.GetRoom((customerId.HasValue ? customerId.GetValueOrDefault() : 0));
							orderModel1.ProductName = (room != null ? room.Name : string.Empty);
							break;
						}
						case 2:
						{
							IRepositoryAsync<Tour> repositoryAsync2 = this._tourRepository;
							customerId = order.ProductId;
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

		public List<Order> GetTourOrders()
		{
			return this._orderRepository.GetTourOrders().ToList<Order>();
		}
	}
}