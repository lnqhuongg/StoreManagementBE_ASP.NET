using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.Services.Interfaces
{
    public interface ILoaiSanPhamService
    {
        Task<PagedResult<LoaiSanPhamDTO>> GetAll(int page, int pageSize, string keyword);
        IQueryable<LoaiSanPham> SearchByKeyword(string keyword);
        Task<LoaiSanPhamDTO> GetById(int category_id);
        Task<LoaiSanPhamDTO> Create(LoaiSanPhamDTO loaiSanPhamDTO);
        Task<LoaiSanPhamDTO> Update(int id, LoaiSanPhamDTO loaiSanPhamDTO);
        Task<bool> isCategoryNameExist(string categoryName, int id = 0);
        Task<bool> isCategoryExist(int category_id);
        Task<bool> Delete(int category_id);
    }
}
