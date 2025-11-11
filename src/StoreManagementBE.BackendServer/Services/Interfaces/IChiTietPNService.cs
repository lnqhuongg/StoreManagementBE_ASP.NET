using StoreManagementBE.BackendServer.DTOs.ChiTietPhieuNhap;

namespace StoreManagementBE.BackendServer.Services.Interfaces
{
    public interface IChiTietPNService
    {
        Task<List<ChiTietPhieuNhapDTO>> GetAll();
        Task<ChiTietPhieuNhapDTO> GetById(int id);
        Task<ChiTietPhieuNhapDTO> Create(ChiTietPhieuNhapDTO chiTietPhieuNhapDto);
        Task<ChiTietPhieuNhapDTO> Update(int id, ChiTietPhieuNhapDTO chiTietPhieuNhapDto);
        Task<bool> Delete(int id);
    }
}
