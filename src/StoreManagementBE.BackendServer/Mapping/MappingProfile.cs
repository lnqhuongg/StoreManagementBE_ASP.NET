// File: Mappings/MappingProfile.cs
using AutoMapper;
using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Loại sản phẩm
            CreateMap<LoaiSanPham, LoaiSanPhamDTO>().ReverseMap();

            // Khách hàng
            CreateMap<KhachHang, KhachHangDTO>().ReverseMap();

            // Thêm các mapping khác nếu cần
            // CreateMap<SanPham, SanPhamDTO>().ReverseMap();
        }
    }
}