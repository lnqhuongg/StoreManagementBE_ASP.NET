using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.Services.Interfaces
{
    public interface ILoaiSanPhamService
    {
        List<LoaiSanPham> GetAll();
        LoaiSanPham GetById(int category_id);
        List<LoaiSanPham> SearchByKeyword(string keyword);
        bool Create(LoaiSanPham loai);
        bool Update(LoaiSanPham loai);
        bool Delete(int category_id);
        bool isExist(int category_id);
    }
}
