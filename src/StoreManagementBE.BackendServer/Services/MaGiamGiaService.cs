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

        public async Task<PagedResult<MaGiamGiaDTO>> GetAll(int page, int pageSize, string? keyword, string? discountType)
        {
            var query = _context.MaGiamGias.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                var lowerKeyword = keyword.ToLower();
                query = query.Where(x => x.PromoCode.ToLower().Contains(lowerKeyword) ||
                                         (x.Description != null && x.Description.ToLower().Contains(lowerKeyword)));
            }

            if (!string.IsNullOrEmpty(discountType))
            {
                query = query.Where(x => x.DiscountType == discountType);
            }

            var total = await query.CountAsync();
            var data = await query
                .OrderByDescending(x => x.PromoId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<MaGiamGiaDTO>
            {
                Data = _mapper.Map<List<MaGiamGiaDTO>>(data),
                Total = total,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<MaGiamGiaDTO?> GetById(int id)
        {
            var entity = await _context.MaGiamGias.FindAsync(id);
            return entity == null ? null : _mapper.Map<MaGiamGiaDTO>(entity);
        }

        public async Task<ApiResponse<MaGiamGiaDTO>> Create(MaGiamGiaDTO dto)
        {
            try
            {
                var exists = await _context.MaGiamGias.AnyAsync(x => x.PromoCode == dto.PromoCode);
                if (exists)
                {
                    return new ApiResponse<MaGiamGiaDTO>
                    {
                        Success = false,
                        Message = "Mã giảm giá đã tồn tại!"
                    };
                }

                var entity = _mapper.Map<MaGiamGia>(dto);
                _context.MaGiamGias.Add(entity);
                await _context.SaveChangesAsync();

                var resultDto = _mapper.Map<MaGiamGiaDTO>(entity);

                return new ApiResponse<MaGiamGiaDTO>
                {
                    Success = true,
                    Message = "Thêm mã giảm giá thành công!",
                    DataDTO = resultDto
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<MaGiamGiaDTO>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ApiResponse<MaGiamGiaDTO>> Update(int id, MaGiamGiaDTO dto)
        {
            try
            {
                var existing = await _context.MaGiamGias.FindAsync(id);
                if (existing == null)
                {
                    return new ApiResponse<MaGiamGiaDTO>
                    {
                        Success = false,
                        Message = "Không tìm thấy mã giảm giá để cập nhật!"
                    };
                }

                if (dto.PromoCode != existing.PromoCode && await _context.MaGiamGias.AnyAsync(x => x.PromoCode == dto.PromoCode))
                {
                    return new ApiResponse<MaGiamGiaDTO>
                    {
                        Success = false,
                        Message = "Mã giảm giá đã được sử dụng bởi mã khác!"
                    };
                }

                existing.PromoCode = dto.PromoCode;
                existing.Description = dto.Description;
                existing.DiscountType = dto.DiscountType;
                existing.DiscountValue = dto.DiscountValue;
                existing.StartDate = dto.StartDate;
                existing.EndDate = dto.EndDate;
                existing.MinOrderAmount = dto.MinOrderAmount;
                existing.UsageLimit = dto.UsageLimit;
                existing.UsedCount = dto.UsedCount;
                existing.Status = dto.Status;

                await _context.SaveChangesAsync();

                var resultDto = _mapper.Map<MaGiamGiaDTO>(existing);

                return new ApiResponse<MaGiamGiaDTO>
                {
                    Success = true,
                    Message = "Cập nhật mã giảm giá thành công!",
                    DataDTO = resultDto
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<MaGiamGiaDTO>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ApiResponse<bool>> Delete(int id)
        {
            try
            {
                var existing = await _context.MaGiamGias.FindAsync(id);
                if (existing == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Không tìm thấy mã giảm giá để xóa!",
                        DataDTO = false
                    };
                }

                _context.MaGiamGias.Remove(existing);
                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Xóa mã giảm giá thành công!",
                    DataDTO = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = ex.Message,
                    DataDTO = false
                };
            }
        }

        public async Task<List<MaGiamGiaDTO>> SearchByKeyword(string keyword)
        {
            IQueryable<MaGiamGia> query = _context.MaGiamGias;

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim().ToLower();
                query = query.Where(x =>
                    x.PromoCode.ToLower().Contains(keyword) ||
                    (x.Description != null && x.Description.ToLower().Contains(keyword)));
            }

            var result = await query.ToListAsync();
            return _mapper.Map<List<MaGiamGiaDTO>>(result);
        }
    }
}