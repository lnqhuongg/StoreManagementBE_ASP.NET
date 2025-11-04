using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models.Entities;
using StoreManagementBE.BackendServer.Services.Interfaces;

namespace StoreManagementBE.BackendServer.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class KhachHangController : ControllerBase
    {
        private readonly IKhachHangService _khService;

        public KhachHangController(IKhachHangService khService)
        {
            _khService = khService;
        }

        
        
        // ✅ GET: api/customers
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _khService.GetAll();
            return Ok(list);
        }

       
        // ✅ GET: api/customers/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _khService.GetById(id);
            if (item == null)
                return NotFound(new { message = "Không tìm thấy khách hàng này!" });

            return Ok(item);
        }

        // ✅ GET: api/customers/search?keyword=abc
        [HttpGet("search")]
        public IActionResult Search([FromQuery] string keyword)
        {
            var result = _khService.SearchByKeyword(keyword);
            return Ok(result);
        }

        // ✅ POST: api/customers
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] KhachHangDTO dto)
        {
            // 1. Validate đầu vào (Data Annotations)
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 2. Gọi Service
            var result = await _khService.Create(dto);
            // 3. Xử lý kết quả
            if (!result.Success)
            {
                // Kiểm tra loại lỗi → trả status phù hợp              
                if (result.Message.Contains("tồn tại"))
                    return Conflict(new { message = result.Message });// 409 Conflict

                return BadRequest(new { message = result.Message });// 400
            }
            // 4. Thành công → 201 Created
            var resultDto = result.DataDTO;
            return CreatedAtAction(nameof(GetById),
                new { id = resultDto.Customer_id },  // route values
                resultDto                           // body
            );
  
        }

        // ✅ PUT: api/customers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] KhachHangDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _khService.Update(id, dto);

            if (!result.Success)
            {
                if (result.Message.Contains("không tìm thấy"))
                    return NotFound(new { message = result.Message });

                return BadRequest(new { message = result.Message });
            }

            return Ok(result.DataDTO);
        }
        
    }
}
