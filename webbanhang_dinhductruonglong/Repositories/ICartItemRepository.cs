using webbanhang_dinhductruonglong.Models;

namespace webbanhang_dinhductruonglong.Repositories
{
    public interface ICartItemRepository
    {
        Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(int cartId);
        Task<CartItem> GetCartItemByIdAsync(int id);
        Task<CartItem> AddCartItemAsync(CartItem cartItem);
        Task<CartItem> UpdateCartItemAsync(CartItem cartItem);
        Task<bool> DeleteCartItemAsync(int id);
    }
}
