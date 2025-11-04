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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAll();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetById(id);
            if (item == null)
                return NotFound(new { message = "Không tìm thấy nhân viên!" });

            return Ok(item);
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string keyword)
        {
            var result = _service.SearchByKeyword(keyword);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NhanVienDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.Create(dto);

            if (!result.Success)
            {
                if (result.Message.Contains("tồn tại"))
                    return Conflict(new { message = result.Message });

                return BadRequest(new { message = result.Message });
            }

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.DataDTO!.UserId},
                result.DataDTO
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] NhanVienDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.Update(id, dto);

            if (!result.Success)
            {
                if (result.Message.Contains("không tìm thấy"))
                    return NotFound(new { message = result.Message });

                if (result.Message.Contains("tồn tại") || result.Message.Contains("sử dụng"))
                    return Conflict(new { message = result.Message });

                return BadRequest(new { message = result.Message });
            }

            return Ok(result.DataDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.Delete(id);

            if (!result.Success)
            {
                if (result.Message.Contains("không tìm thấy"))
                    return NotFound(new { message = result.Message });

                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }
    }
}