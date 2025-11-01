using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.Services.Interfaces
{
    public interface IKhachHangService
    {
        Task<List<KhachHangDTO>> GetAll();
        Task<KhachHangDTO> GetById(int customer_id);
        List<KhachHang> SearchByKeyword(string keyword);
        Task<ApiResponse<KhachHangDTO>> Create(KhachHangDTO khachHangDTO);
        Task<ApiResponse<KhachHangDTO>> Update(int id, KhachHangDTO khachHangDTO);
        Task<ApiResponse<bool>> Delete(int customer_id);
    }
}
