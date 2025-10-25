using Microsoft.AspNetCore.Mvc;
using StoreManagementBE.BackendServer.Models.Entities;
using StoreManagementBE.BackendServer.Services.Interfaces;

namespace StoreManagementBE.BackendServer.Controllers
{
    [ApiController]
    [Route("api/suppliers")]
    public class NhaCungCapController : ControllerBase
    {
        private readonly INhaCungCapService _supplierService;

        public NhaCungCapController(INhaCungCapService supplierService)
        {
            _supplierService = supplierService;
        }

        // ✅ GET: api/suppliers
        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _supplierService.GetAll();
            return Ok(list);
        }

        // ✅ GET: api/suppliers/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = _supplierService.GetById(id);
            if (item == null)
                return NotFound(new { message = "Không tìm thấy nhà cung cấp!" });
            return Ok(item);
        }

        // ✅ GET: api/suppliers/search?keyword=ABC
        [HttpGet("search")]
        public IActionResult Search([FromQuery] string keyword)
        {
            var result = _supplierService.SearchByKeyword(keyword);
            return Ok(result);
        }

        // ✅ POST: api/suppliers
        [HttpPost]
        public IActionResult Create([FromBody] NhaCungCap supplier)
        {
            var success = _supplierService.Create(supplier);
            if (!success)
                return BadRequest(new { message = "Thêm nhà cung cấp thất bại!" });
            return Ok(new { message = "Thêm nhà cung cấp thành công!" });
        }

        // ✅ PUT: api/suppliers/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] NhaCungCap supplier)
        {
            supplier.supplier_id = id;
            var success = _supplierService.Update(supplier);
            if (!success)
                return NotFound(new { message = "Cập nhật thất bại, không tìm thấy nhà cung cấp!" });
            return Ok(new { message = "Cập nhật thành công!" });
        }

        // ✅ DELETE: api/suppliers/{id}
        //[HttpDelete("{id}")]
        //public IActionResult Delete(int id)
        //{
        //    var success = _supplierService.Delete(id);
        //    if (!success)
        //        return NotFound(new { message = "Xóa thất bại, không tìm thấy nhà cung cấp!" });
        //    return Ok(new { message = "Xóa thành công!" });
        //}
    }
}
