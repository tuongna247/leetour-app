using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;
using Vinaday.Data.Models.Extention;

namespace Vinaday.Services
{
	public class HotelService : Service<Hotel>, IHotelService, IService<Hotel>
	{
		private readonly IRepositoryAsync<Hotel> _hotelRepository;

		private readonly IRepositoryAsync<Contact> _contactRepository;

		private readonly IRepositoryAsync<HotelFeatureds> _featuredHotelRepository;

		private readonly IRepositoryAsync<Room> _roomRepository;

		private readonly IRepositoryAsync<RoomControl> _roomControlRepository;

		private readonly IRepositoryAsync<HotelImages> _hotelImagesRepository;

		private readonly IRepositoryAsync<Booking> _bookingRepository;

		private readonly IRepositoryAsync<HotelCancellation> _hotelCancellationRepository;

		private readonly IRepositoryAsync<CancellationPolicy> _cancellationPolicyRepository;

		private readonly IRepositoryAsync<Review> _reviewRepository;

		private readonly IRepositoryAsync<City> _cityRepository;

		private readonly IRepositoryAsync<Country> _countryRepository;

		private readonly IRepositoryAsync<CatDetail> _catDetailRepository;

		private readonly IRepositoryAsync<CategoryDetail> _categoryDetailRepository;

		private readonly IRepositoryAsync<ReviewDetail> _reviewDetailRepository;

		private readonly IRepositoryAsync<Region1> _regionRepository;

		public HotelService(IRepositoryAsync<Hotel> hotelRepository, IRepositoryAsync<HotelFeatureds> featuredHotelRepository, IRepositoryAsync<Room> roomRepository, IRepositoryAsync<RoomControl> roomControlRepository, IRepositoryAsync<HotelImages> hotelImagesRepository, IRepositoryAsync<Booking> bookingRepository, IRepositoryAsync<HotelCancellation> hotelCancellationRepository, IRepositoryAsync<CancellationPolicy> cancellationPolicyRepository, IRepositoryAsync<Review> reviewRepository, IRepositoryAsync<City> cityRepository, IRepositoryAsync<Country> countryRepository, IRepositoryAsync<CatDetail> catDetailRepository, IRepositoryAsync<Contact> contactRepository, IRepositoryAsync<ReviewDetail> reviewDetailRepository, IRepositoryAsync<Region1> regionRepository, IRepositoryAsync<CategoryDetail> categoryDetailRepository) : base(hotelRepository)
		{
			this._hotelRepository = hotelRepository;
			this._featuredHotelRepository = featuredHotelRepository;
			this._roomRepository = roomRepository;
			this._roomControlRepository = roomControlRepository;
			this._hotelImagesRepository = hotelImagesRepository;
			this._bookingRepository = bookingRepository;
			this._hotelCancellationRepository = hotelCancellationRepository;
			this._cancellationPolicyRepository = cancellationPolicyRepository;
			this._reviewRepository = reviewRepository;
			this._cityRepository = cityRepository;
			this._countryRepository = countryRepository;
			this._catDetailRepository = catDetailRepository;
			this._contactRepository = contactRepository;
			this._reviewDetailRepository = reviewDetailRepository;
			this._regionRepository = regionRepository;
			this._categoryDetailRepository = categoryDetailRepository;
		}

		public Hotel Add(Hotel hotel)
		{
			this._hotelRepository.Insert(hotel);
			return hotel;
		}

		public Task<Hotel> FindAsync(string id)
		{
			return this._hotelRepository.FindAsync(new object[] { id });
		}

		public string GenerateSlugHotelKey(Hotel hotel)
		{
			string empty;
			string name;
			if (hotel != null)
			{
				string str = string.Empty;
				string str1 = string.Format("[star-{0}],", hotel.StartRating);
				int? regionId = hotel.RegionId;
				Region1 region = this._regionRepository.GetRegion((regionId.HasValue ? regionId.GetValueOrDefault() : -1));
				if (!string.IsNullOrEmpty(hotel.HotelStyle))
				{
					string[] strArrays = hotel.HotelStyle.Split(new char[] { ',' });
					for (int i = 0; i < strArrays.Count<string>(); i++)
					{
						int num = Utilities.ConvertToInt(strArrays[i]);
						CategoryDetail categoryDetail = this._categoryDetailRepository.GetCategoryDetail(num);
						if (categoryDetail != null)
						{
							name = categoryDetail.Name;
						}
						else
						{
							name = null;
						}
						if (!string.IsNullOrEmpty(name))
						{
							str = string.Concat(str, string.Format("[the-collection-{0}],", Utilities.GenerateSlug(categoryDetail.Name, 100)));
						}
					}
				}
				string str2 = string.Concat(str1, str);
				if (region != null)
				{
					str2 = string.Concat(str2, string.Format("[region-{0}]", Utilities.GenerateSlug(region.Name, 100)));
				}
				empty = str2;
			}
			else
			{
				empty = string.Empty;
			}
			return empty;
		}

		public List<HotelSearchModel> GetAllHotelByCityId(int id)
		{
			List<Hotel> list = this._hotelRepository.GetHotelByCityId(id, 2).ToList<Hotel>();
			List<HotelSearchModel> hotelSearchModels = list.Select<Hotel, HotelSearchModel>((Hotel hotel) => {
				HotelSearchModel hotelSearchModel = new HotelSearchModel()
				{
					Id = hotel.Id,
					Name = (!string.IsNullOrEmpty(hotel.HotelNameLocal) ? hotel.HotelNameLocal : hotel.Name)
				};
				int? startRating = hotel.StartRating;
				hotelSearchModel.Star = (startRating.HasValue ? startRating.GetValueOrDefault() : 0);
				hotelSearchModel.Description = hotel.HotelNameLocalDesc;
				hotelSearchModel.City = hotel.CITY;
				hotelSearchModel.Facilities = hotel.HotelFacilities;
				hotelSearchModel.ReviewModel = this.GetReviewsByHotelId(hotel.Id, 1);
				hotelSearchModel.Address = hotel.StreetAddressLocal;
				hotelSearchModel.HotelUrl = string.Format("/{0}/{1}/{2}", hotel.Country.ToLower(), hotel.Id, Utilities.GenerateSlug(hotel.Name, 100));
				return hotelSearchModel;
			}).ToList<HotelSearchModel>();
			return hotelSearchModels;
		}

