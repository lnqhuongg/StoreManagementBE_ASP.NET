using Microsoft.AspNetCore.Mvc;
using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.DTOs.AuthenticationDTO;
using StoreManagementBE.BackendServer.Services;
using StoreManagementBE.BackendServer.Services.Interfaces;

namespace StoreManagementBE.BackendServer.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class NhanVienController : ControllerBase
    {
        private readonly INhanVienService _service;
        private readonly IAuthenticationService _authenService;

        public NhanVienController(INhanVienService service, IAuthenticationService authenticationService)
        {
            _service = service;
            _authenService = authenticationService;
        }

        // GET: api/users
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 5, [FromQuery] NhanVienFilterDTO filter = null)
        {
            filter ??= new NhanVienFilterDTO(); // Đảm bảo filter không null
            try
            {
                var list = await _service.GetAll(page, pageSize, filter);
                return Ok(new ApiResponse<PagedResult<NhanVienDTO>>
                {
                    Success = true,
                    Message = "Lấy danh sách nhân viên thành công!",
                    DataDTO = list
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<PagedResult<NhanVienDTO>>
                {
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        [ActionName("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetById(id);
            if (item == null)
                return NotFound(new ApiResponse<NhanVienDTO> { Message = "Không tìm thấy nhân viên!", Success = false });

            return Ok(new ApiResponse<NhanVienDTO>
            {
                Success = true,
                Message = "Lấy nhân viên theo ID thành công!",
                DataDTO = item
            });
        }

        // POST: api/users
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NhanVienDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // 400

            try
            {
                var result = await _service.Create(dto);
                return CreatedAtAction(
                    nameof(GetById),
                    new { id = result.UserId },  // Dùng UserId
                    new ApiResponse<NhanVienDTO>
                    {
                        Success = true,
                        Message = "Thêm nhân viên thành công!",
                        DataDTO = result
                    }
                );
            }
            catch (Exception ex) when (ex.Message.Contains("tồn tại") || ex.Message.Contains("Role phải là"))
            {
                return Conflict(new ApiResponse<NhanVienDTO> { Message = ex.Message, Success = false }); // 409
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<NhanVienDTO> { Message = ex.Message, Success = false }); // 400
            }
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] NhanVienDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _service.Update(id, dto);
                if (result == null)
                    return NotFound(new ApiResponse<NhanVienDTO> { Message = "Không tìm thấy nhân viên để cập nhật!", Success = false });

                return Ok(new ApiResponse<NhanVienDTO>
                {
                    Success = true,
                    Message = "Cập nhật nhân viên thành công!",
                    DataDTO = result
                });
            }
            catch (Exception ex) when (ex.Message.Contains("sử dụng") || ex.Message.Contains("tồn tại") || ex.Message.Contains("Role phải là"))
            {
                return Conflict(new ApiResponse<NhanVienDTO> { Message = ex.Message, Success = false }); // 409
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<NhanVienDTO> { Message = ex.Message, Success = false }); // 400
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticationDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _authenService.Authenticate(dto);

                if (user == null)
                    return NotFound(new ApiResponse<NhanVienDTO>
                    {
                        Message = "Sai username hoặc mật khẩu!",
                        Success = false,
                        DataDTO = null
                    });

                return Ok(new ApiResponse<NhanVienDTO>
                {
                    Message = "Đăng nhập thành công!",
                    Success = true,
                    DataDTO = user
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

    }
}