using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CodificoWebAPI.Data;

namespace CodificoWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // Constructor que inyecta el contexto de la base de datos
        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _context.Orders.ToListAsync();
            return Ok(orders);
        }

        // GET: api/Orders/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Where(o => o.OrderId == id)
                .Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    RequireDate = o.RequiredDate,
                    ShippedDate = o.ShippedDate,
                    ShipName = o.ShipName,
                    ShipAddress = o.ShipAddress,
                    ShipCity = o.ShipCity
                })
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(CreateOrderDto orderDto)
        {
            try
            {
                // Verificar si el cliente existe
                var customerExists = await _context.Customers.AnyAsync(c => c.CustId == orderDto.CustId);
                if (!customerExists)
                {
                    return BadRequest(new { message = $"Customer with ID {orderDto.CustId} does not exist." });
                }

                // Verificar si el empleado existe
                var employeeExists = await _context.Employees.AnyAsync(e => e.EmpId == orderDto.EmpId);
                if (!employeeExists)
                {
                    return BadRequest(new { message = $"Employee with ID {orderDto.EmpId} does not exist." });
                }

                // Verificar si el transportista existe
                var shipperExists = await _context.Shippers.AnyAsync(s => s.ShipperId == orderDto.ShipperId);
                if (!shipperExists)
                {
                    return BadRequest(new { message = $"Shipper with ID {orderDto.ShipperId} does not exist." });
                }

                // Verificar si el producto existe
                var productExists = await _context.Products.AnyAsync(p => p.ProductId == orderDto.ProductId);
                if (!productExists)
                {
                    return BadRequest(new { message = $"Product with ID {orderDto.ProductId} does not exist." });
                }

                var order = new Order
                {
                    CustId = orderDto.CustId,
                    EmpId = orderDto.EmpId,
                    OrderDate = orderDto.OrderDate,
                    RequiredDate = orderDto.RequiredDate,
                    ShippedDate = orderDto.ShippedDate,
                    ShipperId = orderDto.ShipperId,
                    Freight = orderDto.Freight,
                    ShipName = orderDto.ShipName,
                    ShipAddress = orderDto.ShipAddress,
                    ShipCity = orderDto.ShipCity,
                    ShipRegion = orderDto.ShipRegion,
                    ShipPostalCode = orderDto.ShipPostalCode,
                    ShipCountry = orderDto.ShipCountry
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                var orderDetail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    ProductId = orderDto.ProductId,
                    UnitPrice = orderDto.UnitPrice,
                    Qty = orderDto.Qty,
                    Discount = orderDto.Discount
                };

                _context.OrderDetails.Add(orderDetail);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }

        // PUT: api/Orders/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // DELETE: api/Orders/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("Test")]
        public IActionResult Test()
        {
            return Ok("OrdersController is working!");
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }

    // DTO para crear una nueva orden
    public class CreateOrderDto
    {
        public int CustId { get; set; }
        public int EmpId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public int ShipperId { get; set; }
        public decimal Freight { get; set; }
        public string? ShipName { get; set; }
        public string? ShipAddress { get; set; }
        public string? ShipCity { get; set; }
        public string? ShipRegion { get; set; }
        public string? ShipPostalCode { get; set; }
        public string? ShipCountry { get; set; }

        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public short Qty { get; set; }
        public decimal Discount { get; set; }
    }
}
