// Controllers/NhanVienController.cs
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
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // 400

            try
            {
                var result = await _service.Create(dto);
                return CreatedAtAction(
                    nameof(GetById),
                    new { id = result.UserId },  // Dùng UserId
                    result
                );
            }
            catch (Exception ex) when (ex.Message.Contains("tồn tại"))
            {
                return Conflict(new { message = ex.Message }); // 409
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message }); // 400
            }
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] NhanVienDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _service.Update(id, dto);
                if (result == null)
                    return NotFound(new { message = "Không tìm thấy nhân viên để cập nhật!" });

                return Ok(result);
            }
            catch (Exception ex) when (ex.Message.Contains("sử dụng") || ex.Message.Contains("tồn tại"))
            {
                return Conflict(new { message = ex.Message }); // 409
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message }); // 400
            }
        }

    
    }
}