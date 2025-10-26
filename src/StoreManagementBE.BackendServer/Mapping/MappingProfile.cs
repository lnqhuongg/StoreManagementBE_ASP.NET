using AutoMapper;
using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PhieuNhap, PhieuNhapDTO>();
            CreateMap<ChiTietPhieuNhap, ChiTietPhieuNhapDTO>();
        }
    }
}