		public string GetCancellationHotel(int id)
		{
			string str;
			string empty = string.Empty;
			HotelCancellation hotelCancellationPolicyById = this._hotelCancellationRepository.GetHotelCancellationPolicyById(id, 2);
			if (hotelCancellationPolicyById != null)
			{
				CancellationPolicy cancellationPolicyById = this._cancellationPolicyRepository.GetCancellationPolicyById(hotelCancellationPolicyById.CancellationID);
				if (cancellationPolicyById != null)
				{
					empty = cancellationPolicyById.Description;
				}
				str = empty;
			}
			else
			{
				str = empty;
			}
			return str;
		}

		public string GetCancellationHotelVn(int id)
		{
			string str;
			string empty = string.Empty;
			HotelCancellation hotelCancellationPolicyById = this._hotelCancellationRepository.GetHotelCancellationPolicyById(id, 2);
			if (hotelCancellationPolicyById != null)
			{
				CancellationPolicy cancellationPolicyById = this._cancellationPolicyRepository.GetCancellationPolicyById(hotelCancellationPolicyById.CancellationID);
				if (cancellationPolicyById != null)
				{
					empty = cancellationPolicyById.DescriptionVn;
				}
				str = empty;
			}
			else
			{
				str = empty;
			}
			return str;
		}

		public List<City> GetCities(int id)
		{
			List<City> list = this._cityRepository.GetCities(id).ToList<City>();
			return list;
		}

		public List<HotelSearchModel> GetCities()
		{
			List<City> list = this._cityRepository.GetCities().ToList<City>();
			List<HotelSearchModel> hotelSearchModels = (
				from city in list
				let country = this._countryRepository.GetCountry(city.CountryId)
				let hotelCount = this._hotelRepository.CountHotelByCityId(city.CityId, 2)
				select new HotelSearchModel()
				{
					Name = city.Description,
					NameVn = city.Name,
					Description = (country != null ? country.NameVn : string.Empty),
					Count = hotelCount
				}).ToList<HotelSearchModel>();
			return hotelSearchModels;
		}

        public List<HotelSearchModel> GetCitieByLuxyHotels()
        {
            List<City> list = this._cityRepository.GetCities().ToList<City>();
            List<HotelSearchModel> hotelSearchModels = (
                from city in list
                let country = this._countryRepository.GetCountry(city.CountryId)
                let hotelCount = this._hotelRepository.CountLuxuryHotelByCityId(city.CityId, 2)
                select new HotelSearchModel()
                {
                    Name = city.Description,
                    NameVn = city.Name,
                    Description = (country != null ? country.NameVn : string.Empty),
                    Count = hotelCount
                }).ToList<HotelSearchModel>();
            return hotelSearchModels;
        }

        public List<HotelSearchModel> GetCitiesEn()
		{
			List<City> list = this._cityRepository.GetCities().ToList<City>();
			List<HotelSearchModel> hotelSearchModels = (
				from city in list
				let country = this._countryRepository.GetCountry(city.CountryId)
				let hotelCount = this._hotelRepository.CountLuxuryHotelByCityId(city.CityId, 2)
				select new HotelSearchModel()
				{
					CityId = city.CityId,
					Name = city.Description,
					NameVn = city.Name,
					Description = (country != null ? country.Name : string.Empty),
					Country = (country != null ? country.Description : string.Empty),
					Count = hotelCount
				}).ToList<HotelSearchModel>();
			return hotelSearchModels;
		}

		public City GetCityByName(string name)
		{
			name = name.Replace(" ", "").ToLower();
			return this._cityRepository.GetCityByName(name);
		}

		public City GetCityIdByName(string name)
		{
			name = name.Replace("-", "").ToLower();
			return this._cityRepository.GetCityByName(name);
		}

		public Contact GetContact(int id)
		{
			return this._contactRepository.GetContact(id);
		}

		public List<CancellationModel> GetHotelCancellationPoliciesByHotelId(int id)
		{
			IEnumerable<HotelCancellation> hotelCancellationPolicys = this._hotelCancellationRepository.GetHotelCancellationPolicys(id, 2);
			List<CancellationModel> cancellationModels = new List<CancellationModel>();
			HotelCancellation[] array = hotelCancellationPolicys as HotelCancellation[] ?? hotelCancellationPolicys.ToArray<HotelCancellation>();
			if (array.Any<HotelCancellation>())
			{
				cancellationModels.AddRange(
					from hotelCanceltionPolicy in (IEnumerable<HotelCancellation>)array
					where hotelCanceltionPolicy != null
					let canceltionPolicy = this._cancellationPolicyRepository.GetCancellationPolicyById(hotelCanceltionPolicy.CancellationID)
					where canceltionPolicy != null
					select new CancellationModel()
					{
						Id = hotelCanceltionPolicy.ID,
						CancellationId = canceltionPolicy.Id,
						Name = canceltionPolicy.Name,
						Description = canceltionPolicy.Description,
						DescriptionVn = canceltionPolicy.DescriptionVn,
						CheckInFrom = hotelCanceltionPolicy.CheckInFrom,
						CheckOutTo = hotelCanceltionPolicy.CheckOutTo
					});
			}
			return cancellationModels;
		}

