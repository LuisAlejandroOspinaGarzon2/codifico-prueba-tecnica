using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CodificoWebAPI.Data;

namespace CodificoWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        // Constructor que inyecta el contexto de la base de datos
        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
        {
            // Consulta que retorna todos los empleados con su nombre completo
            var employees = await _context.Employees
                .Select(e => new EmployeeDto
                {
                    EmpId = e.EmpId,
                    FullName = e.FirstName + " " + e.LastName
                })
                .ToListAsync();

            return Ok(employees);
        }
    }

    // DTO para empleados
    public class EmployeeDto
    {
        public int EmpId { get; set; }
        public string FullName { get; set; }
    }
}
