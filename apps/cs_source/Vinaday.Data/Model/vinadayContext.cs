using Repository.Pattern.Ef6;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models.Mapping;

namespace Vinaday.Data.Models
{
    public class vinadayContext : DataContext
    {
        public DbSet<AccessPermission> AccessPermissions { get; set; }

        public DbSet<AccountantOrderDetails> AccountantOrderDetail { get; set; }

        public DbSet<ADDRESS> ADDRESSes { get; set; }

        public DbSet<Vinaday.Data.Models.aspnet_Applications> aspnet_Applications { get; set; }

        public DbSet<Vinaday.Data.Models.aspnet_Membership> aspnet_Membership { get; set; }

        public DbSet<Vinaday.Data.Models.aspnet_Paths> aspnet_Paths { get; set; }

        public DbSet<Vinaday.Data.Models.aspnet_PersonalizationAllUsers> aspnet_PersonalizationAllUsers { get; set; }

        public DbSet<Vinaday.Data.Models.aspnet_PersonalizationPerUser> aspnet_PersonalizationPerUser { get; set; }

        public DbSet<Vinaday.Data.Models.aspnet_Profile> aspnet_Profile { get; set; }

        public DbSet<Vinaday.Data.Models.aspnet_Roles> aspnet_Roles { get; set; }
        public DbSet<Vinaday.Data.Models.HotelCoupon> HotelCoupon { get; set; }
        public DbSet<Vinaday.Data.Models.CouponCode> CouponCode { get; set; }
        public DbSet<Vinaday.Data.Models.aspnet_Users> aspnet_Users { get; set; }

        public DbSet<BOOKINGHISTORY> BOOKINGHISTORies { get; set; }

        public DbSet<Booking> BOOKINGs { get; set; }


        public DbSet<Vinaday.Data.Models.BusinessCard> BusinessCard { get; set; }

        public DbSet<CancellationPolicy> CancellationPolicies { get; set; }

