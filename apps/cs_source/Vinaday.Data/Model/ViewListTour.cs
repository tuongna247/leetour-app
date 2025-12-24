using Repository.Pattern.Ef6;
using System;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class ViewListTour : Entity
	{
		public int Id { get; set; }

        public string Name { get; set; }
        public string Duration { get; set; }
        public string Location { get; set; }
        public double PriceFrom { get; set; }
        public string ThumbmailPath { get; set; }
	}
}