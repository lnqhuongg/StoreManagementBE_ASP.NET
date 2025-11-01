// Services/Interfaces/INhanVienService.cs
using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.Services.Interfaces
{
    public interface INhanVienService
    {
        Task<List<NhanVienDTO>> GetAll();
        Task<NhanVienDTO?> GetById(int user_id);
        List<NhanVienDTO> SearchByKeyword(string keyword);
        Task<ApiResponse<NhanVienDTO>> Create(NhanVienDTO dto);
        Task<ApiResponse<NhanVienDTO>> Update(int id, NhanVienDTO dto);
        Task<ApiResponse<bool>> Delete(int id);
        Task<bool> IsUsernameExist(string username, int? excludeId = null);
    }
}