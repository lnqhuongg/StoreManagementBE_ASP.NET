using Microsoft.AspNetCore.Mvc;
using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.DTOs.DonHangDTO;
using StoreManagementBE.BackendServer.Services.Interfaces;

namespace StoreManagementBE.BackendServer.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class DonHangController : ControllerBase
    {
        private readonly IDonHangService _service;

        private readonly ITonKhoService _TonKhoservice;

        private readonly IMaGiamGiaService _maGiamGiaService;

        private readonly IKhachHangService _khachHangService;

        public DonHangController(IDonHangService service, ITonKhoService tonKhoservice, IMaGiamGiaService maGiamGiaService, IKhachHangService khachHangService)
        {
            _service = service;
            _TonKhoservice = tonKhoservice;
            _maGiamGiaService = maGiamGiaService;
            _khachHangService = khachHangService;
        }

        // 1. GET: api/orders (Lấy danh sách có phân trang & lọc)
        // URL ví dụ: api/orders?page=1&pageSize=5&keyword=KH01&minTotal=100000
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] OrderFilterDTO? filter = null
        )
        {
            try
            {
                // Nếu filter null thì tạo mới để tránh lỗi logic bên trong service
                filter ??= new OrderFilterDTO();

                var pagedResult = await _service.GetAll(page, pageSize, filter);

                var response = new ApiResponse<PagedResult<DonHangDTO>>
                {
                    Success = true,
                    Message = "Lấy danh sách đơn hàng thành công!",
                    DataDTO = pagedResult
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<PagedResult<DonHangDTO>>
                {
                    Success = false,
                    Message = "Lỗi: " + ex.Message
                });
            }
        }

        // 2. GET: api/orders/{id} (Xem chi tiết đơn hàng)
        [HttpGet("{id}")]
        [ActionName("GetById")] // Đặt tên Action để dùng cho CreatedAtAction
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var dto = await _service.GetById(id);

                if (dto != null)
                {
                    return Ok(new ApiResponse<DonHangDTO>
                    {
                        Success = true,
                        Message = "Lấy chi tiết đơn hàng thành công!",
                        DataDTO = dto
                    });
                }
                else
                {
                    return NotFound(new ApiResponse<DonHangDTO>
                    {
                        Success = false,
                        Message = $"Không tìm thấy đơn hàng có ID = {id}",
                        DataDTO = null
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<DonHangDTO>
                {
                    Success = false,
                    Message = "Lỗi: " + ex.Message
                });
            }
        }

        // 3. POST: api/orders (Tạo đơn hàng mới)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDTO donHangDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Gọi service tạo đơn hàng (bao gồm cả chi tiết sản phẩm nếu có trong DTO)
                var newOrder = await _service.CreateStaff(donHangDTO);

                if (newOrder != null) {
                    // Cập nhật tồn kho cho từng sản phẩm trong đơn hàng
                    foreach (var item in donHangDTO.Items)
                    {
                        await _TonKhoservice.deductQuantityOfCreatedOrder(item.ProductId, item.Quantity);    
                    }
                }

                if (donHangDTO.PromoId != null)
                {
                    await _maGiamGiaService.updateAfterCreatedOrder(donHangDTO.PromoId);
                }
                if (donHangDTO.CustomerId != null)
                {
                    await _khachHangService.addRewardPoints(donHangDTO.CustomerId);
                    if (donHangDTO.rewardPoints != null)
                        await _khachHangService.deductRewardPoints(donHangDTO.CustomerId, donHangDTO.rewardPoints);
                }

                // Trả về mã 201 Created và đường dẫn đến API xem chi tiết đơn hàng vừa tạo
                return CreatedAtAction(
                    nameof(GetById), // Tên của Action GetById ở trên
                    new { id = newOrder.OrderId }, // Tham số id cho Action đó
                    new ApiResponse<DonHangDTO>
                    {
                        Success = true,
                        Message = "Tạo đơn hàng thành công!",
                        DataDTO = newOrder
                    }
                );
            }
            catch (Exception ex)
            {
                // Lấy lỗi gốc từ bên trong (InnerException)
                var realError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                return BadRequest(new ApiResponse<DonHangDTO>
                {
                    Success = false,
                    Message = "Lỗi Database: " + realError // <-- Sẽ hiện rõ lỗi là gì
                });
            }
        }
    }
}