using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.Services.Interfaces
{
    public interface INhaCungCapService
    {
        Task<List<NhaCungCapDTO>> GetAll();
        Task<NhaCungCapDTO?> GetById(int supplierId);
        List<NhaCungCap> SearchByKeyword(string keyword);
        Task<ApiResponse<NhaCungCapDTO>> Create(NhaCungCapDTO nhaCungCapDTO);
        Task<ApiResponse<NhaCungCapDTO>> Update(int id, NhaCungCapDTO nhaCungCapDTO);
        Task<bool> IsSupplierIdExist(int supplierId);
        Task<bool> IsSupplierExist(string name, string email, string phone, int? ignoreId = null);
        //bool Delete(int id);
    }
}
