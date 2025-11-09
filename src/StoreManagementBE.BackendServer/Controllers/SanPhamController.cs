using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StoreManagementBE.BackendServer.DTOs.SanPhamDTO;
using StoreManagementBE.BackendServer.Models.Entities;
using StoreManagementBE.BackendServer.Services;
using StoreManagementBE.BackendServer.Services.Interfaces;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace StoreManagementBE.BackendServer.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class SanPhamController : ControllerBase
    {
        public readonly ISanPhamService _sanPhamService;
        //private readonly IImageSer
        public SanPhamController(ISanPhamService sanPhamService)
        {
            _sanPhamService = sanPhamService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
                                                    [FromQuery] int? supplier_id,
                                                    [FromQuery] int? category_id,
                                                    [FromQuery] string? order, 
                                                    [FromQuery] string? keyword,
                                                    [FromQuery] int page = 1,
                                                    [FromQuery] int pageSize = 5)
        {
            try
            {
                var list = await _sanPhamService.GetAll(page, pageSize, keyword, order, category_id, supplier_id);

                var api = new ApiResponse<PagedResult<SanPhamDTO>>
                {
                    Message = "Lấy danh sách sản phẩm thành công!",
                    DataDTO = list,
                    Success = true
                };
                
                return Ok(api);
            } catch (Exception e)
            {
                return BadRequest(new ApiResponse<SanPhamDTO>
                {
                    Message = e.Message,
                    Success = false
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            
            try
            {
                var sp = await _sanPhamService.GetById(id);
                if (sp != null)
                {
                    return Ok(new ApiResponse<SanPhamDTO>
                    {
                        Message = "Lấy sản phẩm theo ID thành công!",
                        DataDTO = sp,
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
            } catch(Exception e)
            {
                return BadRequest(new ApiResponse<SanPhamDTO>
                {
                    Message = e.Message,
                    Success = false
                });
            }
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromForm] SanPhamRequestDTO sp)
        {
            
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (await _sanPhamService.checkExistBarcode(sp.Barcode))
                {
                    return Conflict(new ApiResponse<SanPhamDTO>
                    {
                        Message = "Sản phẩm trùng mã vạch!",
                        Success = false
                    });
                }

                var newSP = await _sanPhamService.Create(sp);
                return Ok(new ApiResponse<SanPhamDTO>
                { // Sửa thành Ok
                    Message = "Thêm sản phẩm thành công!",
                    DataDTO = newSP,
                    Success = true
                });
            } catch(Exception e)
            {
                return BadRequest(new ApiResponse<SanPhamDTO>
                {
                    Message = e.Message,
                    Success = false
                });
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] SanPhamRequestDTO sp)
        {
            
            try
            {
                if (await _sanPhamService.checkExistID(id) == false)
                {
                    return NotFound(new ApiResponse<SanPhamDTO>
                    {
                        Message = "Sản phẩm không tồn tại!",
                        Success = false
                    });
                } else if(await _sanPhamService.checkBarcodeExistForOtherProducts(id, sp.Barcode))
                {
                    return Conflict(new ApiResponse<SanPhamDTO>
                    {
                        Message = "Trùng mã vạch!",
                        Success = false
                    });
                }

                    var updateSP = await _sanPhamService.Update(id, sp);
                return Ok(new ApiResponse<SanPhamDTO>
                {
                    Message = "Cập nhật sản phẩm thành công!",
                    DataDTO = updateSP,
                    Success = true
                });
            } catch (Exception e)
            {
                return BadRequest(new ApiResponse<SanPhamDTO>
                {
                    Message = e.Message,
                    Success = false
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if(await _sanPhamService.checkExistID(id) == false)
                {
                    return NotFound(new ApiResponse<SanPhamDTO>
                    {
                        Message = "Sản phẩm không tồn tại!",
                        Success = false
                    });
                }
                var result = await _sanPhamService.Delete(id);

                return NoContent();
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


        
    }
}
