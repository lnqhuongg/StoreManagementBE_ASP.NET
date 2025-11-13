using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models.Entities;
namespace StoreManagementBE.BackendServer.Services.Interfaces
{
    public interface IMaGiamGiaService
    {
        Task<PagedResult<MaGiamGiaDTO>> GetAll(int page, int pageSize, string? keyword, string? discountType);
        Task<MaGiamGiaDTO?> GetById(int id);
        Task<ApiResponse<MaGiamGiaDTO>> Create(MaGiamGiaDTO dto);
        Task<ApiResponse<MaGiamGiaDTO>> Update(int id, MaGiamGiaDTO dto);
        Task<ApiResponse<bool>> Delete(int id);
        Task<List<MaGiamGiaDTO>> SearchByKeyword(string keyword);
    }
}