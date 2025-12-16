using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.DTOs.DonHangDTO;
using StoreManagementBE.BackendServer.DTOs.SanPhamDTO;
using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.Services.Interfaces
{
    public interface IDonHangService
    {
        // 1. Lấy danh sách có phân trang & lọc (Giống GetAll bên LoaiSanPham)
        Task<PagedResult<DonHangDTO>> GetAll(int page, int pageSize, OrderFilterDTO filter);

        // 2. Hàm lọc (Thay thế cho SearchByKeyword vì đơn hàng cần lọc nhiều tiêu chí hơn)
        IQueryable<DonHang> ApplyFilter(OrderFilterDTO filter);

        // 3. Lấy chi tiết đơn hàng theo ID
        Task<DonHangDTO?> GetById(int orderId);

        // 4. Tạo mới đơn hàng (Giống Create bên LoaiSanPham)
        Task<DonHangDTO> CreateStaff(CreateOrderDTO dto);

        Task<long> GetTotalRevenue();
        Task<int> GetTotalPaidOrder();

        public List<long> GetRevenueByMonth(int month, int year);
        public List<long> GetRevenueByYear(int year);
        public Task<List<object>> GetTop5Products();
        Task<List<object>> GetPaymentMethodStats();
    }
}
