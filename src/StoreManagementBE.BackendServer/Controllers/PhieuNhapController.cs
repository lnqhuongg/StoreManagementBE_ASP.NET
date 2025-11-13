using Microsoft.AspNetCore.Mvc;
using StoreManagementBE.BackendServer.DTOs.PhieuNhap;
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
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] PhieuNhapFilter? input = null)
        {
            var list = await _phieuNhapService.GetAll(input, pageNumber, pageSize);
            var response = new ApiResponse<PagedResult<PhieuNhapDTO>>();
            if (list == null || list.Data.Count == 0)
            {
                response.Success = false;
                response.Message = "Không có phiếu nhập nào!";
                return NoContent();
            }

            response.Success = true;
            response.Message = "Lấy danh sách phiếu nhập thành công!";
            response.DataDTO = list;

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
        public async Task<IActionResult> Create([FromBody] PhieuNhapDTO phieuNhapDto)
        {
            Console.WriteLine("Received PhieuNhapDTO: " + System.Text.Json.JsonSerializer.Serialize(phieuNhapDto));
            // lấy thời gian hiện tại
            phieuNhapDto.ImportDate = DateTime.Now;

            var response = new ApiResponse<PhieuNhapDTO>();
            var success = await _phieuNhapService.Create(phieuNhapDto);
            if (success == null)
            {
                response.Success = false;
                response.Message = "Thêm phiếu nhập thất bại!";
                return BadRequest(response);
            }
            response.Success = true;    
            response.Message = "Thêm phiếu nhập thành công!";
            response.DataDTO = success;
            return CreatedAtAction(nameof(GetById), new { id = success.ImportId }, response);
        }


        // update
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PhieuNhapDTO phieuNhapDto)
        {
            // Kiểm tra đầu vào
            if (phieuNhapDto == null)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Dữ liệu cập nhật không hợp lệ!"
                });
            }

            // Đảm bảo id trong route và body khớp nhau
            phieuNhapDto.ImportId = id;

            // Gọi service xử lý
            var updated = await _phieuNhapService.Update(phieuNhapDto);

            if (updated == null)
            {
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Không tìm thấy phiếu nhập để cập nhật!"
                });
            }

            // Thành công
            return Ok(new ApiResponse<PhieuNhapDTO>
            {
                Success = true,
                Message = "Cập nhật phiếu nhập thành công!",
                DataDTO = updated
            });
        }


        // create phieu nhap voi chi tiet
        [HttpPost("with-details")]
        public async Task<IActionResult> CreateWithDetails([FromBody] CreatePhieuNhapDTO dto)
        {
            try
            {
                Console.WriteLine("Received imports: " + System.Text.Json.JsonSerializer.Serialize(dto));
                var result = await _phieuNhapService.CreateWithDetails(dto);
                return Ok(new ApiResponse<PhieuNhapDTO>
                {
                    Success = true,
                    Message = "Thêm phiếu nhập và chi tiết thành công!",
                    DataDTO = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<PhieuNhapDTO>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

    }
}
