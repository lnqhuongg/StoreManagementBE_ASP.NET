using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _sanPhamService.GetAll();
                Console.WriteLine(">>> Running /api/products GetAll()");
                if (list.Count > 0)
                {
                    var api = new ApiResponse<List<SanPhamDTO>>
                    {
                        Message = "Lấy danh sách sản phẩm thành công!",
                        DataDTO = list,
                        Success = true
                    };
                    return Ok(api);
                }
                else
                {
                    return NoContent();
                }
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


        [HttpGet("search")]
        public async Task<IActionResult> SearchByKeyword([FromQuery] string keyword)
        {
            try
            {
                var list = _sanPhamService.searchByKeyword(keyword);
                List<SanPhamDTO> ls = await list;
                if(ls.Count > 0)
                {
                    return Ok(new ApiResponse<List<SanPhamDTO>>
                    {
                        Message = "Lấy danh sách sản phẩm theo keyword thành công!",
                        Success = true,
                        DataDTO = ls
                    });
                } else
                {
                    return NoContent();
                }
            } catch (Exception e)
            {
                return BadRequest(new ApiResponse<SanPhamDTO>
                {
                    Message = e.Message,
                    Success = false
                });
            }
        }

        [HttpGet("category/{category_id}")]
        public async Task<IActionResult> GetByCategory([FromRoute] int category_id)
        {
            try
            {
                var list = await _sanPhamService.getByCategoryID(category_id);
                if (list.Count > 0)
                {
                    return Ok(new ApiResponse<List<SanPhamDTO>>
                    {
                        Message = "Lấy danh sách sản phẩm theo category thành công!",
                        Success = true,
                        DataDTO = list
                    });
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<SanPhamDTO>
                {
                    Message = e.Message,
                    Success = false
                });
            }
        }

        [HttpGet("supplier/{supplier_id}")]
        public async Task<IActionResult> GetBySupplier([FromRoute] int supplier_id)
        {
            try
            {
                var list = await _sanPhamService.getBySupplierID(supplier_id);
                if (list.Count > 0)
                {
                    return Ok(new ApiResponse<List<SanPhamDTO>>
                    {
                        Message = "Lấy danh sách sản phẩm theo supplier thành công!",
                        Success = true,
                        DataDTO = list
                    });
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<SanPhamDTO>
                {
                    Message = e.Message,
                    Success = false
                });
            }
        }
        [HttpGet("sort/{order}")]
        public async Task<IActionResult> getProductsSortByPrice([FromRoute]string order)
        {
            try
            {
                var list = await _sanPhamService.getProductsSortByPrice(order);
                return Ok(new ApiResponse<List<SanPhamDTO>>
                {
                    Message = "Lấy danh sách sản phẩm theo giá " + order + " thành công!",
                    DataDTO = list,
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
        [HttpGet("advanced_search")]
        public async Task<IActionResult> getProudctsBysupplierIDAndCategoryIDAndPrice([FromQuery] int? supplier_id, 
                                                                        [FromQuery] int? category_id, 
                                                                        [FromQuery] string? order)
        {
            try
            {
                var list = new List<SanPhamDTO>(); ;
                if(supplier_id.HasValue)
                {
                    if(category_id.HasValue)
                    {
                        if(order != "")
                        {
                            list = await _sanPhamService.getProductsBysupplierIDAndCategoryIDAndPrice(supplier_id, category_id, order);
                        } else
                        {
                            list = await _sanPhamService.getProductsBySupplierIDAndCategoryID(supplier_id, category_id);
                        }
                    } else
                    {
                        if (order != "")
                        {
                            list = await _sanPhamService.getProductsBySupplierIDAndPrice(supplier_id, order);
                        }
                        else
                        {
                            list = await _sanPhamService.getBySupplierID(supplier_id);
                        }
                    }
                } else
                {
                    if (category_id.HasValue)
                    {
                        if (order != "")
                        {
                            list = await _sanPhamService.getProductsByCategoryIDAndPrice(category_id, order);
                        }
                        else
                        {
                            list = await _sanPhamService.getByCategoryID(category_id);
                        }
                    }
                    else
                    {
                        if (order != "")
                        {
                            list = await _sanPhamService.getProductsSortByPrice(order);
                        }
                        else
                        {
                            list = await _sanPhamService.GetAll();
                        }
                    }
                }
                return Ok(new ApiResponse<List<SanPhamDTO>>
                {
                    Message = "Lấy danh sách sản phẩm theo ncc = " + supplier_id + ", lsp = " + category_id + ", giá = " + order + "!",
                    DataDTO = list,
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
    }
}
