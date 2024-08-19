using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CodificoWebAPI.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CodificoWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesPredictionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SalesPredictionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/sales-prediction
        [HttpGet]
        public async Task<ActionResult> GetSalesPredictions(int page = 1, int pageSize = 10, string sortBy = "customerName", string sortDirection = "asc", string searchTerm = "")
        {
            var query = _context.Customers
                .Select(c => new 
                {
                    c.CustId,
                    CustomerName = c.CompanyName,
                    LastOrderDate = c.Orders.Max(o => o.OrderDate),
                    NextOrderDate = c.Orders.Max(o => o.OrderDate).AddDays(30) // Ejemplo de predicción: 30 días después de la última orden
                });

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(c => c.CustomerName.Contains(searchTerm));
            }

            switch (sortBy)
            {
                case "lastOrderDate":
                    query = sortDirection == "asc" ? query.OrderBy(c => c.LastOrderDate) : query.OrderByDescending(c => c.LastOrderDate);
                    break;
                case "nextOrderDate":
                    query = sortDirection == "asc" ? query.OrderBy(c => c.NextOrderDate) : query.OrderByDescending(c => c.NextOrderDate);
                    break;
                default:
                    query = sortDirection == "asc" ? query.OrderBy(c => c.CustomerName) : query.OrderByDescending(c => c.CustomerName);
                    break;
            }

            var totalCount = await query.CountAsync();

            var customers = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                TotalCount = totalCount,
                Customers = customers
            });
        }
    }
}
