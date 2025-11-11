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

        // GET: api/suppliers?page=1&pageSize=5&keyword=abc
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string keyword = "")
        {
            try
            {
                var listDTO = await _service.GetAll(page, pageSize, keyword);

                var response = new ApiResponse<PagedResult<NhaCungCapDTO>>
                {
                    Success = true,
                    Message = "Lấy danh sách nhà cung cấp thành công!",
                    DataDTO = listDTO
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<NhaCungCapDTO>
                {
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpGet("getAllNCC")]
        public async Task<IActionResult> GetAllNCC()
        {
            var data = await _service.GetAllNCC();
            return Ok(new ApiResponse<List<NhaCungCapDTO>>
            {
                Success = true,
                Message = "Lấy danh sách nhà cung cấp thành công",
                DataDTO = data
            });
        }

        // GET: api/suppliers/{id}
        [HttpGet("{id:int}")]
        [ActionName("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var dto = await _service.GetById(id);
                if (dto != null)
                {
                    return Ok(new ApiResponse<NhaCungCapDTO>
                    {
                        Message = "Lấy nhà cung cấp theo ID thành công!",
                        DataDTO = dto,
                        Success = true
                    });
                }

                return NotFound(new ApiResponse<NhaCungCapDTO>
                {
                    Message = "Không tìm thấy nhà cung cấp này!",
                    Success = false
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<NhaCungCapDTO>
                {
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        // POST: api/suppliers
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NhaCungCapDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Check trùng Name/Email/Phone
                if (await _service.IsSupplierExist(dto.Name, dto.Email, dto.Phone))
                {
                    return Conflict(new ApiResponse<NhaCungCapDTO>
                    {
                        Message = "Tên/Email/Số điện thoại nhà cung cấp đã tồn tại!",
                        Success = false
                    });
                }

                var created = await _service.Create(dto);

                return CreatedAtAction(
                    "GetById",
                    new { id = created.SupplierId },
                    new ApiResponse<NhaCungCapDTO>
                    {
                        Message = "Thêm nhà cung cấp thành công!",
                        DataDTO = created,
                        Success = true
                    }
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<NhaCungCapDTO>
                {
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        // PUT: api/suppliers/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] NhaCungCapDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Tồn tại ID?
                if (!await _service.IsSupplierIdExist(id))
                {
                    return NotFound(new ApiResponse<NhaCungCapDTO>
                    {
                        Message = "Mã nhà cung cấp không tồn tại!",
                        Success = false
                    });
                }

                // Check trùng (bỏ qua chính nó)
                if (await _service.IsSupplierExist(dto.Name, dto.Email, dto.Phone, ignoreId: id))
                {
                    return Conflict(new ApiResponse<NhaCungCapDTO>
                    {
                        Message = "Tên/Email/Số điện thoại nhà cung cấp đã tồn tại!",
                        Success = false
                    });
                }

                var updated = await _service.Update(id, dto);

                return Ok(new ApiResponse<NhaCungCapDTO>
                {
                    Message = "Cập nhật nhà cung cấp thành công!",
                    DataDTO = updated,
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<NhaCungCapDTO>
                {
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        // DELETE: api/suppliers/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Tồn tại ID?
                if (!await _service.IsSupplierIdExist(id))
                {
                    return NotFound(new ApiResponse<NhaCungCapDTO>
                    {
                        Message = "Mã nhà cung cấp không tồn tại!",
                        Success = false
                    });
                }

                var ok = await _service.Delete(id);
                if (!ok)
                {
                    return BadRequest(new ApiResponse<NhaCungCapDTO>
                    {
                        Message = "Xoá thất bại!",
                        Success = false
                    });
                }

                // Xoá thành công: trả 204 giống categories
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                // trường hợp service ném lỗi vì đang bị tham chiếu
                return Conflict(new ApiResponse<NhaCungCapDTO>
                {
                    Message = ex.Message,
                    Success = false
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<NhaCungCapDTO>
                {
                    Message = ex.Message,
                    Success = false
                });
            }
        }
    }
}
