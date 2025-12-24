using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
	public class Nationality : Entity
	{
		public virtual ICollection<Booking> BOOKINGs
		{
			get;
			set;
		}

		public virtual ICollection<Customer> Customers
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public int ID
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

		public virtual ICollection<TourOperator> TourOperators
		{
			get;
			set;
		}

		public Nationality()
		{
			this.BOOKINGs = new List<Booking>();
			this.Customers = new List<Customer>();
			this.TourOperators = new List<TourOperator>();
		}
	}
}