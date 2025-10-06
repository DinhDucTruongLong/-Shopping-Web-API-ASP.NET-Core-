using webbanhang_dinhductruonglong.Models;

namespace webbanhang_dinhductruonglong.Repositories
{
    public interface IOrderDetailRepositorycs
    {
        Task<IEnumerable<OrderDetail>> GetAllAsync();
        Task<OrderDetail?> GetByIdAsync(int id);
        Task<IEnumerable<OrderDetail>> GetByOrderIdAsync(int orderId);
        Task AddAsync(OrderDetail orderDetail);
        Task UpdateAsync(OrderDetail orderDetail);
        Task <bool> DeleteAsync(int id);
    }
}
