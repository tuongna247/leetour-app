using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
    public class TourRateOptions : Entity
    {

        public int Id { get; set; }

        public string Name { get; set; }
        public double Rate { get; set; }

        public int Tour_Id { get; set; }

        public TourRateOptions()
        {
        }
    }
}