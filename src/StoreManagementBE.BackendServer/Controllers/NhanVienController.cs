using Microsoft.AspNetCore.Mvc;
using StoreManagementBE.BackendServer.Models.Entities;
using StoreManagementBE.BackendServer.Services.Interfaces;
using System.Collections.Generic;

namespace StoreManagementBE.BackendServer.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class NhanVienController : ControllerBase
    {
        private readonly INhanVienService _nhanVienService;

        public NhanVienController(INhanVienService nhanVienService)
        {
            _nhanVienService = nhanVienService;
        }

        // GET: api/users
        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _nhanVienService.GetAll();
            return Ok(list);
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = _nhanVienService.GetById(id);
            if (item == null)
                return NotFound(new { message = "Không tìm thấy nhân viên!" });
            return Ok(item);
        }

        // GET: api/users/search?keyword=admin
        [HttpGet("search")]
        public IActionResult Search([FromQuery] string keyword)
        {
            var result = _nhanVienService.SearchByKeyword(keyword);
            return Ok(result);
        }

        // POST: api/users
        [HttpPost]
        public IActionResult Create([FromBody] NhanVien nhanVien)
        {
            if (_nhanVienService.IsUsernameExist(nhanVien.username))
                return BadRequest(new { message = "Tên đăng nhập đã tồn tại!" });

            // Mã hóa mật khẩu (nên dùng BCrypt hoặc Identity)
            // nhanVien.password = HashPassword(nhanVien.password);

            var success = _nhanVienService.Create(nhanVien);
            if (!success)
                return BadRequest(new { message = "Thêm nhân viên thất bại!" });

            return Ok(new { message = "Thêm nhân viên thành công!" });
        }

        // PUT: api/users/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] NhanVien nhanVien)
        {
            nhanVien.user_id = id;

            if (!_nhanVienService.IsExist(id))
                return NotFound(new { message = "Không tìm thấy nhân viên!" });

            if (_nhanVienService.IsUsernameExist(nhanVien.username, id))
                return BadRequest(new { message = "Tên đăng nhập đã được sử dụng!" });

            var success = _nhanVienService.Update(nhanVien);
            if (!success)
                return BadRequest(new { message = "Cập nhật thất bại!" });

            return Ok(new { message = "Cập nhật thành công!" });
        }
    }

}