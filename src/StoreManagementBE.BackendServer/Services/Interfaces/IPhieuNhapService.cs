using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.Services.Interfaces
{
    public interface IPhieuNhapService
    {
        Task<List<PhieuNhapDTO>> GetAll();
        Task<PhieuNhapDTO> GetById(int id);
        List<PhieuNhapDTO> SearchByKeyword(string keyword);
        Task<PhieuNhapDTO> Create(PhieuNhapDTO phieuNhap);
        Task<PhieuNhapDTO> Update(PhieuNhapDTO phieuNhap);
        bool Delete(int id);
        bool isExist(int id);
    }
}