using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models;
using StoreManagementBE.BackendServer.Models.Entities;
using StoreManagementBE.BackendServer.Services.Interfaces;

namespace StoreManagementBE.BackendServer.Services
{
    public class NhaCungCapService : INhaCungCapService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public NhaCungCapService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //get all suppliers
        public async Task<List<NhaCungCapDTO>> GetAll()
        {
            var list = await _context.NhaCungCaps.ToListAsync();
            //return list DTO
            return _mapper.Map<List<NhaCungCapDTO>>(list);
        }

        //get 1 supplier by id
        public async Task<NhaCungCapDTO?> GetById(int supplierId)
        {
            var entity = await _context.NhaCungCaps.FindAsync(supplierId);

            return entity == null ? null : _mapper.Map<NhaCungCapDTO>(entity);
        }

        public List<NhaCungCap> SearchByKeyword(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return new List<NhaCungCap>();

            var k = keyword.Trim();
            return _context.NhaCungCaps
                .Where(x =>
                    x.Name.Contains(k) ||
                    x.Email.Contains(k) ||
                    x.Phone.Contains(k) ||
                    x.Address.Contains(k))
                .ToList();
        }


        public async Task<NhaCungCapDTO> Create(NhaCungCapDTO dto)
        {
            try
            {
                var entity = _mapper.Map<NhaCungCap>(dto);

                _context.NhaCungCaps.Add(entity);

                await _context.SaveChangesAsync();

                return _mapper.Map<NhaCungCapDTO>(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm nhà cung cấp: " + ex.Message);
            }
        }

        public async Task<NhaCungCapDTO?> Update(int id, NhaCungCapDTO dto)
        {
            try
            {
                var existing = await _context.NhaCungCaps.FindAsync(id);
                if (existing == null) return null; //để Controller trả 404

                existing.Name = dto.Name;
                existing.Phone = dto.Phone;
                existing.Email = dto.Email;
                existing.Address = dto.Address;
                existing.Status = dto.Status;

                await _context.SaveChangesAsync();
                return _mapper.Map<NhaCungCapDTO>(existing);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật nhà cung cấp: " + ex.Message);
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var existing = await _context.NhaCungCaps.FirstOrDefaultAsync(x => x.SupplierId == id);
                if (existing == null) return false;

                _context.NhaCungCaps.Remove(existing);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xoá nhà cung cấp: " + ex.Message);
            }
        }

        // Helper để Controller dùng validate 
        public async Task<bool> IsSupplierIdExist(int supplierId)
        {
            return await _context.NhaCungCaps.AnyAsync(x => x.SupplierId == supplierId);
        }

        public async Task<bool> IsSupplierExist(string name, string email, string phone, int? ignoreId = null)
        {
            name = name?.Trim() ?? "";
            email = email?.Trim() ?? "";
            phone = phone?.Trim() ?? "";

            var q = _context.NhaCungCaps.AsQueryable();
            if (ignoreId.HasValue) q = q.Where(x => x.SupplierId != ignoreId.Value);

            return await q.AnyAsync(x =>
                (name != "" && x.Name == name) ||
                (email != "" && x.Email == email) ||
                (phone != "" && x.Phone == phone));
        }
    }
}
