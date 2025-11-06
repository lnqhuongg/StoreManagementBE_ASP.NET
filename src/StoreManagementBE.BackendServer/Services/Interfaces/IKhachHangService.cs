using StoreManagementBE.BackendServer.DTOs;

namespace StoreManagementBE.BackendServer.Services.Interfaces
{
    public interface IKhachHangService
    {
        Task<List<KhachHangDTO>> GetAll();
        Task<KhachHangDTO> GetById(int id);
        Task<KhachHangDTO> Create(KhachHangDTO kh);
        Task<KhachHangDTO?> Update(KhachHangDTO kh);
        Task<List<KhachHangDTO>> SearchByKeyword(string keyword);

        // Optional check exist
        Task<bool> CheckExistID(int id);
        Task<bool> CheckExistPhone(string phone);
        Task<bool> CheckExistEmail(string email);
    }
}
