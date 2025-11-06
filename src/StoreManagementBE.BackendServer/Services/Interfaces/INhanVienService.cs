using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.Services.Interfaces
{
    public interface INhanVienService
    {
        Task<List<NhanVienDTO>> GetAll();
        Task<NhanVienDTO?> GetById(int userId);
        List<NhanVien> SearchByKeyword(string keyword);
        Task<NhanVienDTO> Create(NhanVienDTO dto);
        Task<NhanVienDTO?> Update(int id, NhanVienDTO dto);
        Task<bool> isUsernameExist(string username);
        Task<bool> isUserExist(int userId);
    }
}