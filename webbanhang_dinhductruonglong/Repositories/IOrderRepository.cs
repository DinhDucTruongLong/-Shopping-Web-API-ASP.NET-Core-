using webbanhang_dinhductruonglong.Models;

namespace webbanhang_dinhductruonglong.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(int id);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
    }
}
