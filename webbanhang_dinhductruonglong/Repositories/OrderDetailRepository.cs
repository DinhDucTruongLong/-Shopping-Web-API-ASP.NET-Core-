using Microsoft.EntityFrameworkCore;
using webbanhang_dinhductruonglong.Controllers;
using webbanhang_dinhductruonglong.Models;

namespace webbanhang_dinhductruonglong.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepositorycs
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderDetail>> GetAllAsync()
        {
            // Lấy tất cả chi tiết đơn hàng kèm thông tin sản phẩm & đơn hàng
            return await _context.OrderDetails
                .Include(od => od.Product)
                .Include(od => od.Order)
                .ToListAsync();
        }

        public async Task<OrderDetail?> GetByIdAsync(int id)
        {
            // Lấy chi tiết đơn hàng theo ID kèm thông tin sản phẩm
            return await _context.OrderDetails
                .Include(od => od.Product)
                .Include(od => od.Order)
                .FirstOrDefaultAsync(od => od.Id == id);
        }

        public async Task<IEnumerable<OrderDetail>> GetByOrderIdAsync(int orderId)
        {
            // Lấy danh sách chi tiết của 1 đơn hàng cụ thể
            return await _context.OrderDetails
                .Where(od => od.OrderId == orderId)
                .Include(od => od.Product)
                .ToListAsync();
        }

        public async Task AddAsync(OrderDetail orderDetail)
        {
            await _context.OrderDetails.AddAsync(orderDetail);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(OrderDetail orderDetail)
        {
            _context.OrderDetails.Update(orderDetail);
            await _context.SaveChangesAsync();
        }

        public async Task <bool>DeleteAsync(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail == null)
            {
               return false;
            }
            _context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
