using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.Services.Interfaces
{
    public interface INhaCungCapService
    {
        Task<List<NhaCungCapDTO>> GetAll();
        Task<NhaCungCapDTO?> GetById(int supplierId);
        List<NhaCungCap> SearchByKeyword(string keyword);
        Task<NhaCungCapDTO> Create(NhaCungCapDTO dto);
        Task<NhaCungCapDTO?> Update(int id, NhaCungCapDTO dto);
        Task<bool> Delete(int id);
        Task<bool> IsSupplierIdExist(int supplierId);
        Task<bool> IsSupplierExist(string name, string email, string phone, int? ignoreId = null);
    }
}
