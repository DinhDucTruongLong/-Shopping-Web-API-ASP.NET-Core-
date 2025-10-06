using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webbanhang_dinhductruonglong.Models;
using webbanhang_dinhductruonglong.Repositories;

namespace webbanhang_dinhductruonglong.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CategoryApiController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryApiController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET: api/category
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return Ok(categories);
        }

        // GET: api/category/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return NotFound(new { message = "Category not found" });

            return Ok(category);
        }

        // POST: api/category
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Category category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _categoryRepository.AddAsync(category);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        // PUT: api/category/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Category category)
        {
            if (id != category.Id)
                return BadRequest(new { message = "ID mismatch" });

            var existing = await _categoryRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = "Category not found" });

            existing.Name = category.Name;
            await _categoryRepository.UpdateAsync(existing);
            return Ok(new { message = "Category updated successfully", category = existing });
        }

        // DELETE: api/category/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return NotFound(new { message = "Category not found" });

            await _categoryRepository.DeleteAsync(id);
            return Ok(new { message = "Category deleted successfully" });
        }
    }
}
