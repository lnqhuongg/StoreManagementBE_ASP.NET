using Microsoft.AspNetCore.Mvc;
using StoreManagementBE.BackendServer.DTOs;
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

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string keyword = "")
        {
            try
            {
                var result = await _khService.GetAll(page, pageSize, keyword);

                if (result.Data.Count == 0)
                    return NoContent();

                return Ok(new ApiResponse<PagedResult<KhachHangDTO>>
                {
                    Message = "Lấy danh sách khách hàng thành công!",
                    DataDTO = result,
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var kh = await _khService.GetById(id);
                if (kh == null)
                {
                    return NotFound(new ApiResponse<KhachHangDTO>
                    {
                        Message = "Không tìm thấy khách hàng!",
                        Success = false
                    });
                }

                return Ok(new ApiResponse<KhachHangDTO>
                {
                    Message = "Lấy khách hàng theo ID thành công!",
                    DataDTO = kh,
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<KhachHangDTO>
                {
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] KhachHangDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                // Kiểm tra trùng Phone
                if (!string.IsNullOrEmpty(dto.Phone) && await _khService.IsPhoneExist(dto.Phone))
                {
                    return Conflict(new ApiResponse<KhachHangDTO>
                    {
                        Message = "Số điện thoại đã tồn tại!",
                        Success = false
                    });
                }

                // Kiểm tra trùng Email
                if (!string.IsNullOrEmpty(dto.Email) && await _khService.IsEmailExist(dto.Email))
                {
                    return Conflict(new ApiResponse<KhachHangDTO>
                    {
                        Message = "Email đã tồn tại!",
                        Success = false
                    });
                }

                var result = await _khService.Create(dto);

                return Ok(new ApiResponse<KhachHangDTO>
                {
                    Message = "Thêm khách hàng thành công!",
                    DataDTO = result,
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<KhachHangDTO>
                {
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] KhachHangDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                if (!await _khService.IsCustomerExist(id))
                {
                    return NotFound(new ApiResponse<KhachHangDTO>
                    {
                        Message = "Mã khách hàng không tồn tại!",
                        Success = false
                    });
                }

                // ĐÃ XÓA: kiểm tra trùng Phone/Email vì frontend KHÔNG CHO SỬA

                var updated = await _khService.Update(id, dto); // ĐÃ SỬA: truyền id

                if (updated == null)
                    return NotFound();

                return Ok(new ApiResponse<KhachHangDTO>
                {
                    Message = "Cập nhật khách hàng thành công!",
                    DataDTO = updated,
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<KhachHangDTO>
                {
                    Message = ex.Message,
                    Success = false
                });
            }
        }
    }
}