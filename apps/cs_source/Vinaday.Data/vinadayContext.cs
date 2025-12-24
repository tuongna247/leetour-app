using System.Data.Entity;
using Repository.Pattern.Ef6;
using Vinaday.Data.Models;
using Vinaday.Data.Models.Mapping;

namespace Vinaday.Data
{
    public class VinadayContext : DataContext
    {
        static VinadayContext()
        {
            Database.SetInitializer<VinadayContext>(null);
        }

        public VinadayContext()
            : base("Name=vinadayContext")
        {
        }

        public DbSet<AccessPermission> AccessPermissions { get; set; }
        
        public DbSet<aspnet_Applications> aspnet_Applications { get; set; }
        public DbSet<aspnet_Membership> aspnet_Membership { get; set; }
        public DbSet<aspnet_Paths> aspnet_Paths { get; set; }
        public DbSet<aspnet_PersonalizationAllUsers> aspnet_PersonalizationAllUsers { get; set; }
        public DbSet<aspnet_PersonalizationPerUser> aspnet_PersonalizationPerUser { get; set; }
        public DbSet<aspnet_Profile> aspnet_Profile { get; set; }
        public DbSet<aspnet_Roles> aspnet_Roles { get; set; }
        public DbSet<aspnet_SchemaVersions> aspnet_SchemaVersions { get; set; }
        public DbSet<aspnet_Users> aspnet_Users { get; set; }
        public DbSet<aspnet_WebEvent_Events> aspnet_WebEvent_Events { get; set; }
        public DbSet<CancellationPolicy> CancellationPolicies { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryDetail> CategoryDetails { get; set; }
        public DbSet<City> Cities{ get; set; }
        public DbSet<Country> Countries { get; set; }

        public DbSet<LANGUAGE> Languages { get; set; }
        public DbSet<Medium> Media { get; set; }
        public DbSet<ProfileCompany> ProfileCompanies { get; set; }
        public DbSet<Region> Regions { get; set; }
        
        public DbSet<Seo> Seos { get; set; }
        public DbSet<TOURDETAIL> TourDetails { get; set; }
        //public DbSet<tour_FeaturedsMap> TourFeatureds { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<TourRate> TourRates { get; set; }
        public DbSet<Tour> Tours { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Configurations.Add(new AccessPermissionMap());
   
            modelBuilder.Configurations.Add(new aspnet_ApplicationsMap());
            //modelBuilder.Configurations.Add(new AspnetMembershipMap());
            modelBuilder.Configurations.Add(new aspnet_PathsMap());
            modelBuilder.Configurations.Add(new aspnet_PersonalizationAllUsersMap());
            modelBuilder.Configurations.Add(new aspnet_PersonalizationPerUserMap());
            modelBuilder.Configurations.Add(new aspnet_ProfileMap());
            modelBuilder.Configurations.Add(new aspnet_RolesMap());
            //modelBuilder.Configurations.Add(new aspnet_SchemaVersionsMap());
            modelBuilder.Configurations.Add(new aspnet_UsersMap());
            modelBuilder.Configurations.Add(new aspnet_WebEvent_EventsMap());
  
            modelBuilder.Configurations.Add(new CancellationPolicyMap());
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new CategoryDetailMap());
            modelBuilder.Configurations.Add(new CityMap());
            modelBuilder.Configurations.Add(new LANGUAGEMap());
            modelBuilder.Configurations.Add(new MediumMap());
            modelBuilder.Configurations.Add(new ProfileCompanyMap());
            modelBuilder.Configurations.Add(new RegionMap());
            modelBuilder.Configurations.Add(new SeoMap());
            modelBuilder.Configurations.Add(new TOURDETAILMap());
            modelBuilder.Configurations.Add(new tour_FeaturedsMap());
            modelBuilder.Configurations.Add(new OrderDetailMap());
            modelBuilder.Configurations.Add(new OrdersMap());
            modelBuilder.Configurations.Add(new TourRateMap());
            modelBuilder.Configurations.Add(new TOURMap());
            modelBuilder.Configurations.Add(new TourRateMap());

        }
    }
}