		public List<HotelModel> GetHotelFeatured()
		{
            var featureds = _featuredHotelRepository.GetFeaturedAscendingList();
            return (from featured in featureds
                    let hotel = GetHotelSingle(featured.HotelId)
                    where hotel != null
                    let roomControl = GetRoom(featured.HotelId)
                    let image = _hotelImagesRepository.GetImageSingleByHotelId(hotel.Id)
                    select new HotelModel
                    {
                        Id = hotel.Id,
                        Name = hotel.Name,
                        ImageUrl = image != null ? !string.IsNullOrEmpty(image.ImageThumbnail) ? "https://admin.vinaday.com" + $"{image.ImageThumbnail.Substring(1)}-original.png" : "/Content/images/demo/general/no-image.jpg" : "/Content/images/demo/general/no-image.jpg",
                        Star = hotel.StartRating ?? 0,
                        Description = hotel.Description,
                        City = hotel.CITY,
                        HotelUrl = string.Format("http://hotel.vinaday.com/{0}/{1}/{2}", hotel.Country.ToLower(), hotel.Id, Utilities.GenerateSlug(hotel.Name)),
                        Price = roomControl != null ? Math.Round((roomControl.SellingRate), 2).ToString(CultureInfo.InvariantCulture) : "0",
                        PricePromotion = roomControl != null ? Math.Round((roomControl.SellingRate / 1.15m), 2).ToString(CultureInfo.InvariantCulture) : "0"
                    }).ToList();
           
        }

		public List<Hotel> GetHotels()
		{
			List<Hotel> list = this._hotelRepository.GetHotels(2).ToList<Hotel>();
			return list;
		}

		public Hotel GetHotelSingle(int id)
		{
			return this._hotelRepository.GetHotelSingle(id);
		}

		public Hotel GetHotelSingleByHotelName(string name)
		{
			return this._hotelRepository.GetHotelSingleByHotelName(name);
		}

		public Hotel GetHotelSingleByHotelNameVn(string name)
		{
			return this._hotelRepository.GetHotelSingleByHotelNameVn(name);
		}

		public Hotel GetHotelSingleByRoomId(int id)
		{
			Hotel hotelSingle;
			Room room = this._roomRepository.GetRoom(id);
			if (room != null)
			{
				IRepositoryAsync<Hotel> repositoryAsync = this._hotelRepository;
				int? hotelId = room.HotelId;
				hotelSingle = repositoryAsync.GetHotelSingle((hotelId.HasValue ? hotelId.GetValueOrDefault() : 0));
			}
			else
			{
				hotelSingle = new Hotel();
			}
			return hotelSingle;
		}

		public HotelImages GetImage(int id)
		{
			return this._hotelImagesRepository.GetImageSingle(id);
		}

		public List<HotelImages> GetImagesesByHotelId(int id)
		{
			List<HotelImages> list = this._hotelImagesRepository.GetImageListByHotelId(id).ToList<HotelImages>();
			return list;
		}

		public ReviewModel GetReviewsByHotelId(int hotelId, int language)
		{
            var reviews = _reviewRepository.GetReviews(hotelId).ToList();
            if (reviews.Count <= 0) return new ReviewModel();
            float sum = reviews.Sum(t => t.RatingValue);
            float total = reviews.Count;
            var avgScore = sum / total;
            string title;
            ReviewModel reviewModel;
            ReviewDetailModel reviewDetailModel = new ReviewDetailModel();
            //Get review detail 
            List<ReviewDetail> reviewDetails = new List<ReviewDetail>();
            foreach (var review in reviews)
            {
                if (review == null) continue;
                var reviewDetailTemp = _reviewDetailRepository.GetReviews(review.Id);
                reviewDetails.AddRange(reviewDetailTemp);

            }
            if (reviewDetails.Count > 0)
            {
                var service = reviewDetails.Where(r => r.CategoryDetailId == 209);
                var serviceTemp = service as ReviewDetail[] ?? service.ToArray();
                reviewDetailModel.Service = serviceTemp.Any() ? (serviceTemp.Sum(r => r.Value) / serviceTemp.Count()) : 0;

                var room = reviewDetails.Where(r => r.CategoryDetailId == 210);
                var roomTemp = room as ReviewDetail[] ?? room.ToArray();
                reviewDetailModel.Room = roomTemp.Any() ? (roomTemp.Sum(r => r.Value) / roomTemp.Count()) : 0;

                var value = reviewDetails.Where(r => r.CategoryDetailId == 211);
                var valueTemp = value as ReviewDetail[] ?? value.ToArray();
                reviewDetailModel.Value = valueTemp.Any() ? (valueTemp.Sum(r => r.Value) / valueTemp.Count()) : 0;

                var sleepQuality = reviewDetails.Where(r => r.CategoryDetailId == 212);
                var sleepQualityTemp = sleepQuality as ReviewDetail[] ?? sleepQuality.ToArray();
                reviewDetailModel.SleepQuality = sleepQualityTemp.Any() ? (sleepQualityTemp.Sum(r => r.Value) / sleepQualityTemp.Count()) : 0;

                var location = reviewDetails.Where(r => r.CategoryDetailId == 214);
                var locationTemp = location as ReviewDetail[] ?? location.ToArray();
                reviewDetailModel.Location = locationTemp.Any() ? (locationTemp.Sum(r => r.Value) / locationTemp.Count()) : 0;

                var breakfast = reviewDetails.Where(r => r.CategoryDetailId == 214);
                var breakfastTemp = breakfast as ReviewDetail[] ?? breakfast.ToArray();
                reviewDetailModel.Breakfast = breakfastTemp.Any() ? (breakfastTemp.Sum(r => r.Value) / breakfastTemp.Count()) : 0;

                var spa = reviewDetails.Where(r => r.CategoryDetailId == 216);
                var spaTemp = spa as ReviewDetail[] ?? spa.ToArray();
                reviewDetailModel.Spa = spaTemp.Any() ? (spaTemp.Sum(r => r.Value) / spaTemp.Count()) : 0;
            }
            if (language == 1)
            {
                if (avgScore >= 1 && avgScore < 6)
                {
                    title = "Kinh khủng!";
                }
                else if (avgScore >= 6 && avgScore < 7)
                {
                    title = "Trung bình!";

                }
                else if (avgScore >= 7 && avgScore < 8)
                {
                    title = "Tốt!";

                }
                else if (avgScore >= 8 && avgScore < 9)
                {
                    title = "Rất Tốt!";
                }
                else
                {
                    title = "Tuyệt vời!";
                }
                reviewModel = new ReviewModel
                {
                    Title = $"{title}<span itemprop='ratingValue'>{avgScore.ToString("#.#")}</span>/<span itemprop='bestRating'>10</span><meta content='1' itemprop='worstRating'>",
                    ScoreText = title,
                    Score = avgScore.ToString("#.#"),
                    Content = $"{total}",
                    Reviews = reviews,
                    ReviewDetail = reviewDetailModel
                };
            }
            else
            {
                if (avgScore >= 1 && avgScore < 6)
                {
                    title = "Terrible!";
                }
                else if (avgScore >= 6 && avgScore < 7)
                {
                    title = "Average!";

                }
                else if (avgScore >= 7 && avgScore < 8)
                {
                    title = "Good!";

                }
                else if (avgScore >= 8 && avgScore < 9)
                {
                    title = "Very good!";
                }
                else
                {
                    title = "Excellent!";
                }
                reviewModel = new ReviewModel
                {
                    //Title = $"{title} {avgScore.ToString("#.#")}/10",
                    Title = $"{title}<span itemprop='ratingValue'>{avgScore.ToString("#.#")}</span>/<span itemprop='bestRating'>10</span><meta content='1' itemprop='worstRating'>",
                    ScoreText = title,
                    Score = avgScore.ToString("#.#"),
                    Content = $"{total}",
                    Reviews = reviews,
                    ReviewDetail = reviewDetailModel
                };
            }

            return reviewModel;
        }

