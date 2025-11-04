using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models.Entities;
using StoreManagementBE.BackendServer.Services.Interfaces;

namespace StoreManagementBE.BackendServer.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class LoaiSanPhamController : ControllerBase
    {
        private readonly ILoaiSanPhamService _loaiSpService;


        public LoaiSanPhamController(ILoaiSanPhamService loaiSpService)
        {
            _loaiSpService = loaiSpService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _loaiSpService.GetAll();
            return Ok(list); // tra ve status 200
        }

        // ✅ GET: api/categories/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _loaiSpService.GetById(id);
            if (item == null)
                return NotFound(new { message = "Không tìm thấy loại sản phẩm này!" });
            return Ok(item);
        }

        // ✅ GET: api/categories/search?keyword=TV
        [HttpGet("search")]
        public IActionResult Search([FromQuery] string keyword)
        {
            var result = _loaiSpService.SearchByKeyword(keyword);
            return Ok(result);
        }

        // POST: api/categories
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LoaiSanPhamDTO dto)
        {
            // 1. Validate đầu vào (Data Annotations)
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // 400

            // 2. Gọi Service
            var result = await _loaiSpService.Create(dto);

            // 3. Xử lý kết quả
            if (!result.Success)
            {
                // Kiểm tra loại lỗi → trả status phù hợp
                if (result.Message.Contains("tồn tại"))
                    return Conflict(new { message = result.Message }); // 409 Conflict

                return BadRequest(new { message = result.Message }); // 400
            }

            // 4. Thành công → 201 Created
            var resultDto = result.DataDTO;
            return CreatedAtAction(nameof(GetById),
                new { id = resultDto.CategoryId },  // route values
                resultDto                           // body
            );
        }

        // ✅ PUT: api/categories/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LoaiSanPhamDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _loaiSpService.Update(id, dto);

            if (!result.Success)
            {
                if (result.Message.Contains("không tìm thấy", StringComparison.OrdinalIgnoreCase))
                    return NotFound(new { message = result.Message });

                return BadRequest(new { message = result.Message });
            }

            return Ok(result.DataDTO);
        }

        // ✅ DELETE: api/loaisanpham/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _loaiSpService.Delete(id);

            if (!result.Success)
            {
                if (result.Message.Contains("không tìm thấy", StringComparison.OrdinalIgnoreCase))
                    return NotFound(new { message = result.Message });

                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }
    }
}
