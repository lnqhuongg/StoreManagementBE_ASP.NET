using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models.Entities;
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
        public SanPhamController(ISanPhamService sanPhamService)
        {
            _sanPhamService = sanPhamService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _sanPhamService.GetAll();
            Console.WriteLine(">>> Running /api/products GetAll()");
            if (list.Count > 0)
            {
                return Ok(list);
            } else
            {
                return NoContent();
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
                    return Ok(sp);
                }
                else
                {
                    return NotFound(new { message = "Không tìm thấy loại sản phẩm này!" });
                }
            } catch(Exception e)
            {
                return BadRequest();
            }
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SanPhamDTO sp)
        {
            if(!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }
            var newSP = await _sanPhamService.Create(sp);
            if(newSP!=null)
            {
                return Ok(newSP);
            } else
            {
                return BadRequest();
            }
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SanPhamDTO sp)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }
            var updateSP = await _sanPhamService.Update(sp);
            if (updateSP != null)
            {
                return Ok(updateSP);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // THÊM RETURN
            }

            try
            {
                var result = await _sanPhamService.Delete(id);

                if (result != null && result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Lỗi server: {ex.Message}"
                });
            }
        }

        //[HttpPut("status/{id}")]
        //public IActionResult UpdateStatus(int id)
        //{
        //    try
        //    {
        //        var result = _sanPhamService.UpdateStatus(id);
        //        if (result)
        //        {
        //            return Ok(new
        //            {
        //                message = "Cập nhật trạng thái sản phẩm thành công!",
        //                success = true
        //            });
        //        }
        //        return BadRequest(new { message = "Cập nhật thất bại", success = false });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new
        //        {
        //            message = ex.Message,
        //            success = false
        //        });
        //    }
        //}

        [HttpGet("search")]
        public async Task<IActionResult> SearchByKeyword([FromQuery] string keyword)
        {
            try
            {
                var list = _sanPhamService.searchByKeyword(keyword);
                List<SanPhamDTO> ls = await list;
                if(ls.Count > 0)
                {
                    return Ok(new
                    {
                        message = "Lấy danh sách sản phẩm theo keyword thành công!",
                        success = true,
                        data = ls
                    });
                } else
                {
                    return Ok(new
                    {
                        message = "Không tìm thấy sản phẩm theo keyword!",
                        success = false
                    });
                }
            } catch (Exception e)
            {
                return BadRequest(new {
                    message = "Lỗi khi tìm sản phẩm theo keyword!",
                    success = false
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
                    return Ok(new
                    {
                        message = "Lấy danh sách sản phẩm theo category thành công!",
                        success = true,
                        data = list
                    });
                }
                else
                {
                    return Ok(new
                    {
                        message = "Không tìm thấy sản phẩm theo category!",
                        success = false
                    });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    message = "Lỗi khi tìm sản phẩm theo category!",
                    success = false
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
                    return Ok(new
                    {
                        message = "Lấy danh sách sản phẩm theo supplier thành công!",
                        success = true,
                        data = list
                    });
                }
                else
                {
                    return Ok(new
                    {
                        message = "Không tìm thấy sản phẩm theo supplier!",
                        success = false
                    });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    message = "Lỗi khi tìm sản phẩm theo supplier!" + e.Message,
                    success = false
                });
            }
        }
        [HttpGet("sort/{order}")]
        public async Task<IActionResult> getProductsSortByPrice([FromRoute]string order)
        {
            try
            {
                var list = await _sanPhamService.getProductsSortByPrice(order);
                return Ok(new
                {
                    message = "Lấy danh sách sản phẩm theo giá " + order + " thành công!",
                    data = list,
                    success = true
                });
            } catch (Exception e)
            {
                return BadRequest(new
                {
                    message = "Lỗi " + e.Message,
                    success = false
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
                return Ok(new
                {
                    message = "Lấy danh sách sản phẩm theo ncc = " + supplier_id + ", lsp = " + category_id + ", giá = " + order + "!",
                    data = list,
                    success = true
                });
            } catch(Exception e)
            {
                return BadRequest(new
                {
                    message = e.Message,
                    success = false
                });
            }
        }
    }
}
