using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models;
using StoreManagementBE.BackendServer.Models.Entities;
using StoreManagementBE.BackendServer.Services.Interfaces;

namespace StoreManagementBE.BackendServer.Services
{
    public class MaGiamGiaService : IMaGiamGiaService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public MaGiamGiaService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<MaGiamGiaDTO>> GetAll()
        {
            var list = await _context.MaGiamGias.AsNoTracking().ToListAsync();
            return _mapper.Map<List<MaGiamGiaDTO>>(list);
        }

        public async Task<MaGiamGiaDTO?> GetById(int id)
        {
            var entity = await _context.MaGiamGias.FindAsync(id);
            return _mapper.Map<MaGiamGiaDTO>(entity);
        }

        public async Task<MaGiamGiaDTO?> Create(MaGiamGiaDTO dto)
        {
            var entity = _mapper.Map<MaGiamGia>(dto);
            _context.MaGiamGias.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<MaGiamGiaDTO>(entity);
        }

        public async Task<MaGiamGiaDTO?> Update(MaGiamGiaDTO dto)
        {
            var entity = await _context.MaGiamGias.FindAsync(dto.PromoId);
            if (entity == null) return null;

            entity.PromoCode = dto.PromoCode;
            entity.Description = dto.Description;
            entity.DiscountType = dto.DiscountType;
            entity.DiscountValue = dto.DiscountValue;
            entity.StartDate = dto.StartDate;
            entity.EndDate = dto.EndDate;
            entity.MinOrderAmount = dto.MinOrderAmount;
            entity.UsageLimit = dto.UsageLimit;
            entity.UsedCount = dto.UsedCount;
            entity.Status = dto.Status;

            await _context.SaveChangesAsync();
            return _mapper.Map<MaGiamGiaDTO>(entity);
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _context.MaGiamGias.FindAsync(id);
            if (entity == null) return false;
            _context.MaGiamGias.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<MaGiamGiaDTO>> SearchByKeyword(string keyword)
        {
            keyword = keyword?.Trim().ToLower() ?? "";
            var list = await _context.MaGiamGias
                .Where(x => x.PromoCode.ToLower().Contains(keyword) ||
                            (x.Description != null && x.Description.ToLower().Contains(keyword)))
                .ToListAsync();
            return _mapper.Map<List<MaGiamGiaDTO>>(list);
        }
    }
}
