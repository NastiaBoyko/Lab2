﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebApiLab.Models
{
	public class Product
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public int Price { get; set; }

		[JsonIgnore]
		[NotMapped]
		public virtual ICollection<ProductOrder> Orders { get; set; }
		public virtual Category Category { get; set; }

		public Product()
		{
			Orders = new List<ProductOrder>();
		}
	}
}
