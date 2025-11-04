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
            try
            {
                var list = await _service.GetAll();
                if (list.Count > 0)
                    return Ok(new ApiResponse<List<NhaCungCapDTO>>
                    {
                        Success = true,
                        Message = "Lấy danh sách nhà cung cấp thành công!",
                        DataDTO = list
                    });
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<NhaCungCapDTO> { Success = false, Message = ex.Message });
            }
        }

        // GET: api/suppliers/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var data = await _service.GetById(id);
                if (data == null)
                    return NotFound(new ApiResponse<NhaCungCapDTO> { Success = false, Message = "Không tìm thấy nhà cung cấp!" });

                return Ok(new ApiResponse<NhaCungCapDTO>
                {
                    Success = true,
                    Message = "Lấy nhà cung cấp theo ID thành công!",
                    DataDTO = data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<NhaCungCapDTO> { Success = false, Message = ex.Message });
            }
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
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var result = await _service.Create(dto);
                if (!result.Success)
                {
                    if (result.Message.Contains("tồn tại", StringComparison.OrdinalIgnoreCase))
                        return Conflict(new { message = result.Message });
                    return BadRequest(new { message = result.Message });
                }

                return CreatedAtAction(nameof(GetById),
                    new { id = result.DataDTO!.SupplierId },
                    new ApiResponse<NhaCungCapDTO>
                    {
                        Success = true,
                        Message = "Thêm nhà cung cấp thành công!",
                        DataDTO = result.DataDTO
                    });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<NhaCungCapDTO> { Success = false, Message = ex.Message });
            }
        }

        // PUT: api/suppliers/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] NhaCungCapDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                if (!await _service.IsSupplierIdExist(id))
                    return NotFound(new ApiResponse<NhaCungCapDTO> { Success = false, Message = "Mã nhà cung cấp không tồn tại!" });

                var result = await _service.Update(id, dto);
                if (!result.Success)
                {
                    if (result.Message.Contains("đã được dùng", StringComparison.OrdinalIgnoreCase))
                        return Conflict(new { message = result.Message });
                    return BadRequest(new { message = result.Message });
                }

                return Ok(new ApiResponse<NhaCungCapDTO>
                {
                    Success = true,
                    Message = "Cập nhật nhà cung cấp thành công!",
                    DataDTO = result.DataDTO
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<NhaCungCapDTO> { Success = false, Message = ex.Message });
            }
        }

        // DELETE: api/suppliers/{id}
        //[HttpDelete("{id:int}")]
        //public IActionResult Delete(int id)
        //{
        //    try
        //    {
        //        var ok = _service.Delete(id);
        //        if (!ok)
        //            return NotFound(new ApiResponse<NhaCungCapDTO> { Success = false, Message = "Không tìm thấy nhà cung cấp!" });

        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ApiResponse<NhaCungCapDTO> { Success = false, Message = ex.Message });
        //    }
        //}
    }
}
