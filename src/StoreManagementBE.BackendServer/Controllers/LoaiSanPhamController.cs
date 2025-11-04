using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Services.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _loaiSpService.GetAll();
                if (list.Count > 0)
                {
                    var api = new ApiResponse<List<LoaiSanPhamDTO>>
                    {
                        Message = "Lấy danh sách sản phẩm thành công!",
                        DataDTO = list,
                        Success = true
                    };
                    return Ok(api);
                }
                else return NoContent();
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

                return CreatedAtAction("Tạo sản phẩm thành công", new ApiResponse<LoaiSanPhamDTO>
                {
                    Message = "Thêm sản phẩm thành công!",
                    DataDTO = loaiDTO,
                    Success = true
                });
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
                    return NotFound(new ApiResponse<SanPhamDTO>
                    {
                        Message = "Mã loại sản phẩm không tồn tại!",
                        Success = false
                    });
                }

                var loaiDTO = await _loaiSpService.Update(id, loaiSanPhamDTO);

                return Ok(new ApiResponse<LoaiSanPhamDTO>
                {
                    Message = "Cập nhật sản phẩm thành công!",
                    DataDTO = loaiDTO,
                    Success = true
                });
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

                if (await _loaiSpService.isCategoryExist(id))
                {
                    return NotFound(new ApiResponse<SanPhamDTO>
                    {
                        Message = "Mã loại sản phẩm không tồn tại!",
                        Success = false
                    });
                }

                var result = _loaiSpService.Delete(id);

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
