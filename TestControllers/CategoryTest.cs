using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using WebApiLab.Controllers;
using WebApiLab.Data;
using WebApiLab.Models;

namespace TestControllers
{
	public class Tests
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

		protected  void Seed(WebApiLabContext context)
		{
			var Phones = new Category { Name = "Phones" };
			var Laptops = new Category { Name = "Laptops" };
			var Tablets = new Category { Name = "Tablets" };
			context.Category.Add(Phones);
			context.Category.Add(Laptops);
			context.Category.Add(Tablets);


			context.Product.Add(
				new Product { Name = "Xiaomi MI 10", Price = 500,  Category = Phones });
			context.Product.Add(
				new Product { Name = "Samsung S 11", Price = 900,  Category = Phones });
			context.Product.Add(
				new Product { Name = "Iphone 10", Price = 1000,  Category = Phones });
			context.Product.Add(
				 new Product { Name = "Galaxy Tab S", Price = 800,  Category = Tablets });
			context.Product.Add(
				new Product { Name = "Ipad 3", Price = 700,Category = Tablets });
			context.Product.Add(
				 new Product { Name = "Asus Rog", Price = 3000,  Category = Laptops });
			context.Product.Add(
				new Product { Name = "Asus Tuf Gaming", Price = 1500, Category = Laptops });
			context.Product.Add(
				new Product { Name = "Xiaomi Gaming Laptop", Price = 1000,  Category = Laptops });
			context.Product.Add(
				new Product { Name = "Acer Predator", Price = 2000, Category = Laptops });
			context.Product.Add(
				new Product { Name = "Macbook Pro", Price = 3999,  Category = Laptops });

			context.SaveChanges();
		}

		[Test]
		public async Task TestGetByIdShoulReturnPhones()
		{
			// Arrange
			int id = 1;
			CategoriesController controller = new CategoriesController(context);
			string expected = context.Category.Find(id).Name;
			// Act
			var result = await controller.GetCategory(id);
			var actual = result.Value.Name;
			// Assert
			Assert.AreEqual(expected, actual);

		}
		[Test]
		public async Task TestGetAll()
		{
			// Arrange
			CategoriesController controller = new CategoriesController(context);
			var expected = context.Category;
			// Act
			var result = await controller.GetCategory();
			var actual = result.Value;
			// Assert
			Assert.AreEqual(expected, actual);

		}
		
		[Test]
		public async Task TestUpdate()
		{
			// Arrange
			CategoriesController controller = new CategoriesController(context);
			var expected = new Category {Id = 2, Name = "Test" };
			// Act
			await controller.PutCategory(expected.Id, expected);
			var result = await controller.GetCategory(expected.Id);
			var actual = result.Value.Name;
			// Assert
			Assert.AreEqual(expected.Name, actual);

		}
		[Test]
		public async Task TestDelete()
		{
			// Arrange
			int id = 4;
			CategoriesController controller = new CategoriesController(context);
			var expected = context.Category.Find(id);
			// Act
			var result = await controller.DeleteCategory(id);
			var actual = result.Value;
			// Assert
			Assert.AreEqual(expected, actual);

		}
	}
}