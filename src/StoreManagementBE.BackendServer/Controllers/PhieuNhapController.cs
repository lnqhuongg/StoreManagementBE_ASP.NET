using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManagementBE.BackendServer.Services.Interfaces;

namespace StoreManagementBE.BackendServer.Controllers
{
    [Route("api/imports")]
    [ApiController]
    public class PhieuNhapController : ControllerBase
    {
        private readonly IPhieuNhapService _phieuNhapService;
        public PhieuNhapController(IPhieuNhapService phieuNhapService)
        {
            _phieuNhapService = phieuNhapService;
        }

        //get all
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _phieuNhapService.GetAll();
            return Ok(list);
        }

        //get by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _phieuNhapService.GetById(id);
            if (item == null)
                return NotFound(new { message = "Không tìm thấy phiếu nhập này!" });
            else
            {
                System.Console.WriteLine(item.Import_id);
            }
                return Ok(item);
        }

        //create
        [HttpPost]
        public IActionResult Create([FromBody] Models.Entities.PhieuNhap phieuNhap)
        {
            var success = _phieuNhapService.Create(phieuNhap);
            if (!success)
                return BadRequest(new { message = "Thêm phiếu nhập thất bại!" });
            return Ok(new { message = "Thêm phiếu nhập thành công!" });
        }

        //update
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Models.Entities.PhieuNhap phieuNhap)
        {
            phieuNhap.import_id = id; // đảm bảo id trùng với route
            var success = _phieuNhapService.Update(phieuNhap);
            if (!success)
                return NotFound(new { message = "Cập nhật thất bại, không tìm thấy phiếu nhập!" });
            return Ok(new { message = "Cập nhật phiếu nhập thành công!" });
        }

        //search by keyword
        [HttpGet("search")]
        public IActionResult Search([FromQuery] string keyword)
        {
            var result = _phieuNhapService.SearchByKeyword(keyword);
            return Ok(result);
        }
    }
}
