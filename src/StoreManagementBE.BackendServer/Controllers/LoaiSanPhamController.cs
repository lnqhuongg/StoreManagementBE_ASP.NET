using Microsoft.AspNetCore.Mvc;
using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.DTOs.SanPhamDTO;
using StoreManagementBE.BackendServer.Services.Interfaces;

namespace StoreManagementBE.BackendServer.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class LoaiSanPhamController : ControllerBase
    {
        private readonly ILoaiSanPhamService _loaiSpService;


        public LoaiSanPhamController(ILoaiSanPhamService loaiSpService)
        {
            _loaiSpService = loaiSpService;
        }

        // lay tat ca
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 5, string keyword = "")
        {
            try
            {
                // gọi service lấy 5 cái bản ghi theo số trang 
                var listDTO = await _loaiSpService.GetAll(page, pageSize, keyword);

                var response = new ApiResponse<PagedResult<LoaiSanPhamDTO>>
                {
                    Success = true,
                    Message = "Lấy danh sách loại sản phẩm thành công!",
                    DataDTO = listDTO
                };

                return Ok(response);
            } 
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<SanPhamDTO>
                {
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        // lay theo id
        [HttpGet("{id}")]
        [ActionName("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var loai = await _loaiSpService.GetById(id);
                if (loai != null)
                {
                    return Ok(new ApiResponse<LoaiSanPhamDTO>
                    {
                        Message = "Lấy loại sản phẩm theo ID thành công!",
                        DataDTO = loai,
                        Success = true
                    });
                }
                else
                {
                    return NotFound(new ApiResponse<SanPhamDTO>
                    {
                        Message = "Không tìm thấy loại sản phẩm này!",
                        Success = false
                    });
                }
            } 
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<LoaiSanPhamDTO>
                {
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        // them moi 
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LoaiSanPhamDTO loaiSanPhamDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if(await _loaiSpService.isCategoryNameExist(loaiSanPhamDTO.CategoryName)) {
                    return Conflict(new ApiResponse<SanPhamDTO>
                    {
                        Message = "Tên loại sản phẩm đã tồn tại!",
                        Success = false
                    });
                }

                var loaiDTO = await _loaiSpService.Create(loaiSanPhamDTO);

                return CreatedAtAction(
                    "GetById",
                    new { id = loaiDTO.CategoryId }, // THIẾU DÒNG NÀY → LỖI
                    new ApiResponse<LoaiSanPhamDTO>
                    {
                        Message = "Thêm sản phẩm thành công!",
                        DataDTO = loaiDTO,
                        Success = true
                    }
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<LoaiSanPhamDTO>
                {
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        // ✅ PUT: api/categories/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LoaiSanPhamDTO loaiSanPhamDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _loaiSpService.isCategoryExist(id))
                {
                    
                    var loaiDTO = await _loaiSpService.Update(id, loaiSanPhamDTO);

                    if(await _loaiSpService.isCategoryNameExist(loaiSanPhamDTO.CategoryName, id)) {
                        return Conflict(new ApiResponse<SanPhamDTO>
                        {
                            Message = "Tên loại sản phẩm đã tồn tại!",
                            Success = false
                        });
                    }

                    return Ok(new ApiResponse<LoaiSanPhamDTO>
                    {
                        Message = "Cập nhật sản phẩm thành công!",
                        DataDTO = loaiDTO,
                        Success = true
                    });
                } else
                {
                    return NotFound(new ApiResponse<SanPhamDTO>
                    {
                        Message = "Mã loại sản phẩm không tồn tại!",
                        Success = false
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<LoaiSanPhamDTO>
                {
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        // ✅ DELETE: api/loaisanpham/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var exists = await _loaiSpService.isCategoryExist(id);
                if (!exists)
                {
                    return NotFound(new ApiResponse<LoaiSanPhamDTO>
                    {
                        Message = "Mã loại sản phẩm không tồn tại!",
                        Success = false
                    });
                }

                var result = await _loaiSpService.Delete(id); // AWAIT ĐÂY

                if (!result)
                {
                    return BadRequest(new ApiResponse<LoaiSanPhamDTO>
                    {
                        Message = "Xóa thất bại!",
                        Success = false
                    });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<LoaiSanPhamDTO>
                {
                    Message = ex.Message,
                    Success = false
                });
            }
        }
    }
}
