using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.Services.Interfaces
{
    public interface ISanPhamService
    {
        public Task<List<SanPhamDTO>> GetAll();
        public Task<SanPhamDTO> GetById(int id);
        public Task<SanPhamDTO> Create(SanPhamDTO sp);
        public Task<bool> Delete(int id);
        public Task<SanPhamDTO> Update(SanPhamDTO sp);
        //public Task<ApiResponse<SanPhamDTO>> UpdateStatus(int id);
        public Task<List<SanPhamDTO>> searchByKeyword(string keyword);
        public Task<List<SanPhamDTO>> getBySupplierID(int? supplier_id);
        public Task<List<SanPhamDTO>> getByCategoryID(int? category_id);
        public Task<List<SanPhamDTO>> getProductsSortByPrice(string? order);
        public Task<List<SanPhamDTO>> getProductsBySupplierIDAndCategoryID(int? supplier_id, int? category_id);
        public Task<List<SanPhamDTO>> getProductsBySupplierIDAndPrice(int? supplier_id, string? order);
        public Task<List<SanPhamDTO>> getProductsByCategoryIDAndPrice(int? category_id, string? order);
        public Task<List<SanPhamDTO>> getProductsBysupplierIDAndCategoryIDAndPrice(int? supplier_id, int? category_id, string? order);
        public Task<bool> checkExistBarcode(string barcode);
        public Task<bool> checkExistID(int ID);
    }
}