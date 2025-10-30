using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.Services.Interfaces
{
    public interface INhaCungCapService
    {
        Task<List<NhaCungCapDTO>> GetAll();
        Task<NhaCungCapDTO?> GetById(int id);
        List<NhaCungCap> SearchByKeyword(string keyword);

        Task<ApiResponse<NhaCungCapDTO>> Create(NhaCungCapDTO dto);
        Task<ApiResponse<NhaCungCapDTO>> Update(int id, NhaCungCapDTO dto);
    }
}
