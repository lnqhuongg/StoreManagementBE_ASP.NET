using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Services.Interfaces;

namespace StoreManagementBE.BackendServer.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class DonHangController : ControllerBase
    {
        private readonly IDonHangService _service;
        private readonly ILogger<DonHangController> _logger;

        public DonHangController(IDonHangService service, ILogger<DonHangController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET /api/orders
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _service.GetAll();
                var resp = new ApiResponse<List<DonHangDTO>>
                {
                    Success = true,
                    Message = "Lấy danh sách đơn hàng thành công",
                    DataDTO = data
                };
                return Ok(resp); // 200
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET /api/orders failed");
                var resp = new ApiResponse<List<DonHangDTO>>
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi hệ thống khi lấy danh sách đơn hàng.",
                    DataDTO = null
                };
                return StatusCode(500, resp); // 500
            }
        }

        // GET /api/orders/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                if (id <= 0)
                {
                    var bad = new ApiResponse<DonHangDTO>
                    {
                        Success = false,
                        Message = "Id không hợp lệ.",
                        DataDTO = null
                    };
                    return BadRequest(bad); // 400
                }

                var data = await _service.GetById(id);
                if (data == null)
                {
                    var notFound = new ApiResponse<DonHangDTO>
                    {
                        Success = false,
                        Message = $"Không tìm thấy đơn hàng với id = {id}",
                        DataDTO = null
                    };
                    return NotFound(notFound); // 404
                }

                var resp = new ApiResponse<DonHangDTO>
                {
                    Success = true,
                    Message = "Lấy chi tiết đơn hàng thành công",
                    DataDTO = data
                };
                return Ok(resp); // 200
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET /api/orders/{id} failed. id={Id}", id);
                var resp = new ApiResponse<DonHangDTO>
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi hệ thống khi lấy chi tiết đơn hàng.",
                    DataDTO = null
                };
                return StatusCode(500, resp); // 500
            }
        }
    }
}
