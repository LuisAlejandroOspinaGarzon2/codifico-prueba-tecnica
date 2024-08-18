using System.Collections.Generic;
using System.Threading.Tasks;
using CodificoWebAPI.Controllers;
using CodificoWebAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CodificoWebAPI.Tests.Controllers
{
    public class OrdersControllerTests
    {
        private readonly ApplicationDbContext _context;
        private readonly OrdersController? _controller;

        public OrdersControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _controller = new OrdersController(_context);

            // Limpia la base de datos antes de cada prueba
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            // Inserta datos de prueba sin especificar IDs
            var customer = new Customer { CompanyName = "TestCompany" };
            _context.Customers.Add(customer);
            _context.SaveChanges();

            var order1 = new Order
            {
                CustId = customer.CustId,
                OrderDate = DateTime.Now,
                Customer = customer
            };
            var order2 = new Order
            {
                CustId = customer.CustId,
                OrderDate = DateTime.Now.AddDays(-1),
                Customer = customer
            };
            _context.Orders.Add(order1);
            _context.Orders.Add(order2);
            _context.SaveChanges();

            Assert.Equal(2, _context.Orders.Count()); // Verifica que se agregaron las órdenes
        }


        [Fact]
        public async Task GetOrders_ReturnsAllOrders()
        {
            // Verificar si _controller es null
            if (_controller == null)
            {
                Assert.Fail("Controller is not initialized.");
                return;
            }

            // Act
            var result = await _controller.GetOrders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var orders = Assert.IsType<List<Order>>(okResult.Value);
            Assert.Equal(2, orders.Count);  // Verificamos que se obtienen las 2 órdenes.
        }

        [Fact]
        public async Task GetOrder_ReturnsOrderById()
        {
            // Verificar si _controller es null
            if (_controller == null)
            {
                Assert.Fail("Controller is not initialized.");
                return;
            }

            // Act
            var result = await _controller.GetOrder(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var order = Assert.IsType<OrderDto>(okResult.Value);
            Assert.Equal(1, order.OrderId);  // Verificamos que la orden es la correcta.
        }

    }

}
