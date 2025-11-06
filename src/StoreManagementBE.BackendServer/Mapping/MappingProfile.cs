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
            // entity loaisanpham <-> loaisanphamDTO --- reservemap de map nguoc lai, nghia la tu DTO ve entity
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
            // entity donhang <-> donhangDTO
            CreateMap<DonHang, DonHangDTO>().ReverseMap();
            // entity chitietdonhang <-> chitietdonhangDTO
            CreateMap<ChiTietDonHang, ChiTietDonHangDTO>().ReverseMap();
            // entity thanhtoan <-> thanhtoanDTO
            CreateMap<ThanhToan, ThanhToanDTO>().ReverseMap();
        }
    }
}