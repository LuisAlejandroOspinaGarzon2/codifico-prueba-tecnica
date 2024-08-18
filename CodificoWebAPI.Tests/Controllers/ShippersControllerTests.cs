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
    public class ShippersControllerTests
    {
        private readonly ApplicationDbContext _context;
        private readonly ShippersController _controller;

        public ShippersControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _controller = new ShippersController(_context);

            // Limpiar la base de datos antes de agregar nuevos datos
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            // Seed de datos de prueba
            var shipper1 = new Shipper { ShipperId = 1, CompanyName = "Fast Shipping" };
            var shipper2 = new Shipper { ShipperId = 2, CompanyName = "Global Shippers" };
            _context.Shippers.AddRange(shipper1, shipper2);

            _context.SaveChanges();
        }

        [Fact]
        public async Task GetShippers_ReturnsAllShippers()
        {
            // Act
            var result = await _controller.GetShippers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var shippers = Assert.IsType<List<ShipperDto>>(okResult.Value);
            Assert.Equal(2, shippers.Count);  // Verificamos que se obtienen los 2 shippers.
            Assert.Contains(shippers, s => s.CompanyName == "Fast Shipping");
            Assert.Contains(shippers, s => s.CompanyName == "Global Shippers");
        }
    }
}
