using Microsoft.AspNetCore.Mvc;
using webbanhang_dinhductruonglong.Models;
using webbanhang_dinhductruonglong.Repositories;

namespace webbanhang_dinhductruonglong.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartItemApiController : ControllerBase
    {
        private readonly ICartItemRepository _cartItemRepository;

        public CartItemApiController(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int cartId)
        {
            var items = await _cartItemRepository.GetCartItemsByCartIdAsync(cartId);
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _cartItemRepository.GetCartItemByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CartItem cartItem)
        {
            var newItem = await _cartItemRepository.AddCartItemAsync(cartItem);
            return CreatedAtAction(nameof(GetById), new { id = newItem.Id }, newItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CartItem cart)
        {
            if (id != cart.Id) { return BadRequest(); }
            await _cartItemRepository.UpdateCartItemAsync(cart);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _cartItemRepository.DeleteCartItemAsync(id);
            if (success)
            {
                return Ok(success);
            }
            return BadRequest();
        }
    }
}
