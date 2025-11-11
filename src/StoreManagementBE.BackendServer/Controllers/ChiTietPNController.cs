using Microsoft.AspNetCore.Mvc;
using StoreManagementBE.BackendServer.DTOs.ChiTietPhieuNhap;
using StoreManagementBE.BackendServer.Services.Interfaces;

namespace StoreManagementBE.BackendServer.Controllers
{
    [Route("api/import-details")]
    [ApiController]
    public class ChiTietPNController : ControllerBase
    {
        private readonly IChiTietPNService _chiTietPNService;

        public ChiTietPNController(IChiTietPNService chiTietPNService)
        {
            _chiTietPNService = chiTietPNService;
        }

        // get all
        [HttpGet]
        public async Task<IActionResult> GetAllImportDetails()
        {
            var list = await _chiTietPNService.GetAll();
            var response = new ApiResponse<List<ChiTietPhieuNhapDTO>>();
            if (list == null || list.Count == 0)
            {
                response.Success = false;
                response.Message = "Không có chi tiết phiếu nhập nào!";
                response.DataDTO = [];
                return NoContent();
            }
            else
            {
                response.Success = true;
                response.Message = "Lấy danh sách chi tiết phiếu nhập thành công!";
                response.DataDTO = list;
                return Ok(response);
            }
        }

        //get by import id
        [HttpGet("{importId}")]
        public async Task<IActionResult> GetByImportId(int importId)
        {
            var detail = await _chiTietPNService.GetById(importId);
            var response = new ApiResponse<ChiTietPhieuNhapDTO>();
            if (detail == null)
            {
                response.Success = false;
                response.Message = "Không có chi tiết phiếu nhập nào!";
                return NotFound(response);
            }
            else
            {
                response.Success = true;
                response.Message = "Lấy chi tiết phiếu nhập thành công!";
                response.DataDTO = detail;
                return Ok(response);
            }
        }
    }
}
