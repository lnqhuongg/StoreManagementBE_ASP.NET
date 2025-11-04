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
        Task<bool> IsSupplierIdExist(int supplierId);   //Dùng cho luồng GET/PUT/DELETE để quyết định trả 404 Not Found
        Task<bool> IsSupplierExist(string name, string email, string phone, int? ignoreId = null);  //Dùng cho CREATE/UPDATE để trả 409 Conflict (hoặc 400) với thông điệp
        //bool Delete(int id);
    }
}
