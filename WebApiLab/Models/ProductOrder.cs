﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebApiLab.Models
{
	public class ProductOrder
	{
		public int Id { get; set; }
		public int ProductId { get; set; }
		public int OrderId { get; set; }
		[JsonIgnore]
		[NotMapped]
		public virtual Order Order { get; set; }
		public virtual Product Product { get; set; }


	}
}
