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
            // Recupera los datos necesarios de la base de datos
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .ToListAsync();

            // Agrupa por cliente y realiza el cálculo en memoria
            var predictions = orders
                .Where(o => o.Customer != null)
                .GroupBy(o => o.CustId)
                .Select(g => new CustomerOrderPredictionDto
                {
                    CustomerName = g.First().Customer?.CompanyName ?? "Unknown", // Verifica que CompanyName no sea nulo
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
            // Consulta que retorna las órdenes de un cliente específico
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
