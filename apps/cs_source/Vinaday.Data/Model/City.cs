using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
    public class City : Entity
    {
        public int CityId
        {
            get;
            set;
        }

        public int CountryId
        {
            get;
            set;
        }

        public int? DayTrips
        {
            get;
            set;
        }

        public int? DayTripTrend
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public int? Hotels
        {
            get;
            set;
        }

        public int? HotelTrends
        {
            get;
            set;
        }

        public string ImageURL
        {
            get;
            set;
        }

        public double? Latitude
        {
            get;
            set;
        }

        public double? Longtitude
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int Priority
        {
            get;
            set;
        }

        public string SEO_Description
        {
            get;
            set;
        }

        public string SEO_Description_VN
        {
            get;
            set;
        }

        public string SEO_Keyword
        {
            get;
            set;
        }

        public string SEO_Keyword_VN
        {
            get;
            set;
        }

        public string SEO_Meta
        {
            get;
            set;
        }

        public string Seo_Title
        {
            get;
            set;
        }

        public string SEO_Title_VN
        {
            get;
            set;
        }

        public int? Tours
        {
            get;
            set;
        }

        public int? TourTrend
        {
            get;
            set;
        }

        public string URL
        {
            get;
            set;
        }

        public string vn_url
        {
            get;
            set;
        }

        public int? Status { get; set; }

        public City()
        {
        }
    }
}