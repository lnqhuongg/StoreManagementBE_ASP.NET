using StoreManagementBE.BackendServer.DTOs;

namespace StoreManagementBE.BackendServer.Services.Interfaces
{
    public interface IMaGiamGiaService
    {
        Task<List<MaGiamGiaDTO>> GetAll();
        Task<MaGiamGiaDTO?> GetById(int id);
        Task<MaGiamGiaDTO?> Create(MaGiamGiaDTO dto);
        Task<MaGiamGiaDTO?> Update(MaGiamGiaDTO dto);
        Task<bool> Delete(int id);
        Task<List<MaGiamGiaDTO>> SearchByKeyword(string keyword);
    }
}
