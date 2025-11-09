using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.Services.Interfaces
{
    public interface INhanVienService
    {
        Task<PagedResult<NhanVienDTO>> GetAll(int page, int pageSize, NhanVienFilterDTO filter);
        Task<NhanVienDTO?> GetById(int userId);
        IQueryable<NhanVien> Search(NhanVienFilterDTO filter);
        Task<NhanVienDTO> Create(NhanVienDTO dto);
        Task<NhanVienDTO?> Update(int id, NhanVienDTO dto);
        Task<bool> isUsernameExist(string username);
        Task<bool> isUserExist(int userId);
    }
}