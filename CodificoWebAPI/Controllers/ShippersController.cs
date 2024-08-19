using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CodificoWebAPI.Data;

namespace CodificoWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ShippersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Shippers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShipperDto>>> GetShippers()
        {
            var shippers = await _context.Shippers
                .Select(s => new ShipperDto
                {
                    ShipperId = s.ShipperId,
                    CompanyName = s.CompanyName
                })
                .ToListAsync();

            return Ok(shippers);
        }

        // GET: api/Shippers/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ShipperDto>> GetShipper(int id)
        {
            var shipper = await _context.Shippers
                .Where(s => s.ShipperId == id)
                .Select(s => new ShipperDto
                {
                    ShipperId = s.ShipperId,
                    CompanyName = s.CompanyName
                })
                .FirstOrDefaultAsync();

            if (shipper == null)
            {
                return NotFound();
            }

            return Ok(shipper);
        }

        // POST: api/Shippers
        [HttpPost]
        public async Task<ActionResult<Shipper>> CreateShipper(Shipper shipper)
        {
            _context.Shippers.Add(shipper);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetShipper), new { id = shipper.ShipperId }, shipper);
        }

        // PUT: api/Shippers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShipper(int id, Shipper shipper)
        {
            if (id != shipper.ShipperId)
            {
                return BadRequest();
            }

            _context.Entry(shipper).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShipperExists(id))
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

        // DELETE: api/Shippers/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShipper(int id)
        {
            var shipper = await _context.Shippers.FindAsync(id);
            if (shipper == null)
            {
                return NotFound();
            }

            _context.Shippers.Remove(shipper);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("Test")]
        public IActionResult Test()
        {
            return Ok("EmployeesController is working!");
        }

        private bool ShipperExists(int id)
        {
            return _context.Shippers.Any(e => e.ShipperId == id);
        }
    }

    // DTO para transportistas (Shippers)
    public class ShipperDto
    {
        public int ShipperId { get; set; }
        public string? CompanyName { get; set; }
    }
}
