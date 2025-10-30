using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Services.Interfaces;
using StoreManagementBE.BackendServer.Helpers;
using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.Controllers
{
    [Route("api/imports")]
    [ApiController]
    public class PhieuNhapController : ControllerBase
    {
        private readonly IPhieuNhapService _phieuNhapService;
        private readonly IMapper _mapper;
        public PhieuNhapController(IPhieuNhapService phieuNhapService, IMapper mapper)
        {
            _phieuNhapService = phieuNhapService;
            _mapper = mapper;
        }

        //get all
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _phieuNhapService.GetAll();
            var dtoList = _mapper.Map<List<PhieuNhapDTO>>(list);
            var response = new ApiResponse<List<PhieuNhapDTO>>();
            if (dtoList == null || dtoList.Count == 0)
            {
                response.Success = false;
                response.Message = "Không có phiếu nhập nào!";
                return NotFound(response);
            }

            response.Success = true;
            response.Message = "Lấy danh sách phiếu nhập thành công!";
            response.DataDTO = dtoList;

            return Ok(response);
        }

        //get by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _phieuNhapService.GetById(id);
            var response = new ApiResponse<PhieuNhapDTO>();
            if (item == null)
            {
                response.Success = false;
                response.Message = "Không tìm thấy phiếu nhập này!";
                return NotFound(response);
            }
            else
            {
                response.Success = true;
                response.Message = "Lấy phiếu nhập thành công!";
                response.DataDTO = item;
                return Ok(response);
            }
        }


        //create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DTOs.PhieuNhapDTO phieuNhapDto)
        {
            Console.WriteLine("Received PhieuNhapDTO: " + System.Text.Json.JsonSerializer.Serialize(phieuNhapDto));
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Dữ liệu không hợp lệ!",
                    errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                });
            }
            var success = await _phieuNhapService.Create(phieuNhapDto);
            if (success == null)
                return BadRequest(new { message = "Thêm phiếu nhập thất bại!" });
            return Ok(new { message = "Thêm phiếu nhập thành công!" });
        }

        //update
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] PhieuNhapDTO phieuNhapDto)
        {
            phieuNhapDto.ImportId = id; // đảm bảo id trùng với route
            var success = _phieuNhapService.Update(phieuNhapDto);
            if (success == null)
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