        public ReviewModel GetReviewsByHotelIdForSearch(int hotelId, int language)
        {

            var reviews = _reviewRepository.GetReviews(hotelId).ToList();
            if (reviews.Count <= 0) return new ReviewModel();
            float sum = reviews.Sum(t => t.RatingValue);
            float total = reviews.Count;
            var avgScore = sum / total;
            string title;
            ReviewModel reviewModel;

            if (language == 1)
            {
                if (avgScore >= 1 && avgScore < 6)
                {
                    title = "Kinh khủng!";
                }
                else if (avgScore >= 6 && avgScore < 7)
                {
                    title = "Trung bình!";

                }
                else if (avgScore >= 7 && avgScore < 8)
                {
                    title = "Tốt!";

                }
                else if (avgScore >= 8 && avgScore < 9)
                {
                    title = "Rất Tốt!";
                }
                else
                {
                    title = "Tuyệt vời!";
                }
                reviewModel = new ReviewModel
                {
                    Title = String.Format("{0} {1}/10", title, avgScore.ToString("#.#")),
                    Content =
                        String.Format("{0}", total),
                    Reviews = reviews
                };
            }
            else
            {
                if (avgScore >= 1 && avgScore < 6)
                {
                    title = "Terrible!";
                }
                else if (avgScore >= 6 && avgScore < 7)
                {
                    title = "Average!";

                }
                else if (avgScore >= 7 && avgScore < 8)
                {
                    title = "Good!";

                }
                else if (avgScore >= 8 && avgScore < 9)
                {
                    title = "Very good!";
                }
                else
                {
                    title = "Excellent!";
                }
                reviewModel = new ReviewModel
                {
                    Title = $"{title} {avgScore.ToString("#.#")}/10",
                    ScoreText = title,
                    Score = avgScore.ToString("#.#"),
                    Content = $"{total}",
                    Reviews = reviews
                };
            }

            return reviewModel;
        }

        public RoomModel GetRoom(int hotelId)
		{
			RoomModel roomModel;
			List<RoomModel> roomModels = new List<RoomModel>();
			IEnumerable<Room> roomList = this._roomRepository.GetRoomList(hotelId);
			Room[] array = roomList as Room[] ?? roomList.ToArray<Room>();
			if (array.Any<Room>())
			{
				Room[] roomArray = array;
				for (int i = 0; i < (int)roomArray.Length; i++)
				{
					Room room = roomArray[i];
					RoomModel roomModel1 = new RoomModel()
					{
						Id = room.Id,
						Name = room.Name,
						ImageUrl = room.ImageUrl,
						SellingRate = decimal.Zero
					};
					RoomControl roomControlSingleByIdOrderBySellingRate = this._roomControlRepository.GetRoomControlSingleByIdOrderBySellingRate(room.Id);
					if (roomControlSingleByIdOrderBySellingRate != null)
					{
						RoomModel roomModel2 = roomModel1;
						decimal? sellingRate = roomControlSingleByIdOrderBySellingRate.SellingRate;
						roomModel2.SellingRate = (sellingRate.HasValue ? sellingRate.GetValueOrDefault() : decimal.Zero);
					}
					roomModels.Add(roomModel1);
				}
				roomModel = (
					from r in roomModels
					where r.SellingRate > decimal.Zero
					orderby r.SellingRate
					select r).FirstOrDefault<RoomModel>();
			}
			else
			{
				roomModel = new RoomModel();
			}
			return roomModel;
		}

