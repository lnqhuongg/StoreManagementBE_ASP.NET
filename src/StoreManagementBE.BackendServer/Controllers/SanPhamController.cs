using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Helpers;
using StoreManagementBE.BackendServer.Models.Entities;
using StoreManagementBE.BackendServer.Services.Interfaces;
using System.Collections.Generic;

namespace StoreManagementBE.BackendServer.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class SanPhamController : Controller
    {
        public readonly ISanPhamService _sanPhamService;
        public SanPhamController(ISanPhamService sanPhamService)
        {
            _sanPhamService = sanPhamService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<SanPham> list = _sanPhamService.GetAll();
            if(list.Count > 0)
            {
                var result = list.Select(sp => new SanPhamDTO
                {
                    product_id = sp.product_id,
                    Category = sp.Category == null ? null : new LoaiSanPhamDTO { category_id = sp.Category.category_id, category_name = sp.Category.category_name },
                    Supplier = sp.Supplier == null ? null : new NhaCungCapDTO { supplier_id = sp.Supplier.supplier_id, name = sp.Supplier.name, address = sp.Supplier.address, email = sp.Supplier.email, phone = sp.Supplier.phone, status = sp.Supplier.status },
                    product_name = sp.product_name,
                    barcode = sp.barcode,
                    price = sp.price,
                    unit = sp.unit.GetDisplayName(), // <-- trả tiếng Việt
                    created_at = sp.created_at,
                    status = sp.status
                });
                return Ok(new
                {
                    message = "Lấy danh sách sản phẩm thành công!",
                    data = result
                });
            } else
            {
                return NoContent();
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            SanPham sp = _sanPhamService.GetById(id);
            if (sp != null)
            {
                var result = new SanPhamDTO
                {
                    product_id = sp.product_id,
                    Category = sp.Category == null ? null : new LoaiSanPhamDTO { category_id = sp.Category.category_id, category_name = sp.Category.category_name },
                    Supplier = sp.Supplier == null ? null : new NhaCungCapDTO { supplier_id = sp.Supplier.supplier_id, name = sp.Supplier.name, address = sp.Supplier.address, email = sp.Supplier.email, phone = sp.Supplier.phone, status = sp.Supplier.status },
                    product_name = sp.product_name,
                    barcode = sp.barcode,
                    price = sp.price,
                    unit = sp.unit.GetDisplayName(), // <-- trả tiếng Việt
                    created_at = sp.created_at,
                    status = sp.status
                };

                return Ok(new
                {
                    message = "Lấy sản phẩm theo id thành công!",
                    success = true,
                    data = result
                });
            }
            else
            {
                return NotFound(new
                {
                    message = "Không tìm thấy sản phẩm theo id!",
                    success = false
                });
            }
        }


        [HttpPost]
        public IActionResult Create([FromBody] SanPhamDTO sp)
        {
            try
            {
                if (_sanPhamService.Create(sp))
                {
                    return Ok(new
                    {
                        message = "Tạo sản phẩm thành công!",
                        success = true
                    });
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    success = false
                });
            }
        }
        [HttpPut]
        public IActionResult Update([FromBody] SanPhamDTO sp)
        {
            try
            {
                if (_sanPhamService.Update(sp))
                {
                    return Ok(new
                    {
                        message = "Cập nhật sản phẩm thành công!",
                        success = true
                    });
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    success = false
                });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (_sanPhamService.Delete(id))
                {
                    return Ok(new
                    {
                        message = "Xóa sản phẩm thành công!",
                        success = true
                    });
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    success = false
                });
            }
        }

        [HttpPut("status/{id}")]
        public IActionResult UpdateStatus(int id)
        {
            try
            {
                var result = _sanPhamService.UpdateStatus(id);
                if (result)
                {
                    return Ok(new
                    {
                        message = "Cập nhật trạng thái sản phẩm thành công!",
                        success = true
                    });
                }
                return BadRequest(new { message = "Cập nhật thất bại", success = false });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    success = false
                });
            }
        }

        [HttpGet("search")]
        public IActionResult SearchByKeyword([FromQuery] string keyword)
        {
            try
            {
                List<SanPham> list = _sanPhamService.searchByKeyword(keyword);
                if(list.Count > 0)
                {
                    return Ok(new
                    {
                        message = "Lấy danh sách sản phẩm theo keyword thành công!",
                        success = true,
                        data = list
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
        public IActionResult GetByCategory([FromRoute] int category_id)
        {
            try
            {
                List<SanPham> list = _sanPhamService.getByCategoryID(category_id);
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
        public IActionResult GetBySupplier([FromRoute] int supplier_id)
        {
            try
            {
                List<SanPham> list = _sanPhamService.getBySupplierID(supplier_id);
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
                    message = "Lỗi khi tìm sản phẩm theo supplier!",
                    success = false
                });
            }
        }
    }
}
