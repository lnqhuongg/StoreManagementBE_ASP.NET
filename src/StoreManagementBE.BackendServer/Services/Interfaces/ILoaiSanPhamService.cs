using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.Services.Interfaces
{
    public interface ILoaiSanPhamService
    {
        Task<List<LoaiSanPhamDTO>> GetAll();
        Task<LoaiSanPhamDTO> GetById(int category_id);
        List<LoaiSanPham> SearchByKeyword(string keyword);
        Task<LoaiSanPhamDTO> Create(LoaiSanPhamDTO loaiSanPhamDTO);
        Task<LoaiSanPhamDTO> Update(int id, LoaiSanPhamDTO loaiSanPhamDTO);
        Task<bool> isCategoryNameExist(string category_name);
        Task<bool> isCategoryExist(int category_id);
        bool Delete(int category_id);
    }
}
