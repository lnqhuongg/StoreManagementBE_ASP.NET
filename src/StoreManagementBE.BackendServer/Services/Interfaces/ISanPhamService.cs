using StoreManagementBE.BackendServer.DTOs.SanPhamDTO;
using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.Services.Interfaces
{
    public interface ISanPhamService
    {
        public Task<List<SanPhamDTO>> GetAll();
        public Task<SanPhamDTO> GetById(int id);
        public Task<SanPhamDTO> Create(SanPhamRequestDTO sp);
        public Task<bool> Delete(int id);
        public Task<SanPhamDTO> Update(int id, SanPhamRequestDTO sp);
        //public Task<ApiResponse<SanPhamDTO>> UpdateStatus(int id);
        public Task<List<SanPhamDTO>> searchByKeyword(string keyword);
        public Task<List<SanPhamDTO>> getBySupplierID(int? supplier_id);
        public Task<List<SanPhamDTO>> getByCategoryID(int? category_id);
        public Task<List<SanPhamDTO>> getProductsSortByPrice(string? order);
        public Task<List<SanPhamDTO>> getProductsBySupplierIDAndCategoryID(int? supplier_id, int? category_id);
        public Task<List<SanPhamDTO>> getProductsBySupplierIDAndPrice(int? supplier_id, string? order);
        public Task<List<SanPhamDTO>> getProductsByCategoryIDAndPrice(int? category_id, string? order);
        public Task<List<SanPhamDTO>> getProductsBysupplierIDAndCategoryIDAndPriceAndKeyword(int? supplier_id, int? category_id, string? order, string? keyword);
        public Task<bool> checkExistBarcode(string barcode);
        public Task<bool> checkExistID(int ID);
        public Task<bool> checkBarcodeExistForOtherProducts(int id, string barcode);
    }
}