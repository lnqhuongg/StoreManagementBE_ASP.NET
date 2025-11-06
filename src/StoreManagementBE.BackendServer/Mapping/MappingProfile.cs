// File: Mappings/MappingProfile.cs
using AutoMapper;
using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.DTOs.SanPhamDTO;
using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Loại sản phẩm
            CreateMap<LoaiSanPham, LoaiSanPhamDTO>().ReverseMap();
            CreateMap<PhieuNhap, PhieuNhapDTO>().ReverseMap();
            CreateMap<ChiTietPhieuNhap, ChiTietPhieuNhapDTO>().ReverseMap();

            // entity nhacungcap <-> nhacungcapDTO
            CreateMap<NhaCungCap, NhaCungCapDTO>().ReverseMap();

            // entity nhanvien <-> nhanvienDTO
            CreateMap<NhanVien, NhanVienDTO>().ReverseMap();

            // entity sanpham <-> sanphamDTO
            CreateMap<SanPham, SanPhamDTO>().ReverseMap();

            // entity nhacungcap <-> nhacungcapDTO
            CreateMap<NhaCungCap, NhaCungCapDTO>().ReverseMap();
        }
    }
}