﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiLab.Models
{
	public class Order
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public virtual ICollection<ProductOrder> Products { get; set; }

		public Order()
		{
			Products = new List<ProductOrder>();
		}
	}
}
