﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebApiLab.Models
{
	public class Category
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }

		[JsonIgnore]
		[NotMapped]
		public virtual ICollection<Product> Products { get; set; }

		public Category()
		{
			Products = new List<Product>();
		}
	}
}
