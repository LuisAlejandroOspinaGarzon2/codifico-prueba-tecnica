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

        // Constructor que inyecta el contexto de la base de datos
        public ShippersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Shippers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShipperDto>>> GetShippers()
        {
            // Consulta que retorna todos los transportistas (Shippers)
            var shippers = await _context.Shippers
                .Select(s => new ShipperDto
                {
                    ShipperId = s.ShipperId,
                    CompanyName = s.CompanyName
                })
                .ToListAsync();

            return Ok(shippers);
        }
    }

    // DTO para transportistas (Shippers)
    public class ShipperDto
    {
        public int ShipperId { get; set; }
        public string CompanyName { get; set; }
    }
}
