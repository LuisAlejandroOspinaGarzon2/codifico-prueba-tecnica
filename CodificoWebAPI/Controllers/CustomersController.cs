using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CodificoWebAPI.Data;

namespace CodificoWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        // Constructor que inyecta el contexto de la base de datos
        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Customers/LastOrderPrediction
        [HttpGet("LastOrderPrediction")]
        public async Task<ActionResult<IEnumerable<CustomerOrderPredictionDto>>> GetCustomerOrderPredictions()
        {
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .ToListAsync();

            var predictions = orders
                .Where(o => o.Customer != null)
                .GroupBy(o => o.CustId)
                .Select(g => new CustomerOrderPredictionDto
                {
                    CustomerName = g.First().Customer?.CompanyName ?? "Unknown",
                    LastOrderDate = g.Max(o => o.OrderDate),
                    NextPredictedOrder = g.Max(o => o.OrderDate).AddDays(g.Average(o => (o.RequiredDate - o.OrderDate).Days))
                })
                .ToList();

            return Ok(predictions);
        }

        // GET: api/Customers/{id}/Orders
        [HttpGet("{id}/Orders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByCustomer(int id)
        {
            var orders = await _context.Orders
                .Where(o => o.CustId == id)
                .Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    RequireDate = o.RequiredDate,
                    ShippedDate = o.ShippedDate,
                    ShipName = o.ShipName,
                    ShipAddress = o.ShipAddress,
                    ShipCity = o.ShipCity
                })
                .ToListAsync();

            if (orders == null || !orders.Any())
            {
                return NotFound();
            }

            return Ok(orders);
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrdersByCustomer), new { id = customer.CustId }, customer);
        }

        // PUT: api/Customers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, Customer customer)
        {
            if (id != customer.CustId)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        // DELETE: api/Customers/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustId == id);
        }
    }
    // DTO para la predicción de la próxima orden
    public class CustomerOrderPredictionDto
    {
        public string? CustomerName { get; set; }
        public DateTime LastOrderDate { get; set; }
        public DateTime NextPredictedOrder { get; set; }
    }

    // DTO para los detalles de una orden
    public class OrderDto
    {
        public int OrderId { get; set; }
        public DateTime RequireDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public string? ShipName { get; set; }
        public string? ShipAddress { get; set; }
        public string? ShipCity { get; set; }
    }
}

