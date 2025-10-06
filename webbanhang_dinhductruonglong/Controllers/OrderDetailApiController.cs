using Microsoft.AspNetCore.Mvc;
using webbanhang_dinhductruonglong.Models;
using webbanhang_dinhductruonglong.Repositories;

namespace webbanhang_dinhductruonglong.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderDetailApiController : ControllerBase
    {
        private readonly IOrderDetailRepositorycs _orderDetailRepositorycs;

        public OrderDetailApiController (IOrderDetailRepositorycs orderDetailRepositorycs)
        {
            _orderDetailRepositorycs = orderDetailRepositorycs;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var details = await _orderDetailRepositorycs.GetAllAsync();
            return Ok(details);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var detail = await _orderDetailRepositorycs.GetByIdAsync(id);
            if (detail == null)
            {
                return NotFound();
            }
            return Ok(detail);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderDetail orderDetail)
        {
            await _orderDetailRepositorycs.AddAsync(orderDetail);
            return CreatedAtAction(nameof(GetById), new { id = orderDetail.Id }, orderDetail);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, OrderDetail orderDetail)
        {
            if (id != orderDetail.Id)
            {
                return BadRequest();
            }
            await _orderDetailRepositorycs.UpdateAsync(orderDetail);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete( int id)
        {
            bool success = await _orderDetailRepositorycs.DeleteAsync(id); // Gọi phương thức và lưu kết quả

            if (!success) // Kiểm tra kết quả
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