		public List<Room> GetRoomByHotelId(int id)
		{
			List<Room> list = this._roomRepository.GetRoomList(id).ToList<Room>();
			return list;
		}

		public List<HotelImages> GetRoomImagesByHotelId(int id)
		{
			List<HotelImages> list = this._hotelImagesRepository.GetRoomImageListByHotelId(id).ToList<HotelImages>();
			return list;
		}

		public List<HotelModel> GetSimilarHotel(int cityId, int hotelId, int status, int munberOfList)
		{
			List<Hotel> list = this._hotelRepository.GetHotelSimilars(cityId, hotelId, status, munberOfList).ToList<Hotel>();
			List<HotelModel> hotelModels = (
				from hotelSimilar in list
				select new { hotelSimilar = hotelSimilar, roomControl = this.GetRoom(hotelSimilar.Id) } into variable
				select new { h__TransparentIdentifier0 = variable, image = this._hotelImagesRepository.GetImageSingleByHotelId(variable.hotelSimilar.Id) }).Select((argument1) => {
				decimal num;
				string str;
				string str1;
				HotelModel hotelModel = new HotelModel()
				{
					Id = argument1.h__TransparentIdentifier0.hotelSimilar.Id,
					Name = argument1.h__TransparentIdentifier0.hotelSimilar.Name,
					ImageUrl = (argument1.image != null ? (!string.IsNullOrEmpty(argument1.image.ImageThumbnail) ? string.Concat("https://admin.goreise.com", string.Format("{0}-original.jpg", argument1.image.ImageThumbnail.Substring(1))) : "/Content/images/no-image.jpg") : "/Content/images/no-image.jpg")
				};
				int? startRating = argument1.h__TransparentIdentifier0.hotelSimilar.StartRating;
				hotelModel.Star = (startRating.HasValue ? startRating.GetValueOrDefault() : 0);
				hotelModel.Description = argument1.h__TransparentIdentifier0.hotelSimilar.Description;
				hotelModel.City = argument1.h__TransparentIdentifier0.hotelSimilar.CITY;
				hotelModel.HotelUrl = string.Format("/hotel/{0}-p{1}", Utilities.GenerateSlug(argument1.h__TransparentIdentifier0.hotelSimilar.Name, 100), argument1.h__TransparentIdentifier0.hotelSimilar.Id);
				if (argument1.h__TransparentIdentifier0.roomControl != null)
				{
					num = Math.Round(argument1.h__TransparentIdentifier0.roomControl.SellingRate, 2);
					str = num.ToString(CultureInfo.InvariantCulture);
				}
				else
				{
					str = "0";
				}
				hotelModel.Price = str;
				if (argument1.h__TransparentIdentifier0.roomControl != null)
				{
					num = Math.Round(argument1.h__TransparentIdentifier0.roomControl.SellingRate / new decimal(115, 0, 0, false, 2), 2);
					str1 = num.ToString(CultureInfo.InvariantCulture);
				}
				else
				{
					str1 = "0";
				}
				hotelModel.PricePromotion = str1;
				return hotelModel;
			}).ToList<HotelModel>();
			return hotelModels;
		}

		public List<HotelModel> GetSimilarLuxuryHotel(int cityId, int hotelId, int status, int munberOfList)
		{
			List<Hotel> list = this._hotelRepository.GetHotelLuxurySimilars(cityId, hotelId, status, munberOfList).ToList<Hotel>();
		    list = list.Where(a => a.CollectionValue > 0).ToList();
			List<HotelModel> hotelModels = (
				from hotelSimilar in list
				select new { hotelSimilar = hotelSimilar, roomControl = this.GetRoom(hotelSimilar.Id) } into variable
				select new { h__TransparentIdentifier0 = variable, image = this._hotelImagesRepository.GetImageSingleByHotelId(variable.hotelSimilar.Id) }).Select((argument1) => {
				decimal num;
				string str;
				string str1;
				HotelModel hotelModel = new HotelModel()
				{
					Id = argument1.h__TransparentIdentifier0.hotelSimilar.Id,
					Name = argument1.h__TransparentIdentifier0.hotelSimilar.Name,
					ImageUrl = (argument1.image != null ? (!string.IsNullOrEmpty(argument1.image.ImageThumbnail) ? string.Concat("https://admin.goreise.com", string.Format("{0}-original.jpg", argument1.image.ImageThumbnail.Substring(1))) : "/Content/images/no-image.jpg") : "/Content/images/no-image.jpg")
				};
				int? startRating = argument1.h__TransparentIdentifier0.hotelSimilar.StartRating;
				hotelModel.Star = (startRating.HasValue ? startRating.GetValueOrDefault() : 0);
				hotelModel.Description = argument1.h__TransparentIdentifier0.hotelSimilar.Description;
				hotelModel.City = argument1.h__TransparentIdentifier0.hotelSimilar.CITY;
				hotelModel.HotelUrl = string.Format("/hotel/{0}-p{1}", Utilities.GenerateSlug(argument1.h__TransparentIdentifier0.hotelSimilar.Name, 100), argument1.h__TransparentIdentifier0.hotelSimilar.Id);
				if (argument1.h__TransparentIdentifier0.roomControl != null)
				{
					num = Math.Round(argument1.h__TransparentIdentifier0.roomControl.SellingRate, 2);
					str = num.ToString(CultureInfo.InvariantCulture);
				}
				else
				{
					str = "0";
				}
				hotelModel.Price = str;
				if (argument1.h__TransparentIdentifier0.roomControl != null)
				{
					num = Math.Round(argument1.h__TransparentIdentifier0.roomControl.SellingRate / new decimal(115, 0, 0, false, 2), 2);
					str1 = num.ToString(CultureInfo.InvariantCulture);
				}
				else
				{
					str1 = "0";
				}
				hotelModel.PricePromotion = str1;
				return hotelModel;
			}).ToList<HotelModel>();
			return hotelModels;
		}

