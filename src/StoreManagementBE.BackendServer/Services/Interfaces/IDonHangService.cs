using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.Services.Interfaces
{
    public interface IDonHangService
    {
        Task<PagedResult<DonHangDTO>> GetAll(int page, int pageSize, OrderFilterDTO filter);
        IQueryable<DonHang> ApplyFilter(OrderFilterDTO filter); // <- HÀM LỌC RIÊNG
        Task<DonHangDTO?> GetById(int orderId);
    }
}
