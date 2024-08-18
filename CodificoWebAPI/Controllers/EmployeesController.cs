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
            var employees = await _context.Employees
                .Select(e => new EmployeeDto
                {
                    EmpId = e.EmpId,
                    FullName = e.FirstName + " " + e.LastName
                })
                .ToListAsync();

            return Ok(employees);
        }

        // GET: api/Employees/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
        {
            var employee = await _context.Employees
                .Where(e => e.EmpId == id)
                .Select(e => new EmployeeDto
                {
                    EmpId = e.EmpId,
                    FullName = e.FirstName + " " + e.LastName
                })
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmpId }, employee);
        }

        // PUT: api/Employees/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, Employee employee)
        {
            if (id != employee.EmpId)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Employees/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmpId == id);
        }
    }

    // DTO para empleados
    public class EmployeeDto
    {
        public int EmpId { get; set; }
        public string? FullName { get; set; }
    }
}