		public List<HotelModel> GetSimilarVietnamHotel(int cityId, int hotelId, int status, int munberOfList)
		{
			List<Hotel> list = this._hotelRepository.GetHotelSimilars(cityId, hotelId, status, munberOfList).ToList<Hotel>();
			List<HotelModel> hotelModels = (
				from hotelSimilar in list
				select new { hotelSimilar = hotelSimilar, roomControl = this.GetRoom(hotelSimilar.Id) } into variable
				select new { h__TransparentIdentifier0 = variable, image = this._hotelImagesRepository.GetImageSingleByHotelId(variable.hotelSimilar.Id) }).Select((argument1) => {
				decimal num;
				string str;
				string str1;
				HotelModel hotelModel = new HotelModel()
				{
					Id = argument1.h__TransparentIdentifier0.hotelSimilar.Id,
					Name = (!string.IsNullOrEmpty(argument1.h__TransparentIdentifier0.hotelSimilar.HotelNameLocal) ? argument1.h__TransparentIdentifier0.hotelSimilar.HotelNameLocal : argument1.h__TransparentIdentifier0.hotelSimilar.Name),
					ImageUrl = (argument1.image != null ? (!string.IsNullOrEmpty(argument1.image.ImageThumbnail) ? string.Concat("https://admin.goreise.com", string.Format("{0}-original.jpg", argument1.image.ImageThumbnail.Substring(1))) : "/Content/images/no-image.jpg") : "/Content/images/no-image.jpg")
				};
				int? startRating = argument1.h__TransparentIdentifier0.hotelSimilar.StartRating;
				hotelModel.Star = (startRating.HasValue ? startRating.GetValueOrDefault() : 0);
				hotelModel.Description = argument1.h__TransparentIdentifier0.hotelSimilar.HotelNameLocalDesc;
				hotelModel.City = argument1.h__TransparentIdentifier0.hotelSimilar.CITY;
				hotelModel.HotelUrl = string.Format("/{0}/{1}/{2}", argument1.h__TransparentIdentifier0.hotelSimilar.Country.ToLower(), argument1.h__TransparentIdentifier0.hotelSimilar.Id, Utilities.GenerateSlug(argument1.h__TransparentIdentifier0.hotelSimilar.Name, 100));
				if (argument1.h__TransparentIdentifier0.roomControl != null)
				{
					num = Math.Round(argument1.h__TransparentIdentifier0.roomControl.SellingRate * new decimal(22000), 0);
					str = num.ToString("##,###").Replace(",", ".");
				}
				else
				{
					str = "0";
				}
				hotelModel.Price = str;
				if (argument1.h__TransparentIdentifier0.roomControl != null)
				{
					num = Math.Round((argument1.h__TransparentIdentifier0.roomControl.SellingRate / new decimal(115, 0, 0, false, 2)) * new decimal(22000), 0);
					str1 = num.ToString("##,###").Replace(",", ".");
				}
				else
				{
					str1 = "0";
				}
				hotelModel.PricePromotion = str1;
				return hotelModel;
			}).ToList<HotelModel>();
			return hotelModels;
		}

		public List<HotelModel> GetVietnamHotelFeatured()
		{
			IEnumerable<HotelFeatureds> featuredAscendingList = this._featuredHotelRepository.GetFeaturedAscendingList();
			List<HotelModel> list = (
				from featured in featuredAscendingList
				select new { featured = featured, hotel = this.GetHotelSingle(featured.HotelId) } into variable
				where variable.hotel != null
				select new { h__TransparentIdentifier0 = variable, roomControl = this.GetRoom(variable.featured.HotelId) } into variable1
				select new { h__TransparentIdentifier1 = variable1, image = this._hotelImagesRepository.GetImageSingleByHotelId(variable1.h__TransparentIdentifier0.hotel.Id) }).Select((argument3) => {
				decimal num;
				string str;
				string str1;
				HotelModel hotelModel = new HotelModel()
				{
					Id = argument3.h__TransparentIdentifier1.h__TransparentIdentifier0.hotel.Id,
					Name = (!string.IsNullOrEmpty(argument3.h__TransparentIdentifier1.h__TransparentIdentifier0.hotel.HotelNameLocal) ? argument3.h__TransparentIdentifier1.h__TransparentIdentifier0.hotel.HotelNameLocal : argument3.h__TransparentIdentifier1.h__TransparentIdentifier0.hotel.Name),
					ImageUrl = (argument3.image != null ? (!string.IsNullOrEmpty(argument3.image.ImageThumbnail) ? string.Concat("https://admin.goreise.com", string.Format("{0}-original.jpg", argument3.image.ImageThumbnail.Substring(1))) : "/Content/images/demo/general/no-image.jpg") : "/Content/images/demo/general/no-image.jpg")
				};
				int? startRating = argument3.h__TransparentIdentifier1.h__TransparentIdentifier0.hotel.StartRating;
				hotelModel.Star = (startRating.HasValue ? startRating.GetValueOrDefault() : 0);
				hotelModel.Description = argument3.h__TransparentIdentifier1.h__TransparentIdentifier0.hotel.HotelNameLocalDesc;
				hotelModel.City = argument3.h__TransparentIdentifier1.h__TransparentIdentifier0.hotel.CITY;
				hotelModel.HotelUrl = string.Format("/{0}/{1}/{2}", argument3.h__TransparentIdentifier1.h__TransparentIdentifier0.hotel.Country.ToLower(), argument3.h__TransparentIdentifier1.h__TransparentIdentifier0.hotel.Id, Utilities.GenerateSlug(argument3.h__TransparentIdentifier1.h__TransparentIdentifier0.hotel.Name, 100));
				if (argument3.h__TransparentIdentifier1.roomControl != null)
				{
					num = Math.Round(argument3.h__TransparentIdentifier1.roomControl.SellingRate * new decimal(22000), 0);
					str = num.ToString("##,###").Replace(",", ".");
				}
				else
				{
					str = "0";
				}
				hotelModel.Price = str;
				if (argument3.h__TransparentIdentifier1.roomControl != null)
				{
					num = Math.Round((argument3.h__TransparentIdentifier1.roomControl.SellingRate / new decimal(115, 0, 0, false, 2)) * new decimal(22000), 0);
					str1 = num.ToString("##,###").Replace(",", ".");
				}
				else
				{
					str1 = "0";
				}
				hotelModel.PricePromotion = str1;
				return hotelModel;
			}).ToList<HotelModel>();
			return list;
		}

