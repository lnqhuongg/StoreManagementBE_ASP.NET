using Microsoft.AspNetCore.Mvc;
using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Services.Interfaces;
namespace StoreManagementBE.BackendServer.Controllers
{
    [ApiController]
    [Route("api/promocodes")]
    public class MaGiamGiaController : ControllerBase
    {
        private readonly IMaGiamGiaService _service;

        public MaGiamGiaController(IMaGiamGiaService service)
        {
            _service = service;
        }

        // GET: api/promocodes?page=1&pageSize=10&keyword=...&discountType=...
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? keyword = null,
            [FromQuery] string? discountType = null)
        {
            var result = await _service.GetAll(page, pageSize, keyword, discountType);
            return Ok(new ApiResponse<PagedResult<MaGiamGiaDTO>>
            {
                Success = true,
                Message = "Lấy danh sách mã giảm giá thành công!",
                DataDTO = result
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _service.GetById(id);
            if (dto == null)
                return NotFound(new ApiResponse<MaGiamGiaDTO> { Success = false, Message = "Không tìm thấy mã giảm giá!" });

            return Ok(new ApiResponse<MaGiamGiaDTO>
            {
                Success = true,
                Message = "Lấy chi tiết mã giảm giá thành công!",
                DataDTO = dto
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MaGiamGiaDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _service.Create(dto);
            if (!result.Success) return Conflict(result);
            return CreatedAtAction(nameof(GetById), new { id = result.DataDTO?.PromoId }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MaGiamGiaDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _service.Update(id, dto);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.Delete(id);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }

        // GET: api/promocodes/search?keyword=...
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            var result = await _service.SearchByKeyword(keyword);
            return Ok(new ApiResponse<List<MaGiamGiaDTO>>
            {
                Success = true,
                Message = "Tìm kiếm mã giảm giá thành công!",
                DataDTO = result
            });
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetAllActive()
        {
            var result = await _service.GetAllActive();
            return Ok(new ApiResponse<List<MaGiamGiaDTO>>
            {
                Success = true,
                Message = "Lấy danh sách mã giảm giá đang hoạt động thành công!",
                DataDTO = result
            });
        }
    }
}