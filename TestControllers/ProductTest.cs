using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApiLab.Controllers;
using WebApiLab.Data;
using WebApiLab.Models;

namespace TestControllers
{
	class ProductTest
	{
		private WebApiLabContext context;

		[SetUp]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<WebApiLabContext>()
				.UseInMemoryDatabase(databaseName: "Lab2_Orders")
				.Options;
			context = new WebApiLabContext(options);

			Seed(context);
		}

		protected void Seed(WebApiLabContext context)
		{
			var Phones = new Category { Name = "Phones" };
			var Laptops = new Category { Name = "Laptops" };
			var Tablets = new Category { Name = "Tablets" };
			context.Category.Add(Phones);
			context.Category.Add(Laptops);
			context.Category.Add(Tablets);


			context.Product.Add(
				new Product { Name = "Xiaomi MI 10", Price = 500, Category = Phones });
			context.Product.Add(
				new Product { Name = "Samsung S 11", Price = 900, Category = Phones });
			context.Product.Add(
				new Product { Name = "Iphone 10", Price = 1000, Category = Phones });
			context.Product.Add(
				 new Product { Name = "Galaxy Tab S", Price = 800, Category = Tablets });
			context.Product.Add(
				new Product { Name = "Ipad 3", Price = 700, Category = Tablets });
			context.Product.Add(
				 new Product { Name = "Asus Rog", Price = 3000, Category = Laptops });
			context.Product.Add(
				new Product { Name = "Asus Tuf Gaming", Price = 1500, Category = Laptops });
			context.Product.Add(
				new Product { Name = "Xiaomi Gaming Laptop", Price = 1000, Category = Laptops });
			context.Product.Add(
				new Product { Name = "Acer Predator", Price = 2000, Category = Laptops });
			context.Product.Add(
				new Product { Name = "Macbook Pro", Price = 3999, Category = Laptops });

			context.SaveChanges();
		}

		[Test]
		public async Task TestGetByIdShoulReturnPhones()
		{
			// Arrange
			int id = 1;
			ProductsController controller = new ProductsController(context);
			string expected = context.Product.Find(id).Name;
			// Act
			var result = await controller.GetProduct(id);
			var actual = result.Value.Name;
			// Assert
			Assert.AreEqual(expected, actual);

		}
		[Test]
		public async Task TestGetAll()
		{
			// Arrange
			ProductsController controller = new ProductsController(context);
			var expected = context.Product;
			// Act
			var result = await controller.GetProduct();
			var actual = result.Value;
			// Assert
			Assert.AreEqual(expected, actual);

		}
		

		[Test]
		public async Task TestUpdate()
		{
			// Arrange
			ProductsController controller = new ProductsController(context);
			var expected = new Product { Id = 1, Name = "Xiaomi MI 10 SS"};
			// Act
			await controller.PutProduct(expected.Id, expected);
			var result = await controller.GetProduct(expected.Id);
			var actual = result.Value.Name;
			// Assert
			Assert.AreEqual(expected.Name, actual);

		}
		[Test]
		public async Task TestDelete()
		{
			// Arrange
			int id = 4;
			ProductsController controller = new ProductsController(context);
			var expected = context.Product.Find(id);
			// Act
			var result = await controller.DeleteProduct(id);
			var actual = result.Value;
			// Assert
			Assert.AreEqual(expected, actual);

		}
	}
}
