using Microsoft.AspNetCore.Mvc;
using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Services.Interfaces;

namespace StoreManagementBE.BackendServer.Controllers
{
    [ApiController]
    [Route("api/suppliers")]
    public class NhaCungCapController : ControllerBase
    {
        private readonly INhaCungCapService _service;

        public NhaCungCapController(INhaCungCapService service)
        {
            _service = service;
        }

        // GET: api/suppliers
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAll();
            return Ok(data);    //return status 200
        }

        // GET: api/suppliers/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _service.GetById(id);
            if (data == null) return NotFound(new { message = "Không tìm thấy nhà cung cấp!" });
            return Ok(data);
        }

        // GET: api/suppliers/search?keyword=abc
        [HttpGet("search")]
        public IActionResult Search([FromQuery] string keyword)
        {
            var result = _service.SearchByKeyword(keyword);
            return Ok(result);
        }

        // POST: api/suppliers
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NhaCungCapDTO dto)
        {
            // 1. Validate đầu vào (Data Annotations)
            if (!ModelState.IsValid) return BadRequest(ModelState);     //Status 400

            // 2. Gọi Service để tạo mới
            var result = await _service.Create(dto);

            //3. Xử lý kết quả trả về từ Service
            if (!result.Success)
            {
                if (result.Message.Contains("tồn tại"))
                    return Conflict(new { message = result.Message });  // 409

                return BadRequest(new { message = result.Message });    // 400
            }

            // 4. Thành công → 201 Created
            return CreatedAtAction(nameof(GetById), 
                new { id = result.DataDTO!.Supplier_id },   //route value
                result.DataDTO);                            //body
        }

        // PUT: api/suppliers/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] NhaCungCapDTO dto)
        {
            // 1. Validate đầu vào (Data Annotations)
            if (!ModelState.IsValid) return BadRequest(ModelState);     //return status 400

            // 2. Gọi Service để update
            var result = await _service.Update(id, dto);

            //3. Xử lý kết quả trả về từ Service
            if (!result.Success)
            {
                if (result.Message.Contains("không tìm thấy", StringComparison.OrdinalIgnoreCase))
                    return NotFound(new { message = result.Message });  //return status 404
                if (result.Message.Contains("đã được dùng", StringComparison.OrdinalIgnoreCase))
                    return Conflict(new { message = result.Message });  //return status 409
                return BadRequest(new { message = result.Message });    //return status 400
            }

            // 4. Thành công → 200 Update
            return Ok(result.DataDTO);  
        }
    }
}
