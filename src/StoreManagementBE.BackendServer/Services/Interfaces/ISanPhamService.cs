using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.Services.Interfaces
{
    public interface ISanPhamService
    {
        public List<SanPham> GetAll();
        public SanPham GetById(int id);
        public bool Create(SanPhamDTO sp);
        public bool Delete(int id);
        public bool Update(SanPhamDTO sp);
        public bool UpdateStatus(int id);
        public List<SanPham> searchByKeyword(string keyword);
        public List<SanPham> getBySupplierID(int supplier_id);
        public List<SanPham> getByCategoryID(int category_id);

    }
}
