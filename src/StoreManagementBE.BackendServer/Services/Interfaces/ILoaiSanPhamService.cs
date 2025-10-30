using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.Services.Interfaces
{
    public interface ILoaiSanPhamService
    {
        Task<List<LoaiSanPhamDTO>> GetAll();
        Task<LoaiSanPhamDTO> GetById(int category_id);
        List<LoaiSanPham> SearchByKeyword(string keyword);
        Task<ApiResponse<LoaiSanPhamDTO>> Create(LoaiSanPhamDTO loaiSanPhamDTO);
        Task<ApiResponse<LoaiSanPhamDTO>> Update(int id, LoaiSanPhamDTO loaiSanPhamDTO);
        Task<ApiResponse<bool>> Delete(int category_id);
    }
}
