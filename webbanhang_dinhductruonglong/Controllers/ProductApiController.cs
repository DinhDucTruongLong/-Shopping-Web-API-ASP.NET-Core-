using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using webbanhang_dinhductruonglong.Models;
using webbanhang_dinhductruonglong.Repositories;

namespace webbanhang_dinhductruonglong.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductApiController : ControllerBase
    {
        private readonly IproductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductApiController(IproductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // Hiển thị DS sẩn phầm
        [HttpGet]
        [AllowAnonymous]
        // có thể xem khí chưa login
        public async Task<IActionResult> GetAll()
        {
            var product = await _productRepository.GetAllAsync();
            return Ok(product);
        }

        // Hiển thị thông tin chi tiết sản phầm
        [HttpGet("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // Thêm sp 
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromForm] Product product, IFormFile imageUrl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (imageUrl != null)
            {
                product.ImageUrl = await SaveImage(imageUrl);
            }
            await _productRepository.AddAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id, product });
        }





       

        // update product
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromForm] Product product, IFormFile imageUrl)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }
            if (imageUrl != null)
            {
                product.ImageUrl = await SaveImage(imageUrl);
            }
            else
            {
                product.ImageUrl = existingProduct.ImageUrl; // giữ nguyên ảnh cũ
            }
            // Cập nhật thông tin sản phẩm
            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Description = product.Description;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.ImageUrl = product.ImageUrl;
            await _productRepository.UpdateAsync(existingProduct);
            return NoContent(); // Trả về 204 No Content

        }

        // Xóa sản phẩm

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            await _productRepository.DeleteAsync(id);
            return NoContent(); // Trả về 204 No Content
        }

        // Hàm SaveImage sẽ được định nghĩa ở đây
        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("images", image.FileName); //  đổi đường dẫn theo cấu hình của bạn
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "images/" + image.FileName; // Trả về đường dẫn tương đối
        }
    }
}
