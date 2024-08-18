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
    public class EmployeesControllerTests
    {
        private readonly ApplicationDbContext _context;
        private readonly EmployeesController _controller;

        public EmployeesControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _controller = new EmployeesController(_context);

            // Limpiar la base de datos antes de agregar nuevos datos
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            // Seed de datos de prueba
            var employee1 = new Employee { EmpId = 1, FirstName = "John", LastName = "Doe" };
            var employee2 = new Employee { EmpId = 2, FirstName = "Jane", LastName = "Smith" };
            _context.Employees.AddRange(employee1, employee2);

            _context.SaveChanges();
        }

        [Fact]
        public async Task GetEmployees_ReturnsAllEmployees()
        {
            // Act
            var result = await _controller.GetEmployees();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var employees = Assert.IsType<List<EmployeeDto>>(okResult.Value);
            Assert.Equal(2, employees.Count);  // Verificamos que se obtienen los 2 empleados.
            Assert.Contains(employees, e => e.FullName == "John Doe");
            Assert.Contains(employees, e => e.FullName == "Jane Smith");
        }
    }
}
