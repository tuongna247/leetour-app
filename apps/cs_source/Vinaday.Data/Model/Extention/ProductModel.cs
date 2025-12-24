using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Extention
{
    public class ProductModel
    {
        public Vinaday.Data.Models.CancellationPolicy CancellationPolicy { get; set; }

        public DateTime CheckIn { get; set; }

        public DateTime CheckOut { get; set; }

        public string DetailName { get; set; }
        public string GuestFirstName { get; set; }
        public string GuestLastName { get; set; }
        public string GuestNational { get; set; }

        public List<ProductDetailModel> Details { get; set; }

        public decimal FinalPrice { get; set; }

        public int Id { get; set; }

        public string ImageUrl { get; set; }
        public string Include { get; set; }

        public string Location { get; set; }
        public string Name { get; set; }
        public int? Night { get; set; }

        public int ProductId { get; set; }

        public string ProductUrl { get; set; }

        public PromotionModel Promotion { get; set; }

        public int? Quantity { get; set; }

        public int Rating { get; set; }

        public string ShoppingCartUrl { get; set; }

        public string Stay { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal TotalSave { get; set; }

        public decimal TotalSurcharge { get; set; }

        public decimal TotalTaxeFee { get; set; }
        public ProductModel()
        {
        }
    }
}