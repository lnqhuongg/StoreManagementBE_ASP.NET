using Microsoft.AspNetCore.Mvc;
using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Services.Interfaces;

namespace StoreManagementBE.BackendServer.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class DonHangController : ControllerBase
    {
        private readonly IDonHangService _service;

        public DonHangController(IDonHangService service)
        {
            _service = service;
        }

        // GET /api/orders?page=1&pageSize=5&keyword=...&dateFrom=...&dateTo=...&minTotal=...&maxTotal=...
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] OrderFilterDTO? filter = null
        )
        {
            filter ??= new OrderFilterDTO();
            var pr = await _service.GetAll(page, pageSize, filter);

            return Ok(new ApiResponse<PagedResult<DonHangDTO>>
            {
                Success = true,
                Message = "OK",
                DataDTO = pr
            });
        }

        // GET /api/orders/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _service.GetById(id);
            if (dto == null)
            {
                return NotFound(new ApiResponse<DonHangDTO>
                {
                    Success = false,
                    Message = $"Không tìm thấy đơn hàng id={id}",
                    DataDTO = null
                });
            }

            return Ok(new ApiResponse<DonHangDTO>
            {
                Success = true,
                Message = "OK",
                DataDTO = dto
            });
        }
    }
}
