using StoreManagementBE.BackendServer.DTOs;

namespace StoreManagementBE.BackendServer.Services.Interfaces
{
    public interface IDonHangService
    {
        Task<List<DonHangDTO>> GetAll();
        Task<DonHangDTO?> GetById(int id);
    }
}
