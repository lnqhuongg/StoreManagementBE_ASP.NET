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
            return Ok(new ApiResponse<List<NhaCungCapDTO>>
            {
                Success = true,
                Message = "Lấy danh sách nhà cung cấp thành công",
                DataDTO = data
            });
        }

        // GET: api/suppliers/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var found = await _service.GetById(id);
            if (found == null)
                return NotFound(new ApiResponse<NhaCungCapDTO> { Success = false, Message = "Không tìm thấy nhà cung cấp!" });

            return Ok(new ApiResponse<NhaCungCapDTO>
            {
                Success = true,
                Message = "Lấy chi tiết nhà cung cấp thành công",
                DataDTO = found
            });
        }

        // GET: api/suppliers/search?keyword=...
        [HttpGet("search")]
        public IActionResult Search([FromQuery] string keyword)
        {
            var list = _service.SearchByKeyword(keyword);
            var data = list.Select(x => new NhaCungCapDTO
            {
                SupplierId = x.SupplierId,
                Name = x.Name,
                Phone = x.Phone,
                Email = x.Email,
                Address = x.Address,
                Status = x.Status
            }).ToList();

            return Ok(new ApiResponse<List<NhaCungCapDTO>>
            {
                Success = true,
                Message = "Tìm kiếm nhà cung cấp thành công",
                DataDTO = data
            });
        }

        // POST: api/suppliers
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NhaCungCapDTO dto)
        {
            try { 
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _service.IsSupplierExist(dto.Name, dto.Email, dto.Phone))
                {
                    return Conflict(new ApiResponse<NhaCungCapDTO>
                    {
                        Success = false,
                        Message = "Nhà cung cấp đã tồn tại (trùng tên/email/sđt)."
                    });
                }

                var created = await _service.Create(dto);

                return CreatedAtAction(nameof(GetById), new { id = created.SupplierId },
                    new ApiResponse<NhaCungCapDTO>
                    {
                        Success = true,
                        Message = "Tạo nhà cung cấp thành công",
                        DataDTO = created
                    });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<NhaCungCapDTO>
                {
                    Success = false,
                    Message = "Lỗi khi tạo nhà cung cấp: " + ex.Message
                });
            }
        }

        // PUT: api/suppliers/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] NhaCungCapDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!await _service.IsSupplierIdExist(id))
                {
                    return NotFound(new ApiResponse<NhaCungCapDTO>
                    {
                        Success = false,
                        Message = "Không tìm thấy nhà cung cấp!"
                    });
                }

                if (await _service.IsSupplierExist(dto.Name, dto.Email, dto.Phone, ignoreId: id))
                {
                    return Conflict(new ApiResponse<NhaCungCapDTO>
                    {
                        Success = false,
                        Message = "Nhà cung cấp đã tồn tại (trùng tên/email/sđt)."
                    });
                }

                var updatedData = await _service.Update(id, dto);
                // service trả null nếu không tồn tại → đã check trước rồi
                return Ok(new ApiResponse<NhaCungCapDTO>
                {
                    Success = true,
                    Message = "Cập nhật nhà cung cấp thành công",
                    DataDTO = updatedData
                });
            }   
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<NhaCungCapDTO>
                {
                    Success = false,
                    Message = "Lỗi khi cập nhật nhà cung cấp: " + ex.Message
                });
            }
        }

        // DELETE: api/suppliers/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!await _service.IsSupplierIdExist(id))
            {
                return NotFound(new ApiResponse<NhaCungCapDTO>
                {
                    Success = false,
                    Message = "Không tìm thấy nhà cung cấp!"
                });
            }

            var ok = await _service.Delete(id);
            if (!ok)
            {
                return BadRequest(new ApiResponse<NhaCungCapDTO>
                {
                    Success = false,
                    Message = "Xoá nhà cung cấp thất bại."
                });
            }

            return NoContent();
        }
    }
}
