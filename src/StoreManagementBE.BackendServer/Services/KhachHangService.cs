
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

        public async Task<List<KhachHangDTO>> GetAll()
        {
            try
            {
                var list = await _context.KhachHangs.ToListAsync();
                return _mapper.Map<List<KhachHangDTO>>(list);
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi lấy danh sách khách hàng: " + e.Message);
            }
        }

        public async Task<KhachHangDTO> GetById(int id)
        {
            try
            {
                var kh = await _context.KhachHangs.FirstOrDefaultAsync(x => x.CustomerId == id);
                return _mapper.Map<KhachHangDTO>(kh);
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi lấy khách hàng theo ID: " + e.Message);
            }
        }

        public async Task<List<KhachHangDTO>> SearchByKeyword(string keyword)
        {
            try
            {
                var query = _context.KhachHangs.AsQueryable();

                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(x => x.Name.ToLower().Contains(keyword.ToLower()));
                }

                var list = await query.ToListAsync();
                return _mapper.Map<List<KhachHangDTO>>(list);
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi tìm kiếm khách hàng: " + e.Message);
            }
        }

        public async Task<bool> CheckExistID(int id)
        {
            return await _context.KhachHangs.AnyAsync(x => x.CustomerId == id);
        }

        public async Task<bool> CheckExistPhone(string phone)
        {
            return await _context.KhachHangs.AnyAsync(x => x.Phone == phone);
        }

        public async Task<bool> CheckExistEmail(string email)
        {
            return await _context.KhachHangs.AnyAsync(x => x.Email == email);
        }

        public async Task<KhachHangDTO> Create(KhachHangDTO dto)
        {
            try
            {
                var kh = new KhachHang
                {
                    Name = dto.Name,
                    Phone = dto.Phone,
                    Email = dto.Email,
                    Address = dto.Address,
                    CreatedAt = DateTime.Now,
                    RewardPoints = dto.RewardPoints
                };

                _context.KhachHangs.Add(kh);
                await _context.SaveChangesAsync();

                return _mapper.Map<KhachHangDTO>(kh);
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi thêm khách hàng: " + e.Message);
            }
        }

        public async Task<KhachHangDTO?> Update(KhachHangDTO dto)
        {
            try
            {
                var existing = await _context.KhachHangs
                    .FirstOrDefaultAsync(x => x.CustomerId == dto.CustomerId);

                if (existing == null)
                    return null;

                existing.Name = dto.Name;
                existing.Phone = dto.Phone;
                existing.Email = dto.Email;
                existing.Address = dto.Address;
                existing.RewardPoints = dto.RewardPoints;

                await _context.SaveChangesAsync();

                return _mapper.Map<KhachHangDTO>(existing);
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi cập nhật khách hàng: " + e.Message);
            }
        }
    }
}