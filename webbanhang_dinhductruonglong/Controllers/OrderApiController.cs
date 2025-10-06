using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webbanhang_dinhductruonglong.Models;

namespace webbanhang_dinhductruonglong.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class OrderApiController : ControllerBase
    {
        
       
            private readonly ApplicationDbContext _context;

            public OrderApiController(ApplicationDbContext context)
            {
                _context = context;
            }

            // GET: api/orders
            [HttpGet]
            public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
            {
                var orders = await _context.Orders
                    .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                    .ToListAsync();

                return Ok(orders);
            }

            // GET: api/orders/{id}
            [HttpGet("{id}")]
            public async Task<ActionResult<Order>> GetOrder(int id)
            {
                var order = await _context.Orders
                    .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (order == null)
                    return NotFound();

                return Ok(order);
            }

            // POST: api/orders
            [HttpPost]
            public async Task<ActionResult<Order>> CreateOrder([FromBody] Order order)
            {
                if (order == null || order.OrderDetails == null || !order.OrderDetails.Any())
                    return BadRequest("Đơn hàng không hợp lệ.");

                order.OrderDate = DateTime.UtcNow;
                order.TotalPrice = order.OrderDetails.Sum(d => d.Price * d.Quantity);

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
            }

            // PUT: api/orders/{id}
            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order order)
            {
                if (id != order.Id)
                    return BadRequest("ID không khớp.");

                var existingOrder = await _context.Orders
                    .Include(o => o.OrderDetails)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (existingOrder == null)
                    return NotFound();

                // Cập nhật các trường cơ bản
                existingOrder.TotalPrice = order.TotalPrice;
                existingOrder.OrderDate = order.OrderDate;
                existingOrder.UserId = order.UserId;

                // Cập nhật chi tiết đơn hàng (xóa cũ, thêm mới)
                _context.OrderDetails.RemoveRange(existingOrder.OrderDetails);
                existingOrder.OrderDetails = order.OrderDetails;

                await _context.SaveChangesAsync();

                return NoContent();
            }

            // DELETE: api/orders/{id}
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteOrder(int id)
            {
                var order = await _context.Orders
                    .Include(o => o.OrderDetails)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (order == null)
                    return NotFound();

                _context.OrderDetails.RemoveRange(order.OrderDetails);
                _context.Orders.Remove(order);

                await _context.SaveChangesAsync();

                return NoContent();
            }
        }
    }

