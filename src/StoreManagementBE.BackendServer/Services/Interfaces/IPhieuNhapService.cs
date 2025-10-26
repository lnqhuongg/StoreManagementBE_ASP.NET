using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.Services.Interfaces
{
    public interface IPhieuNhapService
    {
        Task<List<PhieuNhapDTO>> GetAll();
        Task<PhieuNhapDTO> GetById(int id);
        List<PhieuNhap> SearchByKeyword(string keyword);
        bool Create(PhieuNhap phieuNhap);
        bool Update(PhieuNhap phieuNhap);
        bool Delete(int id);
        bool isExist(int id);
    }
}