		public List<HotelModel> HotelBookingRecently(int munberOfList)
		{
			List<HotelModel> list = (
				from g in (
					from booking in this._bookingRepository.GetRecentlyBookings()
					where booking != null
					select booking).Select((Booking booking) => {
					Booking booking1 = booking;
					IRepositoryAsync<Room> repositoryAsync = this._roomRepository;
					int? roomId = booking.RoomId;
					return new { booking = booking1, room = repositoryAsync.GetRoom((roomId.HasValue ? roomId.GetValueOrDefault() : 0)) };
				}).Where((argument0) => argument0.room != null).Select((argument1) => {
					var u003cu003eh_TransparentIdentifier0 = argument1;
					IRepositoryAsync<Hotel> repositoryAsync = this._hotelRepository;
					int? hotelId = argument1.room.HotelId;
					return new { h__TransparentIdentifier0 = u003cu003eh_TransparentIdentifier0, hotel = repositoryAsync.GetHotelSingle((hotelId.HasValue ? hotelId.GetValueOrDefault() : 0)) };
				}).Select((argument2) => {
					TimeSpan? nullable;
					TimeSpan value;
					TimeSpan? nullable1;
					TimeSpan? nullable2;
					object empty;
					TimeSpan? nullable3;
					TimeSpan? nullable4;
					HotelModel hotelModel = new HotelModel()
					{
						Name = argument2.hotel.Name,
						Address = argument2.hotel.StreetAddressEnglish,
						City = argument2.hotel.CITY
					};
					HotelModel hotelModel1 = hotelModel;
					object[] englishName = new object[4];
					DateTime now = DateTime.Now;
					DateTime? date = argument2.h__TransparentIdentifier0.booking.Date;
					if (date.HasValue)
					{
						nullable1 = new TimeSpan?(now - date.GetValueOrDefault());
					}
					else
					{
						nullable = null;
						nullable1 = nullable;
					}
					nullable = nullable1;
					if (nullable.Value.Hours != 0)
					{
						now = DateTime.Now;
						date = argument2.h__TransparentIdentifier0.booking.Date;
						if (date.HasValue)
						{
							nullable2 = new TimeSpan?(now - date.GetValueOrDefault());
						}
						else
						{
							nullable = null;
							nullable2 = nullable;
						}
						nullable = nullable2;
						value = nullable.Value;
						empty = string.Format("{0} hours", value.Hours);
					}
					else
					{
						empty = string.Empty;
					}
					englishName[0] = empty;
					now = DateTime.Now;
					date = argument2.h__TransparentIdentifier0.booking.Date;
					if (date.HasValue)
					{
						nullable3 = new TimeSpan?(now - date.GetValueOrDefault());
					}
					else
					{
						nullable = null;
						nullable3 = nullable;
					}
					nullable = nullable3;
					value = nullable.Value;
					englishName[1] = string.Format("{0} minutes", value.Minutes);
					now = DateTime.Now;
					date = argument2.h__TransparentIdentifier0.booking.Date;
					if (date.HasValue)
					{
						nullable4 = new TimeSpan?(now - date.GetValueOrDefault());
					}
					else
					{
						nullable = null;
						nullable4 = nullable;
					}
					nullable = nullable4;
					value = nullable.Value;
					englishName[2] = string.Format("{0} seconds", value.Seconds);
					englishName[3] = argument2.h__TransparentIdentifier0.room.EnglishName;
					hotelModel1.CreateDate = string.Format("<span> {3} booked {0} {1} {2} ago</span>.", englishName);
					hotelModel.HotelUrl = string.Format("/{0}/{1}/{2}", argument2.hotel.Country.ToLower(), argument2.hotel.Id, Utilities.GenerateSlug(argument2.hotel.Name, 100));
					hotelModel.ImageUrl = argument2.hotel.HotelPicture1;
					hotelModel.Rating = (argument2.hotel.StartRating.HasValue ? argument2.hotel.StartRating.Value : 0);
					return hotelModel;
				}).ToList<HotelModel>()
				group g by g.Name into p
				select p.FirstOrDefault<HotelModel>()).Take<HotelModel>(munberOfList).ToList<HotelModel>();
			return list;
		}

		public Contact InsertContact(Contact contact)
		{
			this._contactRepository.Insert(contact);
			return contact;
		}

