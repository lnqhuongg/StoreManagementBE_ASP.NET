using Microsoft.AspNetCore.Mvc;
using StoreManagementBE.BackendServer.DTOs.SanPhamDTO;
using StoreManagementBE.BackendServer.Services.Interfaces;
using System.Collections.Generic;

namespace StoreManagementBE.BackendServer.Controllers
{
    [Route("api/stats")]
    [ApiController]
    public class ThongKecController : ControllerBase
    {
        private readonly IDonHangService _donHangService;
        private readonly IPhieuNhapService _phieuNhapService;
        private readonly ITonKhoService _tonKhoService;
        public ThongKecController(IDonHangService donHangService, IPhieuNhapService phieuNhapService, ITonKhoService tonKhoService)
        {
            _donHangService = donHangService;
            _phieuNhapService = phieuNhapService;
            _tonKhoService = tonKhoService;
        }
        [HttpGet("total-revenue")]
        public async Task<IActionResult> GetTotalRevenue()
        {
            try
            {
                var totalRevenue = await _donHangService.GetTotalRevenue();
                // Phải bọc trong Ok() vì hàm trả về IActionResult
                return Ok(new ApiResponse<long>
                {
                    Success = true,
                    Message = "Lấy tổng doanh thu thành công!",
                    DataDTO = totalRevenue
                });
            }
            catch (Exception ex)
            {
                // Catch phải trả về lỗi (thường là BadRequest 400 hoặc ServerError 500)
                return BadRequest(new ApiResponse<long>
                {
                    Success = false,
                    Message = "Lỗi hệ thống: " + ex.Message,
                    DataDTO = 0
                });
            }
        }

        [HttpGet("total-paid-orders")]
        public async Task<IActionResult> GetTotalPaidOrders()
        {
            try
            {
                var totalPaid = await _donHangService.GetTotalPaidOrder();

                // Phải bọc trong Ok() vì hàm trả về IActionResult
                return Ok(new ApiResponse<int>
                {
                    Success = true,
                    Message = "Lấy tổng số đơn hàng đã thanh toán thành công!",
                    DataDTO = totalPaid
                });
            }
            catch (Exception ex)
            {
                // Catch phải trả về lỗi (thường là BadRequest 400 hoặc ServerError 500)
                return BadRequest(new ApiResponse<int>
                {
                    Success = false,
                    Message = "Lỗi hệ thống: " + ex.Message,
                    DataDTO = 0
                });
            }
        }

        [HttpGet("top-5-products")]
        public async Task<IActionResult> GetTop5Products()
        {
            try
            {
                var topProducts = await _donHangService.GetTop5Products();
                return Ok(new ApiResponse<List<object>>
                {
                    Success = true,
                    Message = "Lấy danh sách 5 sản phẩm bán chạy nhất thành công!",
                    DataDTO = topProducts
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<List<SanPhamDTO>>
                {
                    Success = false,
                    Message = "Lỗi hệ thống: " + ex.Message,
                    DataDTO = null
                });
            }
        }

        [HttpGet("payment-methods")]
        public async Task<IActionResult> GetPaymentMethodStats()
        {
            try
            {
                var paymentStats = await _donHangService.GetPaymentMethodStats();
                return Ok(new ApiResponse<List<object>>
                {
                    Success = true,
                    Message = "Lấy thống kê phương thức thanh toán thành công!",
                    DataDTO = paymentStats
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<List<object>>
                {
                    Success = false,
                    Message = "Lỗi hệ thống: " + ex.Message,
                    DataDTO = null
                });
            }
        }

        [HttpGet("revenue-by-month")]
        public async Task<IActionResult> GetRevenueByMonth([FromQuery] int month, [FromQuery] int year)
        {
            try
            {
                var revenueByMonth = _donHangService.GetRevenueByMonth(month, year);
                return Ok(new ApiResponse<List<long>>
                {
                    Success = true,
                    Message = "Lấy doanh thu theo tháng thành công!",
                    DataDTO = revenueByMonth
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<List<long>>
                {
                    Success = false,
                    Message = "Lỗi hệ thống: " + ex.Message,
                    DataDTO = null
                });
            }
        }

        [HttpGet("revenue-by-year")]
        public async Task<IActionResult> GetRevenueByYear([FromQuery] int year)
        {
            try
            {
                var revenueByYear = _donHangService.GetRevenueByYear(year);
                return Ok(new ApiResponse<List<long>>
                {
                    Success = true,
                    Message = "Lấy doanh thu theo năm thành công!",
                    DataDTO = revenueByYear
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<List<long>>
                {
                    Success = false,
                    Message = "Lỗi hệ thống: " + ex.Message,
                    DataDTO = null
                });
            }
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStockCount()
        {
            try
            {
                var count = await _tonKhoService.GetLowStockCount();

                return Ok(new ApiResponse<int>
                {
                    Success = true,
                    Message = "Lấy số lượng sản phẩm sắp hết hàng thành công",
                    DataDTO = count
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<int> { Success = false, Message = ex.Message });
            }
        }
    }

}
