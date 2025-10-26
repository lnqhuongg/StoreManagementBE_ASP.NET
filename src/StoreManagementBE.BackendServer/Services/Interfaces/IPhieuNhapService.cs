using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.Services.Interfaces
{
    public interface IPhieuNhapService
    {
        Task<List<PhieuNhap>> GetAll();
        Task<PhieuNhap> GetById(int id);
        List<PhieuNhap> SearchByKeyword(string keyword);
        bool Create(PhieuNhap phieuNhap);
        bool Update(PhieuNhap phieuNhap);
        bool Delete(int id);
        bool isExist(int id);
    }
}