		public HotelSearchModel MapHotelToHotelModel(HotelSearchModel hotelModel)
		{
			decimal num;
			string str;
			string str1;
			List<CatDetail> list = this._catDetailRepository.GetCategoriesDetail().ToList<CatDetail>();
			RoomModel room = this.GetRoom(hotelModel.Id);
			List<CatDetail> categoryDetailListItem = Utilities.GetCategoryDetailListItem(hotelModel.Facilities, list);
			HotelImages imageSingleByHotelId = this._hotelImagesRepository.GetImageSingleByHotelId(hotelModel.Id);
			hotelModel.IsWifi = categoryDetailListItem.Find((CatDetail f) => f.Id == 24) != null;
			hotelModel.ParkingAvailable = categoryDetailListItem.Find((CatDetail f) => f.Id == 27) != null;
			hotelModel.IsBreakfast = (room == null ? false : room.IsBreakfast);
			HotelSearchModel hotelSearchModel = hotelModel;
			if (room != null)
			{
				num = Math.Round(room.SellingRate * new decimal(22000), 0);
				str = num.ToString("##,###").Replace(",", ".");
			}
			else
			{
				str = "0";
			}
			hotelSearchModel.Price = str;
			HotelSearchModel hotelSearchModel1 = hotelModel;
			if (room != null)
			{
				num = Math.Round((room.SellingRate / new decimal(115, 0, 0, false, 2)) * new decimal(22000), 0);
				str1 = num.ToString("##,###").Replace(",", ".");
			}
			else
			{
				str1 = "0";
			}
			hotelSearchModel1.PricePromotion = str1;
			hotelModel.ImageUrl = (imageSingleByHotelId != null ? string.Format("https://goreise.com/{0}", imageSingleByHotelId.ImageOrigin.Substring(2)) : "/Content/images/demo/general/no-image.jpg");
			return hotelModel;
		}

		public void Remove(string id)
		{
			this._hotelRepository.Delete(id);
		}

		public override void Update(Hotel hotel)
		{
			this._hotelRepository.Update(hotel);
		}

		public Contact UpdateContact(Contact contact)
		{
			this._contactRepository.Update(contact);
			return contact;
		}

		public List<HotelModel> VietnamHotelBookingRecently(int munberOfList)
		{
			List<HotelModel> list = (
				from g in (
					from booking in this._bookingRepository.GetRecentlyBookings()
					where booking != null
					select booking).Select((Booking booking) => {
					Booking booking1 = booking;
					IRepositoryAsync<Room> repositoryAsync = this._roomRepository;
					int? roomId = booking.RoomId;
					return new { booking = booking1, room = repositoryAsync.GetRoom((roomId.HasValue ? roomId.GetValueOrDefault() : 0)) };
				}).Where((argument0) => argument0.room != null).Select((argument1) => {
					var u003cu003eh_TransparentIdentifier0 = argument1;
					IRepositoryAsync<Hotel> repositoryAsync = this._hotelRepository;
					int? hotelId = argument1.room.HotelId;
					return new { h__TransparentIdentifier0 = u003cu003eh_TransparentIdentifier0, hotel = repositoryAsync.GetHotelSingle((hotelId.HasValue ? hotelId.GetValueOrDefault() : 0)) };
				}).Select((argument2) => {
					TimeSpan? nullable;
					TimeSpan value;
					TimeSpan? nullable1;
					TimeSpan? nullable2;
					object empty;
					TimeSpan? nullable3;
					TimeSpan? nullable4;
					HotelModel hotelModel = new HotelModel()
					{
						Name = (!string.IsNullOrEmpty(argument2.hotel.HotelNameLocal) ? argument2.hotel.HotelNameLocal : argument2.hotel.Name),
						Address = argument2.hotel.StreetAddressLocal,
						City = argument2.hotel.CITY
					};
					HotelModel hotelModel1 = hotelModel;
					object[] name = new object[4];
					DateTime now = DateTime.Now;
					DateTime? date = argument2.h__TransparentIdentifier0.booking.Date;
					if (date.HasValue)
					{
						nullable1 = new TimeSpan?(now - date.GetValueOrDefault());
					}
					else
					{
						nullable = null;
						nullable1 = nullable;
					}
					nullable = nullable1;
					if (nullable.Value.Hours != 0)
					{
						now = DateTime.Now;
						date = argument2.h__TransparentIdentifier0.booking.Date;
						if (date.HasValue)
						{
							nullable2 = new TimeSpan?(now - date.GetValueOrDefault());
						}
						else
						{
							nullable = null;
							nullable2 = nullable;
						}
						nullable = nullable2;
						value = nullable.Value;
						empty = string.Format("{0} giờ", value.Hours);
					}
					else
					{
						empty = string.Empty;
					}
					name[0] = empty;
					now = DateTime.Now;
					date = argument2.h__TransparentIdentifier0.booking.Date;
					if (date.HasValue)
					{
						nullable3 = new TimeSpan?(now - date.GetValueOrDefault());
					}
					else
					{
						nullable = null;
						nullable3 = nullable;
					}
					nullable = nullable3;
					value = nullable.Value;
					name[1] = string.Format("{0} phút", value.Minutes);
					now = DateTime.Now;
					date = argument2.h__TransparentIdentifier0.booking.Date;
					if (date.HasValue)
					{
						nullable4 = new TimeSpan?(now - date.GetValueOrDefault());
					}
					else
					{
						nullable = null;
						nullable4 = nullable;
					}
					nullable = nullable4;
					value = nullable.Value;
					name[2] = string.Format("{0} giây", value.Seconds);
					name[3] = argument2.h__TransparentIdentifier0.booking.Name;
					hotelModel1.CreateDate = string.Format("<span> Đã đặt phòng {3} cách đây {0} {1} {2}.</span>.", name);
					hotelModel.HotelUrl = string.Format("/{0}/{1}/{2}", argument2.hotel.Country.ToLower(), argument2.hotel.Id, Utilities.GenerateSlug(argument2.hotel.Name, 100));
					hotelModel.ImageUrl = argument2.hotel.HotelPicture1;
					hotelModel.Rating = (argument2.hotel.StartRating.HasValue ? argument2.hotel.StartRating.Value : 0);
					return hotelModel;
				}).ToList<HotelModel>()
				group g by g.Name into p
				select p.FirstOrDefault<HotelModel>()).Take<HotelModel>(munberOfList).ToList<HotelModel>();
			return list;
		}
	}
}