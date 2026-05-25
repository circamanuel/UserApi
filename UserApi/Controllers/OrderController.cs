using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserApi.Data;
using UserApi.Models;
using UserApi.DTOs;

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            var products = await _context.Products
                .Where(p => order.Products.Select(x => x.Id).Contains(p.Id))
                .ToListAsync();

            //2. Calculate pcs
            order.Products = products;
            order.TotalPieces = products.Count();

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { Id = order.Id }, order);
        }

        [HttpPost("batch")]
        public async Task<IActionResult> CreateOrders(List<CreateOrderDTO> orderDTOs)
        {
            foreach (var dto in orderDTOs)
            {
                var products = await _context.Products
                    .Where(p => dto.ProductIds.Contains(p.Id))
                    .ToListAsync();

                var order = new Order
                {
                    UserId = dto.UserId,
                    TotalPieces = dto.TotalPieces,
                    Products = products
                };

                _context.Orders.Add(order);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
        {
           var orders = await _context.Orders
                .Include(o => o.Products)
                .ToListAsync();
            return orders.Select(o => OrderToDTO(o)).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            var orders = await _context.Orders
                 .Include(o => o.Products)
                 .FirstOrDefaultAsync(o => o.Id == id);

            if (orders == null)
            {
                return NotFound();
            }

            return OrderToDTO(orders);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, Order order)
        {
            if (id != order.Id)
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
            }

            return NoContent();
        }

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


        private bool OrderExists(int id)
        {
            return _context.Orders.Any(o => o.Id == id);
        }

        private static OrderDTO OrderToDTO(Order order) => new OrderDTO
        {
            Id = order.Id,
            UserId = order.UserId,
            TotalPieces = order.TotalPieces,
            TotalPrice = order.TotalPrice,
            Products = order.Products.Select(p => new ProductDTO
            {
                Id = p.Id,
                ProductName = p.ProductName,
                Price = p.Price
            }).ToList()

        };
    }
}
