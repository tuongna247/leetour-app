using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Extention
{
    public class TourModel
    {
        public string Description { get; set; }

        public List<Detail> Details { get; set; }

        public string Duration { get; set; }
        public string Location { get; set; }

        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public List<Vinaday.Data.Models.Medium> Medium { get; set; }

        public string Name { get; set; }
        public DateTime BookDate { get; set; }
        public int? Children { get; set; }
        public int? Adult { get; set; }
        public decimal? RetailRate1 { get; set; }
        public decimal? FindalRate1 { get; set; }
        public decimal? FindalRate2 { get; set; }
        public decimal? FindalRate3 { get; set; }
        public decimal? RetailRate2 { get; set; }
        public decimal? RetailRate3 { get; set; }

        public List<SpecialRate> Promotions { get; set; }

        public List<Rate> Rates { get; set; }
        public List<Rate2> Rates2 { get; set; }
        public List<Rate3> Rates3 { get; set; }

        public SeoModel Seo { get; set; }

        public List<SpecialRate> Surcharges { get; set; }

        public Vinaday.Data.Models.Tour Tour { get; set; }

        public TourModel()
        {
        }
    }
}