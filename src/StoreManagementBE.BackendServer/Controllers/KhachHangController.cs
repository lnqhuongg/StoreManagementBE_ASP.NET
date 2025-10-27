using Microsoft.AspNetCore.Mvc;
using StoreManagementBE.BackendServer.Models.Entities;
using StoreManagementBE.BackendServer.Services.Interfaces;

namespace StoreManagementBE.BackendServer.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class KhachHangController : ControllerBase
    {
        private readonly IKhachHangService _khService;

        public KhachHangController(IKhachHangService khService)
        {
            _khService = khService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _khService.GetAll();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = _khService.GetById(id);
            if (item == null)
                return NotFound(new { message = "Không tìm thấy khách hàng này!" });
            return Ok(item);
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string keyword)
        {
            var result = _khService.SearchByKeyword(keyword);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create([FromBody] KhachHang kh)
        {
            var success = _khService.Create(kh);
            if (!success)
                return BadRequest(new { message = "Thêm khách hàng thất bại!" });
            return Ok(new { message = "Thêm khách hàng thành công!" });
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] KhachHang kh)
        {
            kh.customer_id = id;
            var success = _khService.Update(kh);
            if (!success)
                return NotFound(new { message = "Cập nhật thất bại, không tìm thấy khách hàng!" });
            return Ok(new { message = "Cập nhật khách hàng thành công!" });
        }

    }
}