        public DbSet<CatDetail> CATDETAILs { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Category1> CATEGORies1 { get; set; }

        public DbSet<CategoryDetail> CategoryDetails { get; set; }

        public DbSet<City> Cities { get; set; }
        public DbSet<ViewListTour> ViewListTours { get; set; }

        public DbSet<COMMENT> COMMENTs { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Country> COUNTRies { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }

        public DbSet<CustomerCall> CustomerCalls { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Vinaday.Data.Models.DAYTRIP_IMAGES> DAYTRIP_IMAGES { get; set; }

        public DbSet<DAYTRIPBOOKINGHISTORY> DAYTRIPBOOKINGHISTORies { get; set; }
        public DbSet<DAYTRIPBOOKING> DAYTRIPBOOKINGs { get; set; }
        public DbSet<DayTripRate> DayTripRates { get; set; }
        public DbSet<DAYTRIP> DAYTRIPs { get; set; }

        public DbSet<Vinaday.Data.Models.Deals> Deals { get; set; }

        public DbSet<Vinaday.Data.Models.Detail> Detail { get; set; }

        public DbSet<Vinaday.Data.Models.ExpandRates> ExpandRates { get; set; }

        public DbSet<ExpiredRoom> ExpiredRooms { get; set; }

        public DbSet<Vinaday.Data.Models.Featured> Featured { get; set; }

        public DbSet<HotelImages> HOTEL_IMAGES { get; set; }

        public DbSet<HotelCancellation> HotelCancellations { get; set; }

        public DbSet<Room> HOTELDETAILs { get; set; }

        public DbSet<HOTELFACILITY> HOTELFACILITies { get; set; }
        public DbSet<Vinaday.Data.Models.HotelFeatureds> HotelFeatureds { get; set; }

        public DbSet<HOTELPROMOTION> HOTELPROMOTIONs { get; set; }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<HOTELSURCHARGE> HOTELSURCHARGEs { get; set; }

        public DbSet<IMAGE> IMAGES { get; set; }
        public DbSet<LANGUAGE> LANGUAGEs { get; set; }

        public DbSet<LottoDetail> LottoDetails { get; set; }

        public DbSet<Lotto> Lottos { get; set; }

        public DbSet<Medium> Media { get; set; }

        public DbSet<Nationality> Nationalities { get; set; }

        public DbSet<Notify> Notifies
        {
            get;
            set;
        }

        public DbSet<OrderDetail> OrderDetails
        {
            get;
            set;
        }

        public DbSet<Vinaday.Data.Models.OrderInformation2s> OrderInformation2s
        {
            get;
            set;
        }

        public DbSet<Vinaday.Data.Models.OrderInformations> OrderInformations
        {
            get;
            set;
        }

        public DbSet<Order> Orders
        {
            get;
            set;
        }

        public DbSet<AccountantOrders> OrdersAccountant
        {
            get;
            set;
        }

        public DbSet<PARTNER> PARTNERs
        {
            get;
            set;
        }

        public DbSet<PasswordResetRequest> PasswordResetRequests
        {
            get;
            set;
        }

        public DbSet<Vinaday.Data.Models.PaymentOrder> PaymentOrder
        {
            get;
            set;
        }

        public DbSet<Vinaday.Data.Models.PaymentOrderDetail> PaymentOrderDetail
        {
            get;
            set;
        }

        public DbSet<PERMISSION> PERMISSIONs
        {
            get;
            set;
        }

        public DbSet<ProfileCompany> ProfileCompanies
        {
            get;
            set;
        }

        public DbSet<Promotion> Promotions
        {
            get;
            set;
        }

        public DbSet<PROMOTIONSTAYPAY> PROMOTIONSTAYPAYs
        {
            get;
            set;
        }

        public DbSet<Vinaday.Data.Models.Rate> Rate
        {
            get;
            set;
        }

        public DbSet<RateControl> RateControls
        {
            get;
            set;
        }

        public DbSet<RateExchange> RATEOFEXCHANGEs
        {
            get;
            set;
        }

        public DbSet<RECEIPT> RECEIPTs
        {
            get;
            set;
        }

        public DbSet<Region> Regions
        {
            get;
            set;
        }

        public DbSet<Region1> Regions1
        {
            get;
            set;
        }

        public DbSet<ReviewDetail> ReviewDetails
        {
            get;
            set;
        }

        public DbSet<Review> Reviews
        {
            get;
            set;
        }

        public DbSet<ROLE> ROLEs
        {
            get;
            set;
        }

        public DbSet<Role1> Roles1
        {
            get;
            set;
        }

        public DbSet<RoomControl> RoomControls
        {
            get;
            set;
        }

        public DbSet<RoomReguest> RoomReguests
        {
            get;
            set;
        }

        public DbSet<Seo> Seos
        {
            get;
            set;
        }

        public DbSet<SpecialRate> SpecialRates
        {
            get;
            set;
        }

        public DbSet<Surcharge> Surcharges
        {
            get;
            set;
        }

        public DbSet<sysdiagram> sysdiagrams
        {
            get;
            set;
        }

        public DbSet<Tip> Tips
        {
            get;
            set;
        }

        public DbSet<Vinaday.Data.Models.Tour> Tour
        {
            get;
            set;
        }

        public DbSet<TOURBOOKINGHISTORY> TOURBOOKINGHISTORies
        {
            get;
            set;
        }

        public DbSet<TOURBOOKING> TOURBOOKINGs
        {
            get;
            set;
        }

        public DbSet<TOURDETAIL> TOURDETAILs
        {
            get;
            set;
        }

        public DbSet<TourOperator> TourOperators
        {
            get;
            set;
        }

        public DbSet<Tour_Promotion> TourPromotions
        {
            get;
            set;
        }

        public DbSet<TourRate> TourRates
        {
            get;
            set;
        }

        public DbSet<TOUR> TOURs
        {
            get;
            set;
        }

        public DbSet<Tour_Surcharge> TourSurcharges { get; set; }

        public DbSet<UserHotel> UserHotels { get; set; }

        public DbSet<USER> USERs { get; set; }

        public DbSet<User1> Users1 { get; set; }

        public DbSet<Voucher> Vouchers { get; set; }

   

        static vinadayContext()
        {
            Database.SetInitializer<vinadayContext>(null);
        }

        public vinadayContext() : base("Name=vinadayContext")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ADDRESSMap());
            modelBuilder.Configurations.Add(new aspnet_ApplicationsMap());
            modelBuilder.Configurations.Add(new aspnet_MembershipMap());
            modelBuilder.Configurations.Add(new aspnet_PathsMap());
            modelBuilder.Configurations.Add(new aspnet_PersonalizationAllUsersMap());
            modelBuilder.Configurations.Add(new aspnet_PersonalizationPerUserMap());
            modelBuilder.Configurations.Add(new aspnet_ProfileMap());
            modelBuilder.Configurations.Add(new aspnet_RolesMap());
            modelBuilder.Configurations.Add(new aspnet_UsersMap());
            modelBuilder.Configurations.Add(new aspnet_WebEvent_EventsMap());
            modelBuilder.Configurations.Add(new BookingMap());
            modelBuilder.Configurations.Add(new BookinghistoryMap());
            modelBuilder.Configurations.Add(new CancellationPolicyMap());
            modelBuilder.Configurations.Add(new CATDETAILMap());
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new CATEGORY1Map());
            modelBuilder.Configurations.Add(new CategoryDetailMap());
            modelBuilder.Configurations.Add(new CityMap());
            modelBuilder.Configurations.Add(new CreditCardMap());
            modelBuilder.Configurations.Add(new LottoMap());
            modelBuilder.Configurations.Add(new LottoDetailMap());
            modelBuilder.Configurations.Add(new COMMENTMap());
            modelBuilder.Configurations.Add(new ContactMap());
            modelBuilder.Configurations.Add(new COUNTRYMap());
            modelBuilder.Configurations.Add(new CUSTOMERMap());
            modelBuilder.Configurations.Add(new CustomerCallMap());
            modelBuilder.Configurations.Add(new DAYTRIPMap());
            modelBuilder.Configurations.Add(new DAYTRIP_IMAGESMap());
            modelBuilder.Configurations.Add(new DAYTRIPBOOKINGMap());
            modelBuilder.Configurations.Add(new DAYTRIPBOOKINGHISTORYMap());
            modelBuilder.Configurations.Add(new DayTripRateMap());
            modelBuilder.Configurations.Add(new ExpiredRoomMap());
            modelBuilder.Configurations.Add(new HotelMap());
            modelBuilder.Configurations.Add(new HOTEL_IMAGESMap());
            modelBuilder.Configurations.Add(new HotelCancellationMap());
            modelBuilder.Configurations.Add(new HOTELDETAILMap());
            modelBuilder.Configurations.Add(new HOTELFACILITYMap());
            modelBuilder.Configurations.Add(new HOTELPROMOTIONMap());
            modelBuilder.Configurations.Add(new HOTELSURCHARGEMap());
            modelBuilder.Configurations.Add(new HotelPackageMap());
            modelBuilder.Configurations.Add(new HotelPackage_SurchargeMap());
            modelBuilder.Configurations.Add(new IMAGEMap());
            modelBuilder.Configurations.Add(new LANGUAGEMap());
            modelBuilder.Configurations.Add(new MediumMap());
            modelBuilder.Configurations.Add(new NationalityMap());
            modelBuilder.Configurations.Add(new NotifyMap());
            modelBuilder.Configurations.Add(new PARTNERMap());
            modelBuilder.Configurations.Add(new PasswordResetRequestMap());
            modelBuilder.Configurations.Add(new PERMISSIONMap());
            modelBuilder.Configurations.Add(new ProfileCompanyMap());
            modelBuilder.Configurations.Add(new PromotionMap());
            modelBuilder.Configurations.Add(new PROMOTIONSTAYPAYMap());
            modelBuilder.Configurations.Add(new RateControlMap());
            modelBuilder.Configurations.Add(new RATEOFEXCHANGEMap());
            modelBuilder.Configurations.Add(new RECEIPTMap());
            modelBuilder.Configurations.Add(new RegionMap());
            modelBuilder.Configurations.Add(new Region1Map());
            modelBuilder.Configurations.Add(new ReviewDetailMap());
            modelBuilder.Configurations.Add(new ReviewMap());
            modelBuilder.Configurations.Add(new ROLEMap());
            modelBuilder.Configurations.Add(new Role1Map());
            modelBuilder.Configurations.Add(new RoomControlMap());
            modelBuilder.Configurations.Add(new RoomReguestMap());
            modelBuilder.Configurations.Add(new SeoMap());
            modelBuilder.Configurations.Add(new SurchargeMap());
            modelBuilder.Configurations.Add(new sysdiagramMap());
            modelBuilder.Configurations.Add(new TipMap());
            modelBuilder.Configurations.Add(new TOURMap());
            modelBuilder.Configurations.Add(new tour_DetailsMap());
            modelBuilder.Configurations.Add(new tour_FeaturedsMap());
            modelBuilder.Configurations.Add(new hotel_FeaturedsMap());
            modelBuilder.Configurations.Add(new OrderDetailMap());
            modelBuilder.Configurations.Add(new OrderDetailMap2());
            modelBuilder.Configurations.Add(new OrdersMap());
            modelBuilder.Configurations.Add(new OrdersMap2());
            modelBuilder.Configurations.Add(new AccountantOrdersMap());
            modelBuilder.Configurations.Add(new AccountantOrderDetailsMap());
            modelBuilder.Configurations.Add(new OrderInformationsMap());
            modelBuilder.Configurations.Add(new tour_RatesMap());
            modelBuilder.Configurations.Add(new SpecialRateMap());
            modelBuilder.Configurations.Add(new tour_ToursMap());
            modelBuilder.Configurations.Add(new tour_Rate2sMap());
            modelBuilder.Configurations.Add(new tour_Rate3sMap());
            modelBuilder.Configurations.Add(new ExpandRatesMap());
            modelBuilder.Configurations.Add(new TOURBOOKINGMap());
            modelBuilder.Configurations.Add(new TOURBOOKINGHISTORYMap());
            modelBuilder.Configurations.Add(new TOURDETAILMap());
            modelBuilder.Configurations.Add(new TourOperatorMap());
            modelBuilder.Configurations.Add(new TourOperatorsMap());
            modelBuilder.Configurations.Add(new TourRateMap());
            modelBuilder.Configurations.Add(new TourReviewMap());
            modelBuilder.Configurations.Add(new TourRateOptionsMap());
            modelBuilder.Configurations.Add(new USERMap());
            modelBuilder.Configurations.Add(new UserHotelMap());
            modelBuilder.Configurations.Add(new User1Map());
            modelBuilder.Configurations.Add(new VoucherMap());
            modelBuilder.Configurations.Add(new CouponCodeMap());
            modelBuilder.Configurations.Add(new DealsMap());
            modelBuilder.Configurations.Add(new PaymentOrderDetailMap());
            modelBuilder.Configurations.Add(new PaymentOrderMap());
            modelBuilder.Configurations.Add(new PaymentOrderDetail2Map());
            modelBuilder.Configurations.Add(new PaymentOrder2Map());
            modelBuilder.Configurations.Add(new TourPromotionMap());
            modelBuilder.Configurations.Add(new DealHotelToursMap());
            modelBuilder.Configurations.Add(new DealHotelToursVNMap());
            modelBuilder.Configurations.Add(new TourSurchargeMap());
            modelBuilder.Configurations.Add(new BusinessCardMap());
            
        }
    }
}