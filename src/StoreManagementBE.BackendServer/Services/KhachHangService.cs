using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models;
using StoreManagementBE.BackendServer.Models.Entities;
using StoreManagementBE.BackendServer.Services.Interfaces;

namespace StoreManagementBE.BackendServer.Services
{
    public class KhachHangService : IKhachHangService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public KhachHangService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Lấy tất cả khách hàng
        public async Task<List<KhachHangDTO>> GetAll()
        {
            var list = await _context.KhachHangs.ToListAsync();
            return _mapper.Map<List<KhachHangDTO>>(list);
        }

        // Lấy khách hàng theo ID
        public async Task<KhachHangDTO> GetById(int customer_id)
        {
            var customer = await _context.KhachHangs.FindAsync(customer_id);
            return customer == null ? null : _mapper.Map<KhachHangDTO>(customer);
        }

        // Tìm kiếm khách hàng theo keyword (theo tên, sđt, email)
        public List<KhachHang> SearchByKeyword(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return _context.KhachHangs.ToList();

            return _context.KhachHangs
                .Where(x => x.Name.Contains(keyword) 
                            || x.Phone.Contains(keyword) 
                            || x.Email.Contains(keyword))
                .ToList();
        }

        // Tạo mới khách hàng
        public async Task<ApiResponse<KhachHangDTO>> Create(KhachHangDTO khachHangDTO)
        {
            try
            {
                var exists = await _context.KhachHangs
                    .AnyAsync(x => x.Email == khachHangDTO.Email || x.Phone == khachHangDTO.Phone);

                if (exists)
                {
                    return new ApiResponse<KhachHangDTO>
                    {
                        Success = false,
                        Message = "Khách hàng đã tồn tại (Email hoặc SĐT trùng)!"
                    };
                }

                var entity = _mapper.Map<KhachHang>(khachHangDTO);
                _context.KhachHangs.Add(entity);
                await _context.SaveChangesAsync();

                return new ApiResponse<KhachHangDTO>
                {
                    Success = true,
                    Message = "Thêm khách hàng thành công!",
                    DataDTO = _mapper.Map<KhachHangDTO>(entity)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<KhachHangDTO>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        // Cập nhật khách hàng
        public async Task<ApiResponse<KhachHangDTO>> Update(int id, KhachHangDTO khachHangDTO)
        {
            try
            {
                var existing = await _context.KhachHangs.FindAsync(id);
                if (existing == null)
                {
                    return new ApiResponse<KhachHangDTO>
                    {
                        Success = false,
                        Message = "Không tìm thấy khách hàng để cập nhật!"
                    };
                }

                // Update fields
                existing.Name = khachHangDTO.Name;
                existing.Phone = khachHangDTO.Phone;
                existing.Email = khachHangDTO.Email;
                existing.Address = khachHangDTO.Address;

                _context.KhachHangs.Update(existing);
                await _context.SaveChangesAsync();

                return new ApiResponse<KhachHangDTO>
                {
                    Success = true,
                    Message = "Cập nhật khách hàng thành công!",
                    DataDTO = _mapper.Map<KhachHangDTO>(existing)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<KhachHangDTO>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public Task<ApiResponse<bool>> Delete(int customer_id)
        {
            throw new NotImplementedException();
        }
    }
}
