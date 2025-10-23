using Microsoft.AspNetCore.Mvc;
using StoreManagementBE.BackendServer.Models.Entities;
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

        // ✅ GET: api/categories
        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _loaiSpService.GetAll();
            return Ok(list);
        }

        // ✅ GET: api/loaisanpham/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = _loaiSpService.GetById(id);
            if (item == null)
                return NotFound(new { message = "Không tìm thấy loại sản phẩm này!" });
            return Ok(item);
        }

        // ✅ GET: api/loaisanpham/search?keyword=TV
        [HttpGet("search")]
        public IActionResult Search([FromQuery] string keyword)
        {
            var result = _loaiSpService.SearchByKeyword(keyword);
            return Ok(result);
        }

        // ✅ POST: api/loaisanpham
        [HttpPost]
        public IActionResult Create([FromBody] LoaiSanPham loai)
        {
            var success = _loaiSpService.Create(loai);
            if (!success)
                return BadRequest(new { message = "Thêm loại sản phẩm thất bại!" });
            return Ok(new { message = "Thêm loại sản phẩm thành công!" });
        }

        // ✅ PUT: api/loaisanpham/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] LoaiSanPham loai)
        {
            loai.category_id = id; // đảm bảo id trùng với route
            var success = _loaiSpService.Update(loai);
            if (!success)
                return NotFound(new { message = "Cập nhật thất bại, không tìm thấy loại sản phẩm!" });
            return Ok(new { message = "Cập nhật thành công!" });
        }

        // ✅ DELETE: api/loaisanpham/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var success = _loaiSpService.Delete(id);
            if (!success)
                return NotFound(new { message = "Xóa thất bại, không tìm thấy loại sản phẩm!" });
            return Ok(new { message = "Xóa thành công!" });
        }
    }
}
