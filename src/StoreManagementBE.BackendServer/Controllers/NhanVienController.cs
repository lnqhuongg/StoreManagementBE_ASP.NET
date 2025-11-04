using Microsoft.AspNetCore.Mvc;
using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Services.Interfaces;

namespace StoreManagementBE.BackendServer.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class NhanVienController : ControllerBase
    {
        private readonly INhanVienService _service;

        public NhanVienController(INhanVienService service)
        {
            _service = service;
        }

        // GET: api/users
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAll();
            return Ok(list); // 200 OK
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetById(id);
            if (item == null)
                return NotFound(new { message = "Không tìm thấy nhân viên!" });

            return Ok(item);
        }

        // GET: api/users/search?keyword=admin
        [HttpGet("search")]
        public IActionResult Search([FromQuery] string keyword)
        {
            var result = _service.SearchByKeyword(keyword);
            return Ok(result);
        }

        // POST: api/users
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NhanVienDTO dto)
        {
            // 1. Validate đầu vào
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // 400

            // 2. Gọi Service
            var result = await _service.Create(dto);

            // 3. Xử lý kết quả
            if (!result.Success)
            {
                if (result.Message.Contains("tồn tại"))
                    return Conflict(new { message = result.Message }); // 409

                return BadRequest(new { message = result.Message }); // 400
            }

            // 4. Thành công → 201 Created
            var resultDto = result.DataDTO;
            return CreatedAtAction(
                nameof(GetById),
                new { id = resultDto!.UserId },  // Dùng UserId
                resultDto
            );
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] NhanVienDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.Update(id, dto);

            if (!result.Success)
            {
                if (result.Message.Contains("không tìm thấy", StringComparison.OrdinalIgnoreCase))
                    return NotFound(new { message = result.Message });

                if (result.Message.Contains("tồn tại") || result.Message.Contains("sử dụng"))
                    return Conflict(new { message = result.Message });

                return BadRequest(new { message = result.Message });
            }

            return Ok(result.DataDTO);
        }
    }
}