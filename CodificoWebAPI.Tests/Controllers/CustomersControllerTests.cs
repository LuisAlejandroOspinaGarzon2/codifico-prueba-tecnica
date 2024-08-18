using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodificoWebAPI.Controllers;
using CodificoWebAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CodificoWebAPI.Tests.Controllers
{
    public class CustomersControllerTests
    {
        private readonly ApplicationDbContext _context;
        private readonly CustomersController _controller;

        public CustomersControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _controller = new CustomersController(_context);

            // Limpiar datos existentes en el contexto
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            // Seed de datos de prueba con claves únicas
            var customer1 = new Customer { CustId = 1, CompanyName = "Test Company 1" };
            var customer2 = new Customer { CustId = 2, CompanyName = "Test Company 2" };

            _context.Customers.Add(customer1);
            _context.Customers.Add(customer2);

            // Añadir órdenes con fechas válidas y únicos OrderId
            _context.Orders.Add(new Order 
            { 
                CustId = 1, 
                OrderId = 1, 
                OrderDate = DateTime.Now.AddDays(-10), 
                RequiredDate = DateTime.Now.AddDays(-5), 
                Customer = customer1,
                ShipName = "Ship1",
                ShipAddress = "Address1",
                ShipCity = "City1"
            });
            _context.Orders.Add(new Order 
            { 
                CustId = 2, 
                OrderId = 2, 
                OrderDate = DateTime.Now.AddDays(-5), 
                RequiredDate = DateTime.Now, 
                Customer = customer2,
                ShipName = "Ship2",
                ShipAddress = "Address2",
                ShipCity = "City2"
            });

            _context.SaveChanges();
        }



        [Fact]
        public async Task GetCustomerOrderPredictions_ReturnsPredictions()
        {
            // Act
            var result = await _controller.GetCustomerOrderPredictions();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var predictions = Assert.IsType<List<CustomerOrderPredictionDto>>(okResult.Value);
            
            Assert.NotEmpty(predictions); 
        }


        [Fact]
        public async Task GetOrdersByCustomer_ReturnsOrders()
        {
            // Act
            var result = await _controller.GetOrdersByCustomer(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var orders = Assert.IsType<List<OrderDto>>(okResult.Value);
            Assert.Single(orders);
        }
    }
}
