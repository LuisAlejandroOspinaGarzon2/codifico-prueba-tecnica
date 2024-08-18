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
    public class ProductsControllerTests
    {
        private readonly ApplicationDbContext _context;
        private readonly ProductsController? _controller;

        public ProductsControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _controller = new ProductsController(_context);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            // Verificar que la base de datos esté vacía
            Assert.Empty(_context.Products.ToList());

            // Seed de datos de prueba
            var product1 = new Product { ProductId = 1, ProductName = "TestProduct1" };
            var product2 = new Product { ProductId = 2, ProductName = "TestProduct2" };
            _context.Products.Add(product1);
            _context.Products.Add(product2);

            _context.SaveChanges();

            // Verificar que los productos se han agregado correctamente
            Assert.Equal(2, _context.Products.Count());
        }

        [Fact]
        public async Task GetProducts_ReturnsAllProducts()
        {
            // Verificar si _controller es null
            if (_controller == null)
            {
                Assert.Fail("Controller is not initialized.");
                return;
            }

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var products = Assert.IsType<List<ProductDto>>(okResult.Value);
            Assert.Equal(2, products.Count);  // Verificamos que se obtienen los 2 productos.
        }

        [Fact]
        public async Task GetProductById_ReturnsProduct()
        {
            if (_controller == null)
            {
                Assert.Fail("Controller is not initialized.");
                return;
            }

            var result = await _controller.GetProduct(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var product = Assert.IsType<ProductDto>(okResult.Value);
            Assert.Equal("TestProduct1", product.ProductName);
        }
    }
}
