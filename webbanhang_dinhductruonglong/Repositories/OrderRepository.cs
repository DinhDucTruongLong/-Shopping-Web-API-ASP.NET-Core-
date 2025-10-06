using Microsoft.EntityFrameworkCore;
using webbanhang_dinhductruonglong.Controllers;
using webbanhang_dinhductruonglong.Models;

namespace webbanhang_dinhductruonglong.Repositories
{
   
        public class OrderRepository : IOrderRepository
        {
            private readonly ApplicationDbContext _context;

            public OrderRepository(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<Order>> GetAllAsync()
            {
                // Lấy toàn bộ đơn hàng kèm theo thông tin User và OrderDetails
                return await _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderDetails)
                    .ToListAsync();
            }

            public async Task<Order?> GetByIdAsync(int id)
            {
                // Lấy 1 đơn hàng cụ thể kèm thông tin User + chi tiết đơn
                return await _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderDetails)
                    .FirstOrDefaultAsync(o => o.Id == id);
            }

            public async Task AddAsync(Order order)
            {
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
            }

            public async Task UpdateAsync(Order order)
            {
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
            }

            public async Task DeleteAsync(int id)
            {
                var order = await _context.Orders.FindAsync(id);
                if (order != null)
                {
                    _context.Orders.Remove(order);
                    await _context.SaveChangesAsync();
                }
            }

            public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
            {
                // Lấy tất cả đơn hàng của 1 user theo Id
                return await _context.Orders
                    .Where(o => o.UserId == userId)
                    .Include(o => o.OrderDetails)
                    .ToListAsync();
            }
        }
}
