using StoreManagementBE.BackendServer.DTOs.PhieuNhap;
using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.Services.Interfaces
{
    public interface IPhieuNhapService
    {
        Task<PagedResult<PhieuNhapDTO>> GetAll(PhieuNhapFilter input, int pageNumber, int pageSize);
        Task<PhieuNhapDTO> GetById(int id);
        Task<PhieuNhapDTO> Create(PhieuNhapDTO phieuNhap);

        Task<PhieuNhapDTO> CreateWithDetails(CreatePhieuNhapDTO phieuNhapDto);
        Task<PhieuNhapDTO> Update(PhieuNhapDTO phieuNhap);
        bool Delete(int id);
        bool isExist(int id);

        IQueryable<PhieuNhap> FilterAndSearch(IQueryable<PhieuNhap> query, PhieuNhapFilter input);

        
    }
}