using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.Services.Interfaces
{
    public interface INhaCungCapService
    {
        List<NhaCungCap> GetAll();
        NhaCungCap GetById(int id);
        List<NhaCungCap> SearchByKeyword(string keyword);
        bool Create(NhaCungCap supplier);
        bool Update(NhaCungCap supplier);
        //bool Delete(int supplier_id);
        bool isExist(int supplier_id);
    }
}
